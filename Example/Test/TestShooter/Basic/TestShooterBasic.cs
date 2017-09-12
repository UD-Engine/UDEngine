using System.Collections;
using System.Collections.Generic;

using System.Threading;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

using UDEngine;
using UDEngine.Components;
using UDEngine.Components.Bullet;
using UDEngine.Components.Collision;
using UDEngine.Components.Actor;
using UDEngine.Components.Pool;
using UDEngine.Components.Shooter;
using UDEngine.Internal;
using UDEngine.Enum;
using UDEngine.Interface;

using UDEngine.Commons.ShootStage;

using DG.Tweening;
using KSM.Tweening;

public class TestShooterBasic : MonoBehaviour {

	public UBulletPoolManager poolManager;
	public UCollisionMonitor monitor;
	public UMonolikeExecutor executor;

	public List<UShooter> shooters;
	//public UShooter firstShooter;
	//public UShooter secondShooter;

	public GameObject mouseBullet;
	public GameObject globalObject;

	public URectBulletRegionTrigger regionTrigger;

	public Text txt;

	public long count = 0L;

	// Use this for initialization
	void Start () {
		//LeanTween.init (10000);


		var mousePos = Input.mousePosition;
		mousePos.z = 10; // select distance = 10 units from the camera
		mouseBullet.GetComponentInChildren<SpriteRenderer> ().color = Color.green;
		mouseBullet.GetComponentInChildren<UTargetCollider> ().SetEnable (true);

		regionTrigger.AddTriggerCallback((UBulletCollider ubc) => {
			ubc.GetObject().Recycle();
		});

		monitor.AddTargetCollider (mouseBullet.GetComponentInChildren<UTargetCollider>());
		monitor.AddBulletRegionTrigger (regionTrigger);

		poolManager.PreloadBulletByID (0, 10000);
		poolManager.PreloadBulletByID (1, 10000);

		StartCoroutine (ShootCoroutine ());
	}
	
	// Update is called once per frame
	void Update () {
		var mousePos = Input.mousePosition;
		mousePos.z = 10; // select distance = 10 units from the camera
		mouseBullet.transform.position = Camera.main.ScreenToWorldPoint(mousePos);

		txt.text = "Bullet: " + monitor.GetBulletColliders ().Count;
	}

	void ConfigureShooter(UShooter shooter, bool isEven, bool shouldChangeColor = false) {
		if (isEven) {
			shooter
				.AddStage (new UShootStageSameInterval (5000, 0.0001f, 0, (UBulletObject bulletObject) => {
					if (shouldChangeColor) {
						bulletObject.GetActor ().AddDefaultCallback (() => {
							bulletObject.GetSpriteRenderer ().color = Color.white;
						});
						bulletObject.GetActor ().AddCollisionCallback (() => {
							bulletObject.GetSpriteRenderer ().color = Color.red;
						});
					}
					bulletObject.GetActor ().AddBoundaryCallback (() => {
						bulletObject.Recycle ();
					});
					bulletObject.GetActor().doTweens.AddTweener(bulletObject.GetTransform ().DOMoveUp (1.5f));
//					int moveID = bulletObject.GetTransform().LeanMoveUp(1.5f).setEase(LeanTweenType.easeOutQuad).id;
//					bulletObject.GetActor().AddLeanTweenID(moveID);
				}))
				.AddStage (new UShootStageWait (1f))
				.AddStage (new UShootStageSameInterval (5000, 0.0001f, 1, (UBulletObject bulletObject) => {
					if (shouldChangeColor) {
						bulletObject.GetActor ().AddDefaultCallback (() => {
							bulletObject.GetSpriteRenderer ().color = Color.white;
						});
						bulletObject.GetActor ().AddCollisionCallback (() => {
							bulletObject.GetSpriteRenderer ().color = Color.red;
						});
					}
					bulletObject.GetActor ().AddBoundaryCallback (() => {
						bulletObject.Recycle ();
					});
					bulletObject.GetActor().doTweens.AddTweener(bulletObject.GetTransform ().DOMoveUp (1.5f));
//					int moveID = bulletObject.GetTransform().LeanMoveUp(1.5f).setEase(LeanTweenType.easeOutQuad).id;
//					bulletObject.GetActor().AddLeanTweenID(moveID);
				}))
				.SetLoopType (EShooterLoopType.BOUNCE);

			shooter.Shoot ();
			shooter.GetTransform ().DORotateForever (1f);
		} else {
			shooter
				.AddStage (new UShootStageSameInterval (10000, 0.0001f, 1, (UBulletObject bulletObject) => {
					if (shouldChangeColor) {
						bulletObject.GetActor ().AddDefaultCallback (() => {
							bulletObject.GetSpriteRenderer ().color = Color.white;
						});
						bulletObject.GetActor ().AddCollisionCallback (() => {
							bulletObject.GetSpriteRenderer ().color = Color.red;
						});
					}
					bulletObject.GetActor ().AddBoundaryCallback (() => {
						bulletObject.Recycle ();
					});
					bulletObject.GetActor().doTweens.AddTweener(bulletObject.GetTransform ().DOMoveUp (1.5f));
//					int moveID = bulletObject.GetTransform().LeanMoveUp(1.5f).setEase(LeanTweenType.easeOutQuad).id;
//					bulletObject.GetActor().AddLeanTweenID(moveID);
				}))
				.AddStage (new UShootStageWait (1f))
				.AddStage (new UShootStageSameInterval (10000, 0.0001f, 0, (UBulletObject bulletObject) => {
					if (shouldChangeColor) {
						bulletObject.GetActor ().AddDefaultCallback (() => {
							bulletObject.GetSpriteRenderer ().color = Color.white;
						});
						bulletObject.GetActor ().AddCollisionCallback (() => {
							bulletObject.GetSpriteRenderer ().color = Color.red;
						});
					}
					bulletObject.GetActor ().AddBoundaryCallback (() => {
						bulletObject.Recycle ();
					});
					bulletObject.GetActor().doTweens.AddTweener(bulletObject.GetTransform ().DOMoveUp (1.5f));
//					int moveID = bulletObject.GetTransform().LeanMoveUp(1.5f).setEase(LeanTweenType.easeOutQuad).id;
//					bulletObject.GetActor().AddLeanTweenID(moveID);
				}))
				.SetLoopType (EShooterLoopType.BOUNCE);

			shooter.Shoot ();
			shooter.GetTransform ().DORotateForever (-1f);
		}
	}

