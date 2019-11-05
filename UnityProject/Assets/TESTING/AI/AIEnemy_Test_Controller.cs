using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RatKing.AI {

	public enum AIState_Test {
		None,
		Idle,
		Animate,
		WalkToTarget,
		//GetHurt,
		//Die,
		Dead
	}

	public class AIEnemy_Test_Controller : AIStateController<AIState_Test> {
	}

}