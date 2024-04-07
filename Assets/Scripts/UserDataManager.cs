using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public void InitializeNewUserData()
    {
        // Создаёт новый экземпляр данных пользователя из префаба
        if (userDataTemplatePrefab != null)
        {
            data = Instantiate(userDataTemplatePrefab).GetComponent<AllUserDataTemplate>().GetTemplateData();
        }
    }

    // Очищает данные пользователя
    public void ClearUserData()
    {
        data = null; // Просто установите data в null или переинициализируйте, если нужно
    }
}
