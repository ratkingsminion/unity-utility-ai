using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RatKing.AI {

	public class AISubAction_SetMemoryBool : AISubAction {
		[Header("SubAction specific")]
		[SerializeField] string boolName = "";
		[SerializeField] bool value = true;

		//

		public override void DoSubAction() {
			agent.Memory.SetBool(boolName, value);
		}
	}

}