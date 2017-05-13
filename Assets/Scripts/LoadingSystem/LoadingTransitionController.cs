using System.Collections;

using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Transition controller for Loading screens and fade screens.
/// </summary>
public class LoadingTransitionController : Singleton<LoadingTransitionController> {

	[SerializeField]
	ScreenFader blackScreenCover = null;
	[SerializeField]
	float minDuration = 1.5f;

	/// <summary>
	/// Ensures the loading controller isnt destroyed on load.
	/// </summary>
	private void Start() {
		DontDestroyOnLoad(gameObject);
	}

	/// <summary>
	/// Check if the transition controller exists.
	/// If it doesn't ensure the scene is set up properly.
	/// </summary>
	/// <returns> True if the LoadingSystem exists.</returns>
	public static bool HasLoadingSystem() {
		LoadingTransitionController foundTransitionController = (LoadingTransitionController)FindObjectOfType(typeof(LoadingTransitionController));
		if (!foundTransitionController) {
			Debug.Log("No Transition Controller, generally ignore if in Editor.");
			return false;
		}
		return true;
	}

	public static string GetActiveScene() {
		return SceneManager.GetActiveScene().name;
	}

	/// <summary>
	/// Loads the scene additively using the Loading System.
	/// Uses the default LoadingScreen scene for transitions.
	/// Falls back to Async load if the loading system doesn't exist.
	/// </summary>
	/// <param name="sceneName"> The name of the scene to load. </param>
	public static void AnimatedLoadSceneAsync(string sceneName) {
		AnimatedLoadSceneAsync(sceneName, "LoadingScreen");
	}

	/// <summary>
	/// Loads the scene additively using the Loading System.
	/// Falls back to Async load if the loading system doesn't exist.
	/// </summary>
	/// <param name="sceneName"> The name of the scene to load. </param>
	/// <param name="loadingScene"> The name of the loading scene to be used whuilst transitioning. </param>
	public static void AnimatedLoadSceneAsync(string sceneName, string loadingScene) {
		try {
			instance.StartCoroutine(instance.LoadSceneAsync(sceneName, loadingScene));
		}
		catch (System.Exception) {
			Debug.Log("Start from Initial scene for animated loading screens.");
			SceneManager.LoadSceneAsync(sceneName);
		}
	}

	/// <summary>
	/// Coroutine IEnumerator for scene loading.
	/// Fades in and out the fadescreeb, Loads the loading scene.
	/// Plays the loading scene whilst the main scene is loading.
	/// </summary>
	/// <param name="sceneName"> The name of the scene to load. </param>
	/// <param name="loadingScene"> The name of the loading scene to be used whuilst transitioning. </param>
	/// <returns> IEnumerator for the time it takes for all the tasks to complete. </returns>
	private IEnumerator LoadSceneAsync(string sceneName, string loadingScene) {
		blackScreenCover.DisableInput();
		// Fade to black
		yield return StartCoroutine(blackScreenCover.FadeIn());

		// Load loading screen
		yield return SceneManager.LoadSceneAsync(loadingScene);

		// Fade to loading screen
		yield return StartCoroutine(blackScreenCover.FadeOut());

		// wait for the initial loading animation to finish.
		yield return LoadingSceneManager.WaitForInitialLoadingAnimation();

		// ensure it w8ts minimum amount of time ( i.e. fast loads)
		float endTime = Time.time + minDuration;

		// Load level async
		yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

		while (Time.time < endTime)
			yield return null;

		// Start the exit animation for the loading screen.
		yield return LoadingSceneManager.StartFinishLoadingAnimation();

		// Fade to black
		yield return StartCoroutine(blackScreenCover.FadeIn());

		// !!! unload loading screen
		LoadingSceneManager.UnloadLoadingScene();

		// set the loading scene to active giving it focus.
		SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));

		// Ensures any final changes are made to the scene.
		EventManagerLoadingSystem.FinishedLoading();

		// Fade to new screen
		yield return StartCoroutine(blackScreenCover.FadeOut());

		blackScreenCover.EnableInput();
		yield return null;
	}
}

