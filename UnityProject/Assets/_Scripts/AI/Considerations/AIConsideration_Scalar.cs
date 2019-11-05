using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace RatKing.AI {

	public class AIConsideration_Scalar : AIConsideration {
		[SerializeField] float factor = 1f;

		//

		public override float GetScore() {
			return factor;
		}
	}

}