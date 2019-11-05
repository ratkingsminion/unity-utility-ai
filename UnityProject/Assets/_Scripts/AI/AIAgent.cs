#define DEBUG_AI

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MonsterLove.StateMachine;

namespace RatKing.AI {

	public enum VariableSource {
		SensorsOnly,
		MemoryOnly,
		MemoryThenSensor // checks memory first, and if value is not there, then sensors
	}

	[RequireComponent(typeof(AIMemory))]
	public class AIAgent : MonoBehaviour {
		[SerializeField] bool debugMode = false;
		[SerializeField] float senseAndDecideTime = 0.25f;
		[SerializeField] AIMemory[] additionalMemories = null;
		//
		//
		public bool DebugMode { get { return debugMode; } }
		public AIMemoriesSelector Memory { get; private set; }
		public int SensorCount { get; private set; }
		public IAIStateController Controller { get; private set; }
		//
		public System.Action ExecuteEveryFrame { get; set; } // gets automatically clear when the action deactivates!
		//
		IAIActionContainer action;
		IAIActionContainer lastActiveAction;
		List<AISensor> sensors;
		int[] sensorDataCount;
		int[] curSensorDataIndices;
		float senseAndDecideTimer, lastSenseAndDecideTime, updateExecutionTimer;
		IAIActionContainer activeAction;
		//
		//StateMachine<AIState> fsm;
		//AIStateInfo nextStateInfo;
		//
		//Transform targetDummy; // for some actions
		//public Transform TargetDummy {
		//	get {
		//		if (targetDummy != null) { return targetDummy; }
		//		targetDummy = new GameObject("<Agent Target " + Root.name + " Transform>").transform;
		//		targetDummy.gameObject.hideFlags = HideFlags.HideAndDontSave;
		//		return targetDummy;
		//	}
		//}
		
		/// <summary>
		/// Called by Controller
		/// </summary>
		public void Init(IAIStateController controller) { //, MonoBehaviour fsmBehaviour) {
			this.Controller = controller;
			//
			// actions
			for (int i = 0; i < transform.childCount; ++i) {
				action = transform.GetChild(i).GetComponent<IAIActionContainer>();
				if (action != null) {
					action.Init(this);
					break;
				}
			}
			//var creature = Root.GetComponentInChildren<Creature>();
			//if (creature != null && !creature.OnFloor) { MEC.Timing.RunCoroutine(ResetStartPosWhenOnFloorCR(creature), creature.gameObject); } // TODO not for flying creatures
			// sensors
			sensors = new List<AISensor>(GetComponentsInChildren<AISensor>());
			SensorCount = sensors.Count;
			sensorDataCount = new int[SensorCount];
			curSensorDataIndices = new int[SensorCount];
			for (var iter = sensors.GetEnumerator(); iter.MoveNext();) { iter.Current.Init(this, controller); }
			// memory
			var mainMemory = GetComponent<AIMemory>();
			if (mainMemory == null) { mainMemory = gameObject.AddComponent<AIMemory>(); }
			Memory = new AIMemoriesSelector(this, mainMemory, additionalMemories);
		}

		//IEnumerator<float> ResetStartPosWhenOnFloorCR(Creature creature) {
		//	while (creature != null) {
		//		if (creature.OnFloor) { ResetStartPos(); break; }
		//		yield return 0f;
		//	}
		//}

