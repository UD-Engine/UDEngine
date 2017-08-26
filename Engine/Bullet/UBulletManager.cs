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
	public class UBulletManager : MonoBehaviour {


		// UNITYFUNC begin

		// Use this for initialization
		void Start () {
			trans = this.transform;
		}
	
		// Update is called once per frame
		void Update () {
			
		}
		// UNITYFUNC end

		// PROP begin
		public SpriteRenderer spriteRenderer;
		public UBulletCollider collider;
		public List<UBulletManager> children;

		public Transform trans; // caching transform can always get better performance
		// PROP end
	}
}
