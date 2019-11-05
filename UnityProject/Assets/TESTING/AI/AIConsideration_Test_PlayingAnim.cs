using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace RatKing.AI {

	public class AIConsideration_Test_PlayingAnim : AIConsideration {
		[SerializeField] Animation anim = null;
		[SerializeField] AnimationClip clip = null;
		[SerializeField] int maxLoops = -1;
		[SerializeField, Range(0.0f, 1.0f)] float scoreForTrue = 1f;
		[SerializeField, Range(0.0f, 1.0f)] float scoreForFalse = 0f;

		public override float GetScore() {
			if (anim == null || clip == null) { return scoreForFalse; }
			Debug.Log(anim.IsPlaying(clip.name) + " " + anim[clip.name].time + " " + clip.length);
			if (!anim.isPlaying || !anim.IsPlaying(clip.name)) { return scoreForFalse; }
			if (!clip.isLooping) { return anim[clip.name].time >= clip.length ? scoreForFalse : scoreForTrue; }
			if (maxLoops > 0) { return anim[clip.name].time >= clip.length * maxLoops ? scoreForFalse : scoreForTrue; }
			return scoreForTrue;
		}
	}

}