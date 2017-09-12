using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

using UDEngine;
using UDEngine.Core;
using UDEngine.Core.Bullet;
using UDEngine.Core.Collision;
using UDEngine.Core.Actor;
using UDEngine.Core.Pool;
using UDEngine.Internal;
using UDEngine.Enum;
using UDEngine.Interface;

using DG.Tweening;


public class TestSimplePool : MonoBehaviour {
	
	public UBulletPoolManager poolManager;


	public UCollisionMonitor monitor;
	public UMonolikeExecutor executor;
	public GameObject shooter;

	public GameObject mouseBullet;

	public GameObject globalObject;

	public URectBulletRegionTrigger regionTrigger;



	// Use this for initialization
	void Start () {
		var mousePos = Input.mousePosition;
		mousePos.z = 10; // select distance = 10 units from the camera
		mouseBullet.GetComponentInChildren<SpriteRenderer> ().color = Color.green;
		mouseBullet.GetComponentInChildren<UTargetCollider> ().SetEnable (true);

		regionTrigger.AddTriggerCallback((UBulletCollider ubc) => {
			ubc.trans.DOKill();
			ubc.GetObject().Recycle();
		});

		monitor.AddTargetCollider (mouseBullet.GetComponentInChildren<UTargetCollider>());
		monitor.AddBulletRegionTrigger (regionTrigger);

		shooter.transform.DOMove (new Vector3 (Random.Range (-5f, 5f), Random.Range (-6f, 6f), 0f), 5f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);

		poolManager.PreloadBulletByID (0, 100);
		poolManager.PreloadBulletByID (1, 100);

		StartCoroutine (ShootCoroutine ());
	}
	
	// Update is called once per frame
	void Update () {
		var mousePos = Input.mousePosition;
		mousePos.z = 10; // select distance = 10 units from the camera
		mouseBullet.transform.position = Camera.main.ScreenToWorldPoint(mousePos);
	}

	IEnumerator ShootCoroutine() {
		bool isEven = true;
		while (true) {
			UBulletObject bulletObject;
			if (isEven) {
				bulletObject = poolManager.FetchBulletByID (0, true);
			} else {
				bulletObject = poolManager.FetchBulletByID (1, true);
			}

			isEven = !isEven;

			bulletObject.trans.position = shooter.transform.position;

			bulletObject.GetActor().AddDefaultCallback(() => {
				bulletObject.GetSpriteRenderer().color = Color.white;
			});
			bulletObject.GetActor ().AddCollisionCallback (() => {
				bulletObject.GetSpriteRenderer ().color = Color.red;
			});
			bulletObject.GetActor ().AddBoundaryCallback (() => {
				bulletObject.trans.DOKill();
				bulletObject.Recycle();
			});

			Vector3 randVec = Random.insideUnitCircle;
			randVec.Scale (new Vector3 (8f, 8f, 0f));
			bulletObject.GetTransform().DOMove (randVec, 5f);

			yield return new WaitForSeconds (0.05f);
		}
	}
}
