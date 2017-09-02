using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UDEngine;
using UDEngine.Components;
using UDEngine.Components.Collision;
using UDEngine.Components.Actor;
using UDEngine.Internal;
using UDEngine.Interface;

namespace UDEngine.Components.Bullet {
	public class UBulletObject : MonoBehaviour {

		#region UNITYFUNC
		// Use this for initialization
		void Start () {
			if (trans == null) {
				trans = this.transform;
			}
			if (spriteRenderer == null) {
				spriteRenderer = trans.GetComponentInChildren<SpriteRenderer> ();
			}
			if (collider == null) {
				collider = trans.GetComponentInChildren<UBulletCollider> ();
			}

			if (_actor == null) {
				_actor = new UBulletActor (this, collider);
			}
		}
	
		// Update is called once per frame
		void Update () {
			
		}
		#endregion

		#region PROP
		public SpriteRenderer spriteRenderer;
		public UBulletCollider collider = null;
		public List<UBulletObject> children;

		public Transform trans = null; // caching transform can always get better performance

		private UBulletActor _actor = null;

		// Recycle ID is used for recycling into the Pool. 
		// This would be MUCH better than using the Dictionary for reversed mapping
		public int poolID = -1;
		#endregion

		#region METHOD
		public int GetPoolID() {
			if (poolID < 0) {
				UDebug.Warning ("negative pool ID, probably not set");
			}
			return poolID;
		}
		public void SetPoolID(int id) {
			if (id < 0) {
				poolID = -1;
			} else {
				poolID = id;
			}
		}

		// FIX: add null checker to avoid race condition that leaves Start() not ran
		// and thus this._actor not created.
		public UBulletActor GetActor() {
			if (this._actor == null) {
				_actor = new UBulletActor (this, collider);
			}
			return this._actor;
		}
		public UBulletCollider GetCollider() {
			if (collider == null) {
				collider = trans.GetComponentInChildren<UBulletCollider> ();
			}
			return this.collider;
		}
		public Transform GetTransform() {
			if (trans == null) {
				trans = this.transform;
			}
			return this.trans;
		}
		public SpriteRenderer GetSpriteRenderer() {
			if (spriteRenderer == null) {
				spriteRenderer = trans.GetComponentInChildren<SpriteRenderer> ();
			}
			return this.spriteRenderer;
		}
		#endregion
	}
}
