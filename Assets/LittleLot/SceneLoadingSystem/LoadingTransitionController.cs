using System.Collections;

using UnityEngine;
using UnityEngine.SceneManagement;

// @StewMcc 2/09/2017
namespace LittleLot.SceneLoading {

	/// <summary>
	/// Transition controller for Loading screens and fade screens.
	/// </summary>
	/// <remarks>
	/// Will fade in a ScreenFader as it starts to load the loading scene which is used for the transition.
	/// </remarks>
	public class LoadingTransitionController : Singleton<LoadingTransitionController> {

		public delegate void EventHandlerLoadingSystem();
		/// <summary>
		/// Event to listen to for when the level has finished loading.
		/// </summary>
		public static event EventHandlerLoadingSystem OnLoadingFinished;

		[SerializeField]
		ScreenFader screenFader = null;

		[SerializeField]
		float defaultMinDuration = 0.5f;

		private float progress_ = 1.0f;

		private bool isLoading_ = false;

		/// <summary>
		/// Launches the event for FinisheLoading so scene will finish loading.
		/// </summary>
		private void Start() {
			// Ensures any final changes are made to the scene.
			FinishedLoading();
		}

		/// <summary>
		/// Whether the Transition controller is currently loading anything.
		/// </summary>
		/// <returns> True if is loading, defaults to false if no transition controller found. </returns>
		public bool IsLoading() {
			if (Instance) {
				return Instance.isLoading_;
			}
			return false;
		}

		/// <summary>
		/// The current Progress of the load scene in a display readable format.
		/// </summary>
		public static float Progress() {
			if (Instance) {
				if (Instance.progress_ < 0.1f) {
					return 0.1f;
				} else {
					return Mathf.Clamp01(Instance.progress_ / 0.9f);
				}
			} else {
				return 1.0f;
			}
		}

		/// <summary>
		/// Check if the transition controller exists.
		/// If it doesn't ensure the scene is set up properly.
		/// </summary>
		/// <returns> True if the LoadingSystem exists.</returns>
		public static bool Exists() {

			LoadingTransitionController foundTransitionController = (LoadingTransitionController)FindObjectOfType(typeof(LoadingTransitionController));
			if (!foundTransitionController) {
				Debug.Log("No Transition Controller, generally ignore if starting from random scene.");
				return false;
			}
			return true;
		}

		public static string GetActiveScene() {
			return SceneManager.GetActiveScene().name;
		}

		/// <summary>
		/// Loads the scene additively using the Loading System. (Only use for fast loads)
		/// </summary>
		/// <remarks>
		/// Fades the screen out and then loads the next scene.
		/// Falls back to Async load if the loading system doesn't exist.
		/// </remarks>
		/// <param name="sceneName"> The name of the scene to load. </param>
		/// <param name="overrideMinDuration"> Overrides the default minimum duration of the transition when greater than 0.0f. </param>
		public static void AnimatedLoadSceneAsync(string sceneName, float overrideMinDuration = -1.0f) {
			if (Instance) {
				if (Instance.isLoading_) {
					Debug.Log("Already Loading can't load " + sceneName);
				} else if (IsSceneInBuild(sceneName)) {
					Instance.isLoading_ = true;
					if (overrideMinDuration < 0.0f) {
						Instance.StartCoroutine(Instance.LoadSceneAsync(sceneName, Instance.defaultMinDuration));
					} else {
						Instance.StartCoroutine(Instance.LoadSceneAsync(sceneName, overrideMinDuration));
					}
				}
			} else {
				if (IsSceneInBuild(sceneName)) {
					SceneManager.LoadSceneAsync(sceneName);
				}
			}
		}

		/// <summary>
		/// Loads the scene additively using the Loading System. (Use when need a loading screen, i.e. for long loads)
		/// </summary>
		/// <remarks>
		/// Will transition to a loading scene in-between loading the desired scene.
		/// Falls back to Async load if the loading system doesn't exist.
		/// </remarks>
		/// <param name="sceneName"> The name of the scene to load. </param>
		/// <param name="loadingScene"> The name of the loading scene to be used whilst transitioning. </param>
		/// <param name="overrideMinDuration"> Overrides the default minimum duration of the transition when greater than 0.0f. </param>
		public static void AnimatedLoadSceneAsync(string sceneName, string loadingScene, float overrideMinDuration = -1.0f) {
			if (Instance) {
				if (Instance.isLoading_) {
					Debug.Log("Already Loading can't load " + sceneName);
				} else if (IsSceneInBuild(sceneName) && IsSceneInBuild(loadingScene)) {
					Instance.isLoading_ = true;

					if (overrideMinDuration < 0.0f) {
						Instance.StartCoroutine(Instance.LoadSceneAsync(sceneName, loadingScene, Instance.defaultMinDuration));
					} else {
						Instance.StartCoroutine(Instance.LoadSceneAsync(sceneName, loadingScene, overrideMinDuration));
					}
				}
			} else {
				if (IsSceneInBuild(sceneName)) {
					SceneManager.LoadSceneAsync(sceneName);
				}
			}
		}

