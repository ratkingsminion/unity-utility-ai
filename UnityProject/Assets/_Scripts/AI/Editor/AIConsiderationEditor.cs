using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace RatKing.AI {
	
	[CustomEditor(typeof(AIConsideration), true)]
	[CanEditMultipleObjects]
	public class AIConsiderationEditor : Editor {
		static GUIStyle style = null;
		static Texture2D styleTex = null;

		static GUIStyle GetStyle() {
			if (style == null) { style = new GUIStyle("box"); }
			if (styleTex != null) { return style; }
			styleTex = new Texture2D(1, 1, TextureFormat.ARGB32, true);
			styleTex.SetPixel(0, 0, Color.green);
			styleTex.Apply();
			style.normal.background =
			style.hover.background =
			style.active.background = styleTex;
			var bor = style.border; bor.bottom = bor.top = 2; style.border = bor;
			return style;
		}

		public override void OnInspectorGUI() {
			if (!serializedObject.isEditingMultipleObjects) {
				var ac = (AIConsideration)serializedObject.targetObject;
				var cs = new List<AIConsideration>(ac.GetComponents<AIConsideration>());
				if (cs.IndexOf(ac) == 0 && ac.GetComponents<MonoBehaviour>().Length > cs.Count) { // is the first one? -> add a green bar
					GUI.Box(new Rect(2, -18, Screen.width - 4, 0), GUIContent.none, GetStyle());
				}
			}

			base.OnInspectorGUI();
		}
	}

}