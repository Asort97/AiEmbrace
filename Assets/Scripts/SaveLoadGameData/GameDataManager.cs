using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

/*
ƒанные дл€ сохранени€ и загрузки:
    пройденные квесты       - ?             
    статы персонажей:       - GameManager   
    - уровень персонажа
    - хп, мп
    - надетые шмотки        - ?
    инвентарь               - ?
    данные всех AI          - AIDataManager     x
    открытые сундуки        - ?
    убитые монстры          - ?
    им€ игрока              - GameManager       x  
    дата сохранени€         - тут
    текущий аккаунт         - GameManager
    - логин                                     x
    токен                   - ClientAPI         x
    игрова€ дата            - TimeManager       x
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
    [SerializeField] public string playerName;
    [SerializeField] public int currentGameDate;
}


public class GameDataManager : MonoBehaviour
{
    // todo: об€зательно ли этому классу быть монобехом/синглтоном?
    //  ласс дл€ сохранени€ и загрузки данных игры

    // singleton
    static public GameDataManager instance;

    // —сылка на скрипты, которые содержат нужные данные
    // пока все скрипты это синглтоны, прокидывать не нужно

    // данные
    // todo: добавить очистку данных, которые не нужны или вообще удалить gameDataForStorage и accountDataForStorage, чтобы они не занимали пам€ть
    [SerializeField]
    public GameDataForStorage gameDataForStorage = null;
    public AccountDataForStorage accountDataForStorage = null;

    const string version = "0.1";

    // название файла дл€ сохранени€
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
        accountDataForStorage.login = GameManager.instance.accountData.Login;
        accountDataForStorage.token = ClientAPI.instance.token;

        return accountDataForStorage;
    }

    private GameDataForStorage CollectGameData()
    {
        var gameDataForStorage = new GameDataForStorage();
        gameDataForStorage.aiData = AIDataManager.instance.aiCharactersData;
        gameDataForStorage.playerName = GameManager.instance.GetPlayerName();
        gameDataForStorage.currentGameDate = TimeManager.instance.currentDay;

        return gameDataForStorage;
    }

    public void ApplyGameData()
    {
        if (gameDataForStorage != null)
        {
            AIDataManager.instance.aiCharactersData = gameDataForStorage.aiData;
            GameManager.instance.SetPlayerName(gameDataForStorage.playerName);
            TimeManager.instance.currentDay = gameDataForStorage.currentGameDate;
        }
    }

    public void ApplyAccountData()
    {
        if (accountDataForStorage != null)
        {
            GameManager.instance.accountData.Login = accountDataForStorage.login;
            ClientAPI.instance.token = accountDataForStorage.token;
        }
    }

    public async Task LoadGameData()
    {
        // загружаем gameDataForStorage из облака в менеджер
        var data = await ClientAPI.instance.LoadData(version);
        gameDataForStorage = data;
    }

    public async Task SaveGameData()
    {
        // сохран€ем gameDataForStorage в облако
        await ClientAPI.instance.SaveData(CollectGameData(), version);
    }

    public void LoadAccountData()
    {
        // пытаемс€ загрузить accountDataForStorage из локального хранилища
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
