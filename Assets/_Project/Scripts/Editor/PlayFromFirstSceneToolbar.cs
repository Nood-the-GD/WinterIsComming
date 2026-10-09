using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEditor.Toolbars;

namespace Piano7.EditorTools
{
    /// <summary>
    /// Adds a toolbar button that enters Play mode starting from the first
    /// enabled scene in Build Settings, no matter which scene is currently open.
    /// </summary>
    public static class PlayFromFirstSceneToolbar
    {
        const string ElementId = "Piano7/PlayFromFirstScene";

        [MainToolbarElement(ElementId, defaultDockPosition = MainToolbarDockPosition.Middle)]
        static MainToolbarElement CreateButton()
        {
            var icon = EditorGUIUtility.IconContent("PlayButton").image as Texture2D;
            var content = new MainToolbarContent(
                EditorApplication.isPlaying ? "Stop Game" : "Start Game",
                icon,
                "Enter Play mode starting from the first scene in Build Settings (works from any scene)");

            return new MainToolbarButton(content, TogglePlayFromFirstScene);
        }

        static void TogglePlayFromFirstScene()
        {
            if (EditorApplication.isPlaying)
            {
                EditorApplication.isPlaying = false;
                return;
            }

            var firstScenePath = EditorBuildSettings.scenes
                .FirstOrDefault(s => s.enabled)?.path;

            if (string.IsNullOrEmpty(firstScenePath))
            {
                EditorUtility.DisplayDialog(
                    "Play First Scene",
                    "No enabled scene found in Build Settings.",
                    "OK");
                return;
            }

            var startScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(firstScenePath);
            if (startScene == null)
            {
                EditorUtility.DisplayDialog(
                    "Play First Scene",
                    $"Could not load the first scene at:\n{firstScenePath}",
                    "OK");
                return;
            }

            // Give the user a chance to save any unsaved scene changes first.
            if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                return;

            EditorSceneManager.playModeStartScene = startScene;
            EditorApplication.isPlaying = true;
        }

        // When leaving Play mode, clear the override so the normal Play button
        // behaves as usual again.
        [InitializeOnLoadMethod]
        static void RegisterPlayModeCleanup()
        {
            EditorApplication.playModeStateChanged += state =>
            {
                if (state == PlayModeStateChange.EnteredEditMode)
                    EditorSceneManager.playModeStartScene = null;

                if (state == PlayModeStateChange.EnteredEditMode || state == PlayModeStateChange.EnteredPlayMode)
                    MainToolbar.Refresh(ElementId);
            };
        }
    }
}
