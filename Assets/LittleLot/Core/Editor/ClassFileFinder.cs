using System.Collections.Generic;
using System.IO;
using System.Linq;

using UnityEngine;

// @StewMcc 20/10/2018
namespace LittleLot {

	/// <summary>
	/// Useful for trying to find a class file within the project.
	/// </summary>
	/// <remarks>
	/// https://stackoverflow.com/questions/18697865/find-a-class-its-cs-file-in-project
	/// Modified so it tries to matches namespace as well, worth noting it relies on single
	/// space between class or namespace and name, and only works with .cs files.
	/// It also isn't tied into a specific IDE as well.
	/// </remarks>
	public static class ClassFileFinder {

		static List<string> classPaths_ = new List<string>();

		/// <summary>
		/// Searches through everything in <seealso cref="Application.dataPath"/>
		/// and tries to find the file path of the type passed in.
		/// </summary>
		/// <param name="t"> The type to search for. </param>
		/// <returns> The fully qualified file path of the class, or NULL if not found. </returns>
		public static string FindClassFile(System.Type t) {

			// Lookup class name in file names
			classPaths_.Clear();
			// fill in cs file Paths
			FindAllScriptFiles(Application.dataPath);
			for (int i = 0; i < classPaths_.Count; i++) {
				if (classPaths_[i].Contains(t.Name)) {
					if (FileContainsClassAndNameSpace(t, classPaths_[i])) {
						return classPaths_[i];
					}
				}
			}
			// Lookup class name in the class file text
			for (int i = 0; i < classPaths_.Count; i++) {
				string codeFile = File.ReadAllText(classPaths_[i]);
				if (codeFile.Contains("class " + t.Name)) {
					if (FileContainsClassAndNameSpace(t, classPaths_[i])) {
						return classPaths_[i];
					}
				}
			}
			return null;
		}

		private static bool FileContainsClassAndNameSpace(System.Type t, string path) {
			string codeFile = File.ReadAllText(path);

			if (codeFile.Contains("class " + t.Name)) {
				if (string.IsNullOrEmpty(t.Namespace)) {
					return true;
				}

				string[] allNamespaces = t.Namespace.Split('.');
				if (allNamespaces.All(codeFile.Contains) && codeFile.Contains("namespace ")) {
					return true;
				}
			}
			return false;
		}

		private static void FindAllScriptFiles(string startDir) {
			try {
				foreach (string file in Directory.GetFiles(startDir)) {
					if (file.Contains(".cs"))
						classPaths_.Add(file);
				}
				foreach (string dir in Directory.GetDirectories(startDir)) {
					FindAllScriptFiles(dir);
				}
			} catch (System.Exception ex) {
				Debug.Log(ex.Message);
			}
		}
	}
}