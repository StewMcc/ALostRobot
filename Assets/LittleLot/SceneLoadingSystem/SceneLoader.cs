using UnityEngine;

// @StewMcc 2/09/2017
namespace LittleLot.SceneLoading {

	/// <summary>
	/// Loads the provided scene using <seealso cref="LoadingTransitionController"/>.
	/// </summary>
	/// <remarks>
	/// Falls back to Async load if the loading system doesn't exist. If useLoadingScene set
	/// will transition to a separate loading scene then load the final scene, otherwise will just
	/// transition between scenes.
	/// </remarks>
	public class SceneLoader : MonoBehaviour {

		[SerializeField, Scene]
		string nextScene = "";
		[SerializeField, Scene]
		string loadingScene = "";
		[SerializeField]
		bool useLoadingScene = false;
		[Tooltip("Value greater than 0.0f will override the minimum transition duration.")]
		[SerializeField]
		float overrideMinTransitionDuration = -1.0f;

		/// <summary>
		/// Loads the scenes set on the object.
		/// </summary>
		public void LoadScene() {
			if (useLoadingScene) {
				LoadingTransitionController.AnimatedLoadSceneAsync(nextScene, loadingScene, overrideMinTransitionDuration);
			} else {
				LoadingTransitionController.AnimatedLoadSceneAsync(nextScene, overrideMinTransitionDuration);
			}
		}
	}
}
