using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace RatKing.AI {

	public class AIAction_Test_FollowTransform : AIAction<AIState_Test> {
		[Header("Follow Specific")]
		[SerializeField] string targetTransformName = null;

		//

		protected override void OnActivateAction() {
			Debug.Log("BEGIN FOLLOWING");

			controller.ChangeState(
				AIState_Test.WalkToTarget,
				agent.GetSensorDataUnityObject(targetTransformName),
				() => { // start state
					foreach (var r in controller.Root.GetComponentsInChildren<Renderer>()) { r.material.color = Color.yellow; }
				}
			);
		}

		protected override void OnDeactivateAction() {
		}

		protected override void OnExecuteAction() {
		}
	}

}