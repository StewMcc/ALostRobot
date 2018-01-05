using UnityEditor;
using UnityEditor.Build;
using UnityEngine;

/// <summary>
/// Extends IPreprocessBuild & IPostprocessBuild to add the correct VideoDecode Shader at build time.
///
/// This ensures the correct VideoDecode Shader is set at build time, this is mainly for when 
/// building from batchmode.
/// </summary>
public class PreProcessBuildVideoFix : IPreprocessBuild, IPostprocessBuild {

	public int callbackOrder { get { return 1; } }

	public void OnPreprocessBuild(BuildTarget target, string path) {
		// add the target shaders.
		LittleLot.VideoShaderBuildFix.AddVideoDecodeShaders(target);
	}

	void IPostprocessBuild.OnPostprocessBuild(BuildTarget target, string path) {
		// remove all of thenm.
		LittleLot.VideoShaderBuildFix.RemoveAllVideoDecodeShaders();
	}
}
