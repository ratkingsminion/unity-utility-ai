using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace RatKing.AI {

	public class AISensor : MonoBehaviour {
		[SerializeField] protected float updateMinTime = 0.5f;
		//
		public List<AISensorData> Data { get; protected set; }
		//
		protected AIAgent agent;
		protected IAIStateController controller;
		//
		float updateTimer;

		//

		public void Init(AIAgent agent, IAIStateController controller) {
			this.agent = agent;
			this.controller = controller;
			Data = new List<AISensorData>();
		}

		public void UpdateByAgent(float deltaTime) {
			updateTimer += deltaTime;
			if (updateTimer >= updateMinTime) {
				updateTimer -= updateMinTime;
				for (int i = Data.Count - 1; i >= 0; --i) { AISensorData.Remove(Data[i]); }
				Data.Clear();
				UpdateSensorData();
			}
		}

		protected AISensorData CreateTemporarySensorData() {
			var data = AISensorData.Create();
			Data.Add(data);
			return data;
		}

		public virtual void UpdateSensorData() {
			throw new System.NotImplementedException();
		}
	}

}