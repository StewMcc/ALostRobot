using UnityEngine;

/// <summary>
/// @StewMcc 2/09/2017
/// </summary>
namespace LittleLot {

	/// <summary>
	/// Scene loader will load the provided scene.
	/// 
	/// Will use basic async if no transition controller or use Loading Scene not set.
	/// Otherwise uses loading screen Setup.
	/// </summary>
	public class SceneLoader : MonoBehaviour {

		[SerializeField, Scene]
		private string nextScene = "";
		[SerializeField, Scene]
		private string loadingScene = "";
		[SerializeField]
		private bool useLoadingScene = false;

		/// <summary>
		/// Loads the scenes set on the object.
		/// </summary>
		public void LoadScene() {
			if (useLoadingScene) {
				LoadingTransitionController.AnimatedLoadSceneAsync(nextScene, loadingScene);
			} else {
				LoadingTransitionController.AnimatedLoadSceneAsync(nextScene);
			}
		}
	}
}
