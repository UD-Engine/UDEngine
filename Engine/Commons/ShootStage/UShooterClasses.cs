using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

using UDEngine;
using UDEngine.Core;
using UDEngine.Core.Actor;
using UDEngine.Core.Bullet;
using UDEngine.Core.Collision;
using UDEngine.Core.Pool;
using UDEngine.Core.Shooter;
using UDEngine.Interface;
using UDEngine.Internal;

using DG.Tweening;

namespace UDEngine.Commons.ShootStage {

	/// <summary>
	/// Pause the execution of stages for a while
	/// </summary>
	public class UShootStageWait : IShootStage {
		public UShootStageWait(float pauseTime) {
			_pauseTime = pauseTime;
			_isComplete = false;
		}

		private float _pauseTime;
		private bool _isComplete = false;

		public IShootStage SetShooter(UShooter shooter) {
			// does nothing, as it never shoots
			return this;
		}

		public IEnumerator StartStage () {
			yield return new WaitForSeconds (_pauseTime);
			_isComplete = true;
			yield break;
		}

		public bool IsComplete() {
			return this._isComplete;
		}

		public IShootStage SetComplete(bool condition) {
			_isComplete = condition;
			return this;
		}
	}

	/// <summary>
	/// Pause and wait for predicate to be true
	/// </summary>
	public class UShootStageWaitFor : IShootStage {
		public UShootStageWaitFor(System.Func<bool> predicate) {
			this._conditionPredicate = predicate;
		}

		private System.Func<bool> _conditionPredicate;

		public IShootStage SetShooter(UShooter shooter) {
			// does nothing, as it never shoots
			return this;
		}

		public IEnumerator StartStage () {
			// does nothing, as we are not starting anything
			yield break;
		}

		public bool IsComplete() {
			if (_conditionPredicate == null) {
				return true;
			}
			return _conditionPredicate ();
		}

		public IShootStage SetComplete(bool condition) {
			UDebug.Warning("UShootStageWaitFor.SetComplete() is called, you may be doing something undesirable. UShootStageWaitFor() is NOT compatible with loop types (since you fed it a predicate)");
			return this;
		}
	}


	public class UShootStageSameInterval : IShootStage {
		public UShootStageSameInterval(int count, float interval, int bulletIndex, UnityAction<UBulletObject> bulletInitAction, bool shouldBulletAddToMonitor = true, bool shouldBulletActivateCollider = true) {
			if (count < 0) {
				UDebug.Warning ("shoot count is negative, set to 0 instead");
				_count = 0; // Set to 0 in case of trouble
			} else {
				_count = count;
			}
			if (interval < 0f) {
				UDebug.Warning ("shoot interval is negative, set to 0 instead");
				_interval = 0f;
			} else {
				_interval = interval;
			}
			if (bulletIndex < 0) {
				UDebug.Warning ("shoot bullet index is negative, nothing will be shot");
				_bulletIndex = -1;
			} else {
				_bulletIndex = bulletIndex;
			}

			if (bulletInitAction != null) {
				_bulletInitEvent = new UShootEvent ();
				_bulletInitEvent.AddListener (bulletInitAction);
			}

			_shouldBulletAddToMonitor = shouldBulletAddToMonitor;
			_shouldBulletActivateCollider = shouldBulletActivateCollider;

			_isComplete = false;
		}

		private int _count;
		private float _interval;
		private int _bulletIndex;
		private UShooter _shooter;
		private bool _isComplete = false;

		private bool _shouldBulletAddToMonitor;
		private bool _shouldBulletActivateCollider;

		private UShootEvent _bulletInitEvent;

		public IShootStage SetShooter(UShooter shooter) {
			// In this case, the shooter is VERY important!!!
			_shooter = shooter;
			return this;
		}

		public IEnumerator StartStage() {
			if (_shooter != null) {
				UBulletPoolManager poolManager = _shooter.GetBulletPoolManager ();
				// Check if the bulletIndex is in valid range (bullet exists)
				if (_bulletIndex >= 0 && _bulletIndex < poolManager.GetPrototypesCount()) {
					for (int i = 0; i < _count; i++) {
						UBulletObject fetchedBullet = poolManager.FetchBulletByID (_bulletIndex, true, _shouldBulletAddToMonitor, _shouldBulletActivateCollider);
						// rotate fetched bullet to have the same rotation as the shooter. 
						// This is meaningful as we want bullets to shoot at the same UP direction of the shooter
						Transform fetchedBulletTrans = fetchedBullet.GetTransform ();
						Transform shooterTrans = _shooter.GetTransform ();
						fetchedBulletTrans.position = shooterTrans.position;
						fetchedBulletTrans.rotation = shooterTrans.rotation;
						// More motion settings on the bullet itself
						_bulletInitEvent.Invoke (fetchedBullet);

						// Invoke shootEvent here
						_shooter.GetActor().InvokeShootEvents(fetchedBullet);

						// all non-last shots should wait for the specified interval
						if (i < _count - 1) {
							yield return new WaitForSeconds (_interval);
						}
					}
				}
			}
			_isComplete = true;
			yield break;
		}

