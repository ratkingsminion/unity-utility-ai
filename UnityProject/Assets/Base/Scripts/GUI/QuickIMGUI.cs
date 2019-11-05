﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace RatKing.Base {

	public class QuickIMGUI : MonoBehaviour {

		static QuickIMGUI inst;
		static event System.Action OnDraw;
		static event System.Action OnDrawOnce;
		static Dictionary<string, System.Action> byIDs;
		static int onDrawOnceCounter;
		//
		public static Texture2D texWhite;
		public static Texture2D texWhiteTransparent;

		//

		public static void CreateInstance() {
			if (inst != null) { return; }
			var go = new GameObject("<Quick IM GUI>");
			DontDestroyOnLoad(go);
			inst = go.AddComponent<QuickIMGUI>();
			//
			byIDs = new Dictionary<string, System.Action>();
			//
			texWhite = new Texture2D(1, 1); texWhite.SetPixel(0, 0, Color.white);
			texWhite.Apply();
			texWhiteTransparent = new Texture2D(1, 1); texWhiteTransparent.SetPixel(0, 0, new Color(1f, 1f, 1f, 0.5f));
			texWhiteTransparent.Apply();
		}

		public static void DrawOnce(System.Action onDraw) {
			if (onDraw == null) { return; }
			CreateInstance();
			if (OnDrawOnce == null && OnDraw == null) { inst.gameObject.SetActive(true); }
			OnDrawOnce += onDraw;
			onDrawOnceCounter = 2;
		}

		public static void Add(System.Action onDraw) {
			if (onDraw == null) { return; }
			CreateInstance();
			if (OnDrawOnce == null && OnDraw == null) { inst.gameObject.SetActive(true); }
			OnDraw += onDraw;
		}

		public static void Remove(System.Action onDraw) {
			if (inst == null || onDraw == null) { return; }
			OnDraw -= onDraw;
			if (OnDrawOnce == null && OnDraw == null) { inst.gameObject.SetActive(false); }
		}

		public static void Add(string ID, System.Action onDraw) {
			if (string.IsNullOrEmpty(ID) || onDraw == null) { return; }
			CreateInstance();
			if (byIDs.ContainsKey(ID)) { Debug.LogError("Trying to add ID twice"); return; }
			if (OnDrawOnce == null && OnDraw == null) { inst.gameObject.SetActive(true); }
			OnDraw += onDraw;
			byIDs[ID] = onDraw;
		}

		public static void Remove(string ID) {
			if (inst == null || string.IsNullOrEmpty(ID)) { return; }
			System.Action onDraw;
			if (byIDs.TryGetValue(ID, out onDraw)) {
				OnDraw -= onDraw;
				byIDs.Remove(ID);
				if (OnDrawOnce == null && OnDraw == null) { inst.gameObject.SetActive(false); }
			}
		}

		//

		void OnGUI() {
			if (OnDraw != null) { OnDraw(); }

			if (OnDrawOnce != null) {
				OnDrawOnce();
				onDrawOnceCounter--;
				if (onDrawOnceCounter <= 0) {
					OnDrawOnce = null;
					if (OnDraw == null) { gameObject.SetActive(false); }
				}
			}

		}
	}

}