		/// <summary>
		/// Called by Controller
		/// </summary>
		public void UpdateActions() {
#if UNITY_EDITOR
			(action as MonoBehaviour).name = action.Name + " > " + Controller.CurStateName.ToUpper();
#endif

			// sense and decide!
			if (senseAndDecideTimer < Time.time) {
				var deltaTime = Time.time - lastSenseAndDecideTime;
				lastSenseAndDecideTime = Time.time;
				for (var iter = sensors.GetEnumerator(); iter.MoveNext(); ) { iter.Current.UpdateByAgent(deltaTime); }

				if (DebugMode) { Debug.Log("<color=green>------- new decide " + Controller.Root.name + " ----</color>"); }

				var testSensorDataIndices = new int[SensorCount]; // TODO: memory
				var bestTestActionScore = float.MinValue;

				// brain update
				Memory.UpdateVariables();

				if (SensorCount == 0) {
					// no sensors? don't need to check much ...
					activeAction = action.CalculateScore();
					bestTestActionScore = activeAction.LastCalculatedScore;
				}
				else {
					// otherwise count sensor datas
					var overallSensorDataCount = 0;
					for (int i = 0; i < SensorCount; ++i) {
						overallSensorDataCount += (sensorDataCount[i] = sensors[i].Data.Count);
						curSensorDataIndices[i] = 0;
						if (DebugMode) { Debug.Log("<color=green>----- sensor " + i + ": " + sensorDataCount[i] + "</color>"); }
					}

					if (overallSensorDataCount == 0) {
						// no data from the sensors? don't need to check much ...
						activeAction = action.CalculateScore();
						bestTestActionScore = activeAction.LastCalculatedScore;
					}
					else {
						// ...otherwise check each possible combination if there is any data, iterate over all combinations
						activeAction = null;

						int sIdx = 0; // index of sensor counter
						bool baseAgain = false; // don't need to check the "base permutation" over and over!
						do {
							sIdx = 0;
							while (sIdx < SensorCount) {
								if (!baseAgain && sensorDataCount[sIdx] > 0) {
#if DEBUG_AI
									if (DebugMode) { Debug.Log("test sensor " + sIdx); }
#endif
									var testAction = action.CalculateScore();
									if (activeAction == null || (testAction != null && testAction.LastCalculatedScore > bestTestActionScore)) {
										activeAction = testAction;
										bestTestActionScore = activeAction.LastCalculatedScore;
										for (int i = 0; i < SensorCount; ++i) { testSensorDataIndices[i] = curSensorDataIndices[i]; }
									}
								}
								// next sensor data check:
								++curSensorDataIndices[sIdx];
								baseAgain = false;
								if (curSensorDataIndices[sIdx] < sensorDataCount[sIdx]) { break; }
								curSensorDataIndices[sIdx] = 0;
								++sIdx;
								baseAgain = true;
							}
						} while (sIdx < SensorCount);
					}
				}

				// got a new action?
				if (lastActiveAction != activeAction) {
					ExecuteEveryFrame = null;
					// Debug.Log("GOT NEW ACTION " + name + " " + lastActiveAction + " " + activeAction);
					if (lastActiveAction != null) { lastActiveAction.Deactivate(); }
					if (activeAction != null) { activeAction.Activate(); }
				}

				// set the right sensor data
				for (int i = 0; i < SensorCount; ++i) { curSensorDataIndices[i] = testSensorDataIndices[i]; }
#if DEBUG_AI
				if (activeAction != null) {
					if (DebugMode) { Debug.Log("<color=yellow>CHOSEN " + activeAction.Name + " (" + bestTestActionScore + ")</color>"); }
				}
#endif

				lastActiveAction = activeAction;
				senseAndDecideTimer = Time.time + senseAndDecideTime;
			}

			if (activeAction != null && ExecuteEveryFrame != null) {
				ExecuteEveryFrame();
			}

			if (activeAction != null && Time.time > updateExecutionTimer) {
				updateExecutionTimer = Time.time + activeAction.UpdateTime;
				activeAction.Execute();
			}
		}

		// SENSORS

		public AISensor GetSensor(int index) {
			if (index < 0 || index >= SensorCount) { return null; }
			return sensors[index];
		}

		public string GetSensorDataInfo() {
			return "<c:" + SensorCount + " i:" + curSensorDataIndices.Length + ">";
		}

		public bool GetSensorDataBool(string varName, bool stdValue = false) {
			//Debug.Log("get bool " + varName);
			bool value;
			for (int i = 0; i < SensorCount; ++i) {
				var sensor = sensors[i];
				if (sensor.Data == null || sensor.Data.Count == 0) { continue; }
				var data = sensor.Data[curSensorDataIndices[i]]; // only pick those datas of the current iteration
				if (data.VarsBool.TryGetValue(varName, out value)) { return value; }
			}
			return stdValue;
		}

		public float GetSensorDataFloat(string varName, float stdValue = 0f) {
			//Debug.Log("get float " + varName);
			float value;
			for (int i = 0; i < SensorCount; ++i) {
				var sensor = sensors[i];
				if (sensor.Data == null || sensor.Data.Count == 0) { continue; }
				var data = sensor.Data[curSensorDataIndices[i]]; // only pick those datas of the current iteration
				if (data.VarsFloat.TryGetValue(varName, out value)) { return value; }
			}
			return stdValue;
		}

		public int GetSensorDataInt(string varName, int stdValue = 0) {
			//Debug.Log("get int " + varName);
			int value;
			for (int i = 0; i < SensorCount; ++i) {
				var sensor = sensors[i];
				if (sensor.Data == null || sensor.Data.Count == 0) { continue; }
				var data = sensor.Data[curSensorDataIndices[i]]; // only pick those datas of the current iteration
				if (data.VarsInt.TryGetValue(varName, out value)) { return value; }
			}
			return stdValue;
		}

