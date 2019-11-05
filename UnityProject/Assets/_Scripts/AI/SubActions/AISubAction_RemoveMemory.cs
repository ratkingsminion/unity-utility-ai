using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RatKing.AI {

	public class AISubAction_RemoveMemory : AISubAction {
		[Header("SubAction specific")]
		[SerializeField] string varName = "";
		[SerializeField, EnumFlag] VariableFlag varType = 0;

		//

		public override void DoSubAction() {
			if ((varType & VariableFlag.Bool) != 0) { agent.Memory.RemoveBool(varName); }
			if ((varType & VariableFlag.Float) != 0) { agent.Memory.RemoveFloat(varName); }
			if ((varType & VariableFlag.Int) != 0) { agent.Memory.RemoveInt(varName); }
			if ((varType & VariableFlag.String) != 0) { agent.Memory.RemoveString(varName); }
			if ((varType & VariableFlag.UnityObject) != 0) { agent.Memory.RemoveUnityObject(varName); }
			if ((varType & VariableFlag.Vector3) != 0) { agent.Memory.RemoveVector3(varName); }
		}
	}

}