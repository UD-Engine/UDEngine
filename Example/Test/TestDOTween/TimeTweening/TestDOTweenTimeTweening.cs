using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using DG.Tweening;
using KSM.Tweening;

public class TestDOTweenTimeTweening : MonoBehaviour {

	public Text txt;

	// Use this for initialization
	void Start () {
		StartCoroutine (TestCoroutine ());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	IEnumerator TestCoroutine() {
		KTweenedFloat interval = new KTweenedFloat (0.1f, 0.01f, 5f);

		interval.SetEase (Ease.Linear).Run ();

		while (true) {
			txt.text = txt.text + "1";

			yield return new WaitForSeconds (interval.GetValue());
		}

		yield break;
	}
}
