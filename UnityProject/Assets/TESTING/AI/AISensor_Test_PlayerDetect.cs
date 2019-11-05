using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace RatKing.AI {

	public class AISensor_Test_PlayerDetect : AISensor {
		[SerializeField] float minDist = 4f;
		[SerializeField] float secondsRememberPlayer = 4f;
		//
		Transform playerTransform; // needed because for this example the player is just the mouse cursor <_<

		//

		void Awake() {
			playerTransform = new GameObject("<Player Test>").transform;
			playerTransform.gameObject.hideFlags = HideFlags.HideInHierarchy;
		}

		public override void UpdateSensorData() {
			var p = new Plane(-Camera.main.transform.forward, controller.Root.transform.position);
			var r = Camera.main.ScreenPointToRay(Input.mousePosition);
			float enter;
			if (p.Raycast(r, out enter)) {
				var point = r.GetPoint(enter);
				var dist = (point - agent.transform.position).magnitude;
				Base.QuickGizmos.DrawSphere(point, 0.25f, Color.yellow, updateMinTime * 1.5f, true);

				if (dist < minDist) {
					// current only
					var data = CreateTemporarySensorData();
					data.VarsBool["PlayerVisible"] = true;
					data.VarsUnityObject["PlayerTransform"] = playerTransform; playerTransform.position = point;
					data.VarsFloat["PlayerDist"] = dist;
					data.VarsFloat["PlayerNormalizedNearness"] = 1f - dist / minDist;
					// memory ("always")
					agent.Memory.SetVector3("PlayerLastPosition", point, secondsRememberPlayer);
				}
			}
		}

#if UNITY_EDITOR
		void OnDrawGizmos() {
			Gizmos.DrawWireSphere(transform.position, minDist);
		}
#endif
	}

}