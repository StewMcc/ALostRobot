using System.Collections;
using UnityEngine;

/// <summary>
/// @StewMcc 2/09/2017
/// </summary>
namespace LittleLot {
	using UnityEngine.UI;

	/// <summary>
	/// Screen fader fades in and out the transparent screen used for scene loading.
	/// </summary>
	public class ImageScreenFader : ScreenFader {

		[SerializeField]
		private bool startsVisible = false;
		[SerializeField]
		private bool fadeOnAwake = false;
		[SerializeField]
		private float fadeSpeed = 1.0f;
		[Range(0, 1)]
		[SerializeField]
		private float minAlpha = 0;
		[Range(0, 1)]
		[SerializeField]
		private float maxAlpha = 1.0f;

		private Image screen_;

		/// <summary>
		/// Ensures the image is in the correct state on start.
		/// </summary>
		private void Start() {
			if (!screen_) {
				screen_ = GetComponent<Image>();
			}
			if (startsVisible) {
				// Set alpha to max
				Color imageColor = screen_.color;
				imageColor.a = maxAlpha;
				screen_.color = imageColor;

				screen_.raycastTarget = true;

				if (fadeOnAwake) {
					StartCoroutine(FadeOut());
				}
			} else {
				// set alpha to min            
				Color imageColor = screen_.color;
				imageColor.a = minAlpha;
				screen_.color = imageColor;

				screen_.raycastTarget = false;

				if (fadeOnAwake) {
					StartCoroutine(FadeIn());
				}
			}
		}

		/// <summary>
		/// Disables input by enabling the raycastarget on the image.
		/// </summary>
		public override void OnStartLoad() {
			screen_.raycastTarget = true;
		}

		/// <summary>
		/// Enables input by Disabling the raycastarget on the image.
		/// </summary>
		public override void OnFinishLoad() {
			screen_.raycastTarget = false;
		}

		/// <summary>
		/// Slowly fades the screen in at the desired fade speed.
		/// </summary>
		public override IEnumerator FadeIn() {
			Color imageColor = screen_.color;

			while (imageColor.a < maxAlpha) {
				yield return null;
				imageColor.a += fadeSpeed * Time.deltaTime;
				screen_.color = imageColor;
			}

			imageColor.a = maxAlpha;
			screen_.color = imageColor;

		}

		/// <summary>
		/// Slowly fades the screen out at the desired fade speed.
		/// </summary>
		public override IEnumerator FadeOut() {
			Color imageColor = screen_.color;

			while (imageColor.a > minAlpha) {
				yield return null;
				imageColor.a -= fadeSpeed * Time.deltaTime;
				screen_.color = imageColor;
			}

			imageColor.a = minAlpha;
			screen_.color = imageColor;

		}

	}
}
