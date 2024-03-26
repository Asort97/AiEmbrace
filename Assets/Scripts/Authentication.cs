using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Linq;
using System;
using Unity.VisualScripting;

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
        // загружаем локальные данные пользователя из файла
        GameDataManager.Instance.LoadAccountData();
        if (GameDataManager.Instance.accountDataForStorage != null)
        {
            GameDataManager.Instance.ApplyAccountData(); // в том числе заполняем ClientAPI.Instance.token

            await GameDataManager.Instance.LoadGameData(); // загружаем игровые данные c сервера
            if (GameDataManager.Instance.gameDataForStorage != null)
            {
                Debug.Log("data = " + GameDataManager.Instance.gameDataForStorage);
                GameDataManager.Instance.ApplyGameData(); // применяем игровые данные
            }
            // todo: 
            SceneManager.LoadScene("GameScene");
        }
    }

    public void ToWelcomeMenu()
    {
        welcomeMenu.SetActive(true);
        registrationMenu.SetActive(false);
        loginMenu.SetActive(false);
    }

    public void ShowError(string error)
    {
        errorLoginPanel.SetActive(true);
        errorLoginText.text = error;
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
        Register(register_emailField.text, register_passwordField.text);
    }

    public void LoginBtn()
    {
        Login(login_emailField.text, login_passwordField.text);
    }

    public async void SetNicknameBtn()
    {
        if(nicknameField.text.Length >= 3)
        {
            // todo: установить никнейм в UserDataManager, чтобы он был доступен везде
            ClientAPI.Instance.PlayerNickname = nicknameField.text;
            // сохраняем никнейм в локальные данные вместе с логином и токеном
            GameDataManager.Instance.SaveAccountData();
            await GameDataManager.Instance.SaveGameData(); // сохраняем (пока только никнейм)
            SceneManager.LoadScene("GameScene");
        }
        else
        {
            ShowError("Too short nickname!");
        }
    }

    public async void Register(string login, string password)
    {
        RegisterResponse response = await ClientAPI.Instance.Register(login, password);

        if(response.success)
        {
            Login(login, password);
        }
        else
        {
            string errors = string.Join("|", response.errors.SelectMany(kv => kv.Value));

            ShowError(errors);
        }
    }
    
    public async void Login(string login, string password)
    {
        LoginResponse response = await ClientAPI.Instance.Login(login, password);

        if(response.success)
        {
            // пытаемся загрузить данные с сервера
            await GameDataManager.Instance.LoadGameData();
            if (GameDataManager.Instance.gameDataForStorage != null)
            {
                GameDataManager.Instance.ApplyGameData();
                // сохраняем логин и токен в локальные данные
                GameDataManager.Instance.SaveAccountData();
                SceneManager.LoadScene("GameScene");
            }
            else
            {
                // если данных нет, то аккаунт пустой и переходим к выбору ника
                ToNicknameMenu();
            }
        }
        else
        {
            string errors = string.Join("|", response.errors.SelectMany(kv => kv.Value));

            ShowError(errors);
        }
    }

}
