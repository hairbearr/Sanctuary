#if UNITY_EDITOR
using UnityEditor;
using System;
using UnityEngine;
using System.IO;
using Ionic.Zip;

public class BuildBuddy : MonoBehaviour {
	// Makes a keyboard shortcut by adding a space then "%" for Cmd or Ctrl, "&" for Alt
	[MenuItem("Pollati Utilities/Build Buddy  %&b",false,1)]
	public static void BuildGame () {
		BuildTarget[] targets = {
#if UNITY_2019_2_OR_NEWER
			BuildTarget.StandaloneLinux64,
#else
			BuildTarget.StandaloneLinuxUniversal,
#endif
			BuildTarget.StandaloneOSX,
			BuildTarget.StandaloneWindows,
			BuildTarget.StandaloneWindows64
		};

		string[] targetPrefix = {"-Linux","-Mac","-Win_x86","-Win_x86-64"};

		string baseDir = "Builds" + Path.DirectorySeparatorChar;
		string projectName = PlayerSettings.productName;
		string buildPath;
		string buildName;

		string failures = "";

		for(var i=0;i<targets.Length;i++) {
			// Figure the name of the folder to build to
			buildPath = baseDir + projectName + targetPrefix [i];

			// Figure the extension for the executable
			if (targets[i]==BuildTarget.StandaloneOSX) {
				buildName = projectName + ".app";
			} else {
				buildName = projectName + ".exe";
			}

			try {
				// Build
				UnityEngine.Debug.Log ("Building"+targetPrefix[i]);
				BuildPipeline.BuildPlayer(EditorBuildSettings.scenes, buildPath + Path.DirectorySeparatorChar + buildName, targets[i], BuildOptions.None);

				// Zip
				UnityEngine.Debug.Log ("Zipping"+targetPrefix[i]);
				using (ZipFile zip = new ZipFile()) {
					zip.AddDirectory (buildPath + Path.DirectorySeparatorChar);
					zip.Save(buildPath + ".zip");
				}

				// Done
				UnityEngine.Debug.Log ("Building"+targetPrefix[i]+ " Complete!");
			} catch(Exception e) {
				// Failed...
				UnityEngine.Debug.Log ("Building"+targetPrefix[i]+ " FAILED! " + e.ToString());
				failures += "\nFailed Building"+targetPrefix[i];
			}
		}

		// Show a dialog so that the user can also open the folder with the builds in it if they want
		string buildCompleteMessage = "Your builds were created for Pollati's class!";
		if(failures.Length>0) {
			buildCompleteMessage += "\n\nHowever, the following targets were not built:\n" + failures;
		}

		if (EditorUtility.DisplayDialog ("Builds complete!", buildCompleteMessage, "Show Build Folder", "Okay")) {
			EditorUtility.RevealInFinder (baseDir);
		}
	}
}
#endif
