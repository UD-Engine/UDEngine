using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

using UDEngine;
using UDEngine.Internal;
using UDEngine.Components;
using UDEngine.Components.Collision;
using UDEngine.Interface;
using UDEngine.Enum;

namespace UDEngine.Components.Actor {
	/// <summary>
	/// Where all the callbacks live
	/// </summary>
	public class UBulletActor {
		public UBulletActor(UBulletCollider ubc) {
			this.collider = ubc;
		}

		// PROP begin
		public UBulletCollider collider; // Reference to UBulletCollider

		public UnityEvent collisionEvent = null; // Event on colliding with player
		public UnityEvent defaultEvent = null; // Event that would be triggered every frame if the collider is monitored

		public UnityEvent boundaryEvent = null; // Event that would be triggered when meeting with the monitor boundary

		// PROP end

		// METHOD begin

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
		// METHOD end
	}
}
