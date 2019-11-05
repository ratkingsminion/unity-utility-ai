using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace RatKing.AI {

	public class AIConsideration : MonoBehaviour, IAIConsideration {
		[SerializeField] bool startEnabled = true;
#if UNITY_EDITOR
		[TextArea] public string comment = "";
#endif
		protected AIAgent agent;

		//

		/// <summary>
		/// Called by AIAction
		/// </summary>
		/// <param name="agent">The agent that considers this Consideration</param>
		public virtual void SetAgent(AIAgent agent) {
			this.agent = agent;
			enabled = startEnabled;
		}

		public bool Enabled { get => enabled; }

		public virtual float GetScore() {
			throw new System.NotImplementedException();
		}
	}

}