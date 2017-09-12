using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UDEngine;

namespace UDEngine.Interface {
	/// <summary>
	/// Interface for Monolike objects, objects that need especially Update() 
	/// but do their initialization through constructor (so they cannot derive from MonoBehaviour)
	/// It is used to define StartFunc() and UpdateFunc()
	/// to simulate Start() and Update() of Monobehaviour derivitives
	/// Somehow it is more like a HACK
	/// </summary>
	public interface IMonolike {
		void StartFunc ();
		void UpdateFunc();
	}
}
