using UnityEngine;

public class HullController : MonoBehaviour {

	[SerializeField]
	GameObject dropButton = null;
	
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
