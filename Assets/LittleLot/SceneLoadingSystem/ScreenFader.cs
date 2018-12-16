using System.Collections;

using UnityEngine;

// @StewMcc 10/12/2017
namespace LittleLot.SceneLoading {

	/// <summary>
	/// A Screen fader fades in and out for scene loading.
	/// </summary>
	/// <remarks>
	/// <seealso cref="LoadingTransitionController"/> for usage.
	/// <seealso cref="ImageScreenFader"/> for example.
	/// </remarks>
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
		/// <para/>
		/// Called when loading starts, twice if using a Loading Screen.
		/// </summary>
		/// <returns> yields until it is finished. </returns>
		public abstract IEnumerator FadeIn();

		/// <summary>
		/// Fade the screen into view, using yields to wait till your finished.
		/// <para/>
		/// Called when loading ends, twice if using a Loading Screen.
		/// </summary>
		/// <returns> yields until it is finished. </returns>
		public abstract IEnumerator FadeOut();

	}
}

