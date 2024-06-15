using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinsManager : MonoBehaviour
{
    public static CoinsManager instance;
    public static Action<int, int> OnAddCash;
    private int amountCoinsEachLevel = 10;
    private int currentCoins;
    private int currentCrystals;

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
        if(currentCoins >= price)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void UseCoins(int amount)
    {
        currentCoins -= amount;
        OnAddCash?.Invoke(currentCoins, currentCrystals);
    }

    private void AddCoins()
    {
        Debug.Log($"Money amount {currentCoins}");

        currentCoins += amountCoinsEachLevel * XpManager.instance.CurrentLvl;

        OnAddCash?.Invoke(currentCoins, currentCrystals);
    }

    private void AddCoins(int amount)
    {
        Debug.Log($"Money amount {currentCoins}");

        currentCoins += amount;

        OnAddCash?.Invoke(currentCoins, currentCrystals);
    }
    
}
