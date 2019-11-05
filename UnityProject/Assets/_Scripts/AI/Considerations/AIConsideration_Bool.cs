using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace RatKing.AI {

	public class AIConsideration_Bool : AIConsideration {
		[SerializeField] string boolName = "";
		[SerializeField] VariableSource source = VariableSource.SensorsOnly;
		[SerializeField, Range(0.0f, 1.0f)] float scoreForTrue = 1f;
		[SerializeField, Range(0.0f, 1.0f)] float scoreForFalse = 0f;

		//

		public override float GetScore() {
			var value = agent.GetDataBool(boolName, source);
			//Debug.Log(boolName + ": " + agent.GetSensorDataBool(boolName) + " " + agent.GetSensorDataInfo());
			return value ? scoreForTrue : scoreForFalse;
		}
	}

}