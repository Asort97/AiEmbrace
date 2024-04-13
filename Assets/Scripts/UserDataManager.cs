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

    [SerializeField] private string[] forbidNicknames = {"Character", "System", "Player"};
    [SerializeField] private string allowsSymbolNickname = @"^[a-zA-Z\s\-]+$";
    public static UserDataManager _instance;
    
    public AllUserDataTemplate userDataTemplatePrefab;

    public AllUserData data;

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
        if(nickname.Length <= 16 && !forbidNicknames.Contains(nickname) && Regex.IsMatch(nickname, allowsSymbolNickname))
        {
            return true;
        }
        else if(forbidNicknames.Contains(nickname))
        {
            PopUpNotifications.instance.ShowNotification("Forbid nickname!");
        }
        else if(!Regex.IsMatch(nickname, allowsSymbolNickname))
        {
            PopUpNotifications.instance.ShowNotification("Forbid symbols!");
        }
        else if(nickname.Length > 16)
        {
            PopUpNotifications.instance.ShowNotification("Too long nickname!");
        }

        return false;
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
}