		/// <summary>
		/// Should be called when the new level has finished loading.
		/// </summary>
		private static void FinishedLoading() {
			// notify all listeners to event.
			if (OnLoadingFinished != null) {
				OnLoadingFinished();
			}
		}

		/// <summary>
		/// Checks if scene is in the current builds scene list.
		/// </summary>
		/// <param name="sceneName"> The scene to look for. </param>
		/// <returns> True if exists in the build. </returns>
		private static bool IsSceneInBuild(string sceneName) {
			if (SceneUtility.GetBuildIndexByScenePath(sceneName) >= 0) {
				return true;
			} else {
				Debug.LogError("Scene not in Build: " + sceneName);
				return false;
			}
		}

		/// <summary>
		/// Fades in a cover image then loads the scene in the background, then fades back out again.
		/// </summary>
		/// <param name="sceneName"> The name of the scene to load. Ensure this is within the Build list before calling.</param>
		/// <param name="minDuration"> The minimum amount of time the transition to the scene should take. </param>
		private IEnumerator LoadSceneAsync(string sceneName, float minDuration) {

			progress_ = 0.0f;

			screenFader.OnStartLoad();
			// Fade to black
			yield return StartCoroutine(screenFader.FadeIn());

			// ensure it waits minimum amount of time ( i.e. fast loads).
			float endTime = Time.time + minDuration;

			// Load level async
			AsyncOperation loadScene = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);

			Resources.UnloadUnusedAssets();

			while ((Time.time < endTime) || (!loadScene.isDone)) {
				progress_ = loadScene.progress;
				yield return null;
			}

			// set the loaded scene to active giving it focus.
			SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));

			// Ensures any final changes are made to the scene.
			FinishedLoading();
			Instance.isLoading_ = false;

			// Fade to new screen
			yield return StartCoroutine(screenFader.FadeOut());

			screenFader.OnFinishLoad();

			yield return null;
		}

		/// <summary>
		/// Fades in a loading scene additively then loads the real scene in the background, then fades back out again.
		/// </summary>
		/// <remarks>
		/// Will be on fade out screen during last 10% i.e awake/start.
		/// </remarks>
		/// <param name="sceneName"> The name of the scene to load. Ensure this is within the Build list before calling.</param>
		/// <param name="loadingScene"> The name of the loading scene to be used whilst transitioning. Ensure this is within the Build list before calling.</param>
		/// <param name="minDuration"> The minimum amount of time the transition to the scene should take. </param>
		private IEnumerator LoadSceneAsync(string sceneName, string loadingScene, float minDuration) {
			progress_ = 0.0f;

			screenFader.OnStartLoad();
			// Fade to black
			yield return StartCoroutine(screenFader.FadeIn());

			// Load loading screen scene
			yield return SceneManager.LoadSceneAsync(loadingScene, LoadSceneMode.Single);

			// Fade to loading screen
			yield return StartCoroutine(screenFader.FadeOut());

			// ensure resources cleaned up.
			yield return Resources.UnloadUnusedAssets();

			// ensure it waits minimum amount of time ( i.e. fast loads).
			float endTime = Time.time + minDuration;

			// Load level Async
			AsyncOperation loadScene = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

			// Disabling allowSceneActivation gives us better control of when a scene activates.
			// This allows us to bring in a blackout screen so lighting doesn't bleed between levels.
			loadScene.allowSceneActivation = false;

			// Wait for main section of load to finish, and min duration.
			while ((Time.time < endTime) || (loadScene.progress < 0.9f)) {
				progress_ = loadScene.progress;
				yield return null;
			}

			yield return new WaitForEndOfFrame();
			progress_ = loadScene.progress;

			// Fade to black
			yield return StartCoroutine(screenFader.FadeIn());

			// Allow awakes etc. to be called, and re-enable scene activation ( otherwise .isDone can never be true)
			loadScene.allowSceneActivation = true;

			// Wait for final scene finish load (awake etc.).
			while (!loadScene.isDone) {
				progress_ = loadScene.progress;
				yield return null;
			}

			// unload loading screen
			yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());

			yield return Resources.UnloadUnusedAssets();

			// set the loaded scene to active giving it focus.
			SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));

			// Ensures any final changes are made to the scene.
			FinishedLoading();
			Instance.isLoading_ = false;

			// Fade to new screen
			yield return StartCoroutine(screenFader.FadeOut());

			screenFader.OnFinishLoad();
			yield return null;
		}

	}
}
