using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace RatKing.AI {

	public enum ScoreCalculation {
		Multiply,
		Max,
		Min,
		Add,
		Average
	}

	public abstract class AIAction<TState> : MonoBehaviour, IAIActionContainer where TState : struct, System.IConvertible, System.IComparable {
		[SerializeField] protected ScoreCalculation scoreCalculation;
		[SerializeField] protected bool scoreEnabledOnly = true;
		[SerializeField] protected float updateTime = 0.1f;
		//
		protected AIAgent agent;
		protected AIStateController<TState> controller;
		protected IAIConsideration[] considerations;
		protected AISubAction[] subActionsOnActivate;
		protected AISubAction[] subActionsOnDeactivate;
		protected AISubAction[] subActionsOnExecute;
		//
#if UNITY_EDITOR
		string originalName;
		public string Name { get { return originalName; } }
#else
		public string Name { get { return name; } }
#endif
		public float LastCalculatedScore { get; protected set; }
		public float UpdateTime { get { return updateTime; } protected set { updateTime = value; } }
		public IAIActionContainer CurrentAction { get { return this; } }

		//

		void IAIActionContainer.Init(AIAgent agent) {
			this.agent = agent;
			this.controller = agent.gameObject.GetComponent<AIStateController<TState>>();
			considerations = GetComponents<IAIConsideration>();
			var subActions = new List<AISubAction>(GetComponents<AISubAction>());
			foreach (var sa in subActions) { sa.SetAgent(agent); }
			subActionsOnActivate = subActions.FindAll(sa => (sa.Occurence & AISubAction.OccurenceType.OnActivate) > 0).ToArray();
			subActionsOnDeactivate = subActions.FindAll(sa => (sa.Occurence & AISubAction.OccurenceType.OnDeactivate) > 0).ToArray();
			subActionsOnExecute = subActions.FindAll(sa => (sa.Occurence & AISubAction.OccurenceType.OnExecute) > 0).ToArray();
			foreach (var c in considerations) { c.SetAgent(agent); }
#if UNITY_EDITOR
			originalName = name;
#endif
			OnInitAgent();
		}

		public virtual void OnInitAgent() {
		}

		IAIActionContainer IAIActionContainer.CalculateScore() {
			if (considerations.Length == 0) {
				// early exit because we don't have to consider this action at all
				LastCalculatedScore = 0f; // scoreCalculation == ScoreCalculation.Multiply ? 1f : 0f;
				return null;
			}

			switch (scoreCalculation) {
				case ScoreCalculation.Multiply:
					LastCalculatedScore = 0f;
					foreach (var c in considerations) {
						if (scoreEnabledOnly && !c.Enabled) { continue; }
						if (LastCalculatedScore == 0f) { LastCalculatedScore = 1f; }
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

#if UNITY_EDITOR
			if (!name.Contains("!!")) { name = originalName + " " + LastCalculatedScore.ToString("0.###"); }
#endif

			return this;
		}

		void IAIActionContainer.MultiplyLastCalculatedScore(float factor) {
			LastCalculatedScore *= factor;
#if UNITY_EDITOR
			if (!name.Contains("!!")) { name = originalName + " " + LastCalculatedScore.ToString("0.###"); }
#endif
		}

		void IAIActionContainer.Deactivate() {
#if UNITY_EDITOR
			name = originalName;
#endif
			foreach (var s in subActionsOnDeactivate) { s.DoSubAction(); }
			OnDeactivateAction();
		}

		IAIActionContainer IAIActionContainer.Execute() {
#if UNITY_EDITOR
			name = "!! " + originalName + " " + LastCalculatedScore.ToString("0.###");
#endif
			OnExecuteAction();
			foreach (var s in subActionsOnExecute) { s.DoSubAction(); }
			return this;
		}

		void IAIActionContainer.Activate() {
			OnActivateAction();
			foreach (var s in subActionsOnActivate) { s.DoSubAction(); }
		}

		//

		protected virtual void OnActivateAction() {
			//throw new System.NotImplementedException();
		}

		protected virtual void OnDeactivateAction() {
			//throw new System.NotImplementedException();
		}

		protected abstract void OnExecuteAction();
	}

}