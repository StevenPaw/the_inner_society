using UnityEngine;
using UnityEngine.SceneManagement;

namespace InnerSociety
{
    public class CreditsMenuManager : MonoBehaviour
    {
        public void OnBackToMenu()
        {
            SceneManager.LoadScene(Scenes.MAINMENU);
        }
    }
}