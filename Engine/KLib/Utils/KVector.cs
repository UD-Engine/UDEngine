using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UDEngine;
using UDEngine.Internal;
using UDEngine.Core;
using UDEngine.Enum;
using UDEngine.Interface;

namespace KSM {
	// Useful helper functions for distance
	public static class KVector2 {
		public static float Distance(Vector2 va, Vector2 vb) {
			float xDistance = vb.x - va.x;
			float yDistance = vb.y - va.y;

			return Mathf.Sqrt (xDistance * xDistance + yDistance * yDistance);
		}
	}

	public static class KVector3 {
		public static float Distance(Vector3 va, Vector3 vb) {
			float xDistance = vb.x - va.x;
			float yDistance = vb.y - va.y;
			float zDistance = vb.z - va.z;

			return Mathf.Sqrt (xDistance * xDistance + yDistance * yDistance + zDistance * zDistance);
		}

		public static float DistanceXY(Vector3 va, Vector3 vb) {
			float xDistance = vb.x - va.x;
			float yDistance = vb.y - va.y;

			return Mathf.Sqrt (xDistance * xDistance + yDistance * yDistance);
		}
	}
}
