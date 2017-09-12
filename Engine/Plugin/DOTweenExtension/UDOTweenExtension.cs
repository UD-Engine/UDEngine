using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

namespace UDEngine.Plugin.DOTweenExtension {
	public static class UDOTweenExtension {
		public static Tweener DOMoveUp(this Transform trans, float speed, bool snapping = false, float displacement = 1000f) {
			Vector3 position = trans.position;
			Vector3 up = trans.up;
			float time = displacement / speed;

			Vector3 endposition = position + new Vector3 (up.x * displacement, up.y * displacement, up.z * displacement);
			return trans.DOMove(endposition, time, snapping).OnPause(() => trans.DOKill());
		}
	}
}