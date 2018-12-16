using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

// @StewMcc 03/05/18
namespace LittleLot.VersionControl {

	/// <summary>
	/// Scriptable object for storing persistent information about the Git state of the repository.
	/// </summary>
	/// <remarks>
	/// When editing scriptable objects remember to use <seealso cref="AssetDatabase.SaveAssets()"/>
	/// to save the changes to disk.
	/// </remarks>
	public class GitInformation : ScriptableSingleton<GitInformation> {

		[SerializeField, HideInInspector]
		private string currentGitCommitHash_ = "";

		[SerializeField, HideInInspector]
		private string currentGitBranch_ = "";

		public string CommitHash {
			get {
				return currentGitCommitHash_;
			}
		}

		public string Branch {
			get {
				return currentGitBranch_;
			}
		}

#if UNITY_EDITOR

		/// <summary>
		/// Sets the commit hash, only usable in Editor.
		/// </summary>
		/// <remarks>
		/// This function will set the <seealso cref="ScriptableSingleton&lt;T&gt;.Instance"/> to dirty.
		/// Remember to use <seealso cref="AssetDatabase.SaveAssets()"/> to save the changes to disk.
		/// </remarks>
		/// <param name="gitCommitHash"> The Git commit hash to store in the scriptable object. </param>
		public static void SetCommitHash(string gitCommitHash) {
			Instance.currentGitCommitHash_ = gitCommitHash;
			EditorUtility.SetDirty(Instance);
		}

		/// <summary>
		/// Sets the git branch, only usable in Editor.
		/// </summary>
		/// <remarks>
		/// This function will set the <seealso cref="ScriptableSingleton&lt;T&gt;.Instance"/> to dirty.
		/// Remember to use <seealso cref="AssetDatabase.SaveAssets()"/> to save the changes to disk.
		/// </remarks>
		/// <param name="gitBranch"> The Git branch to store in the scriptable object. </param>
		public static void SetGitBranch(string gitBranch) {
			Instance.currentGitBranch_ = gitBranch;
			EditorUtility.SetDirty(Instance);
		}
#endif

	}
}
