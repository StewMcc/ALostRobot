using LittleLot.VersionControl;

using UnityEditor;
using UnityEditor.Build;

/// <summary>
/// Extends IPreprocessBuildWithReport to update the Git commit and branch before a build.
/// <para/>
/// <remarks>
/// This ensures the git information supplied to the build is up to date.
/// </remarks>
/// </summary>
public class PreProcessUpdateGitInformation : IPreprocessBuild {

	public int callbackOrder { get { return 1; } }

	/// <summary>
	/// Updates the Git commit and branch in GitInfo before building.
	/// </summary>
	/// <param name="target"> Current Build targets platform. </param>
	/// <param name="path"> Output path of the current build. </param>
	public void OnPreprocessBuild(BuildTarget target, string path) {

		CommonGitCommands.UpdateGitInformation();
	}
}

