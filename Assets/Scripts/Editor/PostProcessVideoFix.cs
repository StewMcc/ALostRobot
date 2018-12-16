using LittleLot;

using UnityEditor;
using UnityEditor.Build;

/// <summary>
/// Extends IPostprocessBuild to remove the VideoDecode Shader after a build.
/// <para/>
/// <remarks>
/// This stops the VCS getting polluted with video decode shaders for different platforms
/// after a build.
/// </remarks>
/// </summary>
public class PostProcessVideoFix : IPostprocessBuild {

	public int callbackOrder { get { return 1; } }

	/// <summary>
	/// Removes all the shaders for Unity Video after building.
	/// <para/>
	/// <remarks>
	/// Ensures source control isn't cluttered with incorrect platform shaders.
	/// </remarks>
	/// </summary>
	/// <param name="target"> Current Build targets platform. </param>
	/// <param name="path"> Output path of the current build. </param>
	public void OnPostprocessBuild(BuildTarget target, string path) {
		VideoShaderBuildFix.RemoveAllVideoDecodeShaders();
	}
}

