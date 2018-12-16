//#undef UNITY_EDITOR // Lets you edit Android code easily with formatting, comment out before going back to editor.
#if UNITY_ANDROID && !UNITY_EDITOR  // stop auto formatter removing unused using.
using UnityEngine;
#endif

// @StewMcc 21/02/2018
namespace LittleLot.Android {

	/// <summary>
	/// Used for retrieving common Android classes, and objects for use with plug-ins or JNI calls.
	/// </summary>
	public static class CommonAndroid {

#if UNITY_ANDROID && !UNITY_EDITOR
		private static AndroidJavaClass androidUnityActivity_ = null;

		/// <summary>
		/// Gets the current UnityActivity used on Android.
		/// </summary>
		/// <remarks>
		/// It will store the AndroidJavaClass for later use ensuring it is not creating a new
		/// class in memory every call.
		/// </remarks>
		/// <returns> The AndroidActivity with the UnityPlayer running in it. </returns>
		public static AndroidJavaObject GetUnityActivity() {
			if (androidUnityActivity_ == null) {
				androidUnityActivity_ = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			}
			return androidUnityActivity_.GetStatic<AndroidJavaObject>("currentActivity");
		}
#endif
	}
}