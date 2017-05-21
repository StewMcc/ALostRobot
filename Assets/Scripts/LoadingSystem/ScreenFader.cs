using System.Collections;

using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Screen fader fades in and out the transparent screen used for scene loading.
/// </summary>
public class ScreenFader : MonoBehaviour {

	[SerializeField]
	private bool startsVisible = false;
	[SerializeField]
	private bool fadeOnAwake = false;
	[SerializeField]
	private float fadeSpeed = 1.0f;
	[SerializeField]
	private float minAlpha = 0;
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
		}
		else {
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
	/// Enables input by Disabling the raycastarget on the image.
	/// </summary>
	public void EnableInput() {
		screen_.raycastTarget = false;
	}

	/// <summary>
	/// Disables input by enabling the raycastarget on the image.
	/// </summary>
	public void DisableInput() {
		screen_.raycastTarget = true;
	}

	/// <summary>
	/// Slowly fades the screen in at the desired fade speed.
	/// </summary>
	/// <returns> Type suitable for Coroutines and yields. </returns>
	public IEnumerator FadeIn() {
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
	/// <returns> Type suitable for Coroutines and yields. </returns>
	public IEnumerator FadeOut() {
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

