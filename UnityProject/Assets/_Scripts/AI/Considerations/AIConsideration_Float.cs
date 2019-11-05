using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace RatKing.AI {

	public class AIConsideration_Float : AIConsideration {
		[SerializeField] string floatName = "";
		[SerializeField] VariableSource source = VariableSource.SensorsOnly;
		[SerializeField] float targetValue = 0f;
		[SerializeField] float scoreIfSmaller = 0f;
		[SerializeField] float score = 0f;
		[SerializeField] float scoreIfBigger = 0f;

		public override float GetScore() {
			var value = agent.GetDataFloat(floatName, source);
			// if (agent.DebugMode) { Debug.Log(value + " -> " + Base.Math.RemapClamped(value, from.min, from.max, to.min, to.max)); }
			return (value < targetValue) ? scoreIfSmaller : (value > targetValue) ? scoreIfBigger : score;
		}
	}

}