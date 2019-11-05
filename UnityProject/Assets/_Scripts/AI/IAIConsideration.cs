using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace RatKing.AI {

	public interface IAIConsideration {
		void SetAgent(AIAgent agent);
		bool Enabled { get; }
		float GetScore();
	}

}