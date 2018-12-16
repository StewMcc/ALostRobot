using System;

using UnityEditor;

using Debug = UnityEngine.Debug; // Stop ambiguous reference with System.Diagnostics

// @StewMcc 14/12/2017
namespace LittleLot.VersionControl {

	/// <summary>
	/// Exposes useful and commonly used git commands.
	/// </summary>
	public static class CommonGitCommands {

		private const int kGitMenuPosition = 3500;

		/// <summary>
		/// Updates the Git Information stored in the scriptable object.
		/// <para/>
		/// Throws exceptions if no git PATH set, or not a git repository.
		/// </summary>
		[MenuItem("Tools/LittleLot/Update Git Information", priority = 100)]
		public static void UpdateGitInformation() {

			// Run the Git Command to get the commit short hash.
			// Throws exceptions if it fails, we want a build to fail if this happens.
			string commit = RetrieveCurrentCommitShorthash();

			string branch = RetrieveCurrentBranch();

			GitInformation.SetCommitHash(commit);
			GitInformation.SetGitBranch(branch);

			// This is to ensure that it is saved properly, if you were to build afterwards it would reset to library setting.
			AssetDatabase.SaveAssets();
			// This Ensures that it will be used if we build directly after calling the function.
			AssetDatabase.Refresh();
		}

		/// <summary>
		/// Retrieves the current Git commit.
		/// </summary>
		/// <returns> The current commits short hash 6 chars instead of ...lots, e.g. 2bb53a. </returns>
		[MenuItem("Tools/LittleLot/Git/Current Commit", priority = kGitMenuPosition + 10)]
		public static string RetrieveCurrentCommitShorthash() {
			string result = GitCommand.RunGitCommand("rev-parse --short --verify HEAD");
			// Clean up whitespace around hash. (seems to just be the way this command returns :/ )
			result = string.Join("", result.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries));
			Debug.Log("Current Commit: " + result);
			return result;
		}

		/// <summary>
		/// Retrieves the current Git Branch.
		/// </summary>
		/// <returns> The current Git branch. </returns>
		[MenuItem("Tools/LittleLot/Git/Current Branch", priority = kGitMenuPosition + 20)]
		public static string RetrieveCurrentBranch() {
			string result = GitCommand.RunGitCommand(@"rev-parse --abbrev-ref HEAD");

			result = string.Join("", result.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries));
			Debug.Log("Current Branch: " + result);
			return result;
		}

		/// <summary>
		/// Git stash, stash all changes and save as "Unity Stash".
		/// True if it succeeds.
		/// </summary>
		/// <returns> True if successfully stashes any changes. </returns>
		[MenuItem("Tools/LittleLot/Git/Stash All Changes", priority = kGitMenuPosition + 100)]
		public static bool Stash() {
			string result = GitCommand.RunGitCommand(@"stash push -m "" Unity Stash"" ");
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
		[MenuItem("Tools/LittleLot/Git/Pop Last Stash", priority = kGitMenuPosition + 110)]
		public static void Pop() {
			string result = GitCommand.RunGitCommand(@"stash pop");
			Debug.Log("Popping: \n" + result);
			AssetDatabase.Refresh();
		}

		public class Dangerous {
			/// <summary>
			/// DANGEROUS: Hard Resets the repository to the last commit.
			/// </summary>
			[MenuItem("Tools/LittleLot/Git/Dangerous/Hard Reset ", priority = kGitMenuPosition + 200)]
			public static void HardReset() {
				string result = GitCommand.RunGitCommand(@"reset --hard");
				Debug.LogWarning("Hard Resetting: \n" + result);
				AssetDatabase.Refresh();
			}

			/// <summary>
			/// DANGEROUS: Purges all changes from the last commit.
			/// </summary>
			[MenuItem("Tools/LittleLot/Git/Dangerous/Purge ", priority = kGitMenuPosition + 200)]
			public static void Purge() {
				string result = GitCommand.RunGitCommand(@"clean -d -x -f");
				Debug.LogWarning("Purging Repo: \n" + result);
				AssetDatabase.Refresh();
			}
		}

	}
}
