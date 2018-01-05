using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// @StewMcc 2/09/2017
/// </summary>
namespace LittleLot {

	/// <summary>
	/// Transition controller for Loading screens and fade screens.
	/// 
	/// Will fade in a screenfader as it starts to load the loading scene which is used for the transition.
	/// <seealso cref="LoadingTransitionController"/> Prefab for an example.
	/// <seealso cref="DontDestroyOnLoad"/> <br/>
	/// <seealso cref="/LittleLot/LoadingSystem/Prefabs/LoadingTransitionController.prefab"/>
	/// </summary>
	public class LoadingTransitionController : Singleton<LoadingTransitionController> {

		[SerializeField]
		private ScreenFader screenFader = null;

		[SerializeField]
		private float minDuration = 0.5f;

		private float progress_ = 1.0f;

		private bool isLoading_ = false;

		/// <summary>
		/// Launches the event for finishloading so scene will finish loading.
		/// 
		/// Mainly used so Loadingtransitioner can be shuved into any scene for testing without
		/// breaking everything.
		/// </summary>
		private void Start() {
			// Ensures any final changes are made to the scene.
			EventManagerLoadingSystem.FinishedLoading();
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
		public static bool HasLoadingSystem() {
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
		/// Fades the screen out and then loads the next scene.
		/// Falls back to Async load if the loading system doesn't exist.
		/// </summary>
		/// <param name="sceneName"> The name of the scene to load. </param>
		public static void AnimatedLoadSceneAsync(string sceneName) {
			if (Instance) {
				if (Instance.isLoading_) {
					Debug.Log("Already Loading can't load " + sceneName);
				} else if (IsSceneInBuild(sceneName)) {
					Instance.isLoading_ = true;
					Instance.StartCoroutine(Instance.LoadSceneAsync(sceneName));
				}
			} else {
				SceneManager.LoadSceneAsync(sceneName);
			}
		}

		/// <summary>
		/// Loads the scene additively using the Loading System. (Use when need a loading screen, i.e. for long loads)
		/// Will transition to a loadingscene inbetween loading the desired scene.
		/// Falls back to Async load if the loading system doesn't exist.
		/// </summary>
		/// <param name="sceneName"> The name of the scene to load. </param>
		/// <param name="loadingScene"> The name of the loading scene to be used whilst transitioning. </param>
		public static void AnimatedLoadSceneAsync(string sceneName, string loadingScene) {
			if (Instance) {
				if (Instance.isLoading_) {
					Debug.Log("Already Loading can't load " + sceneName);
				} else if (IsSceneInBuild(sceneName) && IsSceneInBuild(loadingScene)) {
					Instance.isLoading_ = true;
					Instance.StartCoroutine(Instance.LoadSceneAsync(sceneName, loadingScene));
				}
			} else {
				SceneManager.LoadSceneAsync(sceneName);
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
		private IEnumerator LoadSceneAsync(string sceneName) {

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
			EventManagerLoadingSystem.FinishedLoading();
			Instance.isLoading_ = false;

			// Fade to new screen
			yield return StartCoroutine(screenFader.FadeOut());

			screenFader.OnFinishLoad();

			yield return null;
		}

		/// <summary>
		/// Fades in a loading scene then loads the real scene in the background, then fades back out again.
		/// Handles final loading slightly different for 3DScenes to ensure lighing doesn't bleed into loading screen.
		/// However will be on fade out screen during last 10% i.e awake/start.
		/// </summary>
		/// <param name="sceneName"> The name of the scene to load. Ensure this is within the Build list before calling.</param>
		/// <param name="loadingScene"> The name of the loading scene to be used whilst transitioning. Ensure this is within the Build list before calling.</param>
		private IEnumerator LoadSceneAsync(string sceneName, string loadingScene) {
			progress_ = 0.0f;

			screenFader.OnStartLoad();
			// Fade to black
			yield return StartCoroutine(screenFader.FadeIn());

			// Load loading screen
			yield return SceneManager.LoadSceneAsync(loadingScene);

			LoadingSceneManager sceneManager = FindObjectOfType(typeof(LoadingSceneManager)) as LoadingSceneManager;

			// Fade to loading screen
			yield return StartCoroutine(screenFader.FadeOut());

			Resources.UnloadUnusedAssets();

			// ensure it waits minimum amount of time ( i.e. fast loads).
			float endTime = Time.time + minDuration;

			// Load level async
			AsyncOperation loadScene = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

			if (sceneManager.Is3dLoadingScene()) {
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

				// Allow awakes etc to be called, and renable scene activation ( otherwise .isDone can never be true)
				loadScene.allowSceneActivation = true;

				// Wait for final scene finish load (awake etc).
				while (!loadScene.isDone) {
					progress_ = loadScene.progress;
					yield return null;
				}
			} else {
				loadScene.allowSceneActivation = true;

				// Wait for load to finish, and min duration.
				while ((Time.time < endTime) || !loadScene.isDone) {
					progress_ = loadScene.progress;
					yield return null;
				}

				// Fade to black
				yield return StartCoroutine(screenFader.FadeIn());
			}

			// !!! unload loading screen            
			if (sceneManager != null) {
				sceneManager.UnloadLoadingScene();
			}

			// set the loaded scene to active giving it focus.
			SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));

			// Ensures any final changes are made to the scene.
			EventManagerLoadingSystem.FinishedLoading();
			Instance.isLoading_ = false;

			// Fade to new screen
			yield return StartCoroutine(screenFader.FadeOut());

			screenFader.OnFinishLoad();
			yield return null;
		}

	}
}

