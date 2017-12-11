using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
    private bool PastSplashScreen = false;

    [Header("Options Items")]
    public GameObject OptionsGroup;
    public GameObject KeyboardGroup;
    public GameObject ControllerGroup;
    public bool PauseBool = false;

    [Header("Game Scene Items")]
    public GameObject PauseMenu;
    public Button ContinueGameButton;

    // Use this for initialization
    void Start()
    {
        Scene CurrentScene = SceneManager.GetActiveScene();
        SceneName = CurrentScene.name;

        if (SceneName == "World")
        {
            Time.timeScale = 1;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            print("worked");
        }


        //  CreditsGroup.SetActive(false);
    }

    // Update is called once per frame
    private void Update()
    {
        if (SceneName == "World")
        {
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown("joystick button 7"))
            {
                Pause();
            }
        }

    }

    void FixedUpdate()
    {
        MainMenu();

        if (SceneName == "World")
        {
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown("joystick button 7"))
            {
                Pause();
            }
        }


    }

    public void MainMenu()
    {
        print(PastSplashScreen);
        if (SceneName == "Menu")
        {

            if (Input.anyKey)
            {
                if (PastSplashScreen == false)
                {
                    PastSplashScreen = true;
                    //PlayerPref for invertY. 1 = non inverted
                    PlayerPrefs.SetInt("InvertY", 1);
                    DepthOfField.instance.focalTransform = MenuLookPoint.transform;
                    Invoke("MainMenuButtonSelect", 1);
                  
                }
                else
                {

                }
            }
        }

    }

   void MainMenuButtonSelect()
    {
        ContinueGameButton.Select();
    }

    public void QuitGame()
    {
        Application.Quit();
        print("Quit");
    }

    public void Options()
    {
        OptionsGroup.SetActive(true);
        ContinueGameButton.Select();
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
        ContinueGameButton.Select();
    }

    public void Credits()
    {
        CreditsGroup.SetActive(true);
        Menu.SetActive(false);
        ContinueGameButton.Select();
    }

    public void CloseCredits()
    {
        CreditsGroup.SetActive(false);
        Menu.SetActive(true);
        OptionsGroup.SetActive(false);
        ContinueGameButton.Select();
    }

    public void NewGame()
    {
        LoadingScreen.SetActive(true);
        Application.LoadLevel("World");
    }

    public void Pause()
    {
        PauseMenu.SetActive(true);
        ContinueGameButton.Select();
        Time.timeScale = 0;
        OptionsGroup.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ContinueGame()
    {
        Time.timeScale = 1;
        PauseMenu.SetActive(false);
        OptionsGroup.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }

    public void QuitMenu()
    {
        LoadingScreen.SetActive(true);
        Time.timeScale = 1;
        Application.LoadLevel("Menu");
    }
}



