using System.Collections;

using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Transition controller for Loading screens and fade screens.
/// Will fade in a screenfader as it starts to load the loading scene which is used for the transition.
/// See the LoadingTransitionController Prefab for an example.
/// </summary>
public class LoadingTransitionController : Singleton<LoadingTransitionController> {

	[SerializeField]
	ScreenFader blackScreenCover = null;
	[SerializeField]
	float minDuration = 1.5f;

	private void Start() {
		// Ensures any final changes are made to the scene.
		EventManagerLoadingSystem.FinishedLoading();
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
	/// Fades in a loading scene then loads the real scene in the background, then fades back out again.
	/// <seealso cref="LoadingSceneManager"/> is used to decide if it will wait for enter and exit animations to finish.
	/// </summary>
	/// <param name="sceneName"> The name of the scene to load. </param>
	/// <param name="loadingScene"> The name of the loading scene to be used whilst transitioning. </param>
	private IEnumerator LoadSceneAsync(string sceneName, string loadingScene) {
		blackScreenCover.DisableInput();
		// Fade to black
		yield return StartCoroutine(blackScreenCover.FadeIn());

		// Load loading screen
		yield return SceneManager.LoadSceneAsync(loadingScene);

		// Fade to loading screen
		yield return StartCoroutine(blackScreenCover.FadeOut());

		// ensure it waits minimum amount of time ( i.e. fast loads) + the enter animation time.
		float endTime = Time.time + minDuration + LoadingSceneManager.EnterAnimationTime();

		// Load level async
		yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

		while (Time.time < endTime)
			yield return null;

		// Start the exit animation for the loading screen, then wait for it to finish.
		float exitTime = LoadingSceneManager.ExitAnimationTime();
		if (exitTime > 0) {
			yield return new WaitForSeconds(exitTime);
		}

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

