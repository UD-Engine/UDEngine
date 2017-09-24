using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

using UDEngine.Core.Actor;
using UDEngine.Core.Bullet;
using UDEngine.Core.Collision;
using UDEngine.Core.Pool;
using UDEngine.Core.Shooter;

using UDEngine.Commons.ShootStage;

using UDEngine.Plugin.Patterner;
using UDEngine.Plugin.DOTweenExtension;
using UDEngine.Plugin.ShootDust;

public class Sample_01_Normal : MonoBehaviour {

	public GameObject shootOrigin;
	public GameObject secondShootOrigin;
	public UBulletPoolManager poolManager;
	public UCollisionMonitor collisionMonitor;

	public List<Sprite> shootSprites;

	public bool shouldSpin = false;

	// Use this for initialization
	void Start () {
		// Preload bullet for later use
		poolManager.PreloadBulletByID (0, 1000);
		poolManager.PreloadBulletByID (1, 1000);

		// Start the main coroutine
		StartCoroutine (MainCoroutine ());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	// The main coroutine
	IEnumerator MainCoroutine() {
		List<UShooter> shooters = shootOrigin.transform.PGWholeCircleOutwardPattern (10, 0.6f);
		List<UShooter> secondShooters = secondShootOrigin.transform.PGWholeCircleOutwardPattern (10, 0.6f);

		yield return new WaitForSeconds (1f);

		if (shouldSpin) {
			shootOrigin.transform.DORotate (new Vector3 (0f, 0f, 360f), 4f, RotateMode.WorldAxisAdd).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutQuad);
			secondShootOrigin.transform.DORotate (new Vector3 (0f, 0f, 400f), 4f, RotateMode.WorldAxisAdd).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutQuad);
		}

		foreach (UShooter shooter in shooters) {
			USimpleFadeDustObject dustObject = USimpleFadeDust.Create (shooter.trans, shootSprites [0]);
			dustObject.trans.localScale = new Vector3 (0.4f, 0.4f, 0.4f);
			dustObject.spriteRenderer.sortingOrder = 3;
			dustObject.Fade (0.5f);

			/*
			Transform spriteTrans = new GameObject ("ShooterSprite").transform;

			spriteTrans.position = shooter.GetTransform ().position;
			spriteTrans.rotation = shooter.GetTransform ().rotation;
			spriteTrans.parent = shooter.GetTransform ();

			spriteTrans.localScale = new Vector3 (0.4f, 0.4f, 0.4f);

			SpriteRenderer spriteRenderer = spriteTrans.gameObject.AddComponent<SpriteRenderer> ();
			spriteRenderer.sprite = shootSprites [0];
			spriteRenderer.sortingOrder = 3;
			Tweener fadeTween = spriteRenderer.DOFade (0f, 0.5f);
			*/

			shooter.SetBulletPoolManager (poolManager);

			shooter
				.AddStage (new UShootStageSameIntervalInfinite (0.1f, 0, (UBulletObject bulletObject) => {
					// Recycle on hitting boundary
					bulletObject.GetActor ().AddBoundaryCallback (() => {
						bulletObject.Recycle ();
					});
					// Set bullet to move forward on activation
					bulletObject.GetActor().doTweens.AddTweener(bulletObject.GetTransform ().DOMoveUp (1.5f));

					dustObject.Fade(0.5f);
					/*
					fadeTween.Kill();
					Color lastColor = spriteRenderer.color;
					lastColor.a = 1f;
					spriteRenderer.color = lastColor;
					fadeTween = spriteRenderer.DOFade (0f, 0.5f);
					*/
				}));

			// Trigger shooting
			shooter.Shoot ();
		}

		foreach (UShooter shooter in secondShooters) {
			USimpleFadeDustObject dustObject = USimpleFadeDust.Create (shooter.trans, shootSprites [1]);
			dustObject.trans.localScale = new Vector3 (0.4f, 0.4f, 0.4f);
			dustObject.spriteRenderer.sortingOrder = 3;
			dustObject.Fade (0.5f);

			/*
			Transform spriteTrans = new GameObject ("ShooterSprite").transform;

			spriteTrans.position = shooter.GetTransform ().position;
			spriteTrans.rotation = shooter.GetTransform ().rotation;
			spriteTrans.parent = shooter.GetTransform ();

			spriteTrans.localScale = new Vector3 (0.4f, 0.4f, 0.4f);

			SpriteRenderer spriteRenderer = spriteTrans.gameObject.AddComponent<SpriteRenderer> ();
			spriteRenderer.sprite = shootSprites [1];
			spriteRenderer.sortingOrder = 3;
			Tweener fadeTween = spriteRenderer.DOFade (0f, 0.5f);
			*/


			shooter.SetBulletPoolManager (poolManager);

			shooter
				.AddStage (new UShootStageSameIntervalInfinite (0.1f, 1, (UBulletObject bulletObject) => {
					// Recycle on hitting boundary
					bulletObject.GetActor ().AddBoundaryCallback (() => {
						bulletObject.Recycle ();
					});
					// Set bullet to move forward on activation
					bulletObject.GetActor().doTweens.AddTweener(bulletObject.GetTransform ().DOMoveUp (1.5f));

					dustObject.Fade(0.5f);

					/*
					fadeTween.Kill();
					Color lastColor = spriteRenderer.color;
					lastColor.a = 1f;
					spriteRenderer.color = lastColor;
					fadeTween = spriteRenderer.DOFade (0f, 0.5f);
					*/
				}));

			// Trigger shooting
			shooter.Shoot ();
		}

		yield break;
	}
}
