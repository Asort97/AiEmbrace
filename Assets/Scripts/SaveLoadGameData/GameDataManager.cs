using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;


[Serializable]
public class AccountDataForStorage
{
    [SerializeField] public string login;
    [SerializeField] public string token;
}

[Serializable]
public class GameDataForStorage
{
    [SerializeField] public AllUserData data;
}


public class GameDataManager : MonoBehaviour
{
    /*
     * Class for managing game data.
     * 
     * Requires access to UserDataManager and ClientAPI.
     * 
     */

    [SerializeField]
    public GameDataForStorage gameDataForStorage = null;
    public AccountDataForStorage accountDataForStorage = null;

    // if the game version changes,user data will be reset
    const string version = "0.1";

    // file name for account data
    const string FILE_NAME = "accountData.dat";

    private AccountDataForStorage CollectAccountData()
    {
        var accountDataForStorage = new AccountDataForStorage();
        accountDataForStorage.token = ClientAPI.Instance.token;

        return accountDataForStorage;
    }

    private GameDataForStorage CollectGameData()
    {
        var gameDataForStorage = new GameDataForStorage();
        gameDataForStorage.data = UserDataManager.Instance.data;
        return gameDataForStorage;
    }

    public void ApplyGameData()
    {
        if (gameDataForStorage != null)
        {
            UserDataManager.Instance.data = gameDataForStorage.data;
            // todo: TimeManager.instance.currentDay = gameDataForStorage.currentGameDate;
        }
    }

    public void ApplyAccountData()
    {
        if (accountDataForStorage != null)
        {
            // todo: GameManager._instance.accountData.UILogin = accountDataForStorage.login;
            ClientAPI.Instance.token = accountDataForStorage.token;
        }
    }

    public async Task LoadGameData()
    {
        var data = await ClientAPI.Instance.LoadData(version);
        gameDataForStorage = data;
    }

    public async Task SaveGameData()
    {
        await ClientAPI.Instance.SaveData(CollectGameData(), version);
    }

    public void LoadAccountData()
    {
        if (File.Exists(GetSavePath()))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(GetSavePath(), FileMode.Open);
            accountDataForStorage = (AccountDataForStorage)bf.Deserialize(file);
            file.Close();
        } else
        {
            accountDataForStorage = null;
        }
    }

    public void SaveAccountData()
    {
        // saving account data to local storage
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(GetSavePath());
        bf.Serialize(file, CollectAccountData());
        file.Close();
    }

    public void ClearAccountData()
    {
        // clearing account data from local storage
        if (File.Exists(GetSavePath()))
        {
            File.Delete(GetSavePath());
        }
    }

    private string GetSavePath()
    {
        return Application.persistentDataPath + FILE_NAME;
    }
}
