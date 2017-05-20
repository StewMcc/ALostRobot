using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(NavMeshAgent))]
public class RobotController : MonoBehaviour {

	[SerializeField]
	Button dropItem =null;

	[SerializeField]
	private GameObject inventoryUi = null;

	[SerializeField]
	Text currentItemAlienText =null;

	[SerializeField]
	Text currentItemSensibleText =null;

	// Controls the navigation of the bot on the navmesh
	private NavMeshAgent agent_ = null;
	private bool isMoving_ = false;
	private float stopThreshold_ = 0.1f;

	private Pickup currentItem_ = null;

	private Pickup collidedItem_ = null;

	/// <summary>
	/// Sets up the navmeshagent and Hides the Inventory.
	/// </summary>
	private void Start() {
		agent_ = GetComponent<NavMeshAgent>();
		EmptyInventory();
	}

	/// <summary>
	/// Add dropitem to button OnClick.
	/// </summary>
	private void OnEnable() {
		dropItem.onClick.AddListener(DropCurrentItem);
	}

	/// <summary>
	/// Removes listeners.
	/// </summary>
	private void OnDisable() {
		dropItem.onClick.RemoveAllListeners();
	}

	/// <summary>
	/// When the player taps the screen updates the target for the robot.	
	/// </summary>
	private void Update() {

		if (isMoving_) {
			if (agent_.velocity.magnitude < stopThreshold_) {
				isMoving_ = false;
				SoundManager.StopEvent("Player_Move_START", 1, gameObject);
				SoundManager.PlayEvent("Player_Move_END", gameObject);
			}
		}

		if (OnTap()) {
			// ray trace to check if touching a navmesh.
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

			// if its hit anything
			if (Physics.Raycast(ray, out hit) && !IsOverUi()) {
				agent_.destination = hit.point;
				if (!isMoving_) {
					SoundManager.PlayEvent("Player_Move_START", gameObject);
					isMoving_ = true;
				}
			}
		}

		UpdateCurrentItem();
	}

	/// <summary>
	/// Gizmo whichs draws the forward vector.
	/// Handy for knowing where the front of an object is.
	/// </summary>
	private void OnDrawGizmosSelected() {
		Gizmos.color = Color.red;
		Vector3 direction = transform.TransformDirection(Vector3.forward) * 4;
		Gizmos.DrawRay(transform.position, direction);
	}

	/// <summary>
	/// Updates the usable collided item of the player.	
	/// </summary>
	private void OnTriggerEnter(Collider collider) {
		GameObject collidedGameObject = collider.gameObject;
		// check valid item
		if (collidedGameObject.tag == "Item") {
			// remove the popup on the old item
			if (collidedItem_) {
				collidedItem_.HidePopUp();
			}
			// update and show new popup
			collidedItem_ = collidedGameObject.GetComponent<Pickup>();
			collidedItem_.ShowPopUp();
		}
	}

	/// <summary>
	/// Removes the current usable collidedItem
	/// </summary>
	/// <param name="collider"></param>
	private void OnTriggerExit(Collider collider) {
		GameObject collidedGameObject = collider.gameObject;
		// check valid item
		if (collidedGameObject.tag == "Item") {
			// remove the popup and remove the link to it
			if (collidedItem_) {
				if (collidedGameObject.GetComponent<Pickup>() == collidedItem_) {
					collidedItem_.HidePopUp();
					collidedItem_ = null;
				}
			}
		}
	}

	public bool HasItem() {
		if (currentItem_) {
			return true;
		}
		return false;
	}
	/// <summary>
	/// Hides the collided items popup and sets it to the currently collected item
	/// </summary>
	public void PickUpItem() {
		if (collidedItem_) {
			collidedItem_.HidePopUp();
			collidedItem_.ItemPickedUp();
			DropCurrentItem();
			currentItem_ = collidedItem_;
			SoundManager.PlayEvent("Item_PickUp", gameObject);
		}
		ShowInventory();
	}

	/// <summary>
	/// Drop the current item in the inventory.
	/// </summary>
	public void DropCurrentItem() {
		if (currentItem_) {
			currentItem_.ItemDropped();
			currentItem_ = null;
		}
		EmptyInventory();
	}

	/// <summary>
	/// Drop the current item in the inventory.
	/// </summary>
	public void RespawnCurrentItem() {
		if (currentItem_) {
			currentItem_.Respawn();
			currentItem_ = null;
		}
		EmptyInventory();
	}

	/// <summary>
	/// Checks for tap on screen.
	/// Just Input.Mouse check.
	/// </summary>
	/// <returns></returns>
	private bool OnTap() {
		if (Input.GetMouseButton(0)) {
			return true;
		}
		return false;
	}

	/// <summary>
	/// Check if the finger is over the UI.
	/// </summary>
	/// <returns></returns>
	private bool IsOverUi() {
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
		return EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId);
		
#else
		return EventSystem.current.IsPointerOverGameObject();
#endif
	}

	/// <summary>
	/// Update the items position to follow the player.
	/// </summary>
	private void UpdateCurrentItem() {
		if (currentItem_) {
			currentItem_.transform.position = new Vector3(
				transform.position.x,
				transform.position.y + currentItem_.ItemheightOffset(),
				transform.position.z);
		}
	}

	/// <summary>
	/// Show the item in the inventory
	/// </summary>
	private void ShowInventory() {
		if (currentItem_) {
			inventoryUi.SetActive(true);
			dropItem.interactable = true;
			if (currentItem_.HasRespawned()) {
				currentItemAlienText.enabled = false;
				currentItemSensibleText.enabled = true;
				currentItemSensibleText.text = currentItem_.Name();
			}
			else {
				currentItemSensibleText.enabled = false;
				currentItemAlienText.enabled = true;
				currentItemAlienText.text = currentItem_.Name();
			}
		}
		else {
			inventoryUi.SetActive(false);
		}
	}

	/// <summary>
	/// disable and remove the item from the inventory UI.
	/// </summary>
	private void EmptyInventory() {
		dropItem.interactable = false;
		currentItemAlienText.enabled = false;
		currentItemSensibleText.enabled = true;
		currentItemSensibleText.text = "Empty";
		inventoryUi.SetActive(false);
	}

	/// <summary>
	/// Returns the current item in the inventory.
	/// </summary>
	/// <returns> Returns null if not available. </returns>
	public Pickup CurrentItem() {
		return currentItem_;
	}

	/// <summary>
	/// Hides the Inventory Ui fom the Hud.
	/// </summary>
	public void HideInventory() {
		inventoryUi.SetActive(false);
	}
}
