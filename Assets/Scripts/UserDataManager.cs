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
    public StoreData storeData;
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
    // [SerializeField] private ItemSO[] AllItems;
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
    
    public async void InitAllItems()
    {
        if(data.storeData.Items.Count == 0)
        {
            for (int i = 0; i < userDataTemplatePrefab.AllItems.Length; i++)
            {
                Item serverItem = data.storeData.Items[i];

                if(!serverItem.IsPurchased)
                {
                    Item _item = new Item(userDataTemplatePrefab.AllItems[i].idItem, userDataTemplatePrefab.AllItems[i].itemCategory, userDataTemplatePrefab.AllItems[i].purchasedByDefault, userDataTemplatePrefab.AllItems[i].usedByDefault);
                    data.storeData.Items.Add(_item);
                }
                else
                {
                    Item _item = new Item(userDataTemplatePrefab.AllItems[i].idItem, userDataTemplatePrefab.AllItems[i].itemCategory, serverItem.IsPurchased, serverItem.IsUsed);
                    data.storeData.Items.Add(_item);
                }

                Debug.Log($"{serverItem}");
            }

            // foreach (var item in AllItems)
            // {
            //     Debug.Log($"{item}");
            //     Item _item = new Item(item.idItem, item.itemCategory, item.purchasedByDefault, item.usedByDefault);
            //     Debug.Log($"{_item}");
            //     data.storeData.Items.Add(_item);
            // }            

            var GameDataManager = new GameDataManager();
            await GameDataManager.SaveGameData(); // Если добавлены новые итемы то сохраняем
            // GameDataManager.LoadAccountData();
        }
    }
    
    // private void Update()
    // {
    //     InitAllItems();// Закидываем все возможные вещи в дату
    // }

    public ItemSO GetItemSOById(string id) //  Получение ItemSo через айди
    {
        foreach (var item in userDataTemplatePrefab.AllItems)
        {
            if(item.idItem == id)
            {
                return item;
            }
        }

        return null;
    }

    public Item GetItemById(string id) //  Получение класса Item через айди
    {
        foreach (var item in data.storeData.Items)
        {
            if(item.ItemId == id)
            {
                return item;
            }
        }

        return null;
    }

    public int GetIndexItemById(string id) //  Получение индекса из Items по айди
    {
        for (int i = 0; i < data.storeData.Items.Count; i++)
        {
            if(data.storeData.Items[i].ItemId == id)
            {
                return i;
            }            
        }

        return -1;
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
