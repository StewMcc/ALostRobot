
using System.Collections.Generic;
using System.IO;
using System.Text;

using UnityEditor;
using UnityEditor.Build;

using UnityEngine;

/// <summary>
/// Converts Wwise_Ids c++ file into c# without using python.
/// Based on https://gist.github.com/StewMcc/50576a0e471134f6658e463d17bff6ca
/// </summary>
public class WwiseIDConverter : IPreprocessBuild {
	private enum State {
		Init,
		Replacing,
	}

	public int callbackOrder { get { return 0; } }

	public void OnPreprocessBuild(BuildTarget target, string path) {
		Convert();
	}

	public static void Convert(string inputFilePath, string outputFilePath) {
		string progressTitle = "WwiseUnity: Converting SoundBank IDs";
		string progressMsg = "WwiseUnity: Converting C++ SoundBank IDs into C# ...";
		EditorUtility.DisplayProgressBar(progressTitle, progressMsg, 0.0f);

		var lines = System.IO.File.ReadAllLines(inputFilePath, new UTF8Encoding(false));
		List<string> outputLines = new List<string>();
		State state = State.Init;
		int lineNumber = 0;
		int totalNumberOfLines = lines.Length;
		foreach (var line in lines) {
			var tmpLine = line;
			if (state == State.Init) {
				if (tmpLine.Contains("namespace"))
					state = State.Replacing;
			}

			if (tmpLine.Contains("#endif")) {
				break;
			}

			if (state == State.Replacing) {
				tmpLine = tmpLine.Replace("namespace", "public class");
				tmpLine = tmpLine.Replace("public class AK", "namespace AKID");
				tmpLine = tmpLine.Replace("static const AkUniqueID", "public const uint");
				outputLines.Add(tmpLine);
			}
			lineNumber++;
			EditorUtility.DisplayProgressBar(progressTitle, progressMsg, lineNumber / totalNumberOfLines);
		}
		outputLines[0] = "namespace AKID";
		EditorUtility.DisplayProgressBar(progressTitle, progressMsg, 0.9f);
		System.IO.File.WriteAllLines(outputFilePath, outputLines.ToArray(), new UTF8Encoding(true));
		EditorUtility.ClearProgressBar();
		AssetDatabase.SaveAssets();
		AssetDatabase.Refresh();
		Debug.Log("New Wwise_IDs.cs Created: " + outputFilePath);

	}

    [UnityEditor.MenuItem("Assets/Wwise/Convert Wwise SoundBank IDs", false, (int)AkWwiseMenuOrder.ConvertIDs)]
    public static void Convert() {

		var inputFilePath = Application.dataPath +
			Path.DirectorySeparatorChar + "StreamingAssets" +
			Path.DirectorySeparatorChar + "Audio" +
			Path.DirectorySeparatorChar + "GeneratedSoundBanks" +
			Path.DirectorySeparatorChar + "Wwise_IDs.h";
		var outputFilePath = Application.dataPath +
			Path.DirectorySeparatorChar + "Scripts/Audio" +
			Path.DirectorySeparatorChar + "Wwise_IDs.cs";

		try {
			Convert(inputFilePath, outputFilePath);
		} catch (System.Exception e) {
			EditorUtility.ClearProgressBar();
			throw e;
		}
	}
}