		public bool IsComplete() {
			return _isComplete;
		}

		public IShootStage SetComplete(bool condition) {
			this._isComplete = condition;
			return this;
		}
	}

	public class UShootStageSameIntervalInfinite : IShootStage {
		public UShootStageSameIntervalInfinite(float interval, int bulletIndex, UnityAction<UBulletObject> bulletInitAction, bool shouldBulletAddToMonitor = true, bool shouldBulletActivateCollider = true) {
			if (interval < 0f) {
				UDebug.Warning ("shoot interval is negative, set to 0 instead");
				_interval = 0f;
			} else {
				_interval = interval;
			}
			if (bulletIndex < 0) {
				UDebug.Warning ("shoot bullet index is negative, nothing will be shot");
				_bulletIndex = -1;
			} else {
				_bulletIndex = bulletIndex;
			}

			if (bulletInitAction != null) {
				_bulletInitEvent = new UShootEvent ();
				_bulletInitEvent.AddListener (bulletInitAction);
			}

			_shouldBulletAddToMonitor = shouldBulletAddToMonitor;
			_shouldBulletActivateCollider = shouldBulletActivateCollider;

			_isForceStopped = false;
		}

		private float _interval;
		private int _bulletIndex;
		private UShooter _shooter;

		private bool _isForceStopped = false; // when this is true, the infinite call is forcefully stopped

		private bool _shouldBulletAddToMonitor;
		private bool _shouldBulletActivateCollider;

		private UShootEvent _bulletInitEvent;

		public IShootStage SetShooter(UShooter shooter) {
			// In this case, the shooter is VERY important!!!
			_shooter = shooter;
			return this;
		}

		public IEnumerator StartStage() {
			if (_shooter != null) {
				UBulletPoolManager poolManager = _shooter.GetBulletPoolManager ();
				// Check if the bulletIndex is in valid range (bullet exists)
				if (_bulletIndex >= 0 && _bulletIndex < poolManager.GetPrototypesCount()) {
					// Forever and infinite
					while (true) {
						// when this is true, the infinite call is forcefully stopped
						if (_isForceStopped) {
							break;
						}

						UBulletObject fetchedBullet = poolManager.FetchBulletByID (_bulletIndex, true, _shouldBulletAddToMonitor, _shouldBulletActivateCollider);
						// rotate fetched bullet to have the same rotation as the shooter. 
						// This is meaningful as we want bullets to shoot at the same UP direction of the shooter
						Transform fetchedBulletTrans = fetchedBullet.GetTransform ();
						Transform shooterTrans = _shooter.GetTransform ();
						fetchedBulletTrans.position = shooterTrans.position;
						fetchedBulletTrans.rotation = shooterTrans.rotation;
						// More motion settings on the bullet itself
						_bulletInitEvent.Invoke (fetchedBullet);

						// Invoke shootEvent here
						_shooter.GetActor().InvokeShootEvents(fetchedBullet);

						yield return new WaitForSeconds (_interval);
					}
				}
			}
			yield break;
		}

		public bool IsComplete() {
			return _isForceStopped;
		}

		// This can be used to forcefully stop the infinite state.
		public IShootStage SetComplete(bool condition) {
			this._isForceStopped = condition;
			return this;
		}
	}

	// Use function to determine the interval between this and the next shooting. May require user to write its own state machine
	public class UShootStageFuncInterval : IShootStage {
		public UShootStageFuncInterval(int count, System.Func<float> intervalFunc, int bulletIndex, UnityAction<UBulletObject> bulletInitAction, bool shouldBulletAddToMonitor = true, bool shouldBulletActivateCollider = true) {
			if (count < 0) {
				UDebug.Warning ("shoot count is negative, set to 0 instead");
				_count = 0; // Set to 0 in case of trouble
			} else {
				_count = count;
			}

			_intervalFunc = intervalFunc;

			if (bulletIndex < 0) {
				UDebug.Warning ("shoot bullet index is negative, nothing will be shot");
				_bulletIndex = -1;
			} else {
				_bulletIndex = bulletIndex;
			}

			if (bulletInitAction != null) {
				_bulletInitEvent = new UShootEvent ();
				_bulletInitEvent.AddListener (bulletInitAction);
			}

			_shouldBulletAddToMonitor = shouldBulletAddToMonitor;
			_shouldBulletActivateCollider = shouldBulletActivateCollider;

			_isComplete = false;
		}

		private int _count;
		private System.Func<float> _intervalFunc;
		private int _bulletIndex;
		private UShooter _shooter;
		private bool _isComplete = false;

