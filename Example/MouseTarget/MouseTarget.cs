using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UDEngine.Core.Collision;

public class MouseTarget : MonoBehaviour {

	public Transform trans;
	public SpriteRenderer spriteRenderer;
	public UTargetCollider targetCollider;

	public UCollisionMonitor collisionMonitor;

	void Awake () {
		if (trans == null) {
			trans = this.transform;
		}
		if (spriteRenderer == null) {
			spriteRenderer = GetComponentInChildren<SpriteRenderer> ();
		}
		if (targetCollider == null) {
			targetCollider = GetComponentInChildren<UTargetCollider> ();
		}
	}

	void Start() {
		var mousePos = Input.mousePosition;
		mousePos.z = 10; // select distance = 10 units from the camera
		spriteRenderer.color = Color.green;
		targetCollider.SetEnable (true);

		targetCollider.AddCollisionCallback (() => {
			spriteRenderer.color = Color.red;
		});
		targetCollider.AddNoHitCallback (() => {
			spriteRenderer.color = Color.green;
		});

		collisionMonitor.AddTargetCollider (targetCollider);
	}
	
	// Update is called once per frame
	void Update () {
		var mousePos = Input.mousePosition;
		mousePos.z = 10; // select distance = 10 units from the camera
		trans.position = Camera.main.ScreenToWorldPoint(mousePos);
	}
}
