using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// @StewMcc 14/12/2017
/// </summary>
namespace LittleLot {

	/// <summary>
	/// Workaround which adds the correct video decode shader for Android, OSX, and iOS. and removes unneeded shaders.
	/// </summary>
	public class VideoShaderBuildFix {

		private const string kGraphicsSettingsAssetPath = "ProjectSettings/GraphicsSettings.asset";
		private const string kAlwaysIncludedShaders = "m_AlwaysIncludedShaders";
		private const string kVideoDecoder = "Hidden/VideoDecode";
		private const string kVideoDecoderOsx = "Hidden/VideoDecodeOSX";
		private const string kVideoDecoderAndroid = "Hidden/VideoDecodeAndroid";

		public static void AddVideoDecodeShaders(BuildTarget buildTarget) {

			Debug.Log("Adding " + kVideoDecoder);

			SerializedObject graphicsSettingsObject = new SerializedObject(UnityEditor.AssetDatabase.LoadAllAssetsAtPath(kGraphicsSettingsAssetPath)[0]);
			SerializedProperty alwaysIncludedShaders = graphicsSettingsObject.FindProperty(kAlwaysIncludedShaders);

			// Always need atleast the video Decode Shader (iOS needs only this)
			AddPlatformVideoDecodeShader(alwaysIncludedShaders, kVideoDecoder);
			graphicsSettingsObject.ApplyModifiedProperties();

			if (buildTarget == BuildTarget.Android) {
				Debug.Log("Adding " + kVideoDecoderAndroid);
				AddPlatformVideoDecodeShader(alwaysIncludedShaders, kVideoDecoderAndroid);

				RemovePlatformVideoDecodeShader(alwaysIncludedShaders, kVideoDecoderOsx);
			}

			if (buildTarget == BuildTarget.StandaloneOSX) {

				Debug.Log("Adding " + kVideoDecoderAndroid);
				AddPlatformVideoDecodeShader(alwaysIncludedShaders, kVideoDecoderOsx);

				RemovePlatformVideoDecodeShader(alwaysIncludedShaders, kVideoDecoderAndroid);
			}
			graphicsSettingsObject.ApplyModifiedProperties();
		}

		[MenuItem("LittleLot/Git/RemoveVideoDecodeShaders")]
		public static void RemoveAllVideoDecodeShaders() {

			Debug.Log("Removing All VideoDecode Shaders");
			SerializedObject graphicsSettingsObject = new SerializedObject(UnityEditor.AssetDatabase.LoadAllAssetsAtPath(kGraphicsSettingsAssetPath)[0]);
			SerializedProperty alwaysIncludedShaders = graphicsSettingsObject.FindProperty(kAlwaysIncludedShaders);

			RemovePlatformVideoDecodeShader(alwaysIncludedShaders, kVideoDecoder);
			RemovePlatformVideoDecodeShader(alwaysIncludedShaders, kVideoDecoderOsx);
			RemovePlatformVideoDecodeShader(alwaysIncludedShaders, kVideoDecoderAndroid);
			graphicsSettingsObject.ApplyModifiedProperties();

		}
		static void AddPlatformVideoDecodeShader(SerializedProperty property, string shaderName) {
			for (int i = 0; i < property.arraySize; i++) {
				if (property.GetArrayElementAtIndex(i).objectReferenceValue == Shader.Find(shaderName)) {
					// already exists.
					return;
				}
			}
			// otherwise we add it.
			property.InsertArrayElementAtIndex(property.arraySize);
			property.GetArrayElementAtIndex(property.arraySize - 1).objectReferenceValue = Shader.Find(shaderName);

		}

		static void RemovePlatformVideoDecodeShader(SerializedProperty property, string shaderName) {
			int shaderIndex = -1;
			for (int i = 0; i < property.arraySize; i++) {
				if (property.GetArrayElementAtIndex(i).objectReferenceValue == Shader.Find(shaderName)) {
					shaderIndex = i;
				}
			}
			// if we found the shader we remove it.
			if (shaderIndex > -1) {
				property.GetArrayElementAtIndex(shaderIndex).objectReferenceValue = null;
				property.DeleteArrayElementAtIndex(shaderIndex);
			}

		}
	}
}
