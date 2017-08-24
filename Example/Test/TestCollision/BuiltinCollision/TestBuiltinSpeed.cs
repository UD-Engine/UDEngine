using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

public class TestBuiltinSpeed : MonoBehaviour {

	public GameObject bulletPrefab;
	public GameObject mouseBullet;

	// Use this for initialization
	void Start () {
		for (int i = 0; i < 2000; i++) {
			GameObject newBullet = 
				Instantiate(bulletPrefab, 
					new Vector3(Random.Range(-5f, 5f) , Random.Range(-6f,6f), 0f), 
					Quaternion.identity) as GameObject;
			newBullet.transform.DOMove (new Vector3 (Random.Range (-5f, 5f), Random.Range (-6f, 6f), 0f), 5f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
		}
		var mousePos = Input.mousePosition;
		mousePos.z = 10; // select distance = 10 units from the camera

		//		mouseBullet = Instantiate (bulletPrefab, Camera.main.ScreenToWorldPoint(mousePos), Quaternion.identity) as GameObject;
		//
		//		mouseBullet.transform.localScale = new Vector3 (1f, 1f, 1f);
		//		mouseBullet.GetComponent<UCircleCollider> ().SetRadius (0.15f);
		mouseBullet.GetComponentInChildren<SpriteRenderer> ().color = Color.green;
	}
	
	// Update is called once per frame
	void Update () {
		var mousePos = Input.mousePosition;
		mousePos.z = 10; // select distance = 10 units from the camera
		mouseBullet.transform.position = Camera.main.ScreenToWorldPoint(mousePos);
	}
}
