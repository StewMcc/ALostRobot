using UnityEngine;

/// <summary>
/// @StewMcc 9/10/2017
/// </summary>
namespace LittleLot {

	/// <summary>
	/// Singleton class that ensures there is only ever one of itself in the scene.
	/// The singleton wil delete itself if one already exists.
	/// </summary>
	public class Singleton<T> : MonoBehaviour where T : MonoBehaviour {

		private static T instance_;

		/// <summary>
		/// Ensures only one instance of the singleton exists at any time, will
		///  destroy itself if one already exists.
		/// Must be in awake to ensure it can delete duplicates at the correct time.
		/// </summary>
		private void Awake() {
			if (instance_ != null && instance_ != this) {
				Destroy(gameObject);
				Debug.Log("Destroying Duplicate Singleton -> " + typeof(T));
			} else {
				instance_ = GetComponent<T>();
			}
		}

		/// <summary>
		/// Ensures if this is the Instance it will null out the instance value.
		/// </summary>
		private void OnDestroy() {
			if (instance_ == this) {
				instance_ = null;
			}
		}

		/// <summary>
		/// Returns the single instance of this object.        
		/// </summary>        
		public static T Instance {
			get {
				if (instance_ == null) {
					instance_ = (T)FindObjectOfType(typeof(T));
				}
				if (instance_ == null) {
					Debug.Log("No Instance Available: " + typeof(T));
				}
				return instance_;
			}
		}

	}
}

