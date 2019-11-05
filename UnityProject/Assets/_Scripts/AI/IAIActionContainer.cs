namespace RatKing.AI {

	public interface IAIActionContainer {
		string Name { get; }
		float LastCalculatedScore { get; }
		float UpdateTime { get; }
		IAIActionContainer CurrentAction { get; }
		void Init(AIAgent agent);
		IAIActionContainer CalculateScore();
		void MultiplyLastCalculatedScore(float factor);
		IAIActionContainer Execute();
		void Activate();
		void Deactivate();
	}

}