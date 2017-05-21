using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Collider))]
public class ServicePort : MonoBehaviour {

	[SerializeField]
	private GameObject useIcon = null;

	[SerializeField]
	private Button useButton = null;

	private Room connectedRoom_ = null;
	private RobotController robot_ = null;

	private void OnEnable() {
		useButton.onClick.AddListener(UseItem);
	}

	private void OnDisable() {
		useButton.onClick.RemoveListener(UseItem);
	}

	private void Start() {
		connectedRoom_ = GetComponentInParent<Room>();
		useIcon.SetActive(false);
	}

	private void OnTriggerEnter(Collider other) {
		if (other.tag == "Player" && !connectedRoom_.IsFixed()) {
			robot_ = other.GetComponent<RobotController>();
			if (robot_) {
				if (robot_.HasItem()) {
					ShowUseIcon();
				}
			}
		}
	}

	private void OnTriggerExit(Collider other) {
		if (other.tag == "Player") {
			HideUseIcon();
			robot_ = null;
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
		if (robot_) {
			connectedRoom_.TryFix(robot_.CurrentItem());
			robot_.RespawnCurrentItem();
		}
		HideUseIcon();
	}
}
