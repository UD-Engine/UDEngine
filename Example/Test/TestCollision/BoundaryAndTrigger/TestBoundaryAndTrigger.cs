using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

using UDEngine;
using UDEngine.Core;
using UDEngine.Core.Collision;
using UDEngine.Core.Actor;
using UDEngine.Internal;
using UDEngine.Enum;
using UDEngine.Interface;

using DG.Tweening;

public class TestBoundaryAndTrigger : MonoBehaviour {

	public GameObject bulletPrefab;
	public UCollisionMonitor monitor;
	public UMonolikeExecutor executor;
	public GameObject shooter;

	public GameObject mouseBullet;

	public GameObject globalObject;

	public Text txt;

	public URectBulletRegionTrigger regionTrigger;

	// Use this for initialization
	void Start () {
		var mousePos = Input.mousePosition;
		mousePos.z = 10; // select distance = 10 units from the camera
		mouseBullet.GetComponentInChildren<SpriteRenderer> ().color = Color.green;
		mouseBullet.GetComponentInChildren<UTargetCollider> ().SetEnable (true);


		regionTrigger.AddTriggerCallback((UBulletCollider ubc) => {
			ubc.trans.DOKill();
			ubc.GetActor ().ClearDefaultCallbacks ();
			ubc.GetActor ().ClearDefaultCallbacks ();
			ubc.trans.GetComponentInChildren<SpriteRenderer>().color = Color.yellow;
			ubc.GetActor ().ClearBoundaryCallbacks ();
		});

		monitor.AddTargetCollider (mouseBullet.GetComponentInChildren<UTargetCollider>());
		monitor.AddBulletRegionTrigger (regionTrigger);

		shooter.transform.DOMove (new Vector3 (Random.Range (-5f, 5f), Random.Range (-6f, 6f), 0f), 5f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
		StartCoroutine (TestCoroutine());
	}
	
	// Update is called once per frame
	void Update () {
		var mousePos = Input.mousePosition;
		mousePos.z = 10; // select distance = 10 units from the camera
		mouseBullet.transform.position = Camera.main.ScreenToWorldPoint(mousePos);
	}

	IEnumerator TestCoroutine() {
		int count = 0;

		while (true) {
			count++;

			GameObject newBullet = Instantiate(bulletPrefab, shooter.transform.position, Quaternion.identity) as GameObject;

			Vector3 randVec = Random.insideUnitCircle;
			randVec.Scale (new Vector3 (8f, 8f, 0f));
			newBullet.transform.DOMove (randVec, 5f);

			UBulletCollider ubc = newBullet.GetComponentInChildren<UBulletCollider>();
			ubc.SetEnable (true);

			ubc.GetActor().AddDefaultCallback(() => {
				ubc.trans.GetComponentInChildren<SpriteRenderer>().color = Color.white;
			});
			ubc.GetActor().AddCollisionCallback (() => {
				ubc.trans.GetComponentInChildren<SpriteRenderer>().color = Color.red;
			});
			ubc.GetActor ().AddBoundaryCallback (() => {
				newBullet.transform.DOKill();
				ubc.GetActor ().ClearDefaultCallbacks ();
				ubc.GetActor ().ClearDefaultCallbacks ();
				ubc.trans.GetComponentInChildren<SpriteRenderer>().color = Color.blue;
				ubc.GetActor ().ClearBoundaryCallbacks ();
			});

			monitor.AddBulletCollider (ubc);

			yield return new WaitForSeconds (0.03f);

			if (count % 100 == 0) {
				//Debug.Log ("Bullet: " + count.ToString ());
				txt.text = "Bullet: " + count.ToString ();
			}
		}
	}
}
