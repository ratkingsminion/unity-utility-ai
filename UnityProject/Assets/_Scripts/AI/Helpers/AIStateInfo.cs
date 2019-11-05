namespace RatKing.AI {

	public struct AIStateInfo<TState> where TState : struct, System.IConvertible, System.IComparable {
		public TState state;
		//public Transform target;
		public object userData;
		public System.Action onStart;
		public System.Action onUpdate;
		public System.Action onExit;
		//
		public AIStateInfo(TState state = default(TState)) { this.state = state; userData = null; onStart = onUpdate = onExit = null; }
		public AIStateInfo(TState state, object userData, System.Action onStart, System.Action onUpdate, System.Action onExit) {
			this.state = state; this.userData = userData; this.onStart = onStart; this.onUpdate = onUpdate; this.onExit = onExit;
		}
		//
		public AIStateInfo<TState> SetState(TState state) { this.state = state; return this; }
		public AIStateInfo<TState> SetUserData(object userData) { this.userData = userData; return this; }
		public AIStateInfo<TState> SetOnStart(System.Action onStart) { this.onStart = onStart; return this; }
		public AIStateInfo<TState> SetOnUpdate(System.Action onUpdate) { this.onUpdate = onUpdate; return this; }
		public AIStateInfo<TState> SetOnExit(System.Action onExit) { this.onExit = onExit; return this; }
	}

}