		public string GetSensorDataString(string varName, string stdValue = "") {
			//Debug.Log("get string " + varName);
			string value;
			for (int i = 0; i < SensorCount; ++i) {
				var sensor = sensors[i];
				if (sensor.Data == null || sensor.Data.Count == 0) { continue; }
				var data = sensor.Data[curSensorDataIndices[i]]; // only pick those datas of the current iteration
				if (data.VarsString.TryGetValue(varName, out value)) { return value; }
			}
			return stdValue;
		}

		public Object GetSensorDataUnityObject(string varName, Object stdValue = null) {
			//Debug.Log("get UnityObject " + varName);
			Object value;
			for (int i = 0; i < SensorCount; ++i) {
				var sensor = sensors[i];
				if (sensor.Data == null || sensor.Data.Count == 0) { continue; }
				var data = sensor.Data[curSensorDataIndices[i]]; // only pick those datas of the current iteration
				if (data.VarsUnityObject.TryGetValue(varName, out value)) { return value; }
			}
			return stdValue;
		}

		public Vector3 GetSensorDataVector3(string varName, Vector3? stdValue = null) {
			//Debug.Log("get Vector3 " + varName);
			Vector3 value;
			for (int i = 0; i < SensorCount; ++i) {
				var sensor = sensors[i];
				if (sensor.Data == null || sensor.Data.Count == 0) { continue; }
				var data = sensor.Data[curSensorDataIndices[i]]; // only pick those datas of the current iteration
				if (data.VarsVector3.TryGetValue(varName, out value)) { return value; }
			}
			return stdValue == null ? Vector3.zero : stdValue.Value;
		}
		
		// GET DATA GENERAL
		
		public bool GetDataBool(string varName, VariableSource source, bool stdValue = false) {
			switch (source) {
				case VariableSource.SensorsOnly: return GetSensorDataBool(varName, stdValue);
				case VariableSource.MemoryOnly: return Memory.GetBool(varName, stdValue);
				default: if (Memory.HasBool(varName)) { return Memory.GetBool(varName); } return GetSensorDataBool(varName, stdValue);
			}
		}
		
		public int GetDataInt(string varName, VariableSource source, int stdValue = 0) {
			switch (source) {
				case VariableSource.SensorsOnly: return GetSensorDataInt(varName, stdValue);
				case VariableSource.MemoryOnly: return Memory.GetInt(varName, stdValue);
				default: if (Memory.HasInt(varName)) { return Memory.GetInt(varName); } return GetSensorDataInt(varName, stdValue);
			}
		}

		public float GetDataFloat(string varName, VariableSource source, float stdValue = 0f) {
			switch (source) {
				case VariableSource.SensorsOnly: return GetSensorDataFloat(varName, stdValue);
				case VariableSource.MemoryOnly: return Memory.GetFloat(varName, stdValue);
				default: if (Memory.HasFloat(varName)) { return Memory.GetFloat(varName); } return GetSensorDataFloat(varName, stdValue);
			}
		}
		
		public string GetDataString(string varName, VariableSource source, string stdValue = "") {
			switch (source) {
				case VariableSource.SensorsOnly: return GetSensorDataString(varName, stdValue);
				case VariableSource.MemoryOnly: return Memory.GetString(varName, stdValue);
				default: if (Memory.HasString(varName)) { return Memory.GetString(varName); } return GetSensorDataString(varName, stdValue);
			}
		}
		
		public Object GetDataUnityObject(string varName, VariableSource source, Object stdValue = null) {
			switch (source) {
				case VariableSource.SensorsOnly: return GetSensorDataUnityObject(varName, stdValue);
				case VariableSource.MemoryOnly: return Memory.GetUnityObject(varName, stdValue);
				default: if (Memory.HasUnityObject(varName)) { return Memory.GetUnityObject(varName); } return GetSensorDataUnityObject(varName, stdValue);
			}
		}

		public Vector3 GetDataVector3(string varName, VariableSource source, Vector3 stdValue = default(Vector3)) {
			switch (source) {
				case VariableSource.SensorsOnly: return GetSensorDataVector3(varName, stdValue);
				case VariableSource.MemoryOnly: return Memory.GetVector3(varName, stdValue);
				default: if (Memory.HasVector3(varName)) { return Memory.GetVector3(varName); } return GetSensorDataVector3(varName, stdValue);
			}
		}
	}

}