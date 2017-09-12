using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UDEngine.Internal;

using DG.Tweening;

namespace UDEngine.Core.Tween {
	[System.Serializable]
	public class UDOTweenPlug {
		#region CONSTRUCTOR
		public UDOTweenPlug() {
			doTweenTweeners = new List<Tweener> ();
			doTweenSequences = new List<Sequence> ();
		}
		#endregion

		#region PROP
		// This is used to track all active tween sequences, so that they could be cleanly killed
		public List<Tweener> doTweenTweeners;
		public List<Sequence> doTweenSequences; // This should be lazily initialized.
		#endregion

		#region METHOD
		public UDOTweenPlug AddTweener(Tweener tweener) {
			/*
			if (doTweenTweeners == null) {
				doTweenTweeners = new List<Tweener> ();
			}
			*/
			doTweenTweeners.Add (tweener);

			return this;
		}

		public Tweener GetTweenerAt(int index) {
			/*
			if (doTweenTweeners == null) {
				doTweenTweeners = new List<Tweener> ();
			}
			*/
			if (index >= doTweenTweeners.Count || index < 0) {
				UDebug.Error ("cannot find DOTween Tweener of the given index");
				return null;
			} else {
				return doTweenTweeners [index];
			}
		}

		public UDOTweenPlug KillTweenerAt(int index) {
			/*
			if (doTweenTweeners == null) {
				doTweenTweeners = new List<Tweener> ();
			}
			*/
			if (index >= doTweenTweeners.Count || index < 0) {
				UDebug.Error ("cannot find DOTween Tweener of the given index, kill fails");
			} else {
				doTweenTweeners [index].Kill ();
				doTweenTweeners.RemoveAt (index); // Removing tweener reference. SLOW!!! But this is RARE, so it should be okay
			}
			return this;
		}

		public UDOTweenPlug KillAllTweeners() {
			/*
			if (doTweenTweeners == null) {
				doTweenTweeners = new List<Tweener> ();
			}
			*/
			if (doTweenTweeners != null) { // not null, else do NOTHING
				foreach (Tweener tweener in doTweenTweeners) {
					tweener.Kill ();
				}
			}
			doTweenTweeners = new List<Tweener> (); // Cleanup

			return this;
		}


		public UDOTweenPlug AddSequence(Sequence seq) {
			/*
			if (doTweenSequences == null) {
				doTweenSequences = new List<Sequence> ();
			}
			*/
			doTweenSequences.Add (seq);

			return this;
		}
		public Sequence GetSequenceAt(int index) {
			/*
			if (doTweenSequences == null) {
				doTweenSequences = new List<Sequence> ();
			}
			*/
			if (index >= doTweenSequences.Count || index < 0) {
				UDebug.Error ("cannot find DOTween Sequence of the given index");
				return null;
			} else {
				return doTweenSequences [index];
			}
		}
		public UDOTweenPlug KillSequenceAt(int index) {
			/*
			if (doTweenSequences == null) {
				doTweenSequences = new List<Sequence> ();
			}
			*/
			if (doTweenSequences == null) {
				doTweenSequences = new List<Sequence> ();
			}
			if (index >= doTweenSequences.Count || index < 0) {
				UDebug.Error ("cannot find DOTween Sequence of the given index, kill fails");
			} else {
				doTweenSequences [index].Kill ();
				doTweenSequences.RemoveAt (index); // Removing seq reference. SLOW!!! But this is RARE, so it should be okay
			}

			return this;
		}
		public UDOTweenPlug KillAllSequences() {
			if (doTweenSequences != null) { // not null, else do NOTHING
				foreach (Sequence seq in doTweenSequences) {
					seq.Kill ();
				}
			}
			doTweenSequences = new List<Sequence> (); // Cleanup

			return this;
		}


		public UDOTweenPlug KillAll() {
			KillAllTweeners ();
			KillAllSequences ();
			return this;
		}
		#endregion
	}
}
