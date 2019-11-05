using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RatKing.AI {
	
	public struct MemoryFragment<T> {
		public T value;
		public float forgetTime;
		public MemoryFragment(T value) {
			this.value = value;
			forgetTime = -1f;
		}
		public MemoryFragment(T value, float rememberSeconds) {
			this.value = value;
			forgetTime = Time.time + rememberSeconds;
		}
	}
	
	public class MemoryArea<T> {
		public List<string> ids = new List<string>();
		public List<MemoryFragment<T>> fragments = new List<MemoryFragment<T>>();

		public void UpdateFragments() {
			for (int i = ids.Count - 1; i >= 0; --i) {
				var ft = fragments[i].forgetTime;
				if (ft >= 0f && ft < Time.time) {
					ids.RemoveAt(i);
					fragments.RemoveAt(i);
				}
			}
		}

		public void Set(string id, T value) {
			var i = ids.IndexOf(id);
			if (i >= 0) {
				fragments[i] = new MemoryFragment<T>(value);
			}
			else {
				ids.Add(id);
				fragments.Add(new MemoryFragment<T>(value));
			}
		}

		public void Set(string id, T value, float rememberSeconds) {
			var i = ids.IndexOf(id);
			if (i >= 0) {
				fragments[i] = new MemoryFragment<T>(value, rememberSeconds);
			}
			else {
				ids.Add(id);
				fragments.Add(new MemoryFragment<T>(value, rememberSeconds));
			}
		}
		
		public T Get(string id, T stdValue = default) {
			var i = ids.IndexOf(id);
			if (i >= 0) { return fragments[i].value; }
			return stdValue;
		}

		public void Remove(string id) {
			var i = ids.IndexOf(id);
			if (i >= 0) {
				ids.RemoveAt(i);
				fragments.RemoveAt(i);
			}
		}
	}

}