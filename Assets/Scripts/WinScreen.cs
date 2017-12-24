using UnityEngine;
using UnityEngine.UI;

public class WinScreen : MonoBehaviour {

	private void Start() {
		EventManager.OnGameCompletion += ShowWinScreen;
		gameObject.SetActive(false);
	}

	private void OnDestroy() {
		EventManager.OnGameCompletion -= ShowWinScreen;
	}

	/// <summary>
	/// Displays the win screen when the game is completed, and hides the HUD.
	/// </summary>
	private void ShowWinScreen() {
		gameObject.SetActive(true);
		UiEventManager.HideHud();
	}

}
