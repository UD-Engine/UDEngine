using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using UDEngine.Internal;
using UDEngine.Components;
using UDEngine.Components.Collision;

using DG.Tweening;

public class TestSpaceHash : MonoBehaviour {

	public GameObject bulletPrefab;
	public List<UCircleCollider> bullets;
	public SpatialHash2D<UCircleCollider> hash;

	public GameObject mouseBullet;
	// Use this for initialization
	void Start () {
		hash = new SpatialHash2D<UCircleCollider> (20, 24, 10f, 12f, -5f, -6f);

		for (int i = 0; i < 4000; i++) {
			GameObject newBullet = 
				Instantiate(bulletPrefab, 
					new Vector3(Random.Range(-5f, 5f) , Random.Range(-6f,6f), 0f), 
					Quaternion.identity) as GameObject;
			newBullet.transform.DOMove (new Vector3 (Random.Range (-5f, 5f), Random.Range (-6f, 6f), 0f), 5f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
			bullets.Add (newBullet.GetComponent<UCircleCollider>());
		}

		var mousePos = Input.mousePosition;
		mousePos.z = 10; // select distance = 10 units from the camera

//		mouseBullet = Instantiate (bulletPrefab, Camera.main.ScreenToWorldPoint(mousePos), Quaternion.identity) as GameObject;
//
//		mouseBullet.transform.localScale = new Vector3 (1f, 1f, 1f);
//		mouseBullet.GetComponent<UCircleCollider> ().SetRadius (0.15f);
		mouseBullet.GetComponent<SpriteRenderer> ().color = Color.green;
	}
	
	// Update is called once per frame
	void Update () {
		
		hash.ClearBuckets ();

		foreach (UCircleCollider uc in bullets) {
			uc.GetComponent<SpriteRenderer> ().color = Color.white;
			hash.Insert (uc);
		}

		var mousePos = Input.mousePosition;
		mousePos.z = 10; // select distance = 10 units from the camera
		mouseBullet.transform.position = Camera.main.ScreenToWorldPoint(mousePos);
		//Debug.Log (Camera.main.ScreenToWorldPoint(Input.mousePosition));

		List<UCircleCollider> nearbyBullets = hash.GetNearby (mouseBullet.GetComponent<UCircleCollider>());

		foreach (UCircleCollider uc in nearbyBullets) {
			if (Vector3.Distance (uc.transform.position, mouseBullet.transform.position) < uc.GetRadius () + mouseBullet.GetComponent<UCircleCollider>().GetRadius()) {
				uc.GetComponent<SpriteRenderer> ().color = Color.red;
			}
		}
	}
}
