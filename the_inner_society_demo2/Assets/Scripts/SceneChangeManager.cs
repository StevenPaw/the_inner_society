using System.Collections.Generic;
using UnityEditor;
using UnityEngine.SceneManagement;

namespace farmingsim
{
    public static class SceneChangeManager
    {
        private static Dictionary<string, Scene> scenes;

        public static Dictionary<string, Scene> Scenes
        {
            get => scenes;
            set => scenes = value;
        }

        public static bool ChangeScene(string newScene)
        {
            if(SceneMan)
        }
    }
}