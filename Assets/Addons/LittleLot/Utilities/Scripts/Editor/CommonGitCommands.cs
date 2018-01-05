using System;
using System.Diagnostics;

using UnityEditor;
using Debug = UnityEngine.Debug; // Stop ambiguous refrence with System.Diagnostics

/// <summary>
/// @StewMcc 14/12/2017
/// </summary>
namespace LittleLot {

	/// <summary>
	/// Exposes useful and commonly used git commands.
	/// </summary>
	public class CommonGitCommands {

		/// <summary>
		/// Updates the bundle version to major.minor.patch.commit.
		///
		/// Fails if in anunexpected format.
		/// </summary>
		[MenuItem("LittleLot/Update Bundle Version")]
		public static void UpdateBundleVersion() {
			try {
				// Cut up the version into Major.Minor.Patch.Commit.
				string currentVersion = PlayerSettings.bundleVersion;
				int major = Convert.ToInt32(currentVersion.Split('.')[0]);
				int minor = Convert.ToInt32(currentVersion.Split('.')[1]);
				int patch = Convert.ToInt32(currentVersion.Split('.')[2]);
				string commit = RetrieveCurrentCommitShorthash();

				PlayerSettings.bundleVersion = string.Format("{0}.{1}.{2}.{3}", major, minor, patch, commit);

				Debug.LogFormat("Updated Game Version: {0}.{1}.{2}.{3}", major, minor, patch, commit);
				AssetDatabase.SaveAssets();
				AssetDatabase.Refresh();
			} catch (Exception) {
				// Most likely failed to get major/minor/patch in correct format.
				Debug.LogErrorFormat("Unexpected bundleVersion Format: {0}\nExpected format: *.*.*.* ", PlayerSettings.bundleVersion);
			}
		}

		/// <summary>
		/// Checks whether the BundleVersion is valid, if not sets it to a default value.
		///
		/// Quick tool for making it easier to setup automatic bundle version with commit version.
		/// </summary>
		[MenuItem("LittleLot/Validate BundleVersion")]
		public static void ValidateBundleVersion() {
			try {
				string currentVersion = PlayerSettings.bundleVersion;
				int major = Convert.ToInt32(currentVersion.Split('.')[0]);
				int minor = Convert.ToInt32(currentVersion.Split('.')[1]);
				int patch = Convert.ToInt32(currentVersion.Split('.')[2]);
				string commit = currentVersion.Split('.')[3];
				UnityEngine.Debug.LogFormat("Valid BundleVersion: {0}.{1}.{2}.{3}", major, minor, patch, commit);
			} catch (Exception) {
				PlayerSettings.bundleVersion = "0.0.0.1";
				UnityEngine.Debug.LogError("Invalid BundleVersion setting to 0.0.0.1 ");
				AssetDatabase.SaveAssets();
				AssetDatabase.Refresh();
			}
		}

		/// <summary>
		/// Retrieves the current Git Commit.
		/// </summary>
		/// <returns></returns>
		[MenuItem("LittleLot/Git/Current Commit", priority = 2000)]
		public static string RetrieveCurrentCommitShorthash() {
			string result = RunGitCommand("rev-parse --short --verify HEAD");
			// Clean up whitespace around hash.
			result = string.Join("", result.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries));
			Debug.Log("Current Commit: " + result);
			return result;
		}

		/// <summary>
		/// Retrieves the current Git Branch.
		/// </summary>
		/// <returns></returns>
		[MenuItem("LittleLot/Git/Current Branch", priority = 2000)]
		public static string RetrieveCurrentBranch() {
			string result = RunGitCommand(@"rev-parse --abbrev-ref HEAD");

			result = string.Join("", result.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries));
			Debug.Log("Current Branch: " + result);
			return result;
		}

		/// <summary>
		/// Git stash, stash all changes and save as "Unity Stash".
		/// True if it succeeds.
		/// </summary>
		/// <returns></returns>
		[MenuItem("LittleLot/Git/Stash All Changes", priority = 3000)]
		public static bool Stash() {
			string result = RunGitCommand(@"stash push -m "" Unity Stash"" ");
			Debug.Log("Stashing: " + result);

			if (result == "No local changes to save" + Environment.NewLine) {
				// if nothing got stashed we say it failed.
				return false;
			}

			AssetDatabase.Refresh();
			return true;
		}

		/// <summary>
		/// Git stash pop, reapply the last stash if it exists.
		/// </summary>
		/// <returns></returns>
		[MenuItem("LittleLot/Git/Pop Last Stash", priority = 3000)]
		public static void Pop() {
			string result = RunGitCommand(@"stash pop");
			Debug.Log("Popping: \n" + result);
			AssetDatabase.Refresh();

		}

		public class Dangerous {
			/// <summary>
			/// DANGEROUS: Hard Resets the repository to the last commit.
			/// </summary>
			/// <returns></returns>
			[MenuItem("LittleLot/Git/Dangerous/Hard Reset ", priority = 4000)]
			public static void HardReset() {
				string result = RunGitCommand(@"reset --hard");
				Debug.LogWarning("Hard Resetting: \n" + result);
				AssetDatabase.Refresh();
			}

			/// <summary>
			/// DANGEROUS: Purges all changes from the last commit.
			/// </summary>
			/// <returns></returns>
			[MenuItem("LittleLot/Git/Dangerous/Purge ", priority = 4000)]
			public static void Purge() {
				string result = RunGitCommand(@"clean -d -x -f");
				Debug.LogWarning("Purging Repo: \n" + result);
				AssetDatabase.Refresh();
			}
		}

		/// <summary>
		/// Runs the provided git command.
		///
		/// Will throw an exception if the Command has failed, or results in a 'fatal' git command.
		/// Otherwise returns the result of the git command.
		/// </summary>
		/// <param name="gitCommand"> The Git command without the 'git' part. </param>
		/// <returns> The StandardOutput from the git command. </returns>
		public static string RunGitCommand(string gitCommand) {
			string output = "no-git";
			string errorOutput = "no-git";

			// Set up our process to run the git command. and log to output and errorOutput.
			ProcessStartInfo processInfo = new ProcessStartInfo("git", @gitCommand) {
				CreateNoWindow = true,
				UseShellExecute = false,
				RedirectStandardOutput = true,
				RedirectStandardError = true
			};

			Process process = new Process {
				StartInfo = processInfo
			};

			try {
				process.Start();
			} catch (Exception) {
				Debug.LogError("Git is not set-up correctly, required to be on PATH.");
				throw;
			}

			// Read command result.
			output = process.StandardOutput.ReadToEnd();
			errorOutput = process.StandardError.ReadToEnd();

			process.WaitForExit();
			process.Close();

			// Check for failure due to no git setup in the project itself or other fatal errors.
			if (output.Contains("fatal") || output == "no-git" || output == "") {
				throw new Exception("Command: git " + @gitCommand + " Failed\n" + output + errorOutput);
			}
			// Log any errors.
			if (errorOutput != "") {
				Debug.LogError("Git Error: " + errorOutput);
			}

			return output;
		}
	}
}
