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

using DG.Tweening;

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
		public UBulletObject parent = null;
		public List<UBulletObject> children;
		public UBulletActor actor = null;

		public Transform trans = null; // caching transform can always get better performance
		public Transform childTrans = null;

		// Recycle ID is used for recycling into the Pool. 
		// This would be MUCH better than using the Dictionary for reversed mapping
		public int poolID = -1;
		public UBulletPoolManager poolManager;

		// This props is used for adding cacheable props by the user
		public Dictionary<string, Object> props;
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
		public Transform GetChildTransform() {
			return this.childTrans;
		}
		public void AddChild(UBulletObject obj) {
			if (this.children == null) {
				this.children = new List<UBulletObject> ();
			}
			this.children.Add (obj);
			obj.SetParent (this);
			obj.GetTransform ().SetParent (this.childTrans);
		}
		public void RemoveChild(UBulletObject obj, bool shouldReturnToPool = true) {
			this.children.Remove (obj); // SLOW, but should be okay...
			obj.SetParent(null);
			if (shouldReturnToPool) {
				// Setting its parent transform back to the pool itself
				obj.GetTransform ().SetParent (obj.GetPoolManager ().GetPoolTransform ());
			}
		}


		public Object GetProp(string name) {
			return this.props [name];
		}
		public void SetProp(string name, Object value) {
			this.props.Add (name, value);
		}

		public UBulletPoolManager GetPoolManager() {
			return this.poolManager;
		}
		public void SetPoolManager(UBulletPoolManager manager) {
			this.poolManager = manager;
		}

		public UBulletObject GetParent() {
			return this.parent;
		}

		public void SetParent(UBulletObject obj) {
			this.parent = obj;
		}


		public void Recycle(bool shouldRecycleChildren = false, bool shouldSplitChildrenOnRecycle = false) {
			this.actor.InvokeRecycleCallbacks (); // Recycle callback is called HERE, NOT in the poolManager one...
			this.poolManager.RecycleBullet (this, shouldRecycleChildren, shouldSplitChildrenOnRecycle);
		}


		public void KillAllTweenAndTweenSequence(bool isRecursive = true) {
			this.trans.DOKill ();
			this.actor.KillAllTweenSequences ();

			if (isRecursive) {
				foreach (UBulletObject childObject in this.children) {
					childObject.KillAllTweenAndTweenSequence ();
				}
			}
		}
		#endregion
	}
}
