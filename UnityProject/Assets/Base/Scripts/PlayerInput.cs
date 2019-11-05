using UnityEngine;
using System.Collections;

namespace RatKing.Base {

	public class PlayerInput : MonoBehaviour {
		[Header("Rotating")]
		public float yawRotSpeed = 10f;
		public float pitchRotSpeed = 10f;
		public float pitchMax = 80f;
		public float pitchMin = -80f;
		public float mouseSensitivity = 1f;
		public float smoothness = 1f;
		public bool needRightPressed;
		// public float interactDistance = 1f;
		[Header("Components")]
		[SerializeField] Transform camTransform = null;
		[SerializeField] Transform rotateTransform = null;
		[SerializeField] Creature creature = null;
		//
		float macMouseFactor = 1f;
		float pitch = 0f;
		//
		public Transform SmoothCam { get; private set; }


		//

		void Start() {
			rotateTransform = rotateTransform == null ? transform : rotateTransform;
			creature = creature == null ? GetComponentInChildren<Creature>() : creature;
#if UNITY_WEBPLAYER
			if (Application.platform == RuntimePlatform.OSXWebPlayer) {
#elif UNITY_2017_1_OR_NEWER
			if (Application.platform == RuntimePlatform.OSXPlayer)
#else
			if (Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.OSXDashboardPlayer)
#endif
				macMouseFactor = 0.18f;
			//
			if (camTransform != null) {
				SmoothCam = new GameObject("CAMDUMMY").transform;
				SmoothCam.position = camTransform.position; // + Vector3.up;
				SmoothCam.rotation = camTransform.rotation;
				SmoothCam.parent = camTransform.parent;
				camTransform.parent = null;
			}
		}

		void Update() {
			// move

			creature.FactorX = Input.GetAxis("Horizontal");
			creature.FactorZ = Input.GetAxis("Vertical");

			// jump

			if (Input.GetButtonDown("Jump"))
				creature.Jump();

			// look

			if ((Application.isEditor || needRightPressed) && Input.GetMouseButton(1)) {
#if UNITY_5_0 || UNITY_5_1 || UNITY_5_2 || UNITY_5_3 || UNITY_5_3_OR_NEWER
				if (Cursor.visible) {
					Cursor.visible = false;
					Cursor.lockState = CursorLockMode.Locked;
				}
#else
				Screen.lockCursor = true;
#endif
				rotateTransform.Rotate(rotateTransform.up, Input.GetAxis("Mouse X") * yawRotSpeed * macMouseFactor * mouseSensitivity, Space.World);

				pitch += Input.GetAxis("Mouse Y") * -pitchRotSpeed * macMouseFactor * mouseSensitivity * 1.1f;
				pitch = Mathf.Clamp(pitch, pitchMin, pitchMax);
				if (SmoothCam != null)
					SmoothCam.localEulerAngles = new Vector3(pitch, 0f, 0f);
			}
			else {
#if UNITY_5_0 || UNITY_5_1 || UNITY_5_2 || UNITY_5_3 || UNITY_5_3_OR_NEWER
				if (!Cursor.visible) {
					Cursor.visible = true;
					Cursor.lockState = CursorLockMode.None;
				}
#else
				Screen.lockCursor = false;
#endif
			}

			if (camTransform != null) {
				camTransform.position = SmoothCam.position;
				camTransform.rotation = Quaternion.Slerp(camTransform.rotation, SmoothCam.rotation, smoothness);
			}
		}

		void OnDestroy() {
#if UNITY_5_0 || UNITY_5_1 || UNITY_5_2 || UNITY_5_3 || UNITY_5_3_OR_NEWER
			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.None;
#else
			Screen.lockCursor = false;
#endif
		}
	}

}