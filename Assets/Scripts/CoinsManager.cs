using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinsManager : MonoBehaviour
{
    public static Action<int, int> OnAddCash;
    private int amountCoinsEachLevel = 10;
    private int currentCoins;
    private int currentCrystals;

    public void OnEnable()
    {
        XpManager.OnNewLevel += AddCoins;
    }

    public void OnDisable()
    {
        XpManager.OnNewLevel -= AddCoins;
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
}
