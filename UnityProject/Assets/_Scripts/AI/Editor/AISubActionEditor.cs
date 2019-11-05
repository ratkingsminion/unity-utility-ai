using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace RatKing.AI {
	
	[CustomEditor(typeof(AISubAction), true)]
	[CanEditMultipleObjects]
	public class AISubActionEditor : Editor {
		static GUIStyle style = null;
		static Texture2D styleTex = null;

		static GUIStyle GetStyle() {
			if (style == null) { style = new GUIStyle("box"); }
			if (styleTex != null) { return style; }
			styleTex = new Texture2D(1, 1, TextureFormat.ARGB32, true);
			styleTex.SetPixel(0, 0, Color.yellow);
			styleTex.Apply();
			style.normal.background =
			style.hover.background =
			style.active.background = styleTex;
			var bor = style.border; bor.bottom = bor.top = 1; style.border = bor;
			return style;
		}

		public override void OnInspectorGUI() {
			if (!serializedObject.isEditingMultipleObjects) {
				var asa = (AISubAction)serializedObject.targetObject;
				var cs = new List<AISubAction>(asa.GetComponents<AISubAction>());
				if (cs.IndexOf(asa) == 0) { // is the first one? -> add a green bar
					GUI.Box(new Rect(2, -18, Screen.width - 4, 0), GUIContent.none, GetStyle());
				}
			}

			base.OnInspectorGUI();
		}
	}

}