using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InnerSociety
{
    public class MainMenuManager : MonoBehaviour
    {
        public void StartPrototype()
        {
            //TODO: Open Scene with Prototype
        }

        public void OpenCredits()
        {
            //TODO: Open Scene with Credits
        }

        public void QuitGame()
        {
            Application.Quit();
            Debug.Log("Game quit!");
        }
    }
}