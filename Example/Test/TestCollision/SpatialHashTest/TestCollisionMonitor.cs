using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using UDEngine.Internal;
using UDEngine.Components;
using UDEngine.Components.Collision;
using DG.Tweening;

public class TestCollisionMonitor : MonoBehaviour {

	public GameObject bulletPrefab;
	public UCollisionMonitor monitor;
	public UMonolikeExecutor executor;

	public GameObject mouseBullet;

	public GameObject globalObject;

	// Use this for initialization
	void Start () {

		for (int i = 0; i < 4000; i++) {
			GameObject newBullet = 
				Instantiate(bulletPrefab, 
					new Vector3(Random.Range(-5f, 5f) , Random.Range(-6f,6f), 0f), 
					Quaternion.identity) as GameObject;
			newBullet.transform.DOMove (new Vector3 (Random.Range (-5f, 5f), Random.Range (-6f, 6f), 0f), 5f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);

			UBulletCollider ubc = newBullet.GetComponentInChildren<UBulletCollider>();
			ubc.SetEnable (true);

			ubc.GetActor().AddDefaultCallback(() => {
				ubc.trans.GetComponentInChildren<SpriteRenderer>().color = Color.white;
			});
			ubc.GetActor().AddCollisionCallback (() => {
				ubc.trans.GetComponentInChildren<SpriteRenderer>().color = Color.red;
			});
				
			monitor.AddBulletCollider (ubc);
		}
		var mousePos = Input.mousePosition;
		mousePos.z = 10; // select distance = 10 units from the camera

		//		mouseBullet = Instantiate (bulletPrefab, Camera.main.ScreenToWorldPoint(mousePos), Quaternion.identity) as GameObject;
		//
		//		mouseBullet.transform.localScale = new Vector3 (1f, 1f, 1f);
		//		mouseBullet.GetComponent<UCircleCollider> ().SetRadius (0.15f);
		mouseBullet.GetComponentInChildren<SpriteRenderer> ().color = Color.green;
		mouseBullet.GetComponentInChildren<UTargetCollider> ().SetEnable (true);

		monitor.AddTargetCollider (mouseBullet.GetComponentInChildren<UTargetCollider>());

		// Add monitor to executor for mono-like update and start
		//executor.AddModule (monitor);
	}
	
	// Update is called once per frame
	void Update () {
		var mousePos = Input.mousePosition;
		mousePos.z = 10; // select distance = 10 units from the camera
		mouseBullet.transform.position = Camera.main.ScreenToWorldPoint(mousePos);
	}
}
