using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// @StewMcc 2/09/2017
/// </summary>
namespace LittleLot {

	/// <summary>
	/// Updates the attached text to match the loading progress.
	/// </summary>
	public class LoadingProgressReader : MonoBehaviour {

		[SerializeField]
		Text text = null;

		void Update() {
			text.text = "Loading... " + LoadingTransitionController.Progress().ToString("P0");
		}
	}
}