		private bool _shouldBulletAddToMonitor;
		private bool _shouldBulletActivateCollider;

		private UShootEvent _bulletInitEvent;

		public IShootStage SetShooter(UShooter shooter) {
			// In this case, the shooter is VERY important!!!
			_shooter = shooter;
			return this;
		}

		public IEnumerator StartStage() {
			if (_shooter != null) {
				UBulletPoolManager poolManager = _shooter.GetBulletPoolManager ();
				// Check if the bulletIndex is in valid range (bullet exists)
				if (_bulletIndex >= 0 && _bulletIndex < poolManager.GetPrototypesCount()) {
					for (int i = 0; i < _count; i++) {
						UBulletObject fetchedBullet = poolManager.FetchBulletByID (_bulletIndex, true, _shouldBulletAddToMonitor, _shouldBulletActivateCollider);
						// rotate fetched bullet to have the same rotation as the shooter. 
						// This is meaningful as we want bullets to shoot at the same UP direction of the shooter
						Transform fetchedBulletTrans = fetchedBullet.GetTransform ();
						Transform shooterTrans = _shooter.GetTransform ();
						fetchedBulletTrans.position = shooterTrans.position;
						fetchedBulletTrans.rotation = shooterTrans.rotation;
						// More motion settings on the bullet itself
						_bulletInitEvent.Invoke (fetchedBullet);

						// Invoke shootEvent here
						_shooter.GetActor().InvokeShootEvents(fetchedBullet);

						// all non-last shots should wait for the specified interval
						if (i < _count - 1) {
							yield return new WaitForSeconds (_intervalFunc());
						}
					}
				}
			}
			_isComplete = true;
			yield break;
		}

		public bool IsComplete() {
			return _isComplete;
		}

		public IShootStage SetComplete(bool condition) {
			this._isComplete = condition;
			return this;
		}
	}

	public class UShootStageFuncIntervalInfinite : IShootStage {
		public UShootStageFuncIntervalInfinite(System.Func<float> intervalFunc, int bulletIndex, UnityAction<UBulletObject> bulletInitAction, bool shouldBulletAddToMonitor = true, bool shouldBulletActivateCollider = true) {
			_intervalFunc = intervalFunc;

			if (bulletIndex < 0) {
				UDebug.Warning ("shoot bullet index is negative, nothing will be shot");
				_bulletIndex = -1;
			} else {
				_bulletIndex = bulletIndex;
			}

			if (bulletInitAction != null) {
				_bulletInitEvent = new UShootEvent ();
				_bulletInitEvent.AddListener (bulletInitAction);
			}

			_shouldBulletAddToMonitor = shouldBulletAddToMonitor;
			_shouldBulletActivateCollider = shouldBulletActivateCollider;

			_isForceStopped = false;
		}

		private System.Func<float> _intervalFunc;
		private int _bulletIndex;
		private UShooter _shooter;

		private bool _isForceStopped = false; // when this is true, the infinite call is forcefully stopped

		private bool _shouldBulletAddToMonitor;
		private bool _shouldBulletActivateCollider;

		private UShootEvent _bulletInitEvent;

		public IShootStage SetShooter(UShooter shooter) {
			// In this case, the shooter is VERY important!!!
			_shooter = shooter;
			return this;
		}

		public IEnumerator StartStage() {
			if (_shooter != null) {
				UBulletPoolManager poolManager = _shooter.GetBulletPoolManager ();
				// Check if the bulletIndex is in valid range (bullet exists)
				if (_bulletIndex >= 0 && _bulletIndex < poolManager.GetPrototypesCount()) {
					// Forever and infinite
					while (true) {
						// when this is true, the infinite call is forcefully stopped
						if (_isForceStopped) {
							break;
						}

						UBulletObject fetchedBullet = poolManager.FetchBulletByID (_bulletIndex, true, _shouldBulletAddToMonitor, _shouldBulletActivateCollider);
						// rotate fetched bullet to have the same rotation as the shooter. 
						// This is meaningful as we want bullets to shoot at the same UP direction of the shooter
						Transform fetchedBulletTrans = fetchedBullet.GetTransform ();
						Transform shooterTrans = _shooter.GetTransform ();
						fetchedBulletTrans.position = shooterTrans.position;
						fetchedBulletTrans.rotation = shooterTrans.rotation;
						// More motion settings on the bullet itself
						_bulletInitEvent.Invoke (fetchedBullet);

						// Invoke shootEvent here
						_shooter.GetActor().InvokeShootEvents(fetchedBullet);

						yield return new WaitForSeconds (_intervalFunc());
					}
				}
			}
			yield break;
		}

		public bool IsComplete() {
			return _isForceStopped;
		}

		// This can be used to forcefully stop the infinite state.
		public IShootStage SetComplete(bool condition) {
			this._isForceStopped = condition;
			return this;
		}
	}
}
