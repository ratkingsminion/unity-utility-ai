using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace RatKing.AI {

	public class AIConsideration_FloatMapped : AIConsideration {
		[SerializeField] string floatName = "";
		[SerializeField] VariableSource source = VariableSource.SensorsOnly; 
		[SerializeField] Base.RangeFloat input = new Base.RangeFloat(0f, 1f);
		[SerializeField] Base.RangeFloat output = new Base.RangeFloat(0f, 1f);

		public override float GetScore() {
			var value = agent.GetDataFloat(floatName, source);
			// if (agent.DebugMode) { Debug.Log(value + " -> " + Base.Math.RemapClamped(value, from.min, from.max, to.min, to.max)); }
			return Base.Math.RemapClamped(value, input.min, input.max, output.min, output.max);
		}
	}

}