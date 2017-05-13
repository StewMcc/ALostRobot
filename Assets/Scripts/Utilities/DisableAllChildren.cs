using UnityEngine;

/// <summary>
/// Simple Utility class that disables all child game objects on start.
/// </summary>
public class DisableAllChildren : MonoBehaviour {

	/// <summary>
	/// Disables all child game objects.
	/// </summary>
	void Start() {
		foreach (Transform child in transform) {
			child.gameObject.SetActive(false);
		}
	}
}
