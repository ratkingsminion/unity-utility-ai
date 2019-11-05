using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace RatKing.AI {

	public enum ActionSelectMethod {
		Top
	}

	public sealed class AIActionSelector : MonoBehaviour, IAIActionContainer {
		[SerializeField] ScoreCalculation scoreCalculation = 0;
		[SerializeField] ActionSelectMethod actionSelection = 0;
		[SerializeField] bool scoreEnabledOnly = true;
		//
		List<IAIActionContainer> actions;
		//
#if UNITY_EDITOR
		string originalName;
		public string Name { get { return originalName; } }
#else
		public string Name { get { return name; } }
#endif
		public float LastCalculatedScore { get; private set; }
		public float UpdateTime { get; private set; }
		IAIActionContainer lastSelectedAction;
		public IAIActionContainer CurrentAction { get { return lastSelectedAction; } }
		//
		AIAgent agent;
		IAIConsideration[] considerations;

		//

		//public AIActionSelector(params IAIActionContainer[] actions) {
		//	actionSelection = ActionSelectMethod.Top;
		//	this.actions = new List<IAIActionContainer>(actions);
		//}

		//public AIActionSelector(ActionSelectMethod actionSelection, params IAIActionContainer[] actions) {
		//	this.actionSelection = actionSelection;
		//	this.actions = new List<IAIActionContainer>(actions);
		//}

		//

		void IAIActionContainer.Init(AIAgent agent) {
			this.agent = agent;
			considerations = GetComponents<IAIConsideration>();
			foreach (var c in considerations) { c.SetAgent(agent); }
			actions = new List<IAIActionContainer>();
			for (int i = 0; i < transform.childCount; ++i) {
				var child = transform.GetChild(i);
				if (!child.gameObject.activeSelf) { continue; }
				var action = child.GetComponent<IAIActionContainer>();
				if (action != null) { actions.Add(action); }
			}
			foreach (var a in actions) {
				a.Init(agent);
			}
#if UNITY_EDITOR
			originalName = name;
#endif
		}

		IAIActionContainer IAIActionContainer.CalculateScore() {
			LastCalculatedScore = 1f;
			if (considerations.Length > 0) {
				// only do considerations if there are some - those will then
				// change the calculations of the child actions!

				switch (scoreCalculation) {
					case ScoreCalculation.Multiply:
						foreach (var c in considerations) {
							if (scoreEnabledOnly && !c.Enabled) { continue; }
							var s = c.GetScore();
							// if (s <= 0f) { return Score = 0f; } // uncommented because minus * minus is positive ...
							if (s == 0f) { LastCalculatedScore = 0f; break; }
							LastCalculatedScore *= s;
						}
						break;

					case ScoreCalculation.Max:
						LastCalculatedScore = 0f;
						foreach (var c in considerations) {
							if (scoreEnabledOnly && !c.Enabled) { continue; }
							LastCalculatedScore = Mathf.Max(LastCalculatedScore, c.GetScore());
						}
						break;

					case ScoreCalculation.Min:
						var considered = false;
						foreach (var c in considerations) {
							if (scoreEnabledOnly && !c.Enabled) { continue; }
							if (!considered) { considered = true; LastCalculatedScore = c.GetScore(); }
							else { LastCalculatedScore = Mathf.Min(LastCalculatedScore, c.GetScore()); }
						}
						break;

					case ScoreCalculation.Add:
						LastCalculatedScore = 0f;
						foreach (var c in considerations) {
							if (scoreEnabledOnly && !c.Enabled) { continue; }
							LastCalculatedScore += c.GetScore();
						}
						break;

					case ScoreCalculation.Average:
						LastCalculatedScore = 0f;
						foreach (var c in considerations) {
							if (scoreEnabledOnly && !c.Enabled) { continue; }
							LastCalculatedScore += c.GetScore();
						}
						LastCalculatedScore /= considerations.Length;
						break;
				}
			}
			
#if UNITY_EDITOR
			if (!name.Contains("!!")) { name = originalName + " " + LastCalculatedScore.ToString("0.###"); }
#endif
			
			if (LastCalculatedScore > 0f) {
				foreach (var a in actions) {
					a.CalculateScore();
					a.MultiplyLastCalculatedScore(LastCalculatedScore);
				}

				if (agent.DebugMode) { foreach (var a in actions) { Debug.Log("--> " + a.Name + " " + a.LastCalculatedScore); } }

				if (actionSelection == ActionSelectMethod.Top) {
					actions.Sort((a, b) => {
						var res = b.LastCalculatedScore - a.LastCalculatedScore;
						return res > 0f ? 1 : (res < 0f ? -1 : 0);
					});
					if (agent.DebugMode) { Debug.Log("Selector -> " + actions[0].Name + " " + actions[0].LastCalculatedScore); }
					lastSelectedAction = actions[0].CurrentAction;
					LastCalculatedScore = lastSelectedAction.LastCalculatedScore;
					UpdateTime = lastSelectedAction.UpdateTime;
					return lastSelectedAction;
				}
			}

			return null;
		}

		void IAIActionContainer.MultiplyLastCalculatedScore(float factor) {
			LastCalculatedScore *= factor;
#if UNITY_EDITOR
			if (!name.Contains("!!")) { name = originalName + " " + LastCalculatedScore.ToString("0.###"); }
#endif
		}

		void IAIActionContainer.Activate() {
			if (actions.Count == 0) { return; }
			lastSelectedAction.Activate();
		}

		void IAIActionContainer.Deactivate() {
			if (actions.Count == 0) { return; }
			lastSelectedAction.Deactivate();
		}
		
		IAIActionContainer IAIActionContainer.Execute() {
			if (actions.Count == 0) { return null; }
			return lastSelectedAction.Execute();
		}
	}

}