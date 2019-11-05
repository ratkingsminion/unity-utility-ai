using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace RatKing.AI {

	public class AIAction_Test_FollowPosition : AIAction<AIState_Test> {
		[Header("Follow Specific")]
		[SerializeField] string targetPositionName = "";
		[SerializeField] float randomDistance = 0f;

		//

		protected override void OnActivateAction() {
			Debug.Log("BEGIN FOLLOWING");

			controller.ChangeState(
				AIState_Test.WalkToTarget,
				agent.Memory.GetVector3(targetPositionName),
				() => { // start state
					foreach (var r in controller.Root.GetComponentsInChildren<Renderer>()) { r.material.color = Color.cyan; }
				}, (randomDistance <= 0f) ? (System.Action)null : (() => {
					if (controller.DistanceToCur() < 0.05f) {
						controller.CurStateInfo = controller.CurStateInfo.SetUserData(
							agent.Memory.GetVector3(targetPositionName) +
							(Vector3)Random.insideUnitCircle.normalized * randomDistance
						);
						Debug.Log(controller.CurStateInfo.userData);
					}
				})
			);
		}

		protected override void OnDeactivateAction() {
		}

		protected override void OnExecuteAction() {
		}
	}

}