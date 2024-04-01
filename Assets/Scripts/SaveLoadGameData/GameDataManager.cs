using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

/*
������ ��� ���������� � ��������:
    ���������� ������       - ?             
    ����� ����������:       - GameManager   
    - ������� ���������
    - ��, ��
    - ������� ������        - ?
    ���������               - ?
    ������ ���� AI          - AIDataManager     x
    �������� �������        - ?
    ������ �������          - ?
    ��� ������              - GameManager       x  
    ���� ����������         - ���
    ������� �������         - GameManager
    - �����                                     x
    �����                   - ClientAPI         x
    ������� ����            - TimeManager       x
 */
[Serializable]
public class AccountDataForStorage
{
    [SerializeField] public string login;
    [SerializeField] public string token;
}

[Serializable]
public class GameDataForStorage
{
    [SerializeField] public AICharacterDataWrapper aiData;
    [SerializeField] public UserData userData;
    // [SerializeField] public string playerName;
    // [SerializeField] public int playerMoney;
    // [SerializeField] public int playerCrystals;
    [SerializeField] public int currentGameDate;
}


public class GameDataManager : MonoBehaviour
{
    // todo: ����������� �� ����� ������ ���� ���������/����������?
    // ����� ��� ���������� � �������� ������ ����

    // singleton
    static public GameDataManager instance;

    // ������ �� �������, ������� �������� ������ ������
    // ���� ��� ������� ��� ���������, ����������� �� �����

    // ������
    // todo: �������� ������� ������, ������� �� ����� ��� ������ ������� gameDataForStorage � accountDataForStorage, ����� ��� �� �������� ������
    [SerializeField]
    public GameDataForStorage gameDataForStorage = null;
    public AccountDataForStorage accountDataForStorage = null;

    const string version = "0.1";

    // �������� ����� ��� ����������
    const string FILE_NAME = "accountData.dat";

    public UnityEvent onGameDataLoaded;


    void Start()
    {
        // singleton
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
    }

    private AccountDataForStorage CollectAccountData()
    {
        var accountDataForStorage = new AccountDataForStorage();
        // todo: accountDataForStorage.login = GameManager.instance.accountData.Login;
        accountDataForStorage.token = ClientAPI.Instance.token;

        return accountDataForStorage;
    }

    private GameDataForStorage CollectGameData()
    {
        var gameDataForStorage = new GameDataForStorage();

        // gameDataForStorage.playerName = UserDataManager.instance.GetUserNickname();
        // gameDataForStorage.playerMoney = UserDataManager.instance.GetUserMoney();
        // gameDataForStorage.playerCrystals = UserDataManager.instance.GetUserCrystals();

        gameDataForStorage.userData = UserDataManager.instance.userData;
        gameDataForStorage.aiData = AIDataManager.instance.aiCharactersData;

        // todo: gameDataForStorage.playerName = GameManager.instance.GetPlayerName();
        // todo: gameDataForStorage.currentGameDate = TimeManager.instance.currentDay;

        return gameDataForStorage;
    }

    public void ApplyGameData()
    {
        if (gameDataForStorage != null)
        {
            AIDataManager.instance.aiCharactersData = gameDataForStorage.aiData;
            UserDataManager.instance.userData = gameDataForStorage.userData;
            
            // todo: GameManager.instance.SetPlayerName(gameDataForStorage.playerName);
            // todo: TimeManager.instance.currentDay = gameDataForStorage.currentGameDate;
        }
    }

    public void ApplyAccountData()
    {
        if (accountDataForStorage != null)
        {
            // todo: GameManager.instance.accountData.Login = accountDataForStorage.login;
            ClientAPI.Instance.token = accountDataForStorage.token;
        }
    }

    public async Task LoadGameData()
    {
        // ��������� gameDataForStorage �� ������ � ��������
        var data = await ClientAPI.Instance.LoadData(version);
        Debug.Log(data.GetType());
        gameDataForStorage = data;
    }

    public async Task SaveGameData()
    {
        // ��������� gameDataForStorage � ������
        await ClientAPI.Instance.SaveData(CollectGameData(), version);
    }

    public void LoadAccountData()
    {
        // �������� ��������� accountDataForStorage �� ���������� ���������
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

    private string GetSavePath()
    {
        return Application.persistentDataPath + FILE_NAME;
    }
}
