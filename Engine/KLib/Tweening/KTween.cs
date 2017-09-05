using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

namespace KSM.Tweening {

	/// <summary>
	/// Tweened float. Useful for time interval changes (e.g. for yield return new WaitForSeconds)
	/// </summary>
	public class KTweenedFloat {
		public KTweenedFloat(float start, float end, float time) {
			this.startValue = start;
			this.value = start;
			this.endValue = end;
			this.interval = time;
		}

		public float value;

		public float startValue;
		public float endValue;
		public float interval;

		public Ease easeType;
		private bool _isEaseSet = false;

		public float GetValue() {
			return this.value;
		}

		public KTweenedFloat SetEase(Ease easeType) {
			this.easeType = easeType;
			_isEaseSet = true;
			return this;
		}

		public KTweenedFloat Run() {
			if (!_isEaseSet) {
				easeType = DOTween.defaultEaseType;
			}

			DOTween.To (() => this.value, x => this.value = x, this.endValue, this.interval).SetEase(this.easeType);
			return this;
		}
	}

}
