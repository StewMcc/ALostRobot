using UnityEngine;
using UnityEngine.UI;

public class ErrorMessage : MonoBehaviour {

	[SerializeField]
	Text errorText = null;

	[SerializeField]
	Image warningImage = null;

	private Vector3 initialPosition_ = Vector3.zero;

	private float initialFadeRate_ = 0.05f;
	private float lateFadeRate_ = 0.1f;

	private float currentAlpha_ = 1.0f;

	private void Start() {
		initialPosition_ = transform.position;
	}

	private void LateUpdate() {
		if (currentAlpha_ >= 0.9f) {
			// slow alpha blend away to start
			currentAlpha_ -= currentAlpha_ * initialFadeRate_ * Time.deltaTime;
			SetAlpha(currentAlpha_);
		}
		else if (currentAlpha_ >= 0) {
			// faster blend once below certain alpha value.
			currentAlpha_ -= currentAlpha_ * lateFadeRate_;
			SetAlpha(currentAlpha_);
		}
	}

	/// <summary>
	/// Updates the Error errorText to provided string.
	/// </summary>
	/// <param name="newErrorText"></param>
	public void SetText(string newErrorText) {
		errorText.text = newErrorText;
	}

	/// <summary>
	/// Set errorText and warningImage to provided alpha.
	/// </summary>
	/// <param name="newAlphaValue"> Alpha Value to change to. </param>
	public void SetAlpha(float newAlphaValue) {
		currentAlpha_ = newAlphaValue;
		Color newColor = errorText.color;
		newColor.a = newAlphaValue;
		errorText.color = newColor;

		newColor = warningImage.color;
		newColor.a = newAlphaValue;
		warningImage.color = newColor;

	}

	/// <summary>
	/// Reset to initial position
	/// </summary>
	public void ResetPosition() {
		transform.position = initialPosition_;
	}

	/// <summary>
	/// Shifts the message up by the y offset locally.
	/// </summary>
	/// <param name="yOffset"> The amount to move the message by. </param>
	public void ShiftUp(float yOffset) {
		Vector3 currentPos = transform.localPosition;
		transform.localPosition = new Vector3(currentPos.x, currentPos.y + yOffset, currentPos.z);
	}
}
