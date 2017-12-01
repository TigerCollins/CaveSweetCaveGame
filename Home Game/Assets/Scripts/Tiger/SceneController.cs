using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace UnityStandardAssets.ImageEffects
{
    public class SceneController : MonoBehaviour
    {
        public static SceneController instance;

        [Header("Scene Controller")]
        public string SceneName;

        [Header("Main Menu Items")]
        public GameObject SplashScreen;
        public GameObject MenuLookPoint;
        public GameObject Cam;
        public GameObject Menu;
        public GameObject CreditsGroup;

        [Header("Game Scene Items")]
        public GameObject PauseMenu;

        // Use this for initialization
        void Start()
        {
            Scene CurrentScene = SceneManager.GetActiveScene();
            SceneName = CurrentScene.name;

            //  CreditsGroup.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {
            MainMenu();

            if(Input.GetKeyDown(KeyCode.Escape))
            {
                Pause();
            }
        }

        public void MainMenu()
        {
            if (SceneName == "Menu")
            {
                if (Input.anyKey)
                {
                    DepthOfField.instance.focalTransform = MenuLookPoint.transform;
                }
            }
        }

        public void QuitGame()
        {
            Application.Quit();
            print("Quit");
        }

        public void Options()
        {
            print("Options");
        }

        public void Credits()
        {
            CreditsGroup.SetActive(true);
            Menu.SetActive(false);
        }

        public void CloseCredits()
        {
            CreditsGroup.SetActive(false);
            Menu.SetActive(true);
        }

        public void NewGame()
        {
            Application.LoadLevel("World");
        }

        public void Pause()
        {
            PauseMenu.SetActive(true);
            Time.timeScale = 0;
        }

        public void ContinueGame()
        {
            Time.timeScale = 1;
            PauseMenu.SetActive(false);
        }

        public void QuitMenu()
        {
            Time.timeScale = 1;
            Application.LoadLevel("Menu");
        }
    }
}
