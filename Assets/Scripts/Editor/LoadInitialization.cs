using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

[InitializeOnLoad]
public class LoadInitialization
{
    static string initializationScene => EditorBuildSettings.scenes[0].path;

    //if there are any new non-test scenes we want to add, put it here and it add it to the ShouldLoadInitScene method
    static string mainMenuScene = EditorBuildSettings.scenes[1].path;
    static string gameScene = EditorBuildSettings.scenes[2].path;

    private static bool restartingToSwitchScene;

    // to track where to go back to
    static string PreviousScene
    {
        get => EditorPrefs.GetString("Previous Scene");
        set => EditorPrefs.SetString("Previous Scene", value);
    }

    static LoadInitialization()
    {
        EditorApplication.playModeStateChanged += EditorApplicationOnplayModeStateChanged;
    }

    private static bool ShouldLoadInitScene()
    {
        string name = SceneManager.GetActiveScene().path;
        return name == mainMenuScene || name == gameScene || name == initializationScene;
    }

    private static void EditorApplicationOnplayModeStateChanged(PlayModeStateChange playModeStateChange)
    {
        if (!ShouldLoadInitScene())
        {
            return;
        }

        //this function will get called multiple times when it's changing scenes because of how it does it, so we make sure
        //that it doesn't make any changes after the first instance
        //restartingToSwitchScene is set to true when it first tries to switch, and the code first exits play mode then enters play mode again, so
        //this handles both of those changes
        if (restartingToSwitchScene)
        {
            if (playModeStateChange == PlayModeStateChange.EnteredPlayMode)
            {
                restartingToSwitchScene = false;
            }
            return;
        }

        if (playModeStateChange == PlayModeStateChange.ExitingEditMode)
        {
            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                // user either hit "Save" or "Don't Save"; open initialization scene
                var activeScene = SceneManager.GetActiveScene();
                // cache previous scene so we return to this scene after play session, if possible
                PreviousScene = activeScene.path;

                // we only manually inject init scene if we are in a blank empty scene,
                // or if the active scene is not already the init scene
                restartingToSwitchScene = activeScene.path == string.Empty || !initializationScene.Contains(activeScene.path);
                if (restartingToSwitchScene)
                {
                    EditorApplication.isPlaying = false;

                    // scene is included in build settings; open it
                    EditorSceneManager.OpenScene(initializationScene);

                    EditorApplication.isPlaying = true;
                }
            }
            else
            {
                // user either hit "Cancel" or exited window; don't open bootstrap scene & return to editor
                EditorApplication.isPlaying = false;
            }
        }
        else if (playModeStateChange == PlayModeStateChange.EnteredEditMode)
        {
            if (!string.IsNullOrEmpty(PreviousScene))
            {
                EditorSceneManager.OpenScene(PreviousScene);
            }
        }
    }
}
