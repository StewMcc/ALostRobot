using UnityEngine;

/// <summary>
/// @StewMcc 2/09/2017
/// </summary>
namespace LittleLot {

	/// <summary>
	/// Forces the object to not be destroyed when a new scene loads.
	/// </summary>
	public class DontDestroyOnLoad : MonoBehaviour {

		/// <summary>
		/// Utilises DontDestroyOnLoad, to stop the object being destroyed.
		/// </summary>
		void Start() {
			DontDestroyOnLoad(this);
		}
	}
}
