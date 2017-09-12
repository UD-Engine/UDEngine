using UnityEngine;
using System.Collections;

/// <summary>
/// UDEngine.Internal.UDebug
/// Used for Debug Message Logging (mainly for development)
/// </summary>
namespace UDEngine.Internal {
	public static class UDebug {
		/// <summary>
		/// suppressed condition of debug logging
		/// </summary>
		public static bool _isSuppressed = false;
		/// <summary>
		/// Log and Warning Suppression (to distinguish from development and production)
		/// </summary>
		/// <param name="condition">If set to <c>true</c> condition.</param>
		public static void Suppress(bool condition = true) {
			_isSuppressed = condition;
		}

		/* Suppressable Messages */

		/// <summary>
		/// Log the specified debug message
		/// </summary>
		/// <param name="msg">Message.</param>
		public static void Log(string msg) {
			if (_isSuppressed == false) {
				Debug.Log ("UDEngine Log: " + msg);
			}
		}
		/// <summary>
		/// Warning-styled log of the the debug message
		/// </summary>
		/// <param name="msg">Message.</param>
		public static void Warning(string msg) {
			if (_isSuppressed == false) {
				Debug.LogWarning ("UDEngine Warning: " + msg);
			}
		}


		/* NON-Suppressable Messages */
		/// <summary>
		/// Log Errors with specified message
		/// </summary>
		/// <param name="msg">Message.</param>
		public static void Error(string msg) {
			Debug.LogError ("UDEngine Error: " + msg);
		}
	}
}
