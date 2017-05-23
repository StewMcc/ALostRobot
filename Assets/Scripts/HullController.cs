using UnityEngine;

/// <summary>
/// Hides the drop button when inside the ship, and sets the ship name when inside.
/// Will reset to world name on exit.
/// </summary>
[RequireComponent(typeof(Collider))]
public class HullController : MonoBehaviour {

	[SerializeField]
	private GameObject dropButton = null;

	private void OnTriggerEnter(Collider other) {
		if (other.tag == "Player") {

			EventManager.NameChanged(EventManager.NameUpdateType.ShipName);
			dropButton.SetActive(false);
		}
	}
	private void OnTriggerExit(Collider other) {
		if (other.tag == "Player") {

			dropButton.SetActive(true);
			EventManager.NameChanged(EventManager.NameUpdateType.WorldName);
		}
	}
}
