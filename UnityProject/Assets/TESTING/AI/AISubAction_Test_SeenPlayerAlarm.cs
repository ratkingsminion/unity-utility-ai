using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RatKing.AI;

namespace RatKing {

	public class AISubAction_Test_SeenPlayerAlarm : AISubAction {
		public override void DoSubAction() {
			Base.Events.BroadcastAll(AIEnemy_Test.EVENT_PLAYER_SEEN, agent.Memory.GetVector3("PlayerLastPosition"));
		}
	}

}