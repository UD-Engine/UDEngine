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
		}
	
		// Update is called once per frame
		void Update () {
			
		}
		#endregion

		#region PROP
		public SpriteRenderer spriteRenderer;
		public UBulletCollider collider;
		public List<UBulletObject> children;

		public Transform trans = null; // caching transform can always get better performance

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
		#endregion
	}
}
