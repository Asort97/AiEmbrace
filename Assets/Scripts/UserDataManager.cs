using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserDataManager: MonoBehaviour
{
    public static UserDataManager instance;
    public UserData userData;

    private void Awake()
    {
        instance = this;

        DontDestroyOnLoad(gameObject);
    }

    public void ChangeUserNickname(string name)
    {
        userData.userNickname = name;
    }

    public string GetUserNickname()
    {
        return userData.userNickname;
    }

    public int GetUserMoney()
    {
        return userData.userMoney;
    }

    public int GetUserCrystals()
    {
        return userData.userCrystals;
    }
}
