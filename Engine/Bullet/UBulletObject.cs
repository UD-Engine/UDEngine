using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UDEngine;
using UDEngine.Components;
using UDEngine.Components.Collision;
using UDEngine.Components.Actor;
using UDEngine.Components.Pool;
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
			if (actor == null) {
				actor = trans.GetComponentInChildren<UBulletActor> ();
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
		public UBulletActor actor = null;

		public Transform trans = null; // caching transform can always get better performance

		// Recycle ID is used for recycling into the Pool. 
		// This would be MUCH better than using the Dictionary for reversed mapping
		public int poolID = -1;
		public UBulletPoolManager poolManager;
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

		// As UBulletActor is MONO-ized, the old problem should no longer exist anymore
		public UBulletActor GetActor() {
			return this.actor;
		}
		public UBulletCollider GetCollider() {
			return this.collider;
		}
		public Transform GetTransform() {
			return this.trans;
		}
		public SpriteRenderer GetSpriteRenderer() {
			return this.spriteRenderer;
		}
		public List<UBulletObject> GetChildren() {
			return this.children;
		}


		public void SetPoolManager(UBulletPoolManager manager) {
			this.poolManager = manager;
		}


		public void Recycle(bool shouldRecycleChildren = false, bool shouldSplitChildrenOnRecycle = false) {
			this.poolManager.RecycleBullet (this, shouldRecycleChildren, shouldSplitChildrenOnRecycle);
			//this.collider.SetRecyclable (true); // True recycling is called in UCollisionMonitor
		}
		#endregion
	}
}
