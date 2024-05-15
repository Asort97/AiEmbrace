using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Linq;
using System;
using Unity.VisualScripting;
using UnityEditor.PackageManager;

public class Authentication : MonoBehaviour
{
    [SerializeField] private GameObject welcomeMenu;
    [SerializeField] private GameObject loginMenu;
    [SerializeField] private GameObject registrationMenu;
    [SerializeField] private GameObject nicknameMenu;
    [SerializeField] private GameObject errorLoginPanel;
    [SerializeField] private TMP_Text errorLoginText;
    [SerializeField] private TMP_InputField login_emailField;
    [SerializeField] private TMP_InputField login_passwordField;
    [SerializeField] private TMP_InputField nicknameField;

    [SerializeField] private TMP_InputField register_emailField;
    [SerializeField] private TMP_InputField register_passwordField;

    private string accountToken;

    private async void Start()
    {
        // check if account data exists and load it
        var GameDataManager = new GameDataManager();
        GameDataManager.LoadAccountData();
        if (GameDataManager.accountDataForStorage != null)
        {
            GameDataManager.ApplyAccountData(); // set token in ClientAPI.Instance.token
            Login(false);
        }
    }

    public void ToWelcomeMenu()
    {
        welcomeMenu.SetActive(true);
        registrationMenu.SetActive(false);
        loginMenu.SetActive(false);
    }

    private void ShowError(string error)
    {
        errorLoginPanel.SetActive(true);
        errorLoginText.text = error;
    }

    private void ShowErrors(Dictionary<string, List<string>> errors)
    {
        string error = string.Join("|", errors.SelectMany(kv => kv.Value));
        ShowError(error);
    }

    public void CloseError()
    {
        errorLoginPanel.SetActive(false);
    }

    public void ToLoginMenu()
    {
        welcomeMenu.SetActive(false);
        registrationMenu.SetActive(false);
        loginMenu.SetActive(true);
    }

    public void ToRegisterMenu()
    {
        welcomeMenu.SetActive(false);
        loginMenu.SetActive(false);
        registrationMenu.SetActive(true);
    }

    public void ToNicknameMenu()
    {
        welcomeMenu.SetActive(false);
        loginMenu.SetActive(false);
        registrationMenu.SetActive(false);
        nicknameMenu.SetActive(true);
    }

    public void RegisterBtn()
    {
        UIRegister(register_emailField.text, register_passwordField.text);
    }

    public void LoginBtn()
    {
        UILogin(login_emailField.text, login_passwordField.text);
    }

    public async void SetNicknameBtn()
    {
        if(UserDataManager.Instance.IsNicknameValid(nicknameField.text))
        {
            RegisterFinish(nicknameField.text);
        }
        // else
        // {
        //     ShowError("Too short nickname!");
        // }
    }

    public async void UIRegister(string login, string password)
    {
        RegisterResponse response = await ClientAPI.Instance.Register(login, password);
        if(response.success)
        {
            LoginResponse loginResponse = await ClientAPI.Instance.Login(login, password);
            if (loginResponse.success)
            {
                // todo: add this when will be added logic to login without gamedata: GameDataManager.Instance.SaveAccountData();
                // set username
                ToNicknameMenu();
            }
            else
            {
                ShowErrors(loginResponse.errors);
            }
        }
        else
        {
            ShowErrors(response.errors);
        }
    }

    public async void UILogin(string login, string password)
    {
        LoginResponse response = await ClientAPI.Instance.Login(login, password);

        if(response.success)
        {
            var GameDataManager = new GameDataManager();
            GameDataManager.SaveAccountData();
            Login(true);
        }
        else
        {
            ShowErrors(response.errors);
        }
    }

    private async void Login(bool showErrors)
    {
        /* 
         * Login. Required to have token in ClientAPI
         */
        var GameDataManager = new GameDataManager();
        await GameDataManager.LoadGameData();
        if (GameDataManager.gameDataForStorage != null)
        {
            // apply game data for UserDataManager
            GameDataManager.ApplyGameData();
            Debug.Log("Login success");
            SceneManager.LoadScene("GameScene");
        }
        else
        {
            if (showErrors)
            {
                ShowError("Error loading game data");
            } else
            {
                Debug.LogWarning("Login failed: Error loading game data");
            }
        }
    }

    private async void RegisterFinish(string username)
    {
        /* 
         * RegisterFinish. Required to have token in ClientAPI
         */
        if (ClientAPI.Instance.token == null)
        {
            ShowError("Can't finish registration: failed to receive token.");
            return;
        }
        var GameDataManager = new GameDataManager();
        // todo: add checks for all api calls
        UserDataManager.Instance.InitializeNewUserData();
        UserDataManager.Instance.data.userData.userNickname = username;
        // save account and game data (nickname and default) before loading game scene
        GameDataManager.SaveAccountData();
        await GameDataManager.SaveGameData();
        // started data saved, load game scene
        SceneManager.LoadScene("GameScene");
    }

}
