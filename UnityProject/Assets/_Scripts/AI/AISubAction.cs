using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RatKing.AI {

	public abstract class AISubAction : MonoBehaviour {
		
		[System.Flags]
		public enum OccurenceType { OnActivate = 1, OnDeactivate = 2, OnExecute = 4 };

		[SerializeField, EnumFlag] OccurenceType occurence = OccurenceType.OnActivate;
		public OccurenceType Occurence { get { return occurence; } }
		protected AIAgent agent;

		//

		/// <summary>
		/// Called by AIAction
		/// </summary>
		/// <param name="agent">The agent that does this subaction</param>
		public void SetAgent(AIAgent agent) {
			this.agent = agent;
		}

		//

		public abstract void DoSubAction();
	}

}