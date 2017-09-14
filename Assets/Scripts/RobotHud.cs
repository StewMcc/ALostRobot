using UnityEngine;

public class RobotHud : MonoBehaviour {

	private void Start() {
		UiEventManager.OnHideHud += DisableHud;
		UiEventManager.OnShowHud += ShowHud;
		EventManager.OnShipFixed += DisableHud;
	}

	private void OnDestroy() {
		UiEventManager.OnHideHud -= DisableHud;
		UiEventManager.OnShowHud -= ShowHud;
		EventManager.OnShipFixed -= DisableHud;

	}

	/// <summary>
	/// Enables the robots HUD.	
	/// </summary>
	private void ShowHud() {
		gameObject.SetActive(true);
	}

	/// <summary>
	/// Disables the robots HUD.
	/// </summary>
	private void DisableHud() {
		gameObject.SetActive(false);
	}

}

