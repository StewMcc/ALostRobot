using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Collider))]
public class ServicePort : MonoBehaviour {

	[SerializeField]
	GameObject useIcon = null;

	[SerializeField]
	Button useButton = null;

	private Room connectedRoom = null;
	private RobotController robot = null;

	private void OnEnable() {
		useButton.onClick.AddListener(UseItem);
	}

	private void OnDisable() {
		useButton.onClick.RemoveListener(UseItem);
	}

	private void Start() {
		connectedRoom = GetComponentInParent<Room>();
		useIcon.SetActive(false);
	}

	private void OnTriggerEnter(Collider other) {
		if (other.tag == "Player" && !connectedRoom.IsFixed()) {
			robot = other.GetComponent<RobotController>();
			if (robot) {
				if (robot.HasItem()) {
					ShowUseIcon();
				}
			}
		}
	}

	private void OnTriggerExit(Collider other) {
		if (other.tag == "Player") {
			HideUseIcon();
			robot = null;
		}
	}

	public void ShowUseIcon() {
		useIcon.SetActive(true);
		SoundManager.PlayEvent("Item_PopUp", gameObject);
	}

	public void HideUseIcon() {
		if (useIcon.activeInHierarchy) {
			useIcon.SetActive(false);
			SoundManager.PlayEvent("Item_PopUp", gameObject);
		}
	}

	private void UseItem() {
		if (robot) {
			connectedRoom.TryFixRoom(robot.CurrentItem());
			robot.RespawnCurrentItem();
		}
		HideUseIcon();
	}
}
