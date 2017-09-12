using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UDEngine.Core.Actor;
using UDEngine.Core.Bullet;
using UDEngine.Core.Collision;
using UDEngine.Core.Pool;
using UDEngine.Core.Shooter;

namespace UDEngine.Plugin.Patterner {
	public static class UPatternGenerator {
		public static UShooter PGAttachShooter(this GameObject go) {
			UShooter shooter = go.AddComponent<UShooter> ();
			UShootActor shootActor = go.AddComponent<UShootActor> ();
			shooter.SetTransform (go.transform);
			shooter.SetActor (shootActor);
			shootActor.SetShooter (shooter);

			return shooter;
		}


		public static UShooter PGOneOrigin(this Transform trans, Vector3 relativePosition = default(Vector3), Quaternion relativeRotation = default(Quaternion)) {
			GameObject gun = new GameObject ("PG_Gun");
			Transform gunTrans = gun.transform;
			gunTrans.parent = trans;
			gunTrans.localPosition = relativePosition;
			gunTrans.localRotation = relativeRotation;

			return gun.PGAttachShooter ();
		}

		public static List<UShooter> PGOutwardPattern(this Transform trans, int amount, float angle_from, float angle_to, float dist_to_center) {
			//Get the angular interval between guns;
			float gun_interval = (angle_to - angle_from) / (amount - 1);

			float curr_angle = angle_from;
			Vector3 startVector = new Vector3 (0, dist_to_center, 0);
			startVector = Quaternion.Euler (0f, 0f, curr_angle) * startVector;

			List<UShooter> shooters = new List<UShooter> ();

			for (int i = 0; i < amount; i++) {
				GameObject one_gun = new GameObject ("PG_Gun " + i.ToString());
				Transform one_gun_trans = one_gun.transform;
				one_gun_trans.parent = trans;
				one_gun_trans.localPosition = startVector;
				one_gun_trans.rotation = Quaternion.Euler (new Vector3(0f, 0f, curr_angle));

				// Stick UShooter on it
				shooters.Add(one_gun.PGAttachShooter());

				//Rotate the Vector for later use
				startVector = Quaternion.Euler (0f, 0f, gun_interval) * startVector;
				curr_angle += gun_interval;
			}

			return shooters;
		}

		public static List<UShooter> PGWholeCircleOutwardPattern(this Transform trans, int amount, float dist_to_center) {
			//Get the angular interval between guns;
			float gun_interval = 360f / amount;

			float curr_angle = 0f + trans.rotation.eulerAngles.z;
			Vector3 startVector = new Vector3 (0, dist_to_center, 0);
			startVector = Quaternion.Euler (0f, 0f, curr_angle) * startVector;

			List<UShooter> shooters = new List<UShooter> ();

			for (int i = 0; i < amount; i++) {
				GameObject one_gun = new GameObject ("PG_Gun " + i.ToString());
				Transform one_gun_trans = one_gun.transform;

				one_gun_trans.parent = trans;
				one_gun_trans.localPosition = startVector;
				one_gun_trans.rotation = Quaternion.Euler (new Vector3(0f, 0f, curr_angle));

				// Stick UShooter on it
				shooters.Add(one_gun.PGAttachShooter());

				startVector = Quaternion.Euler (0f, 0f, gun_interval) * startVector;
				curr_angle += gun_interval;
			}

			return shooters;
		}

		public static List<UShooter> PGWholeCircleSameDirectionPattern(this Transform trans, int amount, float direction, float angle_from, float angle_to, float dist_to_center) {

			//Get the angular interval between guns;
			float gun_interval = (angle_to - angle_from) / (amount - 1);

			float curr_angle = angle_from;
			Vector3 startVector = new Vector3 (0, dist_to_center, 0);
			startVector = Quaternion.Euler (0f, 0f, curr_angle) * startVector;

			List<UShooter> shooters = new List<UShooter> ();

			for (int i = 0; i < amount; i++) {
				GameObject one_gun = new GameObject ("PG_Gun " + i.ToString());
				Transform one_gun_trans = one_gun.transform;

				one_gun_trans.parent = trans;
				one_gun_trans.localPosition = startVector;

				// Stick UShooter on it
				shooters.Add(one_gun.PGAttachShooter());

				one_gun.transform.rotation = Quaternion.Euler (new Vector3(0f, 0f, direction));
				startVector = Quaternion.Euler (0f, 0f, gun_interval) * startVector;
				curr_angle += gun_interval;
			}

			return shooters;
		}
	}
}
