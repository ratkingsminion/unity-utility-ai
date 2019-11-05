﻿#if UNITY_2_6 || UNITY_2_6_1 || UNITY_3_0 || UNITY_3_0_0 || UNITY_3_1 || UNITY_3_2 || UNITY_3_3 || UNITY_3_4 || UNITY_3_5 || UNITY_4_0 || UNITY_4_0_1 || UNITY_4_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_5 || UNITY_4_6 || UNITY_4_7
#define UNITY_OLD
#else
#define UNITY_5
#endif

#if UNITY_5 && !UNITY_5_0 && !UNITY_5_1 && !UNITY_5_2
#define PARTICLE_SYSTEM_UPDATE_5_3
#endif

using UnityEngine;
using System.Collections;

namespace RatKing.Base {

	public class Particler : MonoBehaviour {
		[Tooltip("Can be null, will get the first ParticleSystem component.")]
		[SerializeField] ParticleSystem particles = null;
		[Tooltip("Use Follow() to change during gameplay.")]
		[SerializeField] Transform follow = null;
		[SerializeField] bool followWithoutRotation = false;
		//
		Coroutine following;
		Vector3 followOffset;
		Quaternion startRotDelta;

		//

		public Particler Instantiate(Transform start, bool follow) {
			var go = (GameObject)Instantiate(gameObject, start.position, start.rotation);
			var bp = go.GetComponent<Particler>();
			if (follow) { bp.Follow(start, followWithoutRotation); }
			return bp;
		}

		public Particler Instantiate(Vector3 position, Quaternion rotation) {
			var go = (GameObject)Instantiate(gameObject, position, rotation);
			return go.GetComponent<Particler>();
		}

		public Particler Instantiate(Vector3 position) {
			var go = (GameObject)Instantiate(gameObject, position, Quaternion.identity);
			return go.GetComponent<Particler>();
		}

		public Particler Instantiate() {
			var go = (GameObject)Instantiate(gameObject);
			return go.GetComponent<Particler>();
		}

		//

		public void Follow(Transform follow, bool followWithoutRotation) {
			this.followWithoutRotation = followWithoutRotation;
			if (this.follow == null) {
				StopAllCoroutines();
			}
			else {
				StopCoroutine(following);
			}
			this.follow = follow;
			if (follow != null) {
				followOffset = follow.InverseTransformPoint(transform.position);
				startRotDelta = Quaternion.Inverse(follow.rotation) * transform.rotation;
				following = StartCoroutine(FollowCR());
			}
		}

		public void StopParticles() {
            if (particles != null) {
#if PARTICLE_SYSTEM_UPDATE_5_3
				var pe = particles.emission;
                pe.enabled = false;
#else
                particles.enableEmission = false;
#endif
            }
        }

		//

		IEnumerator Start() {
			if (particles == null) {
				particles = GetComponent<ParticleSystem>();
			}
			if (follow != null) {
				StartCoroutine(FollowCR());
			}

#if !UNITY_5_5_OR_NEWER
			yield return new WaitForSeconds(particles.startDelay);
#endif
			var wait = new WaitForSeconds(0.25f);

			for (;;) {
				yield return wait;
				if (!particles.IsAlive() || particles.particleCount == 0) {
					Destroy(gameObject);
				}
			}
		}

		IEnumerator FollowCR() {
			while (follow != null) {
				transform.position = follow.TransformPoint(followOffset);
				if (!followWithoutRotation) { transform.rotation = follow.rotation * startRotDelta; }

				yield return null;
			}
		}
	}

}