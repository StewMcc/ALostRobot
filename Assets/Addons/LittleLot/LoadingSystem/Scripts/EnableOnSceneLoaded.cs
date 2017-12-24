using UnityEngine;

/// <summary>
/// @StewMcc 2/09/2017
/// </summary>
namespace LittleLot {

	/// <summary>
	/// Utility script for loading system.
	/// 
	/// Disables the object on launch and re-enables when loading has finished.
	/// Handy for lighting etc that will incorrectly add to the previous scene.
	/// </summary>
	public class EnableOnSceneLoaded : MonoBehaviour {

		/// <summary>
		/// Adds the listener for loading finished.
		/// Ensures that the object will be reactivted if without a loading system active,
		/// for example when playing the level in the editor out of sequence.
		/// </summary>
		private void Awake() {
			gameObject.SetActive(false);
			EventManagerLoadingSystem.OnLoadingFinished += ActivateGameObject;
			if (!LoadingTransitionController.HasLoadingSystem()) {
				ActivateGameObject();
			}
		}

		/// <summary>
		/// Removes the listener for loading finished.
		/// </summary>
		private void OnDestroy() {
			EventManagerLoadingSystem.OnLoadingFinished -= ActivateGameObject;
		}

		/// <summary>
		/// Sets the gameObject to active.
		/// </summary>
		private void ActivateGameObject() {
			gameObject.SetActive(true);
		}

	}
}