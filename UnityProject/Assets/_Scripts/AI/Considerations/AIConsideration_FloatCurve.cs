using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace RatKing.AI {

	public class AIConsideration_FloatCurve : AIConsideration {
		[SerializeField] string floatName = "";
		[SerializeField] VariableSource source = VariableSource.SensorsOnly;
		[Tooltip("X = Input, Y = Output")]
		[SerializeField] AnimationCurve curve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

		public override float GetScore() {
			var value = agent.GetDataFloat(floatName, source);
			// if (agent.DebugMode) { Debug.Log(value + " -> " + Base.Math.RemapClamped(value, from.min, from.max, to.min, to.max)); }
			return curve.Evaluate(value);
		}
	}

}