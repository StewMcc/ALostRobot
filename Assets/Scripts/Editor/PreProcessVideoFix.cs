using LittleLot;

using UnityEditor;
using UnityEditor.Build;

/// <summary>
/// Extends IPreprocessBuildWithReport to add the correct VideoDecode Shader at build time.
/// <para/>
/// <remarks>
/// This ensures the correct VideoDecode Shader is set at build time, this is mainly for when
/// building from batchmode.
/// </remarks>
/// </summary>
public class PreProcessVideoFix : IPreprocessBuild {

	public int callbackOrder { get { return 1; } }

	/// <summary>
	/// Adds the required shaders for Unity Video before building.
	/// </summary>
	/// <param name="target"> Current Build targets platform. </param>
	/// <param name="path"> Output path of the current build. </param>
	public void OnPreprocessBuild(BuildTarget target, string path) {
		VideoShaderBuildFix.AddVideoDecodeShaders(target);
	}
}

