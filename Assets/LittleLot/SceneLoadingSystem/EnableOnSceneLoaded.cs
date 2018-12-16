using UnityEngine;

// @StewMcc 2/09/2017
namespace LittleLot.SceneLoading {

	/// <summary>
	/// Disables the object on launch and re-enables when loading has finished.
	/// </summary>
	/// <remarks>
	/// Handy for lighting etc. that will incorrectly add to the previous scene.
	/// </remarks>
	public class EnableOnSceneLoaded : MonoBehaviour {

		/// <summary>
		/// Adds the listener for loading finished.
		/// </summary>
		/// <remarks>
		/// Ensures that the object will be reactivated if without a loading system active,
		/// for example when playing the level in the editor out of sequence.
		/// </remarks>
		private void Awake() {
			if (LoadingTransitionController.Exists()) {
				gameObject.SetActive(false);
			}
			LoadingTransitionController.OnLoadingFinished += ActivateGameObject;
		}

		/// <summary>
		/// Removes the listener for loading finished.
		/// </summary>
		private void OnDestroy() {
			LoadingTransitionController.OnLoadingFinished -= ActivateGameObject;
		}

		/// <summary>
		/// Sets the gameObject to active.
		/// </summary>
		private void ActivateGameObject() {
			gameObject.SetActive(true);
		}

	}
}