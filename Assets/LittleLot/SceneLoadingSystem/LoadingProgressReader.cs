using UnityEngine;
using UnityEngine.UI;

// @StewMcc 2/09/2017
namespace LittleLot.SceneLoading {

	/// <summary>
	/// Updates the attached text to match the loading progress.
	/// </summary>
	public class LoadingProgressReader : MonoBehaviour {

		[SerializeField]
		Text text = null;

		/// <summary>
		/// Handles if there is no transition controller.
		/// </summary>
		private void Start() {
			if (!LoadingTransitionController.Exists()) {
				// no transition controller so disable updating.
				enabled = false;
			}
		}

		/// <summary>
		/// Updates the loading display text.
		/// </summary>
		private void Update() {
			text.text = "Loading... " + LoadingTransitionController.Progress().ToString("P0");
		}
	}
}
