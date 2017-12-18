﻿using System;
using System.IO;

using UnityEngine;
using UnityEditor;
using UnityEditor.Build;
using System.Diagnostics;

public class WwiseUnityAppBuildCallbacks : IPreprocessBuild, IPostprocessBuild {

    [SerializeField]
    private static string sourceSoundBankFolder = "";

    [SerializeField]
    private static string destinationSoundBankFolder = "";

    public int callbackOrder { get { return 0; } }

    public void OnPreprocessBuild(BuildTarget target, string path) {

        PopulateSourceAndDestinationFolderPaths();
        UnityEngine.Debug.Log("Copying "+sourceSoundBankFolder+ "\nTo "+destinationSoundBankFolder);

        //Copy the soundbank from the Wwise project to the unity project (Inside the StreamingAssets folder as defined in Window->Wwise Settings)
        if (!AkUtilities.DirectoryCopy(sourceSoundBankFolder, 	//source folder
            destinationSoundBankFolder,                         //destination
            true						                        //copy subfolders
        )) {
            UnityEngine.Debug.LogError("WwiseUnity: The soundbank folder for the " + target + " platform doesn't exist. Make sure it was generated in your Wwise project");
        }

        UpdateBundleVersion();
    }

    public void OnPostprocessBuild(BuildTarget target, string path) {

        PopulateSourceAndDestinationFolderPaths();

        UnityEngine.Debug.Log("Deleting "+destinationSoundBankFolder);
        //Delete the soundbank from the unity project so they don't get copied in the game folder of future builds
        Directory.Delete(destinationSoundBankFolder, true);
    }

    [MenuItem("Tools/Test-Source")]
    private static void PopulateSourceAndDestinationFolderPaths() {
        if (!(sourceSoundBankFolder == "" && destinationSoundBankFolder == "")) {
            return;
        }        
        
        //get Wwise platform string (the string isn't the same as unity for some platforms)
        string wwisePlatformString = UnityToWwisePlatformString(EditorUserBuildSettings.activeBuildTarget.ToString());
        
        // Get the soureSoundbank folder, and trim the incorrect OS off (will give you editor source)
        sourceSoundBankFolder = AkBasePathGetter.GetPlatformBasePath().TrimEnd(Path.DirectorySeparatorChar,Path.AltDirectorySeparatorChar);
        sourceSoundBankFolder = sourceSoundBankFolder.Remove(sourceSoundBankFolder.LastIndexOf(Path.DirectorySeparatorChar));
        sourceSoundBankFolder = Path.Combine(sourceSoundBankFolder,wwisePlatformString+Path.DirectorySeparatorChar);

        //get soundbank destination in the Unity project for the current platform target
        destinationSoundBankFolder = Path.Combine(Application.dataPath + Path.DirectorySeparatorChar + "StreamingAssets",    //Soundbank must be inside the StreamingAssets folder
            Path.Combine(WwiseSetupWizard.Settings.SoundbankPath, wwisePlatformString)
        );
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

    [MenuItem("Tools/Test-Git")]
    private static string RetrieveCommitShortHash() {
        string output = "no-git";

        ProcessStartInfo processInfo = new ProcessStartInfo("git", @"rev-parse --short HEAD");
        processInfo.CreateNoWindow = true;
        processInfo.UseShellExecute = false;
        processInfo.RedirectStandardOutput = true;

        Process process = new Process();
        process.StartInfo = processInfo;
        try {
            process.Start();
        } catch (System.Exception) {
            UnityEngine.Debug.Log("Current commit " + output);
            UnityEngine.Debug.LogError("Check Git is set-up correctly, required on PATH.\n");
            throw;
        }

        output = process.StandardOutput.ReadToEnd();
        // clean up whitespace
        output = string.Join("", output.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries));

        process.WaitForExit();
        process.Close();

        return output;
    }

    private static void UpdateBundleVersion() {
        string currentVersion = PlayerSettings.bundleVersion;
        int major = Convert.ToInt32(currentVersion.Split('.')[0]);
        int minor = Convert.ToInt32(currentVersion.Split('.')[1]);
        int patch = Convert.ToInt32(currentVersion.Split('.')[2]);
        string commit = RetrieveCommitShortHash();

        PlayerSettings.bundleVersion = string.Format("{0}.{1}.{2}.{3}", major, minor, patch, commit);

        UnityEngine.Debug.LogFormat("Updated Game Version: {0}.{1}.{2}.{3}", major, minor, patch, commit);
    }
}