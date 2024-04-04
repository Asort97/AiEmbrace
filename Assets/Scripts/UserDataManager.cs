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
     */

    public static UserDataManager _instance;

    public AllUserData data;

    private void Start()
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

    public static UserDataManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<UserDataManager>();
                if (_instance == null)
                {
                    GameObject go = new GameObject();
                    _instance = go.AddComponent<UserDataManager>();
                }
                DontDestroyOnLoad(_instance.gameObject);
            }
            return _instance;
        }
    }
}
