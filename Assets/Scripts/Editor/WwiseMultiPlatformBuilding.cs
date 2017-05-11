using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

public class WwiseSoundBankWindow : EditorWindow {

	static List<string> soundBanks = new List<string>();

	static List<string> defaultSoundBanks = new List<string>(new string[]{ "Init.bnk", "Audio_Soundbank.bnk" });

	/// <summary>
	/// Renders a custom editor window to expose certain settings.
	/// </summary>
	private void OnGUI() {
		titleContent.text = "Wwise Soundbank Selection";


		GUILayout.Label("InvalidOperationException: Operation is not valid due to the current state of the object...");

		GUILayout.Label("Error caused but can be ignored. Unity implementation of mono Error.");

		if (GUILayout.Button("Add Default Sound Banks")) {
			AddDefaultSoundBanks();
		}

		if (GUILayout.Button("Add More Sound Banks")) {
			soundBanks.Add("");
		}

		for (int i = 0; i < soundBanks.Count; i++) {
			GUILayout.BeginHorizontal();
			GUILayout.TextField(soundBanks[i]);
			if (GUILayout.Button("-", GUILayout.Width(20f))) {
				soundBanks.RemoveAt(i);
			}
			GUILayout.EndHorizontal();
		}
		GUILayout.BeginHorizontal();
		if (GUILayout.Button("Build")) {
			Close();
			BuildWwiseGame(false);
		}
		if (GUILayout.Button("Build And Run")) {
			Close();
			BuildWwiseGame(true);
		}
		GUILayout.EndHorizontal();
	}
	#if UNITY_CLOUD_BUILD
	public static void UcbTransferDefaultSoundBanks(UnityEngine.CloudBuild.BuildManifestObject manifest) {
		
		//get Wwise project file (.wproj) path
		string wwiseProjFile = Path.Combine(Application.dataPath, WwiseSetupWizard.Settings.WwiseProjectPath).Replace('/', '\\');

		//get Wwise project root folder path
		string wwiseProjectFolder = wwiseProjFile.Remove(wwiseProjFile.LastIndexOf(Path.DirectorySeparatorChar));

		//get Wwise platform string (the string isn't the same as unity for some platforms)
		string wwisePlatformString = UnityToWwisePlatformString(EditorUserBuildSettings.activeBuildTarget.ToString());

		//get soundbank location in the Wwise project for the current platform target
		string sourceSoundBankFolder = Path.Combine(wwiseProjectFolder, AkBasePathGetter.GetPlatformBasePath());

		//get soundbank location in the Wwise project for the current platform target
		sourceSoundBankFolder = Path.Combine(wwiseProjectFolder + Path.DirectorySeparatorChar, "GeneratedSoundBanks");
		sourceSoundBankFolder = Path.Combine(sourceSoundBankFolder + Path.DirectorySeparatorChar, wwisePlatformString);
		Debug.Log(sourceSoundBankFolder);

		//get soundbank destination in the Unity project for the current platform target
		string destinationSoundBankFolder = Path.Combine(Application.dataPath + Path.DirectorySeparatorChar + "StreamingAssets",
			Path.Combine(WwiseSetupWizard.Settings.SoundbankPath, wwisePlatformString));

		// loop through soundbanks and copy them over.
		foreach (string soundBank in defaultSoundBanks) {
			string sourcefile = Path.Combine(sourceSoundBankFolder, soundBank);
			string destinationfile = Path.Combine(destinationSoundBankFolder, soundBank);

			FileUtil.CopyFileOrDirectory(sourcefile, destinationfile);

		}
	}
#endif
	/// <summary>
	/// Expose the Build version Manager in the editor.
	/// Simplifies the proccess and ensures everything is on the same style.
	/// </summary>	
	[MenuItem("Build With Wwise/Select Sound Banks", false, 0)]
	public static void ShowWindow() {
		GetWindow(typeof(WwiseSoundBankWindow), true);
	}

	/// <summary>
	/// Adds the default soundbanks to the Wwise build.
	/// </summary>
	private static void AddDefaultSoundBanks() {
		foreach (string soundBank in defaultSoundBanks) {
			soundBanks.Add(soundBank);
		}
	}

