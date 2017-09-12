using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UDEngine;
using UDEngine.Core;
using UDEngine.Core.Collision;
using UDEngine.Core.Actor;
using UDEngine.Core.Pool;
using UDEngine.Internal;
using UDEngine.Interface;

using DG.Tweening;

namespace UDEngine.Core.Bullet {
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
		public Dictionary<string, object> props;
		#endregion

		#region METHOD
		public int GetPoolID() {
			if (poolID < 0) {
				UDebug.Warning ("negative pool ID, probably not set");
			}
			return poolID;
		}
		public UBulletObject SetPoolID(int id) {
			if (id < 0) {
				poolID = -1;
			} else {
				poolID = id;
			}

			return this;
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
		public UBulletObject AddChild(UBulletObject obj) {
			if (this.children == null) {
				this.children = new List<UBulletObject> ();
			}
			this.children.Add (obj);
			obj.SetParent (this);
			obj.GetTransform ().SetParent (this.childTrans);

			return this;
		}
		public UBulletObject RemoveChild(UBulletObject obj, bool shouldReturnToPool = true) {
			this.children.Remove (obj); // SLOW, but should be okay...
			obj.SetParent(null);
			if (shouldReturnToPool) {
				// Setting its parent transform back to the pool itself
				obj.GetTransform ().SetParent (obj.GetPoolManager ().GetPoolTransform ());
			}

			return this;
		}


		public object GetProp(string name) {
			return this.props [name];
		}
		public UBulletObject SetProp(string name, object value) {
			this.props.Add (name, value);

			return this;
		}

		public UBulletPoolManager GetPoolManager() {
			return this.poolManager;
		}
		public UBulletObject SetPoolManager(UBulletPoolManager manager) {
			this.poolManager = manager;

			return this;
		}

		public UBulletObject GetParent() {
			return this.parent;
		}

		public UBulletObject SetParent(UBulletObject obj) {
			this.parent = obj;

			return this;
		}

		// Keeping this as void, as NO CHAINING allowed here
		public void Recycle(bool shouldRecycleChildren = false, bool shouldSplitChildrenOnRecycle = false) {
			this.actor.InvokeRecycleCallbacks (); // Recycle callback is called HERE, NOT in the poolManager one...
			this.poolManager.RecycleBullet (this, shouldRecycleChildren, shouldSplitChildrenOnRecycle);
		}


		public UBulletObject KillAllDOTweenAndDOTweenSequence(bool isRecursive = true) {
			this.actor.doTweens.KillAll ();
			/*
			this.actor.KillAllDOTweenTweeners ();
			this.actor.KillAllDOTweenSequences ();
			*/

			if (isRecursive) {
				foreach (UBulletObject childObject in this.children) {
					childObject.KillAllDOTweenAndDOTweenSequence ();
				}
			}

			return this;
		}

		public UBulletObject KillAllLeanTweenAndLeanTweenSequence(bool isRecursive = true) {
			this.actor.leanTweens.KillAll ();
			/*
			this.actor.KillAllLeanTweens ();
			this.actor.KillAllLeanTweenSequences ();
			*/

			if (isRecursive) {
				foreach (UBulletObject childObject in this.children) {
					childObject.KillAllLeanTweenAndLeanTweenSequence ();
				}
			}

			return this;
		}
		#endregion
	}
}
