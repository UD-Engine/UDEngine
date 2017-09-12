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
using UDEngine.Components.Tween;
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
		void Awake() {
			if (doTweens == null) {
				doTweens = new UDOTweenPlug ();
			}
			if (leanTweens == null) {
				leanTweens = new ULeanTweenPlug ();
			}

			if (collisionEvent == null) {
				collisionEvent = new UnityEvent ();
			}
			if (defaultEvent == null) {
				defaultEvent = new UnityEvent ();
			}
			if (boundaryEvent == null) {
				boundaryEvent = new UnityEvent ();
			}
			if (recycleEvent == null) {
				recycleEvent = new UnityEvent ();
			}
		}

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

		/*
		// This is used to track all active tween sequences, so that they could be cleanly killed
		public List<Tweener> doTweenTweeners;
		public List<Sequence> doTweenSequences; // This should be lazily initialized.

		public List<int> leanTweenSequences; // Testing for LeanTween
		public List<int> leanTweenIDs; // Testing for LeanTween
		// HIGHLIGHT: after consideration, I decide NOT to move to LinkedList, as killing all the actual most common action
		*/

		public UDOTweenPlug doTweens;
		public ULeanTweenPlug leanTweens;
		#endregion

		#region METHOD
		public UBulletObject GetObject() {
			return this.bulletObject;
		}
		public UBulletCollider GetCollider() {
			return this.collider;
		}

		public UBulletActor AddCollisionCallback(UnityAction callback) {
			if (collisionEvent == null) {
				collisionEvent = new UnityEvent ();
			}
			collisionEvent.AddListener (callback);

			return this;
		}

		public UBulletActor InvokeCollisionCallbacks() {
			if (collisionEvent == null) {
				// DO NOTHING, and don't warn anything, as this might be useful
			} else {
				collisionEvent.Invoke ();
			}

			return this;
		}

		public UBulletActor ClearCollisionCallbacks() {
			collisionEvent = null;

			return this;
		}

		public UBulletActor AddDefaultCallback(UnityAction callback) {
			if (defaultEvent == null) {
				defaultEvent = new UnityEvent ();
			}
			defaultEvent.AddListener (callback);

			return this;
		}

		public UBulletActor InvokeDefaultCallbacks() {
			if (defaultEvent == null) {
				// DO NOTHING, and don't warn anything, as this might be useful
			} else {
				defaultEvent.Invoke ();
			}

			return this;
		}

		public UBulletActor ClearDefaultCallbacks() {
			defaultEvent = null;

			return this;
		}

		public UBulletActor AddBoundaryCallback(UnityAction callback) {
			if (boundaryEvent == null) {
				boundaryEvent = new UnityEvent ();
			}
			boundaryEvent.AddListener (callback);

			return this;
		}

		public UBulletActor InvokeBoundaryCallbacks() {
			if (boundaryEvent == null) {
				// DO NOTHING, and don't warn anything, as this might be useful
			} else {
				boundaryEvent.Invoke ();
			}

			return this;
		}

		public UBulletActor ClearBoundaryCallbacks() {
			boundaryEvent = null;

			return this;
		}

		public UBulletActor AddRecycleCallback(UnityAction callback) {
			if (recycleEvent == null) {
				recycleEvent = new UnityEvent ();
			}
			recycleEvent.AddListener (callback);

			return this;
		}

		public UBulletActor InvokeRecycleCallbacks() {
			if (recycleEvent == null) {
				// DO NOTHING, and don't warn anything, as this might be useful
			} else {
				recycleEvent.Invoke ();
			}

			return this;
		}

		public UBulletActor ClearRecycleCallbacks() {
			recycleEvent = null;

			return this;
		}

		public UBulletActor ClearAllCallbacks() {
			ClearDefaultCallbacks ();
			ClearCollisionCallbacks ();
			ClearBoundaryCallbacks ();
			ClearRecycleCallbacks ();

			return this;
		}

		/*
		public UBulletActor AddDOTweenTweener(Tweener tweener) {
			if (doTweenTweeners == null) {
				doTweenTweeners = new List<Tweener> ();
			}
			doTweenTweeners.Add (tweener);

			return this;
		}

		public Tweener GetDOTweenTweenerAt(int index) {
			if (doTweenTweeners == null) {
				doTweenTweeners = new List<Tweener> ();
			}
			if (index >= doTweenTweeners.Count || index < 0) {
				UDebug.Error ("cannot find DOTween Tweener of the given index");
				return null;
			} else {
				return doTweenTweeners [index];
			}
		}

		public UBulletActor KillDOTweenTweenerAt(int index) {
			if (doTweenTweeners == null) {
				doTweenTweeners = new List<Tweener> ();
			}
			if (index >= doTweenTweeners.Count || index < 0) {
				UDebug.Error ("cannot find DOTween Tweener of the given index, kill fails");
			} else {
				doTweenTweeners [index].Kill ();
				doTweenTweeners.RemoveAt (index); // Removing tweener reference. SLOW!!! But this is RARE, so it should be okay
			}
			return this;
		}

		public UBulletActor KillAllDOTweenTweeners() {
			if (doTweenTweeners != null) { // not null, else do NOTHING
				foreach (Tweener tweener in doTweenTweeners) {
					tweener.Kill ();
				}
			}
			doTweenTweeners = new List<Tweener> (); // Cleanup

			return this;
		}


		public UBulletActor AddDOTweenSequence(Sequence seq) {
			if (doTweenSequences == null) {
				doTweenSequences = new List<Sequence> ();
			}
			doTweenSequences.Add (seq);

			return this;
		}
		public Sequence GetDOTweenSequenceAt(int index) {
			if (doTweenSequences == null) {
				doTweenSequences = new List<Sequence> ();
			}
			if (index >= doTweenSequences.Count || index < 0) {
				UDebug.Error ("cannot find DOTween Sequence of the given index");
				return null;
			} else {
				return doTweenSequences [index];
			}
		}
		public UBulletActor KillDOTweenSequenceAt(int index) {
			if (doTweenSequences == null) {
				doTweenSequences = new List<Sequence> ();
			}
			if (index >= doTweenSequences.Count || index < 0) {
				UDebug.Error ("cannot find DOTween Sequence of the given index, kill fails");
			} else {
				doTweenSequences [index].Kill ();
				doTweenSequences.RemoveAt (index); // Removing seq reference. SLOW!!! But this is RARE, so it should be okay
			}

			return this;
		}
		public UBulletActor KillAllDOTweenSequences() {
			if (doTweenSequences != null) { // not null, else do NOTHING
				foreach (Sequence seq in doTweenSequences) {
					seq.Kill ();
				}
			}
			doTweenSequences = new List<Sequence> (); // Cleanup

			return this;
		}


		public UBulletActor AddLeanTweenSequenceID(int seqID) {
			if (leanTweenSequences == null) {
				leanTweenSequences = new List<int> ();
			}
			leanTweenSequences.Add (seqID);

			return this;
		}
		public int GetLeanTweenSequenceIDAt(int index) {
			if (leanTweenSequences == null) {
				leanTweenSequences = new List<int> ();
			}
			if (index >= leanTweenSequences.Count || index < 0) {
				UDebug.Error ("cannot find tween sequence of the given index");
			}
			return leanTweenSequences [index];
		}
		public UBulletActor KillLeanTweenSequenceAt(int index) {
			if (leanTweenSequences == null) {
				leanTweenSequences = new List<int> ();
			}
			if (index >= leanTweenSequences.Count || index < 0) {
				UDebug.Error ("cannot find tween sequence of the given index, kill fails");
			} else {
				LeanTween.cancel(leanTweenSequences [index]);
				leanTweenSequences.RemoveAt (index); // Removing seq reference. SLOW!!! But this is RARE, so it should be okay
			}

			return this;
		}
		public UBulletActor KillAllLeanTweenSequences() {
			if (leanTweenSequences != null) { // not null, else do NOTHING
				foreach (int seqID in leanTweenSequences) {
					LeanTween.cancel (seqID);
				}
			}

			// BUGFIX: resetting leanTweenSequences after this;
			leanTweenSequences = new List<int>();

			return this;
		}


		public UBulletActor AddLeanTweenID(int tweenID) {
			if (leanTweenIDs == null) {
				leanTweenIDs = new List<int> ();
			}
			leanTweenIDs.Add (tweenID);

			return this;
		}
		public int GetLeanTweenIDAt(int index) {
			if (leanTweenIDs == null) {
				leanTweenIDs = new List<int> ();
			}
			if (index >= leanTweenIDs.Count || index < 0) {
				UDebug.Error ("cannot find tween sequence of the given index");
			}
			return leanTweenIDs [index];
		}
		public UBulletActor KillLeanTweenAt(int index) {
			if (leanTweenIDs == null) {
				leanTweenIDs = new List<int> ();
			}
			if (index >= leanTweenIDs.Count || index < 0) {
				UDebug.Error ("cannot find tween sequence of the given index, kill fails");
			} else {
				LeanTween.cancel(leanTweenIDs [index]);
				leanTweenIDs.RemoveAt (index); // Removing tween reference. SLOW!!! But this is RARE, so it should be okay
			}

			return this;
		}
		public UBulletActor KillAllLeanTweens() {
			if (leanTweenIDs != null) { // not null, else do NOTHING
				foreach (int tweenID in leanTweenIDs) {
					while (LeanTween.isTweening (tweenID)) { // BUGFIX: Safety check... THIS IS NOT WORKING!!!
						LeanTween.cancel (tweenID);
					}
				}
			}

			// BUGFIX: resetting leanTweenIDs after this;
			leanTweenIDs = new List<int>();

			return this;
		}
		*/
		#endregion
	}
}
