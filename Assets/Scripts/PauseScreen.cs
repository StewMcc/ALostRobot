using UnityEngine;
using UnityEngine.UI;

public class PauseScreen : MonoBehaviour {

	[SerializeField]
	private Button openPauseMenu = null;

	[SerializeField]
	private Button closePauseMenu = null;

	[SerializeField]
	private Button restartButton =null;

	[SerializeField]
	private Button muteMusicButton =null;

	[SerializeField]
	private GameObject musicMutedIcon = null;
	private bool isMusicOn = true;
	private bool isSfxOn = true;

	private void Start() {
		restartButton.onClick.AddListener(RestartGame);
		muteMusicButton.onClick.AddListener(ToggleMusic);
		openPauseMenu.onClick.AddListener(OpenPauseMenu);
		closePauseMenu.onClick.AddListener(ClosePauseMenu);
		gameObject.SetActive(false);
	}
	
	private void OnDestroy() {
		restartButton.onClick.RemoveListener(RestartGame);
		muteMusicButton.onClick.RemoveListener(ToggleMusic);
		openPauseMenu.onClick.RemoveListener(OpenPauseMenu);
		closePauseMenu.onClick.RemoveListener(ClosePauseMenu);
	}

	private void OpenPauseMenu() {
		gameObject.SetActive(true);
		UiEventManager.HideHud();
		SoundManager.PlayEvent("Menu_Pause", gameObject);

		Time.timeScale = 0;
	}

	private void ClosePauseMenu() {
		gameObject.SetActive(false);
		UiEventManager.ShowHud();
		SoundManager.PlayEvent("Menu_Resume", gameObject);
		Time.timeScale = 1;
	}

	public void RestartGame() {
		Time.timeScale = 1;
		LoadingTransitionController.AnimatedLoadSceneAsync("Splash_Scene", "LoadingSpinnerScene");
	}

	public void ToggleMusic(){
		if (isMusicOn) {
			isMusicOn = false;
			SoundManager.PlayEvent("Music_Mute", gameObject);
			musicMutedIcon.SetActive (false);
		} else {
			isMusicOn = true;
			SoundManager.PlayEvent("Music_Reset", gameObject);
			musicMutedIcon.SetActive (true);
		}
	}

}
