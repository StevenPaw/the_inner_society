using UnityEngine;
using UnityEngine.SceneManagement;

namespace InnerSociety
{
    public class MainMenuManager : MonoBehaviour
    {
        public void StartPrototype()
        {
            SceneManager.LoadScene(Scenes.GAME);
        }

        public void OpenCredits()
        {
            SceneManager.LoadScene(Scenes.CREDITS);
        }

        public void QuitGame()
        {
            Application.Quit();
            Debug.Log("Game quit!");
        }
    }
}