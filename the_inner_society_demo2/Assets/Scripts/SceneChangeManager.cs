using System.Collections.Generic;
using UnityEditor;
using UnityEngine.SceneManagement;

namespace farmingsim
{
    public static class SceneChangeManager
    {
        private static Dictionary<string, string> scenes;
        private static string firstScene;

        public static string FirstScene
        {
            get => firstScene;
            set => firstScene = value;
        }

        public static Dictionary<string, string> Scenes
        {
            get => scenes;
            set => scenes = value;
        }

        public static void InitiateSceneChangeManager()
        {
            scenes = new Dictionary<string, string>();
        }

        public static void LoadAllScenes()
        {
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(firstScene));
        }

        public static bool ChangeScene(string newScene)
        {
            string newLoadedSceneString;
            Scenes.TryGetValue(newScene, out newLoadedSceneString);
            SceneManager.LoadScene(newLoadedSceneString);
            return true;
        }
    }
}