using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace RatKing.AI {

	[System.Flags]
	public enum VariableFlag {
		Bool = 1,
		Float = 2,
		Int = 4,
		String = 8,
		UnityObject = 16,
		Vector3 = 32,
	}

	public class AIMemory : MonoBehaviour {
		MemoryArea<bool> varsBool = new MemoryArea<bool>();
		MemoryArea<float> varsFloat = new MemoryArea<float>();
		MemoryArea<int> varsInt = new MemoryArea<int>();
		MemoryArea<string> varsString = new MemoryArea<string>();
		MemoryArea<Object> varsUnityObject = new MemoryArea<Object>();
		MemoryArea<Vector3> varsVector3 = new MemoryArea<Vector3>();
#if UNITY_EDITOR
		public MemoryArea<bool> VarsBool { get { return varsBool; } }
		public MemoryArea<float> VarsFloat { get { return varsFloat; } }
		public MemoryArea<int> VarsInt { get { return varsInt; } }
		public MemoryArea<string> VarsString { get { return varsString; } }
		public MemoryArea<Object> VarsUnityObject { get { return varsUnityObject; } }
		public MemoryArea<Vector3> VarsVector3 { get { return varsVector3; } }
#endif
		//
		int lastUpdateFrame = -1;


		//

		public AIMemory() {
		}

		public void UpdateVariables() {
			if (Time.frameCount == lastUpdateFrame) { return; }
			lastUpdateFrame = Time.frameCount;
			//
			varsBool.UpdateFragments();
			varsFloat.UpdateFragments();
			varsInt.UpdateFragments();
			varsString.UpdateFragments();
			varsUnityObject.UpdateFragments();
			varsVector3.UpdateFragments();
		}
		
		public void SetBool(string varName, bool value) { varsBool.Set(varName, value); }
		public void SetBool(string varName, bool value, float rememberSeconds) { varsBool.Set(varName, value, rememberSeconds); }
		public bool GetBool(string varName, bool stdValue = false) { return varsBool.Get(varName, stdValue); }
		public bool HasBool(string varName) { return varsBool.ids.Contains(varName); }
		public void RemoveBool(string varName) { varsBool.Remove(varName); }

		public void SetFloat(string varName, float value) { varsFloat.Set(varName, value); }
		public void SetFloat(string varName, float value, float rememberSeconds) { varsFloat.Set(varName, value, rememberSeconds); }
		public float GetFloat(string varName, float stdValue = 0f) { return varsFloat.Get(varName, stdValue); }
		public bool HasFloat(string varName) { return varsFloat.ids.Contains(varName); }
		public void RemoveFloat(string varName) { varsFloat.Remove(varName); }

		public void SetInt(string varName, int value) { varsInt.Set(varName, value); }
		public void SetInt(string varName, int value, float rememberSeconds) { varsInt.Set(varName, value, rememberSeconds); }
		public int GetInt(string varName, int stdValue = 0) { return varsInt.Get(varName, stdValue); }
		public bool HasInt(string varName) { return varsInt.ids.Contains(varName); }
		public void RemoveInt(string varName) { varsInt.Remove(varName); }

		public void SetString(string varName, string value) { varsString.Set(varName, value); }
		public void SetString(string varName, string value, float rememberSeconds) { varsString.Set(varName, value, rememberSeconds); }
		public string GetString(string varName, string stdValue = "") { return varsString.Get(varName, stdValue); }
		public bool HasString(string varName) { return varsString.ids.Contains(varName); }
		public void RemoveString(string varName) { varsString.Remove(varName); }

		public void SetUnityObject(string varName, Object value) { varsUnityObject.Set(varName, value); }
		public void SetUnityObject(string varName, Object value, float rememberSeconds) { varsUnityObject.Set(varName, value, rememberSeconds); }
		public Object GetUnityObject(string varName, Object stdValue = null) { return varsUnityObject.Get(varName, stdValue); }
		public bool HasUnityObject(string varName) { return varsUnityObject.ids.Contains(varName); }
		public void RemoveUnityObject(string varName) { varsUnityObject.Remove(varName); }

		public void SetVector3(string varName, Vector3 value) { varsVector3.Set(varName, value); }
		public void SetVector3(string varName, Vector3 value, float rememberSeconds) { varsVector3.Set(varName, value, rememberSeconds); }
		public Vector3 GetVector3(string varName, Vector3 stdValue = default) { return varsVector3.Get(varName, stdValue); }
		public bool HasVector3(string varName) { return varsVector3.ids.Contains(varName); }
		public void RemoveVector3(string varName) { varsVector3.Remove(varName); }
	}

}