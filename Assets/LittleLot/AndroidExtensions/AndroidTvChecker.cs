//#undef UNITY_EDITOR
#if UNITY_ANDROID && !UNITY_EDITOR  // stop auto formatter removing unused using.
using UnityEngine;
#endif

// @StewMcc 21/02/2018
namespace LittleLot.Android {

	/// <summary>
	/// Used to retrieve if the hardware is set to AndroidTv or FireTv
	/// </summary>
	public static class AndroidTvChecker {

		private static bool? isTv_ = null;
		/// <summary>
		/// Gets the current Android packageManager and checks if it is leanback or television.
		/// </summary>
		/// <returns> True if the device is an androidTv, false if not on android. </returns>
		public static bool IsAndroidOrFireTv() {
			if (!isTv_.HasValue) {
#if UNITY_ANDROID && !UNITY_EDITOR
				try {
					using (AndroidJavaObject context = CommonAndroid.GetUnityActivity().Call<AndroidJavaObject>("getApplicationContext"))
					using (AndroidJavaObject packageManager = context.Call<AndroidJavaObject>("getPackageManager")) {
						isTv_ = packageManager.Call<bool>("hasSystemFeature", "android.software.leanback") || packageManager.Call<bool>("hasSystemFeature", "android.hardware.type.television");
					}
				} catch (System.Exception e) {
					Debug.LogError("Couldn't Retrieve IsAndroidOrFireTv" + e);
					isTv_ = false;
				}
#else
				isTv_ = false;
#endif
			}

			return (bool)isTv_;
		}
	}
}
