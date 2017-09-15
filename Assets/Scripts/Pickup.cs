using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Controls the UI for the pickups and adds the pickup to the collided <seealso cref="RobotController"/>.
/// 
/// Best to duplicate one of the PickUp prefabs for all the connected UI etc.
/// Will show the Grabicon with its Canvas/Button, when a robot is within the collider.
/// Also allows the Pickup to be respawned and dropped, but would require it to also be removed from the robots inventory.
/// </summary>
[RequireComponent(typeof(Collider))]
public class Pickup : MonoBehaviour {

	public enum PickupType {
		tryytium, skudian, nusluathil, criasium, ublyx, zushese, vufrum, eshian, kreasten,
		qostralt
	};

	[SerializeField]
	private IdleAnimation pickupAnimation = null;

	[SerializeField]
	private Text pickupText = null;

	[SerializeField]
	private GameObject grabIcon = null;

	[SerializeField]
	private Button grabButton = null;

	[SerializeField]
	private ParticleSystem dropEffect = null;

	[Header("Pickup Settings")]
	[SerializeField]
	private PickupType pickupType = PickupType.tryytium;

	[SerializeField]
	private string pickupName = "DefaultName";

	private Vector3 respawnPosition_ = Vector3.zero;

	private Quaternion initialRotation_;

	private Vector3 initialScale_ = Vector3.zero;

	private Vector3 droppedTargetPosition_ = Vector3.zero;

	private bool hasRespawned_ = false;

	private bool isPickedUp_ = false;

	private RobotController robot_ = null;

	private void OnEnable() {
		grabButton.onClick.AddListener(GrabPickup);
	}

	private void OnDisable() {
		grabButton.onClick.RemoveListener(GrabPickup);
	}

	private void Start() {
		// save original position.
		respawnPosition_ = transform.position;
		initialRotation_ = transform.rotation;
		initialScale_ = transform.localScale;
		pickupText.text = pickupName;

		pickupText.enabled = false;

		grabIcon.SetActive(false);

	}

#if UNITY_EDITOR
	/// <summary>
	/// Draws Gizmo of item name in editor.
	/// </summary>
	void OnValidate() {
		if (pickupText) {
			pickupText.text = pickupName;
			name = pickupName;
		}
	}
#endif

	private void OnTriggerEnter(Collider other) {
		if (other.tag == "Player" && !isPickedUp_) {
			robot_ = other.GetComponent<RobotController>();
			if (robot_) {
				ShowGrabIcon();
			}
		}
	}

	private void OnTriggerExit(Collider other) {
		if (other.tag == "Player" && !isPickedUp_) {
			HideGrabIcon();
			robot_ = null;
		}
	}

	public string Name() {
		return pickupName;
	}

	public PickupType CheckType() {
		return pickupType;
	}

	public bool HasNameDiscovered() {
		return hasRespawned_;
	}

	public void PackForTeleport(Transform animationAnchor) {
		transform.parent = animationAnchor;
	}

	public void UnpackFromTeleport() {
		transform.parent = null;
	}

	/// <summary>
	/// Drops the item at a random position near the player on the NavMesh.	
	/// </summary>
	public void Drop() {
		// drop it if it can find a valid point on the navmesh
		if (NavMeshUtil.RandomPointOnNavMesh(transform.position, 2, out droppedTargetPosition_)) {

			transform.SetPositionAndRotation(droppedTargetPosition_, initialRotation_);

			pickupAnimation.ResumeAnimation();
			dropEffect.Play();
			SoundManager.PlayEvent("Item_PutDown", gameObject);
			ShowGrabIcon();
			isPickedUp_ = false;
		}
		else {
			SoundManager.PlayEvent("Item_Port_Negative", gameObject);
		}
		transform.parent = null;
		transform.localScale = initialScale_;
	}

	/// <summary>
	/// Respawns the item at its initial position.
	/// </summary>
	public void Respawn() {
		transform.SetPositionAndRotation(respawnPosition_, initialRotation_);

		pickupAnimation.ResumeAnimation();
		hasRespawned_ = true;
		isPickedUp_ = false;
		dropEffect.Play();
		transform.parent = null;
		transform.localScale = initialScale_;
	}

	private void ShowGrabIcon() {
		if (hasRespawned_) {
			pickupText.enabled = true;
		}
		grabIcon.SetActive(true);
		SoundManager.PlayEvent("Item_PopUp", gameObject);
	}

	private void HideGrabIcon() {
		grabIcon.SetActive(false);
		SoundManager.PlayEvent("Item_PopUp", gameObject);
	}

	private void GrabPickup() {
		pickupAnimation.PauseAnimation();
		HideGrabIcon();
		isPickedUp_ = true;
		if (robot_) {
			robot_.AddPickup(this);
		}
	}

}
