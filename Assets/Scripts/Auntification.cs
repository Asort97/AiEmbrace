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

    [SerializeField] private TMP_InputField login_emailField;
    [SerializeField] private TMP_InputField login_passwordField;

    [SerializeField] private TMP_InputField register_emailField;
    [SerializeField] private TMP_InputField register_passwordField;

    private int isAlreadyLogin  = 0;
    private string savedLogin;
    private string savedPassword;

    private void Start()
    {
        isAlreadyLogin = PlayerPrefs.GetInt("IS_LOGIN", 0);   

        savedLogin = PlayerPrefs.GetString("LOGIN");
        savedPassword = PlayerPrefs.GetString("PASSWORD");

        if(isAlreadyLogin == 1)
        {
            Login(savedLogin, savedPassword);
        }
    }

    public void ToWelcomeMenu()
    {
        welcomeMenu.SetActive(true);
        registrationMenu.SetActive(false);
        loginMenu.SetActive(false);
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
        RegisterResponse response = await ClientAPI.instance.Register(register_emailField.text, register_passwordField.text);

        Debug.Log(register_emailField.text);
        Debug.Log(register_passwordField.text);

        if(response.success)
        {
            Debug.Log($"Success register");
            
            Login(register_emailField.text, register_passwordField.text);
        }
        else
        {
            string errors = string.Join("|", response.errors.SelectMany(kv => kv.Value));

            Debug.Log($"Non Success register {errors}");
        }
    }
    
    public async void Login(string login, string password)
    {
        LoginResponse response = await ClientAPI.instance.Login(login, password);

        if(response.success)
        {
            isAlreadyLogin = 1;

            savedLogin = login;
            savedPassword = password;     

            PlayerPrefs.SetInt("IS_LOGIN", 1);
            PlayerPrefs.SetString("LOGIN", login);
            PlayerPrefs.SetString("PASSWORD", password);

            Debug.Log($"Success login");

            SceneManager.LoadScene("GameScene");
        }
        else
        {
            Debug.Log($"Non Success login");
        }
    }

}
