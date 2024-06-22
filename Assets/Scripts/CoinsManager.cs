using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinsManager : MonoBehaviour
{
    public static CoinsManager instance;
    public static Action<int, int> OnAddCash;
    private int amountCoinsEachLevel = 10;
    // private int currentCoins;
    // private int currentCrystals;

    private void Awake()
    {
        instance = this;
    }

    private void Start() 
    {
        AddCoins(1000);
    }

    public void OnEnable()
    {
        XpManager.OnNewLevel += AddCoins;
        ItemClothes.OnBuyItem += UseCoins;
    }

    public void OnDisable()
    {
        XpManager.OnNewLevel -= AddCoins;
        ItemClothes.OnBuyItem -= UseCoins;
    }
    
    public bool CheckEnoughCoins(int price)
    {
        if(UserDataManager.Instance.data.userData.userMoney >= price)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private async void UseCoins(int amount)
    {
        UserDataManager.Instance.data.userData.userMoney -= amount;
        OnAddCash?.Invoke(UserDataManager.Instance.data.userData.userMoney, UserDataManager.Instance.data.userData.userCrystals);
        
        // UserDataManager.Instance.data.userData.userMoney = currentCoins;

        var GameDataManager = new GameDataManager();
        await GameDataManager.SaveGameData();
    }

    private async void AddCoins()
    {
        Debug.Log($"Money amount {UserDataManager.Instance.data.userData.userMoney}");

        UserDataManager.Instance.data.userData.userMoney += amountCoinsEachLevel * XpManager.instance.CurrentLvl;

        // UserDataManager.Instance.data.userData.userMoney = currentCoins;

        OnAddCash?.Invoke(UserDataManager.Instance.data.userData.userMoney, UserDataManager.Instance.data.userData.userCrystals);

        var GameDataManager = new GameDataManager();
        await GameDataManager.SaveGameData();
    }

    private async void AddCoins(int amount)
    {
        Debug.Log($"Money amount {UserDataManager.Instance.data.userData.userMoney}");

        UserDataManager.Instance.data.userData.userMoney += amount;

        OnAddCash?.Invoke(UserDataManager.Instance.data.userData.userMoney, UserDataManager.Instance.data.userData.userCrystals);

        // UserDataManager.Instance.data.userData.userMoney = currentCoins;

        var GameDataManager = new GameDataManager();
        await GameDataManager.SaveGameData();
    }
    
}
