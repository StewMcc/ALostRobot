using System.Collections;
using UnityEngine;

/// <summary>
/// @StewMcc 10/12/2017
/// </summary>
namespace LittleLot {

	/// <summary>
	/// A Screen fader fades in and out for scene loading.
	/// 
	/// <seealso cref="LoadingTransitionController"/> for usage.
	/// <seealso cref="ImageScreenFader"/> for example. 
	/// </summary>
	public abstract class ScreenFader : MonoBehaviour {

		/// <summary>
		/// Called before loading starts.
		/// </summary>
		public abstract void OnStartLoad();

		/// <summary>
		/// Called After loading Finishes.
		/// </summary>
		public abstract void OnFinishLoad();

		/// <summary>
		/// Fade the screen into view, using yields to wait till your finished.
		/// Called when loading starts, twice if using a Loading Screen.
		/// </summary>
		/// <returns> yields until it is finished. </returns>
		public abstract IEnumerator FadeIn();

		/// <summary>
		/// Fade the screen into view, using yields to wait till your finished.
		/// Called when loading ends, twice if using a Loading Screen.
		/// </summary>
		/// <returns> yields until it is finished. </returns>
		public abstract IEnumerator FadeOut();

	}
}