	IEnumerator ShootCoroutine() {
		/*shooter
			.AddStage (new UShootStageSameInterval (5000, 0.001f, 0, (UBulletObject bulletObject) => {
				bulletObject.GetActor ().AddDefaultCallback (() => {
					bulletObject.GetSpriteRenderer ().color = Color.white;
				});
				bulletObject.GetActor ().AddCollisionCallback (() => {
					bulletObject.GetSpriteRenderer ().color = Color.red;
				});
				bulletObject.GetActor ().AddBoundaryCallback (() => {
					count--;
					txt.text = "Bullet: " + count;

					bulletObject.trans.DOKill ();
					bulletObject.Recycle ();
				});
				bulletObject.GetTransform ().DOMoveUp (2f);

				count++;
				txt.text = "Bullet: " + count;
			}))
			.AddStage (new UShootStageWait (1f))
			.AddStage (new UShootStageSameInterval (5000, 0.001f, 1, (UBulletObject bulletObject) => {
				bulletObject.GetActor ().AddDefaultCallback (() => {
					bulletObject.GetSpriteRenderer ().color = Color.white;
				});
				bulletObject.GetActor ().AddCollisionCallback (() => {
					bulletObject.GetSpriteRenderer ().color = Color.red;
				});
				bulletObject.GetActor ().AddBoundaryCallback (() => {
					count--;
					txt.text = "Bullet: " + count;

					bulletObject.trans.DOKill ();
					bulletObject.Recycle ();
				});
				bulletObject.GetTransform ().DOMoveUp (2f);

				count++;
				txt.text = "Bullet: " + count;
			}))
			.SetLoopType (EShooterLoopType.BOUNCE);

		shooter.Shoot ();
		//shooter.GetTransform ().DOBlendableRotateBy (new Vector3 (0f, 0f, 720f), 5f, RotateMode.FastBeyond360);
		shooter.GetTransform().DORotateForever(1f);

		secondShooter
			.AddStage (new UShootStageSameInterval (10000, 0.0001f, 1, (UBulletObject bulletObject) => {
				bulletObject.GetActor ().AddDefaultCallback (() => {
					bulletObject.GetSpriteRenderer ().color = Color.white;
				});
				bulletObject.GetActor ().AddCollisionCallback (() => {
					bulletObject.GetSpriteRenderer ().color = Color.red;
				});
				bulletObject.GetActor ().AddBoundaryCallback (() => {
					count--;
					txt.text = "Bullet: " + count;

					bulletObject.trans.DOKill ();
					bulletObject.Recycle ();
				});
				bulletObject.GetTransform ().DOMoveUp (2f);

				count++;
				txt.text = "Bullet: " + count;
			}))
			.AddStage (new UShootStageWait (1f))
			.AddStage (new UShootStageSameInterval (10000, 0.0001f, 0, (UBulletObject bulletObject) => {
				bulletObject.GetActor ().AddDefaultCallback (() => {
					bulletObject.GetSpriteRenderer ().color = Color.white;
				});
				bulletObject.GetActor ().AddCollisionCallback (() => {
					bulletObject.GetSpriteRenderer ().color = Color.red;
				});
				bulletObject.GetActor ().AddBoundaryCallback (() => {
					count--;
					txt.text = "Bullet: " + count;

					bulletObject.trans.DOKill ();
					bulletObject.Recycle ();
				});
				bulletObject.GetTransform ().DOMoveUp (2f);

				count++;
				txt.text = "Bullet: " + count;
			}))
			.SetLoopType (EShooterLoopType.BOUNCE);

		secondShooter.Shoot ();

		//secondShooter.GetTransform ().DOBlendableRotateBy (new Vector3 (0f, 0f, -720f), 5f, RotateMode.FastBeyond360);
		secondShooter.GetTransform().DORotateForever(-1f);
		*/

		bool isEven = false;

		foreach (UShooter shooter in shooters) {
			ConfigureShooter (shooter, isEven);
			isEven = !isEven;
		}


		//ConfigureShooter (shooters[0], true);

		//yield return new WaitForSeconds (3.2f);
		//Destroy (shooter.gameObject);
		yield break;
	}
}
