using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RatKing.AI {

	public interface IAIStateController {
		GameObject Root { get; }
		AIAgent Agent { get; }
		string CurStateName { get; }
	}

}