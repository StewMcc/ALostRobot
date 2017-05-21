using UnityEngine;

public class WinScreen : MonoBehaviour {

	[SerializeField]
	private GameObject winScreen = null;

	[SerializeField]
	private GameObject hud = null;

	private void Start() {
		winScreen.SetActive(false);
	}

	private void OnEnable() {
		EventManager.OnGameCompletion += ShowWinScreen;
		EventManager.OnShipFixed += DisableHud;
	}

	private void OnDisable() {
		EventManager.OnGameCompletion -= ShowWinScreen;
		EventManager.OnShipFixed -= DisableHud;
	}

	/// <summary>
	/// Displays the win screen when the game is completed, and hides the HUD.
	/// </summary>
	private void ShowWinScreen() {
		winScreen.SetActive(true);
	}

	/// <summary>
	/// Disables the players HUD.
	/// </summary>
	private void DisableHud() {
		hud.SetActive(false);
	}
}
