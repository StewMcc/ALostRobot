using System;

using UnityEngine;

// @StewMcc 22/02/2018
namespace LittleLot.Android {

	/// <summary>
	/// Controls the allowed auto-rotate settings, whilst honouring device auto-rotate settings.
	/// <para/>
	/// Allows the modification of auto-rotate settings exposed in player settings at runtime.
	/// </summary>
	public class AutoRotationMaintainer : Singleton<AutoRotationMaintainer> {

		/// <summary>
		/// Possible available rotation settings.
		/// Mask values, use | to combine multiple options.
		/// </summary>
		[Flags]
		public enum AllowedOrientation {
			kPortrait = 1 << 0,
			kPortraitUpsideDown = 1 << 1,
			kLandscapeLeft = 1 << 2,
			kLandscapeRight = 1 << 3
		}

		/// <summary>
		/// Mask of orientations it is allowed to rotate too.
		/// </summary>
		private AllowedOrientation allowedOrientations_;

		/// <summary>
		/// Generates the <seealso cref="AllowedOrientation"/>s from the player settings in the editor.
		/// </summary>
		protected new void Awake() {
			base.Awake();

			if (Screen.autorotateToPortrait) {
				allowedOrientations_ = allowedOrientations_ | AllowedOrientation.kPortrait;
			}
			if (Screen.autorotateToPortraitUpsideDown) {
				allowedOrientations_ = allowedOrientations_ | AllowedOrientation.kPortraitUpsideDown;
			}
			if (Screen.autorotateToLandscapeLeft) {
				allowedOrientations_ = allowedOrientations_ | AllowedOrientation.kLandscapeLeft;
			}
			if (Screen.autorotateToLandscapeRight) {
				allowedOrientations_ = allowedOrientations_ | AllowedOrientation.kLandscapeRight;
			}

			UpdateAutoRotationSettings();
		}

		/// <summary>
		/// Checks the devices current settings when the app has regained focus.
		/// </summary>
		/// <remarks>
		/// This happens when the user changes activity or goes outside the Unity Player i.e when user
		/// goes to settings bar and changes Auto-rotates.
		/// </remarks>
		/// <param name="focusStatus"> From Unity, true if the application has focus. </param>
		private void OnApplicationFocus(bool focusStatus) {
			if (focusStatus) {
				UpdateAutoRotationSettings();
			}
		}

		/// <summary>
		/// The currently allowed orientations, if the device has auto-rotate enabled.
		/// </summary>
		/// <remarks>
		/// This will be the same as the settings in player settings if it has not been modified.
		/// </remarks>
		/// <returns> The current allowed orientations for Auto-rotates. </returns>
		public static AllowedOrientation CurrentAllowedOrientations() {
			if (Instance) {
				return Instance.allowedOrientations_;
			}
			return 0;
		}

		/// <summary>
		/// Sets the current <seealso cref="AllowedOrientation"/>s mask, this does not force it to a valid orientation.
		/// <para/>
		/// Use <seealso cref="SetAllowedOrientation(AllowedOrientation,ScreenOrientation)"/> to
		/// force a change in the orientation.
		/// </summary>
		/// <param name="allowedAutoOrientations"> New mask of <seealso cref="AllowedOrientation"/>. </param>
		public static void SetAllowedOrientation(AllowedOrientation allowedOrientations) {
			if (Instance) {
				Instance.allowedOrientations_ = allowedOrientations;
				Instance.UpdateAutoRotationSettings();
			}
		}

		/// <summary>
		/// Sets the current <seealso cref="AllowedOrientation"/>s mask, and forces it to a specified orientation.
		/// </summary>
		/// <remarks>
		/// Worth noting that when auto-rotate is off on the device, <seealso cref="ScreenOrientation.PortraitUpsideDown"/>
		/// will only allow Portrait, but when they re-enable auto-rotate it will jump to upside down, best
		/// setting newOrientation to standard Landscape or Portrait.
		/// </remarks>
		/// <param name="allowedAutoOrientations"> New mask of <seealso cref="AllowedOrientation"/>s. </param>
		/// <param name="newOrientation"> Orientation the device will be forced to</param>
		public static void SetAllowedOrientation(AllowedOrientation allowedOrientations, ScreenOrientation newOrientation) {
			Screen.orientation = newOrientation;
			SetAllowedOrientation(allowedOrientations);
		}

		/// <summary>
		/// Checks if autorotation is possible then updates the applicable <seealso cref="Screen"/>.autorotate
		/// rotations that are allowed.
		/// </summary>
		private void UpdateAutoRotationSettings() {
			// Disable all the auto-rotate settings
			Screen.autorotateToPortrait = false;
			Screen.autorotateToPortraitUpsideDown = false;
			Screen.autorotateToLandscapeLeft = false;
			Screen.autorotateToLandscapeRight = false;

			if (AndroidDetectAutoRotation.IsAutoRotateEnabled()) {
				// Re enable any allowed auto-rotate settings.

				if (IsOrientationAllowed(AllowedOrientation.kPortrait)) {
					Screen.autorotateToPortrait = true;
				}
				if (IsOrientationAllowed(AllowedOrientation.kPortraitUpsideDown)) {
					Screen.autorotateToPortraitUpsideDown = true;
				}
				if (IsOrientationAllowed(AllowedOrientation.kLandscapeLeft)) {
					Screen.autorotateToLandscapeLeft = true;
				}
				if (IsOrientationAllowed(AllowedOrientation.kLandscapeRight)) {
					Screen.autorotateToLandscapeRight = true;
				}

				// Reset screen orientation to auto-rotate so it can change to an allowed orientation.
				// This does not force it to an allowed orientation if it is not on a correct orientation.
				// Use SetAllowedOrientation(AllowedOrientation,ScreenOrientation) to force a change
				// in the orientation.
				Screen.orientation = ScreenOrientation.AutoRotation;
			}
		}

		/// <summary>
		/// Checks if the passed in orientation is set in the <seealso cref="allowedOrientations_"/> mask.
		/// </summary>
		/// <param name="allowedOrientation"> The orientation to check is set. </param>
		/// <returns> True if it is set in the mask. </returns>
		private bool IsOrientationAllowed(AllowedOrientation allowedOrientation) {
			return ((allowedOrientations_ & allowedOrientation) != 0);
		}
	}
}
