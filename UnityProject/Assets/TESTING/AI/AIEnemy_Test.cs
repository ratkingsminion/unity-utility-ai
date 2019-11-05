using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RatKing.AI;

namespace RatKing {

	public struct WalkInfo {
		public Vector3 target;
		public float speedFactor;
	}

	public class AIEnemy_Test : MonoBehaviour {

		public static readonly Base.Event<Vector3> EVENT_PLAYER_SEEN = new Base.Event<Vector3>("enemy__player_seen");

		[SerializeField] AIEnemy_Test_Controller controller = null;
		[SerializeField] Animation anim = null;
		[SerializeField] AnimationClip clipIdle = null;
		[SerializeField] AnimationClip clipWalkToTarget = null;
		[SerializeField] float walkSpeed = 3f;

		//

		void Start() {
			controller.Init(gameObject, this, AIState_Test.Idle); //, t => { return Vector3.Distance(transform.position, t.position) < 0.1f; });
			Base.Events.Register(gameObject, EVENT_PLAYER_SEEN, OnPlayerSeen);
		}

		void OnDestroy() {
			Base.Events.UnregisterAll(gameObject);
		}

		// EVENTS

		void OnPlayerSeen(Vector3 pos) {
			var dir = pos - transform.position;
			var dist = dir.magnitude;
			if (dist > 20f) { return; }
			controller.Agent.Memory.SetVector3("PlayerLastPosition", pos, 3.0f);
		}

		//

		// STATES IMPLEMENTATION
		// states: None / Idle / Animate / WalkToTarget / Dead
		// supported: _Enter / _Exit / _FixedUpdate / _Update / _LateUpdate / _Finally

		void Idle_Enter() {
			anim.CrossFade(clipIdle.name, 0.5f);
		}

		void WalkToTarget_Enter() {
			anim.CrossFade(clipWalkToTarget.name, 0.35f);
			//animator.SetBool("WalkToTarget", true);
		}

		void WalkToTarget_Update() {
			Vector3 target;
			var data = controller.CurStateInfo.userData;
			if (data is Vector3) { target = (Vector3)data; }
			else if (data is Transform) { target = ((Transform)data).position; }
			else { return; } // no target
			transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * walkSpeed);
		}
	}

}