using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

using UDEngine;
using UDEngine.Internal;
using UDEngine.Components;
using UDEngine.Components.Actor;
using UDEngine.Components.Bullet;
using UDEngine.Components.Collision;
using UDEngine.Interface;
using UDEngine.Enum;

using DG.Tweening;

namespace UDEngine.Components.Actor {
	/// <summary>
	/// Where all the callbacks live
	/// </summary>
	public class UBulletActor : MonoBehaviour {
		public UBulletActor(UBulletObject obj, UBulletCollider ubc) {
			this.bulletObject = obj;
			this.collider = ubc;
		}

		#region UNITYFUNC
		void Start() {
			if (this.bulletObject == null) {
				UDebug.Error ("bullet object not set");
			}
			if (this.collider == null) {
				UDebug.Error ("bullet collider not set");
			}
		}

		void Update() {
		}
		#endregion

		#region PROP
		public UBulletCollider collider = null; // Reference to UBulletCollider
		public UBulletObject bulletObject = null;

		public UnityEvent collisionEvent = null; // Event on colliding with player
		public UnityEvent defaultEvent = null; // Event that would be triggered every frame if the collider is monitored
		public UnityEvent boundaryEvent = null; // Event that would be triggered when meeting with the monitor boundary
		public UnityEvent recycleEvent = null; // Event triggered on recycling, it will be called in UBulletObject.Recycle()

		// This is used to track all active tween sequences, so that they could be cleanly killed
		public List<Sequence> tweenSequences; // This should be lazily initialized.
		#endregion

		#region METHOD
		public UBulletObject GetObject() {
			return this.bulletObject;
		}
		public UBulletCollider GetCollider() {
			return this.collider;
		}

		public void AddCollisionCallback(UnityAction callback) {
			if (collisionEvent == null) {
				collisionEvent = new UnityEvent ();
			}
			collisionEvent.AddListener (callback);
		}

		public void InvokeCollisionCallbacks() {
			if (collisionEvent == null) {
				// DO NOTHING, and don't warn anything, as this might be useful
			} else {
				collisionEvent.Invoke ();
			}
		}

		public void ClearCollisionCallbacks() {
			collisionEvent = null;
		}

		public void AddDefaultCallback(UnityAction callback) {
			if (defaultEvent == null) {
				defaultEvent = new UnityEvent ();
			}
			defaultEvent.AddListener (callback);
		}

		public void InvokeDefaultCallbacks() {
			if (defaultEvent == null) {
				// DO NOTHING, and don't warn anything, as this might be useful
			} else {
				defaultEvent.Invoke ();
			}
		}

		public void ClearDefaultCallbacks() {
			defaultEvent = null;
		}

		public void AddBoundaryCallback(UnityAction callback) {
			if (boundaryEvent == null) {
				boundaryEvent = new UnityEvent ();
			}
			boundaryEvent.AddListener (callback);
		}

		public void InvokeBoundaryCallbacks() {
			if (boundaryEvent == null) {
				// DO NOTHING, and don't warn anything, as this might be useful
			} else {
				boundaryEvent.Invoke ();
			}
		}

		public void ClearBoundaryCallbacks() {
			boundaryEvent = null;
		}

		public void AddRecycleCallback(UnityAction callback) {
			if (recycleEvent == null) {
				recycleEvent = new UnityEvent ();
			}
			recycleEvent.AddListener (callback);
		}

		public void InvokeRecycleCallbacks() {
			if (recycleEvent == null) {
				// DO NOTHING, and don't warn anything, as this might be useful
			} else {
				recycleEvent.Invoke ();
			}
		}

		public void ClearRecycleCallbacks() {
			recycleEvent = null;
		}

		public void ClearAllCallbacks() {
			ClearDefaultCallbacks ();
			ClearCollisionCallbacks ();
			ClearBoundaryCallbacks ();
			ClearRecycleCallbacks ();
		}


		public void AddTweenSequence(Sequence seq) {
			if (tweenSequences == null) {
				tweenSequences = new List<Sequence> ();
			}
			tweenSequences.Add (seq);
		}
		public Sequence GetTweenSequenceAt(int index) {
			if (tweenSequences == null) {
				tweenSequences = new List<Sequence> ();
			}
			if (index >= tweenSequences.Count || index < 0) {
				UDebug.Error ("cannot find tween sequence of the given index");
				return null;
			} else {
				return tweenSequences [index];
			}
		}
		public void KillTweenSequenceAt(int index) {
			if (tweenSequences == null) {
				tweenSequences = new List<Sequence> ();
			}
			if (index >= tweenSequences.Count || index < 0) {
				UDebug.Error ("cannot find tween sequence of the given index, kill fails");
			} else {
				tweenSequences [index].Kill ();
			}
		}
		public void KillAllTweenSequences() {
			if (tweenSequences != null) { // not null, else do NOTHING
				foreach (Sequence seq in tweenSequences) {
					seq.Kill ();
				}
			}
		}
		#endregion
	}
}
