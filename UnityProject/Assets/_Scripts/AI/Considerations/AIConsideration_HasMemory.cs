using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace RatKing.AI {

	public class AIConsideration_HasMemory : AIConsideration {
		[SerializeField] string varName = "";
		[SerializeField, EnumFlag] VariableFlag varType = 0;
		[SerializeField, Range(0.0f, 1.0f)] float scoreForTrue = 1f;
		[SerializeField, Range(0.0f, 1.0f)] float scoreForFalse = 0f;

		//

		public override float GetScore() {
			if ((varType & VariableFlag.Bool) != 0 && agent.Memory.HasBool(varName)) { return scoreForTrue; }
			if ((varType & VariableFlag.Float) != 0 && agent.Memory.HasFloat(varName)) { return scoreForTrue; }
			if ((varType & VariableFlag.Int) != 0 && agent.Memory.HasInt(varName)) { return scoreForTrue; }
			if ((varType & VariableFlag.String) != 0 && agent.Memory.HasString(varName)) { return scoreForTrue; }
			if ((varType & VariableFlag.UnityObject) != 0 && agent.Memory.HasUnityObject(varName)) { return scoreForTrue; }
			if ((varType & VariableFlag.Vector3) != 0 && agent.Memory.HasVector3(varName)) { return scoreForTrue; }

			return scoreForFalse;
		}
	}

}