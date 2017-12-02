using System.Collections;
using System.Collections.Generic;
using System.IO;

using UnityEngine;
using UnityEditor;
using UnityEditor.Build;


public class WwiseUnityAppBuildCallbacks : IPreprocessBuild, IPostprocessBuild {
    
    public int callbackOrder { get { return 0; } }
    public void OnPreprocessBuild(BuildTarget target, string path) {
        //get Wwise project file (.wproj) path
        string wwiseProjFile = Path.Combine(Application.dataPath, WwiseSetupWizard.Settings.WwiseProjectPath).Replace('/', '\\');

        //get Wwise project root folder path
        string wwiseProjectFolder = wwiseProjFile.Remove(wwiseProjFile.LastIndexOf(Path.DirectorySeparatorChar));

        //get Wwise platform string (the string isn't the same as unity for some platforms)
        string wwisePlatformString = UnityToWwisePlatformString(EditorUserBuildSettings.activeBuildTarget.ToString());

        //get soundbank location in the Wwise project for the current platform target
        string sourceSoundBankFolder = Path.Combine(wwiseProjectFolder, AkBasePathGetter.GetPlatformBasePath());

        //get soundbank destination in the Unity project for the current platform target
        string destinationSoundBankFolder = Path.Combine(Application.dataPath + Path.DirectorySeparatorChar + "StreamingAssets",    //Soundbank must be inside the StreamingAssets folder
            Path.Combine(WwiseSetupWizard.Settings.SoundbankPath, wwisePlatformString)
        );

        //Copy the soundbank from the Wwise project to the unity project (Inside the StreamingAssets folder as defined in Window->Wwise Settings)
        if (!AkUtilities.DirectoryCopy(sourceSoundBankFolder, 	//source folder
            destinationSoundBankFolder,                         //destination
            true						                        //copy subfolders
        )) {
            UnityEngine.Debug.LogError("WwiseUnity: The soundbank folder for the " + wwisePlatformString + " platform doesn't exist. Make sure it was generated in your Wwise project");
        }
        
        Debug.Log("WwiseUnityAppBuildCallbacks.OnPreprocessBuild for target " + target + " at path " + path);
    }
    
    public void OnPostprocessBuild(BuildTarget target, string path) {
        //get Wwise project file (.wproj) path
        string wwiseProjFile = Path.Combine(Application.dataPath, WwiseSetupWizard.Settings.WwiseProjectPath).Replace('/', '\\');

        //get Wwise project root folder path
        string wwiseProjectFolder = wwiseProjFile.Remove(wwiseProjFile.LastIndexOf(Path.DirectorySeparatorChar));

        //get Wwise platform string (the string isn't the same as unity for some platforms)
        string wwisePlatformString = UnityToWwisePlatformString(EditorUserBuildSettings.activeBuildTarget.ToString());

        //get soundbank location in the Wwise project for the current platform target
        string sourceSoundBankFolder = Path.Combine(wwiseProjectFolder, AkBasePathGetter.GetPlatformBasePath());

        //get soundbank destination in the Unity project for the current platform target
        string destinationSoundBankFolder = Path.Combine(Application.dataPath + Path.DirectorySeparatorChar + "StreamingAssets",    //Soundbank must be inside the StreamingAssets folder
            Path.Combine(WwiseSetupWizard.Settings.SoundbankPath, wwisePlatformString)
        );

        //Copy the soundbank from the Wwise project to the unity project (Inside the StreamingAssets folder as defined in Window->Wwise Settings)
        if (!AkUtilities.DirectoryCopy(sourceSoundBankFolder, 	//source folder
            destinationSoundBankFolder,                         //destination
            true						                        //copy subfolders
        )) {
            UnityEngine.Debug.LogError("WwiseUnity: The soundbank folder for the " + wwisePlatformString + " platform doesn't exist. Make sure it was generated in your Wwise project");
        }
        

        //Delete the soundbank from the unity project so they don't get copied in the game folder of future builds
        Directory.Delete(destinationSoundBankFolder, true);
        Debug.Log("WwiseUnityAppBuildCallbacks.OnPostprocessBuild for target " + target + " at path " + path);
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

        else if (unityPlatormString == BuildTarget.iOS.ToString())
            return "iOS";

        return unityPlatormString;
    }

}
