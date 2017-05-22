﻿using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(NavMeshAgent))]
public class RobotController : MonoBehaviour {

	[SerializeField]
	private GameObject inventoryUi = null;

	[SerializeField]
	private Button dropButton =null;
	
	[SerializeField]
	private Text currentPickupNormalText =null;
	
	[SerializeField]
	private Text currentPickupAlienText =null;

	[SerializeField]
	private Transform pickupPosition = null;

	// Controls the navigation of the bot on the navmesh
	private NavMeshAgent agent_ = null;
	private bool isMoving_ = false;
	private float stopThreshold_ = 0.1f;

	private Pickup currentPickup_ = null;

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
		dropButton.onClick.AddListener(DropCurrentPickup);
	}

	/// <summary>
	/// Removes listeners.
	/// </summary>
	private void OnDisable() {
		dropButton.onClick.RemoveAllListeners();
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
	
	public bool HasItem() {
		if (currentPickup_) {
			return true;
		}
		return false;
	}

	/// <summary>
	/// Returns the current item in the inventory.
	/// </summary>
	/// <returns> Returns null if not available. </returns>
	public Pickup CurrentItem() {
		return currentPickup_;
	}

	/// <summary>
	/// Hides the collided items popup and sets it to the currently collected item
	/// </summary>
	public void AddPickup(Pickup newPickup) {
		DropCurrentPickup();
		currentPickup_ = newPickup;
		SoundManager.PlayEvent("Item_PickUp", gameObject);
	
		ShowInventory();
	}

	/// <summary>
	/// Drop the current item in the inventory.
	/// </summary>
	public void DropCurrentPickup() {
		if (currentPickup_) {
			currentPickup_.Drop();
			currentPickup_ = null;
		}
		EmptyInventory();
	}

	/// <summary>
	/// Respawn the current item in the inventory.
	/// </summary>
	public void RespawnCurrentPickup() {
		if (currentPickup_) {
			currentPickup_.Respawn();
			currentPickup_ = null;
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
		if (currentPickup_) {
			currentPickup_.transform.position = new Vector3(
				transform.position.x + pickupPosition.localPosition.x,
				transform.position.y + pickupPosition.localPosition.y,
				transform.position.z + pickupPosition.localPosition.z);
		}
	}

	/// <summary>
	/// Show the item in the inventory
	/// </summary>
	private void ShowInventory() {
		if (currentPickup_) {
			inventoryUi.SetActive(true);
			dropButton.interactable = true;
			if (currentPickup_.HasNameDiscovered()) {
				currentPickupAlienText.enabled = false;
				currentPickupNormalText.enabled = true;
				currentPickupNormalText.text = currentPickup_.Name();
			}
			else {
				currentPickupNormalText.enabled = false;
				currentPickupAlienText.enabled = true;
				currentPickupAlienText.text = currentPickup_.Name();
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
		dropButton.interactable = false;
		currentPickupAlienText.enabled = false;
		currentPickupNormalText.enabled = true;
		currentPickupNormalText.text = "Empty";
		inventoryUi.SetActive(false);
	}
	
	/// <summary>
	/// Hides the Inventory Ui fom the Hud.
	/// </summary>
	private void HideInventory() {
		inventoryUi.SetActive(false);
	}
}
