using UnityEngine;

/// <summary>
/// Sets the ship name when inside.
/// Will reset to world name on exit.
/// </summary>
[RequireComponent(typeof(Collider))]
public class HullController : MonoBehaviour {

	
	private void OnTriggerEnter(Collider other) {
		if (other.tag == "Player") {

			EventManager.NameChanged(EventManager.NameUpdateType.ShipName);			
		}
	}
	private void OnTriggerExit(Collider other) {
		if (other.tag == "Player") {

			EventManager.NameChanged(EventManager.NameUpdateType.WorldName);
		}
	}
}
