using UnityEngine;
using UnityEngine.UI;

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

	[SerializeField]
	GameNotificationData notification = new GameNotificationData();

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

	public GameNotificationData GameNotificationData() {
		return notification;
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
		if (RandomPointOnNavMesh(transform.position, 2, out droppedTargetPosition_)) {

			transform.SetPositionAndRotation(droppedTargetPosition_, initialRotation_);

			pickupAnimation.ResumeAnimation();
			dropEffect.Play();
			SoundManager.PlayEvent(AKID.EVENTS.ITEM_PUTDOWN, gameObject);
			ShowGrabIcon();
			isPickedUp_ = false;
			transform.parent = null;
			transform.localScale = initialScale_;
		} else {
			SoundManager.PlayEvent(AKID.EVENTS.ITEM_PORT_NEGATIVE, gameObject);
		}
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
		SoundManager.PlayEvent(AKID.EVENTS.ITEM_POPUP, gameObject);
	}

	private void HideGrabIcon() {
		grabIcon.SetActive(false);
		SoundManager.PlayEvent(AKID.EVENTS.ITEM_POPUP, gameObject);
	}

	private void GrabPickup() {
		pickupAnimation.PauseAnimation();
		HideGrabIcon();
		isPickedUp_ = true;
		if (robot_) {
			robot_.AddPickup(this);
		}
	}

	/// <summary>
	/// Generates a random point on the navmesh within the radius around the centre.
	/// </summary>
	/// <param name="center"> Centre point to create sphere around. </param>
	/// <param name="radius"> Radius of Sphere. </param>
	/// <param name="result"> Resultant NavMesh Point found. Vector3.zero if not found. </param>
	/// <param name="maxAttempts"> Maximum number of times to try and find a point default value 30. </param>
	/// <returns> Whether or not a navmesh point was found. If false result is Vector3.zero (bad idea to use this) </returns>
	public static bool RandomPointOnNavMesh(Vector3 center, float radius, out Vector3 result, int maxAttempts = 30) {
		for (int i = 0; i < maxAttempts; i++) {
			Vector3 randomPoint = center + Random.insideUnitSphere * radius;
			UnityEngine.AI.NavMeshHit hit;
			if (UnityEngine.AI.NavMesh.SamplePosition(randomPoint, out hit, 1.0f, UnityEngine.AI.NavMesh.AllAreas)) {
				result = hit.position;
				return true;
			}
		}
		result = Vector3.zero;
		return false;
	}

}
