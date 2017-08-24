using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UDEngine;
using UDEngine.Interface;

namespace UDEngine.Components {
	/// <summary>
	/// Execute MonoBehaviour like classes (which may not be allowed to be MonoBehaviour due to "new" instantiation)
	/// </summary>
	public class UMonolikeExecutor : MonoBehaviour {

		// PROP begin
		/// <summary>
		/// A list of all Monolike objects
		/// </summary>
		private List<IMonolike> _monolikes;
		// PROP end

		// METHOD begin
		// Use this for initialization
		void Start () {
			_monolikes = new List<IMonolike> ();
		}

		// Update is called once per frame
		void Update () {
			// Monolike objects have their UpdateFunc()s called here
			// as if they are derived from MonoBehaviour

			if (_monolikes == null) { // Fix weird reload bug
				_monolikes = new List<IMonolike> ();
			}

			foreach (IMonolike module in _monolikes) {
				module.UpdateFunc ();
			}
		}

		/// <summary>
		/// Adding a Monolike module to Executor (and invoke its .StartFunc)
		/// </summary>
		/// <param name="module">Module.</param>
		public void AddModule(IMonolike module) {
			_monolikes.Add (module);
			// StartFunc is called here, as whenever we add the Monolike object, 
			// the addition itself could be seen as initialization (Start() in Monobehaviour)
			module.StartFunc ();
		}

		/// <summary>
		/// Remove a Monolike module so its UpdateFunc would no longer be called
		/// </summary>
		/// <param name="module">Module.</param>
		public void RemoveModule(IMonolike module) {
			_monolikes.Remove (module);
		}
		// METHOD end
	}
}
