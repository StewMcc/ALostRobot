using System;

using LittleLot.VersionControl;

using UnityEngine;
using UnityEngine.UI;

namespace LittleLotExamples {

	public class DisplayGitInfo : MonoBehaviour {

		[SerializeField]
		GitInformation gitInformation = null;
		[SerializeField]
		Text displayGitCommit = null;

		private void Start() {
			displayGitCommit.text = string.Format("Version: {1}{0}Branch: {2}{0}Commit: {3}{0}",
												  Environment.NewLine,
												  Application.version,
												  gitInformation.Branch,
												  gitInformation.CommitHash);
		}
	}
}
