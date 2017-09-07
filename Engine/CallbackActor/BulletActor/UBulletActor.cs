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

		public List<int> leanTweenSequences; // Testing for LeanTween
		public List<int> leanTweenIDs; // Testing for LeanTween
		// HIGHLIGHT: after consideration, I decide NOT to move to LinkedList, as killing all the actual most common action
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
				tweenSequences.RemoveAt (index); // Removing seq reference. SLOW!!! But this is RARE, so it should be okay
			}
		}
		public void KillAllTweenSequences() {
			if (tweenSequences != null) { // not null, else do NOTHING
				foreach (Sequence seq in tweenSequences) {
					seq.Kill ();
				}
			}
			tweenSequences = new List<Sequence> (); // Cleanup
		}


		public void AddLeanTweenSequenceID(int seqID) {
			if (leanTweenSequences == null) {
				leanTweenSequences = new List<int> ();
			}
			leanTweenSequences.Add (seqID);
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
		public void KillLeanTweenSequenceAt(int index) {
			if (leanTweenSequences == null) {
				leanTweenSequences = new List<int> ();
			}
			if (index >= leanTweenSequences.Count || index < 0) {
				UDebug.Error ("cannot find tween sequence of the given index, kill fails");
			} else {
				LeanTween.cancel(leanTweenSequences [index]);
				leanTweenSequences.RemoveAt (index); // Removing seq reference. SLOW!!! But this is RARE, so it should be okay
			}
		}
		public void KillAllLeanTweenSequences() {
			if (leanTweenSequences != null) { // not null, else do NOTHING
				foreach (int seqID in leanTweenSequences) {
					LeanTween.cancel (seqID);
				}
			}

			// BUGFIX: resetting leanTweenSequences after this;
			leanTweenSequences = new List<int>();
		}


		public void AddLeanTweenID(int tweenID) {
			if (leanTweenIDs == null) {
				leanTweenIDs = new List<int> ();
			}
			leanTweenIDs.Add (tweenID);
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
		public void KillLeanTweenAt(int index) {
			if (leanTweenIDs == null) {
				leanTweenIDs = new List<int> ();
			}
			if (index >= leanTweenIDs.Count || index < 0) {
				UDebug.Error ("cannot find tween sequence of the given index, kill fails");
			} else {
				LeanTween.cancel(leanTweenIDs [index]);
				leanTweenIDs.RemoveAt (index); // Removing tween reference. SLOW!!! But this is RARE, so it should be okay
			}
		}
		public void KillAllLeanTweens() {
			if (leanTweenIDs != null) { // not null, else do NOTHING
				foreach (int tweenID in leanTweenIDs) {
					while (LeanTween.isTweening (tweenID)) { // BUGFIX: Safety check... THIS IS NOT WORKING!!!
						LeanTween.cancel (tweenID);
					}
				}
			}

			// BUGFIX: resetting leanTweenIDs after this;
			leanTweenIDs = new List<int>();
		}
		#endregion
	}
}
