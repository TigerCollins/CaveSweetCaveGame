using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.ImageEffects;

    public class SceneController : MonoBehaviour
    {
        public static SceneController instance;

        [Header("Scene Controller")]
        public string SceneName;
    public GameObject LoadingScreen;

        [Header("Main Menu Items")]
        public GameObject SplashScreen;
        public GameObject MenuLookPoint;
        public GameObject Cam;
        public GameObject Menu;
        public GameObject CreditsGroup;

        [Header("Options Items")]
        public GameObject OptionsGroup;
        public GameObject KeyboardGroup;
        public GameObject ControllerGroup;

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

        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown("joystick button 7"))
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
                    //PlayerPref for invertY. 1 = non inverted
                    PlayerPrefs.SetInt("InvertY", 1);
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
            OptionsGroup.SetActive(true);
            Menu.SetActive(false);
        }

        public void OptionsKeyboard()
        {
            KeyboardGroup.SetActive(true);
            ControllerGroup.SetActive(false);
        }

        public void OptionsController()
        {
            ControllerGroup.SetActive(true);
            KeyboardGroup.SetActive(false);
        }

        public void IngameOptions()
        {
            OptionsGroup.SetActive(true);
        }

        public void CloseIngameOptions()
        {
            OptionsGroup.SetActive(false);
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
            OptionsGroup.SetActive(false);
        }

        public void NewGame()
        {
        LoadingScreen.SetActive(true);
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
            OptionsGroup.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = false;
        InputController.instance.Pause = false;
    }

        public void QuitMenu()
        {
        LoadingScreen.SetActive(true);
        Time.timeScale = 1;
            Application.LoadLevel("Menu");
        }
    }

