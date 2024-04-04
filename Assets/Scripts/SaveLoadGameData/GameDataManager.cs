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
    static private GameDataManager _instance;

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

    void Update()
    {
    }

    public static GameDataManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameDataManager>();
                if (_instance == null)
                {
                    GameObject go = new GameObject();
                    _instance = go.AddComponent<GameDataManager>();
                }
                DontDestroyOnLoad(_instance.gameObject);
            }
            return _instance;
        }
    }

    private AccountDataForStorage CollectAccountData()
    {
        var accountDataForStorage = new AccountDataForStorage();
        accountDataForStorage.token = ClientAPI.Instance.token;

        return accountDataForStorage;
    }

    private GameDataForStorage CollectGameData()
    {
        var gameDataForStorage = new GameDataForStorage();

        gameDataForStorage.userData = UserDataManager.Instance.data.userData;
        gameDataForStorage.aiData = AIDataManager.Instance.aiCharactersData;

        return gameDataForStorage;
    }

    public void ApplyGameData()
    {
        if (gameDataForStorage != null)
        {
            UserDataManager.Instance.data.userData = gameDataForStorage.userData;
            // todo: TimeManager.instance.currentDay = gameDataForStorage.currentGameDate;
        }
    }

    public void ApplyAccountData()
    {
        if (accountDataForStorage != null)
        {
            // todo: GameManager._instance.accountData.Login = accountDataForStorage.login;
            ClientAPI.Instance.token = accountDataForStorage.token;
        }
    }

    public async Task LoadGameData()
    {
        // ��������� gameDataForStorage �� ������ � ��������
        var data = await ClientAPI.Instance.LoadData(version);
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

    public void InitializeNewPlayerData(string name)
    {
        // Пример начальных данных
        gameDataForStorage.userData = new UserData()
        {
            userNickname = name,
            userExp = 0,
            userCrystals = 0,
            userMoney = 0
        };
        // todo: aiCharactersData
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
