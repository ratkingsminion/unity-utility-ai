using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace RatKing.AI {
	
	[CustomEditor(typeof(AIAgent))]
	[CanEditMultipleObjects]
	public class AIAgentEditor : Editor {
		SerializedProperty debugMode;
		SerializedProperty senseAndDecideTime;
		SerializedProperty additionalMemories;

		//

		void OnEnable() {
			debugMode = serializedObject.FindProperty("debugMode");
			senseAndDecideTime = serializedObject.FindProperty("senseAndDecideTime");
			additionalMemories = serializedObject.FindProperty("additionalMemories");
		}

		void DrawMemoryVars<T>(string type, MemoryArea<T> ma) {
			var c = ma.ids.Count;
			GUILayout.Label(type + "s: " + c);
			for (int i = 0; i < c; ++i) {
				var id = ma.ids[i];
				var fr = ma.fragments[i];
				GUILayout.BeginHorizontal();
				GUILayout.Label("> " + id, GUILayout.MinWidth(130));
				if (fr.value is Object) { GUILayout.Label((fr.value as Object).name, GUILayout.MinWidth(120)); }
				else { GUILayout.Label(fr.value.ToString(), GUILayout.MinWidth(120)); }
				GUILayout.Label("(" + fr.forgetTime + ")", GUILayout.MinWidth(120));
				GUILayout.EndHorizontal();
			}
			GUILayout.Space(2);
		}

		void DrawSensorVars<T>(string type, Dictionary<string, T> sd) {
			GUILayout.Label(type + "s: " + sd.Count);
			foreach (var d in sd) {
				GUILayout.BeginHorizontal();
				GUILayout.Label("> " + d.Key, GUILayout.MinWidth(180));
				if (d.Value is Object) { GUILayout.Label((d.Value as Object).name, GUILayout.MinWidth(150)); }
				else { GUILayout.Label(d.Value.ToString(), GUILayout.MinWidth(150)); }
				GUILayout.EndHorizontal();
			}
			GUILayout.Space(2);
		}

		public override void OnInspectorGUI() {
			EditorGUILayout.PropertyField(debugMode, new GUIContent("Debug Mode"));
			EditorGUILayout.PropertyField(senseAndDecideTime, new GUIContent("Sense & Decide Time"));
			EditorGUILayout.PropertyField(additionalMemories, new GUIContent("Additional Memories"), true);

			if (!serializedObject.isEditingMultipleObjects) {
				//GUI.changed = true;
				var aa = (AIAgent)serializedObject.targetObject;

				// MEMORY
				if (aa != null && aa.Memory != null) {
					GUILayout.Space(4);
					for (int i = 0; i < aa.Memory.All.Count; ++i) {
						GUILayout.Space(4);
						EditorGUILayout.Separator();
						GUILayout.Label("MEMORY (" + aa.Memory[i].name + ")");
						if (aa.Memory[i].VarsBool.ids.Count > 0) { DrawMemoryVars("Bool", aa.Memory[i].VarsBool); }
						if (aa.Memory[i].VarsFloat.ids.Count > 0) { DrawMemoryVars("Float", aa.Memory[i].VarsFloat); }
						if (aa.Memory[i].VarsInt.ids.Count > 0) { DrawMemoryVars("Int", aa.Memory[i].VarsInt); }
						if (aa.Memory[i].VarsString.ids.Count > 0) { DrawMemoryVars("String", aa.Memory[i].VarsString); }
						if (aa.Memory[i].VarsUnityObject.ids.Count > 0) { DrawMemoryVars("Unity Object", aa.Memory[i].VarsUnityObject); }
						if (aa.Memory[i].VarsVector3.ids.Count > 0) { DrawMemoryVars("Vector3", aa.Memory[i].VarsVector3); }
					}
				}

				// SENSORS
				if (aa != null && aa.SensorCount > 0) {
					for (int i = 0; i < aa.SensorCount; ++i) {
						GUILayout.Space(4);
						EditorGUILayout.Separator();
						var s = aa.GetSensor(i);
						var splitType = s.GetType().ToString().Split('.');
						GUILayout.Label("SENSOR " + splitType[splitType.Length - 1]);
						for (int j = 0, c = s.Data.Count; j < c; ++j) {
							DrawSensorVars("Bool", s.Data[i].VarsBool);
							DrawSensorVars("Float", s.Data[i].VarsFloat);
							DrawSensorVars("Int", s.Data[i].VarsInt);
							DrawSensorVars("String", s.Data[i].VarsString);
							DrawSensorVars("UnityObject", s.Data[i].VarsUnityObject);
							DrawSensorVars("Vector3", s.Data[i].VarsVector3);
						}
					}
				}

				//if (GUI.changed) { EditorUtility.SetDirty(aa); }
			}
		}
	}

}