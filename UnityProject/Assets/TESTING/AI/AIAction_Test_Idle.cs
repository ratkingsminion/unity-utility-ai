using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace RatKing.AI {

	public class AIAction_Test_Idle : AIAction<AIState_Test> {
		[Header("Idle Specific")]
		[SerializeField] Animation anim = null;
		[SerializeField] AnimationClip randomAnimNonlooped = null;
		[SerializeField] AnimationClip randomAnimLooped = null;
		//
		AnimationClip curAnim;
		bool animated;

		//

		protected override void OnActivateAction() {
			Debug.Log("BEGIN IDLE");

			controller.ChangeState(
				AIState_Test.Idle,
				null,
				() => { // start state
					animated = true;
					foreach (var r in controller.Root.GetComponentsInChildren<Renderer>()) { r.material.color = Color.white; }
				});

			agent.ExecuteEveryFrame += () => {
				Base.QuickIMGUI.DrawOnce(() => GUI.Label(new Rect(100, 100, 100, 100), "NOW SNEAK!"));
			};
		}

		protected override void OnDeactivateAction() {
		}

		protected override void OnExecuteAction() {
			// make a random idle animation
			var factor = Mathf.PerlinNoise(1000f + Time.time * 0.3f, 1000f);
			//Debug.Log(controller.CurState + " " + (curAnim != null ? curAnim.isLooping : false) + " T:" + (curAnim != null ? anim[curAnim.name].time : -1.0f) + " W:" + (curAnim != null ? anim[curAnim.name].weight : -1.0f));
			if (!animated && controller.CurState == AIState_Test.Idle && factor < 0.4f) {
				animated = true;
				curAnim = Random.value < 0.5f ? randomAnimNonlooped : randomAnimLooped;
				var curAnimName = curAnim.name;
				controller.ChangeState(
					AIState_Test.Animate,
					null,
					() => {
						Debug.Log("test anim BEGIN " + curAnimName);
						anim[curAnimName].time = 0f;
						anim.CrossFade(curAnimName, 0.5f);
					}, null, () => {
						Debug.Log("test anim STOP BY STATE");
						curAnim = null;
					});
			}
			else if (controller.CurState == AIState_Test.Animate && !curAnim.isLooping && (anim[curAnim.name].time <= 0f || anim[curAnim.name].time >= curAnim.length - 0.5f)) {
				Debug.Log("test anim STOP BY CODE (?)");
				controller.ChangeState(AIState_Test.Idle);
			}
			else if (controller.CurState == AIState_Test.Animate && curAnim.isLooping && factor > 0.5f) {
				Debug.Log("test anim STOP BY CODE (?)");
				controller.ChangeState(AIState_Test.Idle);
			}
			if (animated && factor > 0.5f) {
				animated = false;
			}
		}
	}

}