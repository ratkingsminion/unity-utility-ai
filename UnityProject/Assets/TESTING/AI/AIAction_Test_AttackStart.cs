using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace RatKing.AI {

	public class AIAction_Test_AttackStart : AIAction<AIState_Test> {
		[Header("Attack Start Specific")]
		[SerializeField] Animation anim = null;
		[SerializeField] AnimationClip clipAttack = null;

		//

		protected override void OnActivateAction() {
			Debug.Log("BEGIN ATTACK");
			
			controller.ChangeState(
				AIState_Test.Animate,
				agent.GetSensorDataUnityObject("PlayerTransform") as Transform,
				() => { // start state
					foreach (var r in controller.Root.GetComponentsInChildren<Renderer>()) { r.material.color = Color.red; }
					agent.Memory.SetBool("attack", true);
					anim[clipAttack.name].time = 0f;
					anim.CrossFade(clipAttack.name, 0.35f);
				}, null, null
			);
		}

		protected override void OnDeactivateAction() {
		}

		protected override void OnExecuteAction() {
			//agent.PlayAnimation("TestCubeWalkToAnim_ExtraAnim1", 0.25f, agent.GetSensorDataUnityObject("PlayerTransform") as Transform);
		}
	}

}