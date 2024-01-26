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

    public async void Register()
    {
        RegisterResponse response = await ClientAPI.instance.Register(register_emailField.text, register_passwordField.text);

        Debug.Log(register_emailField.text);
        Debug.Log(register_passwordField.text);

        if(response.success)
        {
            Debug.Log($"Success register");

            LoginResponse responseLogin = await ClientAPI.instance.Login(register_emailField.text, register_passwordField.text);

            if(responseLogin.success)
            {
                SceneManager.LoadScene("GameScene");
            }
        }
        else
        {
            string errors = string.Join("|", response.errors.SelectMany(kv => kv.Value));

            Debug.Log($"Non Success register {errors}");
        }
    }

    public async void Login()
    {
        LoginResponse response = await ClientAPI.instance.Login(login_emailField.text, login_passwordField.text);

        if(response.success)
        {
            Debug.Log($"Success login");
            SceneManager.LoadScene("GameScene");
        }
        else
        {
            Debug.Log($"Non Success login");
        }
    }

}
