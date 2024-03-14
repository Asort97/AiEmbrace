using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Linq;
using System;

public class Auntification : MonoBehaviour
{
    [SerializeField] private GameObject welcomeMenu;
    [SerializeField] private GameObject loginMenu;
    [SerializeField] private GameObject registrationMenu;
    [SerializeField] private GameObject errorLoginPanel;
    [SerializeField] private TMP_Text errorLoginText;
    [SerializeField] private TMP_InputField login_emailField;
    [SerializeField] private TMP_InputField login_passwordField;

    [SerializeField] private TMP_InputField register_emailField;
    [SerializeField] private TMP_InputField register_passwordField;

    private string accountToken;

    private void Start()
    {
        accountToken = PlayerPrefs.GetString("TOKEN");

        if(accountToken.Length != 0)
        {
            ClientAPI.Instance.token = accountToken;
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

    public void RegisterBtn()
    {
        Register(register_emailField.text, register_passwordField.text);
    }

    public void LoginBtn()
    {
        Login(login_emailField.text, login_passwordField.text);
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
            accountToken = ClientAPI.Instance.token;

            PlayerPrefs.SetString("TOKEN", ClientAPI.Instance.token);

            SceneManager.LoadScene("GameScene");
        }
        else
        {
            string errors = string.Join("|", response.errors.SelectMany(kv => kv.Value));

            ShowError(errors);
        }
    }

}
