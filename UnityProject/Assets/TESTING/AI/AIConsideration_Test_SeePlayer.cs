using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace RatKing.AI {

	public class AIConsideration_Test_SeePlayer : AIConsideration {
		[SerializeField] bool playerIsVisible = true;
		[SerializeField] Base.RangeFloat fromNearness = new Base.RangeFloat(0f, 1f);
		[SerializeField] Base.RangeFloat toFactor = new Base.RangeFloat(0f, 1f);

		public override float GetScore() {
			var see = agent.GetSensorDataBool("PlayerVisible", false);
			if (see != playerIsVisible) { return 0f; }
			Debug.Log("notice player");
			return Base.Math.RemapClamped(agent.GetSensorDataFloat("PlayerNormalizedNearness", 1f), fromNearness.min, fromNearness.max, toFactor.min, toFactor.max);
		}
	}

}