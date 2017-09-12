using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UDEngine.Internal;

namespace UDEngine.Core.Tween {
	[System.Serializable]
	public class ULeanTweenPlug {
		#region CONSTRUCTOR
		public ULeanTweenPlug() {
			leanTweenIDs = new List<int> ();
			leanTweenSequenceIDs = new List<int> ();
		}
		#endregion

		#region PROP
		public List<int> leanTweenSequenceIDs; // Testing for LeanTween
		public List<int> leanTweenIDs; // Testing for LeanTween
		#endregion

		#region METHOD
		public ULeanTweenPlug AddTweenID(int tweenID) {
			/*
			if (leanTweenIDs == null) {
				leanTweenIDs = new List<int> ();
			}
			*/
			leanTweenIDs.Add (tweenID);

			return this;
		}
		public int GetTweenIDAt(int index) {
			/*
			if (leanTweenIDs == null) {
				leanTweenIDs = new List<int> ();
			}
			*/
			if (index >= leanTweenIDs.Count || index < 0) {
				UDebug.Error ("cannot find tween sequence of the given index");
			}
			return leanTweenIDs [index];
		}
		public ULeanTweenPlug KillTweenAt(int index) {
			/*
			if (leanTweenIDs == null) {
				leanTweenIDs = new List<int> ();
			}
			*/
			if (index >= leanTweenIDs.Count || index < 0) {
				UDebug.Error ("cannot find tween sequence of the given index, kill fails");
			} else {
				LeanTween.cancel(leanTweenIDs [index]);
				leanTweenIDs.RemoveAt (index); // Removing tween reference. SLOW!!! But this is RARE, so it should be okay
			}

			return this;
		}
		public ULeanTweenPlug KillAllTweens() {
			/*
			if (leanTweenIDs == null) {
				leanTweenIDs = new List<int> ();
			}
			*/
			if (leanTweenIDs != null) { // not null, else do NOTHING
				foreach (int tweenID in leanTweenIDs) {
					while (LeanTween.isTweening (tweenID)) { // BUGFIX: Safety check... THIS IS NOT WORKING!!!
						LeanTween.cancel (tweenID);
					}
				}
			}

			// BUGFIX: resetting leanTweenIDs after this;
			leanTweenIDs = new List<int>();

			return this;
		}


		public ULeanTweenPlug AddSequenceID(int seqID) {
			/*
			if (leanTweenSequenceIDs == null) {
				leanTweenSequenceIDs = new List<int> ();
			}
			*/
			leanTweenSequenceIDs.Add (seqID);

			return this;
		}
		public int GetSequenceIDAt(int index) {
			/*
			if (leanTweenSequenceIDs == null) {
				leanTweenSequenceIDs = new List<int> ();
			}
			*/
			if (index >= leanTweenSequenceIDs.Count || index < 0) {
				UDebug.Error ("cannot find tween sequence of the given index");
			}
			return leanTweenSequenceIDs [index];
		}
		public ULeanTweenPlug KillSequenceAt(int index) {
			/*
			if (leanTweenSequenceIDs == null) {
				leanTweenSequenceIDs = new List<int> ();
			}
			*/
			if (index >= leanTweenSequenceIDs.Count || index < 0) {
				UDebug.Error ("cannot find tween sequence of the given index, kill fails");
			} else {
				LeanTween.cancel(leanTweenSequenceIDs [index]);
				leanTweenSequenceIDs.RemoveAt (index); // Removing seq reference. SLOW!!! But this is RARE, so it should be okay
			}

			return this;
		}
		public ULeanTweenPlug KillAllSequences() {
			if (leanTweenSequenceIDs != null) { // not null, else do NOTHING
				foreach (int seqID in leanTweenSequenceIDs) {
					LeanTween.cancel (seqID);
				}
			}

			// BUGFIX: resetting leanTweenSequenceIDs after this;
			leanTweenSequenceIDs = new List<int>();

			return this;
		}

		public ULeanTweenPlug KillAll() {
			KillAllTweens ();
			KillAllSequences ();

			return this;
		}
		#endregion
	}
}
