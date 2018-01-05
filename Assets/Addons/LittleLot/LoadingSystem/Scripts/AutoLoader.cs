using System.Collections;
using UnityEngine;

/// <summary>
/// @StewMcc 2/09/2017
/// </summary>
namespace LittleLot {

	/// <summary>
	/// Automatically loads the scene at the end of the frame and after a set delay.
	/// </summary>
	public class AutoLoader : MonoBehaviour {

		[SerializeField, Scene]
		private string nextScene = "";
		[SerializeField, Scene]
		private string loadingScene = "";
		[SerializeField]
		private bool useLoadingScene = false;
		[SerializeField]
		private float delay = 2.0f;

		/// <summary>
		/// Automatically loads the set scene at the end of the frame.
		/// </summary>
		private IEnumerator Start() {
			// Ensures it happens after everything else has finished loading.
			yield return new WaitForEndOfFrame();
			// w8 for the delay
			yield return new WaitForSeconds(delay);
			if (useLoadingScene) {
				LoadingTransitionController.AnimatedLoadSceneAsync(nextScene, loadingScene);
			} else {
				LoadingTransitionController.AnimatedLoadSceneAsync(nextScene);
			}
		}
	}
}
