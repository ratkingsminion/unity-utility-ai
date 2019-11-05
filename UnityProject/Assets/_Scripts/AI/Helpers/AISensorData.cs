using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace RatKing.AI {

	public class AISensorData {
		static Stack<AISensorData> pool = new Stack<AISensorData>(256);
		public static AISensorData Create() {
			AISensorData data;
			if (pool.Count == 0) { data = new AISensorData(); }
			else { data = pool.Pop(); }
			return data;
		}
		public static void Remove(AISensorData data) {
			if (data == null) { return; }
			data.VarsBool.Clear();
			data.VarsFloat.Clear();
			data.VarsInt.Clear();
			data.VarsUnityObject.Clear();
			data.VarsVector3.Clear();
			// TODO
			pool.Push(data);
		}

		//
		
		public Dictionary<string, bool> VarsBool { get; private set; }
		public Dictionary<string, float> VarsFloat { get; private set; }
		public Dictionary<string, int> VarsInt { get; private set; }
		public Dictionary<string, string> VarsString { get; private set; }
		public Dictionary<string, Object> VarsUnityObject { get; private set; }
		public Dictionary<string, Vector3> VarsVector3 { get; private set; }

		//

		protected AISensorData() {
			VarsBool = new Dictionary<string, bool>();
			VarsFloat = new Dictionary<string, float>();
			VarsInt = new Dictionary<string, int>();
			VarsString = new Dictionary<string, string>();
			VarsUnityObject = new Dictionary<string, Object>();
			VarsVector3 = new Dictionary<string, Vector3>();
		}

		public void SetVarBool(string varName, bool value = false) { VarsBool[varName] = value; }
		public bool TryGetVarBool(string varName, bool standardValue = false) { bool val; if (VarsBool.TryGetValue(varName, out val)) { return val; } return standardValue; }
		
		public void SetVarFloat(string varName, float value = 0f) { VarsFloat[varName] = value; }
		public float TryGetVarFloat(string varName, float standardValue = 0f) { float val; if (VarsFloat.TryGetValue(varName, out val)) { return val; } return standardValue; }
		
		public void SetVarInt(string varName, int value = 0) { VarsInt[varName] = value; }
		public int TryGetVarInt(string varName, int standardValue = 0) { int val; if (VarsInt.TryGetValue(varName, out val)) { return val; } return standardValue; }
		
		public void SetVarString(string varName, string value = "") { VarsString[varName] = value; }
		public string TryGetVarString(string varName, string standardValue = "") { string val; if (VarsString.TryGetValue(varName, out val)) { return val; } return standardValue; }

		public void SetVarUnityObject(string varName, Object value = null) { VarsUnityObject[varName] = value; }
		public Object TryGetVarUnityObject(string varName, Object standardValue = null) { Object val; if (VarsUnityObject.TryGetValue(varName, out val)) { return val; } return standardValue; }
		
		public void SetVector3(string varName, Vector3 value = default(Vector3)) { VarsVector3[varName] = value; }
		public Vector3 TryGetVector3(string varName, Vector3 standardValue = default(Vector3)) { Vector3 val; if (VarsVector3.TryGetValue(varName, out val)) { return val; } return standardValue; }

		public void SetVar<T>(string varName, T value) {
			if (typeof(T) == typeof(bool)) { VarsBool[varName] = (bool)(object)value; }
			else if (typeof(T) == typeof(float)) { VarsFloat[varName] = (float)(object)value; }
			else if (typeof(T) == typeof(int)) { VarsInt[varName] = (int)(object)value; }
			else if (typeof(T) == typeof(string)) { VarsString[varName] = (string)(object)value; }
			else if (typeof(T).IsSubclassOf(typeof(Object)) || typeof(T) == typeof(Object)) { VarsUnityObject[varName] = (Object)(object)value; }
			else if (typeof(T) == typeof(Vector3)) { VarsVector3[varName] = (Vector3)(object)value; }
#if UNITY_EDITOR
			else { Debug.LogWarning("Missing variable container for type " + typeof(T)); }
#endif
		}
		
//		public T GetVar<T>(string varName) {
//			if (typeof(T) == typeof(bool)) { return (T)(object)VarsBool[varName]; }
//			else if (typeof(T) == typeof(float)) { return (T)(object)VarsFloat[varName]; }
//			else if (typeof(T) == typeof(int)) { return (T)(object)VarsInt[varName]; }
//			else if (typeof(T) == typeof(string)) { return (T)(object)VarsString[varName]; }
//			else if (typeof(T).IsSubclassOf(typeof(Object)) || typeof(T) == typeof(Object)) { return (T)(object)VarsUnityObject[varName]; }
//			else if (typeof(T) == typeof(Vector3)) { return (T)(object)VarsVector3[varName]; }
//#if UNITY_EDITOR
//			else { Debug.LogWarning("Missing variable container for type " + typeof(T)); }
//#endif
//			return default(T);
//		}
		
		public T TryGetVar<T>(string varName, T standardValue = default(T)) {
			if (typeof(T) == typeof(bool)) { bool val; if (VarsBool.TryGetValue(varName, out val)) { return (T)(object)val; } }
			else if (typeof(T) == typeof(float)) { float val; if (VarsFloat.TryGetValue(varName, out val)) { return (T)(object)val; } }
			else if (typeof(T) == typeof(int)) { int val; if (VarsInt.TryGetValue(varName, out val)) { return (T)(object)val; } }
			else if (typeof(T) == typeof(string)) { string val; if (VarsString.TryGetValue(varName, out val)) { return (T)(object)val; } }
			else if (typeof(T) == typeof(Object)) { Object val; if (VarsUnityObject.TryGetValue(varName, out val)) { return (T)(object)val; } }
			else if (typeof(T) == typeof(Vector3)) { Vector3 val; if (VarsVector3.TryGetValue(varName, out val)) { return (T)(object)val; } }
#if UNITY_EDITOR
			else { Debug.LogWarning("Missing variable container for type " + typeof(T)); }
#endif
			return standardValue;
		}
	}

}