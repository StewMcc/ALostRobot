//#undef UNITY_EDITOR // Lets you edit Android code easily with formatting, comment out before going back to editor.
#if UNITY_ANDROID && !UNITY_EDITOR  // stop auto formatter removing unused using.
using UnityEngine;
#endif

// @StewMcc 21/02/2018
namespace LittleLot.Android {

	/// <summary>
	/// Used for detecting if Auto rotation is enabled on android devices.
	/// <para/>
	/// There is an example of this being used in the <seealso cref="AutoRotationMaintainer"/> for
	/// maintaining the users auto rotations settings whilst also allowing the apps rotation options.
	/// </summary>
	public static class AndroidDetectAutoRotation {

		/// <summary>
		/// Checks if Android Device has auto rotation enabled.
		/// </summary>
		/// <returns> True if AutoRotate is enabled, is not Android, or it fails. </returns>
		public static bool IsAutoRotateEnabled() {
			bool isAutoRotateEnabled = true;
#if UNITY_ANDROID && !UNITY_EDITOR
			try {
				// $ as System is subclass of Settings?
				using (AndroidJavaClass systemSettings = new AndroidJavaClass("android.provider.Settings$System"))
				using (AndroidJavaObject contentResolver = CommonAndroid.GetUnityActivity().Call<AndroidJavaObject>("getContentResolver")) {
					isAutoRotateEnabled = (systemSettings.CallStatic<int>("getInt", contentResolver, "accelerometer_rotation") == 1);
				}
			} catch (System.Exception e) {
				Debug.LogError(e);
			}
#endif
			return isAutoRotateEnabled;
		}
	}
}
