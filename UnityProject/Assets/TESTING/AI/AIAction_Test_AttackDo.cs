using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace RatKing.AI {

	public class AIAction_Test_AttackDo : AIAction<AIState_Test> {
		[Header("Attack Do Specific")]
		[SerializeField] Animation anim = null;
		[SerializeField] AnimationClip clipAttack = null;
		// TODO: AIAction_Test_AttackStart as field

		//

		protected override void OnActivateAction() {
			Debug.Log("DO ATTACK");
		}

		protected override void OnDeactivateAction() {
			agent.Memory.RemoveBool("attack");
		}

		//void OnGUI() {
		//	GUI.Label(new Rect(10,10,300,100),
		//		":" + anim.IsPlaying(clipAttack) + " " + anim.CurrentState.Clip.name + " " + anim.CurrentState.Time);
		//}

		protected override void OnExecuteAction() {
			//if (!anim.IsPlaying(clipAttack) || anim.CurrentState.Time >= clipAttack.length) {
			if (!anim.IsPlaying(clipAttack.name) || anim[clipAttack.name].time >= clipAttack.length - 0.15f) {
				agent.Memory.RemoveBool("attack");
			}
			//agent.PlayAnimation("TestCubeWalkToAnim_ExtraAnim1", 0.25f, agent.GetSensorDataUnityObject("PlayerTransform") as Transform);
		}

		//

		//void OnAnimationStateFinished(AIAction activeAction) {
		//	if (activeAction == this) { Debug.Log("redo?"); }
		//	if (activeAction != this) { Debug.Log("watt?"); }
		//}
	}

}