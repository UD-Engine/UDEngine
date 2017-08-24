using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using DG.Tweening;

using UDEngine;
using UDEngine.Internal;
using UDEngine.Components;

namespace UDEngine.Components.Action {
	// This class handles all the sequential actions that will be executed -- unless explicitly paused by some others 
	// (so that the collision actions or clearup actions could be run)
	public class UBulletAction : MonoBehaviour {

		public UBulletAction(Transform trans = null) {
			if (trans == null) {
				if (transform.parent != null) {
					this.trans = transform.parent;
				}
			} else {
				this.trans = trans;
			}
		}

		// PROP begin
		public Transform trans;

		private Sequence _actionSequence = null;
		// PROP end

		// METHOD begin
		public Sequence GetActionSequence() {
			if (_actionSequence == null) {
				_actionSequence = DOTween.Sequence ();
			}
			return _actionSequence;
		}
		// METHOD end
	}
}
