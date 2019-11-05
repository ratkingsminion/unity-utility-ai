using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RatKing.AI {

	public class AIMemoriesSelector {
		public AIAgent Agent { get; private set; }
		public List<AIMemory> All { get; private set; }
		public AIMemory Main { get; private set; }
		public int Count { get { return count; } }
		//
		public AIMemory this[int key] {
			get { return All[key]; }
			// set { All[key] = value; }
		}
		//
		int count;

		//

		public AIMemoriesSelector(AIAgent agent, AIMemory mainMemory, AIMemory[] additionalMemories = null) {
			this.Agent = agent;
			if (mainMemory == null) { Debug.LogWarning("No Memory for " + agent.name); return; }
			All = new List<AIMemory>(1);
			this.Main = mainMemory;
			All.Add(mainMemory);
			if (additionalMemories != null) { All.AddRange(additionalMemories); }
			count = All.Count;
		}

		public void UpdateVariables() {
			for (int i = 0; i < count; ++i) {
				All[i].UpdateVariables();
			}
		}

		// SET and REMOVE only work on Main Memory!
		
		public void SetBool(string varName, bool value) { Main.SetBool(varName, value); }
		public void SetBool(string varName, bool value, float rememberSeconds) { Main.SetBool(varName, value, rememberSeconds); }
		public void RemoveBool(string varName) { Main.RemoveBool(varName); }
		
		public void SetFloat(string varName, float value) { Main.SetFloat(varName, value); }
		public void SetFloat(string varName, float value, float rememberSeconds) { Main.SetFloat(varName, value, rememberSeconds); }
		public void RemoveFloat(string varName) { Main.RemoveFloat(varName); }
		
		public void SetInt(string varName, int value) { Main.SetInt(varName, value); }
		public void SetInt(string varName, int value, float rememberSeconds) { Main.SetInt(varName, value, rememberSeconds); }
		public void RemoveInt(string varName) { Main.RemoveInt(varName); }
		
		public void SetString(string varName, string value) { Main.SetString(varName, value); }
		public void SetString(string varName, string value, float rememberSeconds) { Main.SetString(varName, value, rememberSeconds); }
		public void RemoveString(string varName) { Main.RemoveString(varName); }
		
		public void SetUnityObject(string varName, Object value) { Main.SetUnityObject(varName, value); }
		public void SetUnityObject(string varName, Object value, float rememberSeconds) { Main.SetUnityObject(varName, value, rememberSeconds); }
		public void RemoveUnityObject(string varName) { Main.RemoveUnityObject(varName); }
		
		public void SetVector3(string varName, Vector3 value) { Main.SetVector3(varName, value); }
		public void SetVector3(string varName, Vector3 value, float rememberSeconds) { Main.SetVector3(varName, value, rememberSeconds); }
		public void RemoveVector3(string varName) { Main.RemoveVector3(varName); }

		public bool GetBool(string varName, bool stdValue = false) {
			if (count == 1) { return Main.GetBool(varName, stdValue); }
			for (int i = 0; i < count; ++i) { if (All[i].HasBool(varName)) { return All[i].GetBool(varName); } }
			return stdValue;
		}
		public bool HasBool(string varName) {
			for (int i = 0; i < count; ++i) { if (All[i].HasBool(varName)) { return true; } }
			return false;
		}
		
		public float GetFloat(string varName, float stdValue = 0f) {
			if (count == 1) { return Main.GetFloat(varName, stdValue); }
			for (int i = 0; i < count; ++i) { if (All[i].HasFloat(varName)) { return All[i].GetFloat(varName); } }
			return stdValue;
		}
		public bool HasFloat(string varName) {
			for (int i = 0; i < count; ++i) { if (All[i].HasFloat(varName)) { return true; } }
			return false;
		}
		
		public int GetInt(string varName, int stdValue = 0) {
			if (count == 1) { return Main.GetInt(varName, stdValue); }
			for (int i = 0; i < count; ++i) { if (All[i].HasInt(varName)) { return All[i].GetInt(varName); } }
			return stdValue;
		}
		public bool HasInt(string varName) {
			for (int i = 0; i < count; ++i) { if (All[i].HasInt(varName)) { return true; } }
			return false;
		}
		
		public string GetString(string varName, string stdValue = "") {
			if (count == 1) { return Main.GetString(varName, stdValue); }
			for (int i = 0; i < count; ++i) { if (All[i].HasString(varName)) { return All[i].GetString(varName); } }
			return stdValue;
		}
		public bool HasString(string varName) {
			for (int i = 0; i < count; ++i) { if (All[i].HasString(varName)) { return true; } }
			return false;
		}
		
		public Object GetUnityObject(string varName, Object stdValue = null) {
			if (count == 1) { return Main.GetUnityObject(varName, stdValue); }
			for (int i = 0; i < count; ++i) { if (All[i].HasUnityObject(varName)) { return All[i].GetUnityObject(varName); } }
			return stdValue;
		}
		public bool HasUnityObject(string varName) {
			for (int i = 0; i < count; ++i) { if (All[i].HasUnityObject(varName)) { return true; } }
			return false;
		}
		
		public Vector3 GetVector3(string varName, Vector3 stdValue = default) {
			if (count == 1) { return Main.GetVector3(varName, stdValue); }
			for (int i = 0; i < count; ++i) { if (All[i].HasVector3(varName)) { return All[i].GetVector3(varName); } }
			return stdValue;
		}
		public bool HasVector3(string varName) {
			for (int i = 0; i < count; ++i) { if (All[i].HasVector3(varName)) { return true; } }
			return false;
		}
	}

}