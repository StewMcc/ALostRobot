using UnityEngine;

/// <summary>
/// Singleton class that ensures there is only ever one of itself in the scene.
/// The singleton wil delete itself if one already exists.
/// </summary>
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour {

	private static T instance_;

	/// <summary>
	/// Ensures only one instance of the singleton exists at any time,
	///  will destroy itself if one already exists.
	/// </summary>
	private void Awake() {
		if (instance_ != null && instance_ != this) {
			Destroy(gameObject);
			Debug.Log("Destroying Duplicate Singleton");
		}
		else {
			instance_ = GetComponent<T>();
		}
	}

	/// <summary>
	/// Returns the single instance of this object.		
	/// </summary>		
	public static T instance {
		get {
			if (instance_ == null) {
				instance_ = (T)FindObjectOfType(typeof(T));
			}
			return instance_;
		}
	}

}

