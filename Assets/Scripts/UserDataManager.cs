using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Text.RegularExpressions;

[Serializable]
public class AllUserData
{
    /*
     * All sorts of player game data.
     * 
     */

    public UserData userData;
    public AICharactersPersonalData charactersData;
}

public class UserDataManager: MonoBehaviour
{
    /*
     * Global container for accessing for all user data.
     * 
     * Warning: This script must be attached to a ROOT GameObject in the scene.
     * 
     */

    [SerializeField] private string[] forbidNicknames;
    [SerializeField] private string[] errors;
    [SerializeField] private string allowsSymbolNickname = @"^[a-zA-Z0-9]*$";
    public static UserDataManager _instance;
    
    public AllUserDataTemplate userDataTemplatePrefab;
    public AllUserData data;

    public delegate bool ValidationCheck(string nickname);


    public static UserDataManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<UserDataManager>();
                if (_instance == null)
                {
                    GameObject go = new GameObject("UserDataManager");
                    _instance = go.AddComponent<UserDataManager>();
                    DontDestroyOnLoad(_instance.gameObject);
                }
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }

    public bool IsNicknameValid(string nickname)
    {
        ValidationCheck[] validationChecks = { CheckForbidNicknames, CheckNicknameSymbols, CheckNicknameLength }; //  Массив c методами проверки никнейма

        foreach (var check in validationChecks) // Пробегаемся по всем методам 
        {
            if(!check(nickname))
            {
                return false;
            }
        }

        return true;
    }

    public void InitializeNewUserData()
    {
        // ������ ����� ��������� ������ ������������ �� �������
        if (userDataTemplatePrefab != null)
        {
            data = Instantiate(userDataTemplatePrefab).GetComponent<AllUserDataTemplate>().GetTemplateData();
        }
    }

    // ������� ������ ������������
    public void ClearUserData()
    {
        data = null; // ������ ���������� data � null ��� �������������������, ���� �����
    }

    private bool CheckForbidNicknames(string nickname)
    {
        if (forbidNicknames.Contains(nickname))
        {
            PopUpNotifications.instance.ShowNotification(errors[0]);
            return false;
        }
        return true;
    }

    private bool CheckNicknameSymbols(string nickname)
    {
        Regex regex = new Regex($"^[a-zA-Z0-9]+$");

        if (!regex.IsMatch(nickname))
        {
            PopUpNotifications.instance.ShowNotification(errors[1]);
            return false;
        }

        if(!char.IsLetter(nickname[0]))
        {
            PopUpNotifications.instance.ShowNotification(errors[2]);
            return false;
        }

        return true;
    }    

    private bool CheckNicknameLength(string nickname)
    {
        if (string.IsNullOrEmpty(nickname) || nickname.Length > 16)
        {
            PopUpNotifications.instance.ShowNotification(errors[3]);
            return false;
        }

        return true;
    }
}