	/// <summary>
	/// Builds the game with wwise integration for the current platform.
	/// Moves the required SoundBanks over to StreamingAssets folder,
	/// and deletes them when completed.
	/// </summary>
	/// <param name="autoRun"> With to launch the player after build finishes.</param>
	/// <returns> True if succesfull.</returns>
	private static bool BuildWwiseGame(bool autoRun) {
		//Choose app name and location
		string appPath = EditorUtility.SaveFilePanel("Save Wwise Build",
			Application.dataPath.Remove(Application.dataPath.LastIndexOf('/')),
			"WwiseBuild",
			getPlatFormExtension()
			);

		//check if the build was cancelled
		bool isUserCancelledBuild = appPath == "";
		if (isUserCancelledBuild) {
			UnityEngine.Debug.Log("WwiseUnity: User cancelled the build.");
			return false;
		}

		//get Wwise project file (.wproj) path
		string wwiseProjFile = Path.Combine(Application.dataPath, WwiseSetupWizard.Settings.WwiseProjectPath).Replace('/', '\\');

		//get Wwise project root folder path
		string wwiseProjectFolder = wwiseProjFile.Remove(wwiseProjFile.LastIndexOf(Path.DirectorySeparatorChar));

		//get Wwise platform string (the string isn't the same as unity for some platforms)
		string wwisePlatformString = UnityToWwisePlatformString(EditorUserBuildSettings.activeBuildTarget.ToString());

		//get soundbank location in the Wwise project for the current platform target
		string sourceSoundBankFolder = Path.Combine(wwiseProjectFolder, AkBasePathGetter.GetPlatformBasePath());

		//get soundbank location in the Wwise project for the current platform target
		sourceSoundBankFolder = Path.Combine(wwiseProjectFolder + Path.DirectorySeparatorChar, "GeneratedSoundBanks");
		sourceSoundBankFolder = Path.Combine(sourceSoundBankFolder + Path.DirectorySeparatorChar, wwisePlatformString);
		Debug.Log(sourceSoundBankFolder);

		//get soundbank destination in the Unity project for the current platform target
		string destinationSoundBankFolder = Path.Combine(Application.dataPath + Path.DirectorySeparatorChar + "StreamingAssets",
			Path.Combine(WwiseSetupWizard.Settings.SoundbankPath, wwisePlatformString));

		// loop through soundbanks and copy them over.
		foreach (string soundBank in soundBanks) {
			string sourcefile = Path.Combine(sourceSoundBankFolder, soundBank);
			string destinationfile = Path.Combine(destinationSoundBankFolder, soundBank);

			FileUtil.CopyFileOrDirectory(sourcefile, destinationfile);

		}

		//Get all the scenes to build as defined in File->Build Settings
		string[] scenes = new string[EditorBuildSettings.scenes.Length];
		for (int i = 0; i < EditorBuildSettings.scenes.Length; i++) {
			scenes[i] = EditorBuildSettings.scenes[i].path;
		}
		if (autoRun) {
			//Build the app
			BuildPipeline.BuildPlayer(scenes,                                       //scenes to build 
											appPath,                                    //Location of the app to create
											EditorUserBuildSettings.activeBuildTarget,  //Platform for which to build the app 
											BuildOptions.AutoRunPlayer
										);

		}
		else {
			//Build the app
			BuildPipeline.BuildPlayer(scenes,                                       //scenes to build 
											appPath,                                    //Location of the app to create
											EditorUserBuildSettings.activeBuildTarget,  //Platform for which to build the app 
											BuildOptions.None
										);
		}


		// Delete the soundbank from the unity project so they dont get copied in the game folder of future builds		
		foreach (string soundBank in soundBanks) {
			string destinationfile = Path.Combine(destinationSoundBankFolder, soundBank);
			FileUtil.DeleteFileOrDirectory(destinationfile);
			// meta files
			destinationfile = destinationfile + ".meta";
			FileUtil.DeleteFileOrDirectory(destinationfile);
		}


		return true;
	}

	private static string UnityToWwisePlatformString(string unityPlatormString) {
		if (unityPlatormString == BuildTarget.StandaloneWindows.ToString()
			||
			unityPlatormString == BuildTarget.StandaloneWindows64.ToString()
		   )
			return "Windows";

		else if (unityPlatormString == BuildTarget.StandaloneOSXIntel.ToString()
					||
					unityPlatormString == BuildTarget.StandaloneOSXIntel64.ToString()
					||
					unityPlatormString == BuildTarget.StandaloneOSXUniversal.ToString()
				)
			return "Mac";

#if UNITY_5
		else if (unityPlatormString == BuildTarget.iOS.ToString())
#else
		else if (unityPlatormString == BuildTarget.iPhone.ToString())
#endif
			return "iOS";
#if !UNITY_5_5_OR_NEWER
		else if (unityPlatormString == BuildTarget.XBOX360.ToString())
			return "Xbox360";
#endif

		//Android, PS3 and Wii have the correct strings in Wwise v2013.2.7 and Unity version 4.3.4
		return unityPlatormString;
	}

	private static string getPlatFormExtension() {
		string unityPlatormString = EditorUserBuildSettings.activeBuildTarget.ToString();

		if (unityPlatormString == BuildTarget.StandaloneWindows.ToString()
			||
			unityPlatormString == BuildTarget.StandaloneWindows64.ToString()
		   )
			return "exe";

		else if (unityPlatormString == BuildTarget.StandaloneOSXIntel.ToString()
					||
					unityPlatormString == BuildTarget.StandaloneOSXIntel64.ToString()
					||
					unityPlatormString == BuildTarget.StandaloneOSXUniversal.ToString()
				)
			return "app";

#if UNITY_5
		else if (unityPlatormString == BuildTarget.iOS.ToString())
#else
		else if (unityPlatormString == BuildTarget.iPhone.ToString())
#endif
			return "ipa";

		else if (unityPlatormString == BuildTarget.Android.ToString())
			return "apk";
#if !UNITY_5_5_OR_NEWER
		else if (unityPlatormString == BuildTarget.XBOX360.ToString())
			return "XEX";
		else if (unityPlatormString == BuildTarget.PS3.ToString())
			return "self";
#endif
		return "";
	}

}
