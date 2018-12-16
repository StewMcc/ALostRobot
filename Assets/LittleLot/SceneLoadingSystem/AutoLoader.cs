using System.Collections;

using UnityEngine;

// @StewMcc 2/09/2017
namespace LittleLot.SceneLoading {

	/// <summary>
	/// Automatically loads the scene at the end of the frame and after a set delay.
	/// </summary>
	public class AutoLoader : MonoBehaviour {

		[SerializeField]
		SceneLoader sceneLoader = null;
		[SerializeField]
		float delay = 2.0f;

		/// <summary>
		/// Automatically loads the set scene at the end of the frame.
		/// </summary>
		private IEnumerator Start() {
			// Ensures it happens after everything else has finished loading.
			yield return new WaitForEndOfFrame();
			// w8 for the delay
			yield return new WaitForSeconds(delay);
			sceneLoader.LoadScene();
		}
	}
}
