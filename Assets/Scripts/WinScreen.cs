using UnityEngine;
using UnityEngine.UI;

public class WinScreen : MonoBehaviour {

	[SerializeField]
	private Button restartButton = null;

	private void Start() {
		EventManager.OnGameCompletion += ShowWinScreen;
		gameObject.SetActive(false);
		restartButton.onClick.AddListener(RestartGame);
	}

	private void OnDestroy() {
		EventManager.OnGameCompletion -= ShowWinScreen;
		restartButton.onClick.RemoveListener(RestartGame);
	}

	/// <summary>
	/// Displays the win screen when the game is completed, and hides the HUD.
	/// </summary>
	private void ShowWinScreen() {
		gameObject.SetActive(true);
		UiEventManager.HideHud();
	}

	public void RestartGame() {
		LoadingTransitionController.AnimatedLoadSceneAsync("Splash_Scene", "LoadingSpinnerScene");
	}

}
