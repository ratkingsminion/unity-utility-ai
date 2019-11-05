using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace RatKing.AI {

	public class AIConsiderationSelector : AIConsideration {
		[SerializeField] GameObject objectWithConsiderations = null;
		[SerializeField] ScoreCalculation scoreCalculation = 0;
		[SerializeField] bool scoreEnabledOnly = true;
		//
		IAIConsideration[] considerations;

		//
		
		public override void SetAgent(AIAgent agent) {
			base.SetAgent(agent);
			if (objectWithConsiderations == gameObject) { Debug.LogError("Selected considerations may not be same game object!"); return; }
			considerations = objectWithConsiderations.GetComponents<IAIConsideration>();
			foreach (var c in considerations) { c.SetAgent(agent); }
		}

		public override float GetScore() {
			if (considerations.Length == 0) {
				// early exit because we don't have to consider this action at all
				return 0f;
			}

			var calculatedScore = 1f;

			switch (scoreCalculation) {
				case ScoreCalculation.Multiply:
					calculatedScore = 0f;
					foreach (var c in considerations) {
						if (scoreEnabledOnly && !c.Enabled) { continue; }
						if (calculatedScore == 0f) { calculatedScore = 1f; }
						var s = c.GetScore();
						// if (s <= 0f) { return Score = 0f; } // uncommented because minus * minus is positive ...
						if (s == 0f) { calculatedScore = 0f; break; }
						calculatedScore *= s;
					}
					break;

				case ScoreCalculation.Max:
					calculatedScore = 0f;
					foreach (var c in considerations) {
						if (scoreEnabledOnly && !c.Enabled) { continue; }
						calculatedScore = Mathf.Max(calculatedScore, c.GetScore());
					}
					break;

				case ScoreCalculation.Min:
					var considered = false;
					foreach (var c in considerations) {
						if (scoreEnabledOnly && !c.Enabled) { continue; }
						if (!considered) { considered = true; calculatedScore = c.GetScore(); }
						else { calculatedScore = Mathf.Min(calculatedScore, c.GetScore()); }
					}
					break;

				case ScoreCalculation.Add:
					calculatedScore = 0f;
					foreach (var c in considerations) {
						if (scoreEnabledOnly && !c.Enabled) { continue; }
						calculatedScore += c.GetScore();
					}
					break;

				case ScoreCalculation.Average:
					calculatedScore = 0f;
					foreach (var c in considerations) {
						if (scoreEnabledOnly && !c.Enabled) { continue; }
						calculatedScore += c.GetScore();
					}
					calculatedScore /= considerations.Length;
					break;
			}

			return calculatedScore;
		}
	}

}