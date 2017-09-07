using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

namespace KSM.Tweening {
	/// <summary>
	/// TEMPORARY extension methods on DOTween
	/// </summary>
	public static class KTweenExtension {
		public static Tweener DOMoveUp(this Transform trans, float speed, bool snapping = false, float displacement = 1000f) {
			Vector3 position = trans.position;
			Vector3 up = trans.up;
			float time = displacement / speed;

			Vector3 endposition = position + new Vector3 (up.x * displacement, up.y * displacement, up.z * displacement);
			return trans.DOMove(endposition, time, snapping).OnPause(() => trans.DOKill());
		}

		public static Tweener DORotateForever(this Transform trans, float speed) {
			if (speed == 0)
				return null;

			float dummy_dist = 1f;
			float dummy_time = dummy_dist / speed;

			if (dummy_time > 0) {
				return trans.DORotate (new Vector3 (0f, 0f, 360f), dummy_time, RotateMode.WorldAxisAdd).SetEase (Ease.Linear).SetLoops (-1).OnPause(() => trans.DOKill());
			} else {
				return trans.DORotate (new Vector3 (0f, 0f, -360f), Mathf.Abs(dummy_time), RotateMode.WorldAxisAdd).SetEase (Ease.Linear).SetLoops (-1).OnPause(() => trans.DOKill());
			}
		}

		public static LTDescr LeanMoveUp(this Transform trans, float speed, bool snapping = false, float displacement = 1000f) {
			Vector3 position = trans.position;
			Vector3 up = trans.up;
			float time = displacement / speed;

			Vector3 endposition = position + new Vector3 (up.x * displacement, up.y * displacement, up.z * displacement);

			return LeanTween.move (trans.gameObject, endposition, time);
		}
	}
}
