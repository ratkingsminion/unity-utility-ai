using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonsterLove.StateMachine;

namespace RatKing.AI {

	[RequireComponent(typeof(AIAgent))]
	public class AIStateController<TState> : MonoBehaviour, IAIStateController where TState : struct, System.IConvertible, System.IComparable {
		[SerializeField] bool ignoreYCoord = true;
		//
		public GameObject Root { get; private set; }
		public AIStateInfo<TState> CurStateInfo { get; set; }
		public TState CurState { get { return CurStateInfo.state; } }
		public string CurStateName { get { return CurStateInfo.state.ToString(); } }
		//
		public Vector3 StartPos { get; private set; }
		public Vector3 StartDir { get; private set; }
		public Quaternion StartRot { get; private set; }
		public AIAgent Agent { get; private set; }
		//
		StateMachine<TState> fsm;
		float stoodStillTime;
		Vector3 lastRootPosition;

		//

		void Awake() {
			Agent = GetComponent<AIAgent>();
		}

#if UNITY_EDITOR
		IEnumerator Start() {
			yield return null;
			if (Root == null) {
				Debug.LogWarning("THIS AGENT DOESN'T HAVE A ROOT");
				gameObject.SetActive(false);
			}
		}
#endif

		protected virtual void Update() {
			if (Root == null) { return; }

			// general knowledge
			UpdateGeneralKnowledge();

			Agent.UpdateActions();

			if (CurStateInfo.onUpdate != null) {
				CurStateInfo.onUpdate();
			}

			// check standing still
			var dist = Vector3.SqrMagnitude(Root.transform.position - lastRootPosition);
			if (dist > 0.02f * 0.02f) {
				stoodStillTime = Time.time;
				lastRootPosition = Root.transform.position;
			}
		}

		//

		public virtual void Init(GameObject root, MonoBehaviour fsmBehaviour, TState startState) {
			Root = root;
			Agent.Init(this);
			// fsm
			fsm = StateMachine<TState>.Initialize(fsmBehaviour); // https://github.com/thefuntastic/Unity3d-Finite-State-Machine
			ChangeState(startState);
			// general knowledge
			ResetStartPos();
		}

		// GENERAL KNOWLEDGE

		void UpdateGeneralKnowledge() {
			// TODO elsewhere?
			var startDir = StartPos - Root.transform.position;
			if (ignoreYCoord) { startDir.y = 0f; }
			Agent.Memory.SetFloat("Start_Distance", startDir.magnitude);
		}

		public void ResetStartPos() {
			if (Root == null) { return; }
			StartPos = Root.transform.position;
			StartDir = Root.transform.forward;
			StartRot = Root.transform.rotation;
			Agent.Memory.SetVector3("Start_Position", StartPos);
		}

		// TARGET TRANSFORM / OTHER POSITION BASED STUFF
		// TODO not really the right place?

		public Vector3 GetPosition() {
			return Root.transform.position;
		}

		public float DistanceToCur() {
			Vector3 target;
			if (CurStateInfo.userData is Vector3) { target = (Vector3)CurStateInfo.userData; }
			else if (CurStateInfo.userData is Transform) { target = ((Transform)CurStateInfo.userData).position; }
			else { return float.MaxValue; } // userdata not available
			var dir = target - Root.transform.position;
			if (ignoreYCoord) { dir.y = 0f; }
			return dir.magnitude;
		}

		public float DistanceTo(Transform trans) {
			if (trans == null) { return float.MaxValue; } // TODO error?
			var dir = trans.position - Root.transform.position;
			if (ignoreYCoord) { dir.y = 0f; }
			return dir.magnitude;
		}

		public float DistanceTo(Vector3 pos) {
			var dir = pos - Root.transform.position;
			if (ignoreYCoord) { dir.y = 0f; }
			return dir.magnitude;
		}

		public bool StoodStillSinceStateChange(float seconds) {
			return Time.time - stoodStillTime >= seconds;
		}

		// STATES

		public virtual void ChangeState(TState state, object userData = null, System.Action onStart = null, System.Action onUpdate = null, System.Action onExit = null) {
			// TODO: check connections
#if DEBUG_AI
			//Debug.Log("CHANGE STATE NOW " + state);
#endif
			//
			if (CurStateInfo.onExit != null) { CurStateInfo.onExit(); }
			//
			var oldStateInfo = CurStateInfo;
			CurStateInfo = new AIStateInfo<TState>(state, userData, null, onUpdate, onExit); // doesn't need onStart anymore, will get called now
			//nextStateInfo = new AIStateInfo(); // empty
			//
			if (onStart != null) { onStart(); }
			//
			fsm.ChangeState(CurState);
		}
	}

}