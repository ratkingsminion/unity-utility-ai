using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace RatKing.AI {

	public class AIConsideration_String : AIConsideration {
		[System.Serializable]
		public struct NameScorePair {
			public string name;
			[Range(0.0f, 1.0f)] public float score;
		}

		//

		[SerializeField] string stringName = "";
		[SerializeField] VariableSource source = VariableSource.SensorsOnly; 
		[SerializeField] NameScorePair[] values = null;
		[SerializeField, Range(0.0f, 1.0f)] float defaultScore = 0f;

		//

		public override float GetScore() {
			var data = agent.GetDataString(stringName, source);
			if (data != "") {
				for (int i = values.Length - 1; i >= 0; --i) { if (values[i].name == data) { return values[i].score; } }
			}
			return defaultScore;
		}
	}

}