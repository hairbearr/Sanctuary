#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class ProfPollatiUtilites : EditorWindow {
	// Adding a menu item, with "false" and then a number allows you to order items. Skipping by 10 should give you a divider
	[MenuItem("Pollati Utilities/Go to GitHub page...",false,101)]
	public static void OpenGitHub() {
		Application.OpenURL("https://github.com/ProfPollati/ProfPollatiUnityUtilities");
	}

	[MenuItem("Pollati Utilities/Go to latest releases...",false,102)]
	public static void OpenLatestRelases() {
		Application.OpenURL("https://github.com/ProfPollati/ProfPollatiUnityUtilities/releases/latest");
	}
}
#endif