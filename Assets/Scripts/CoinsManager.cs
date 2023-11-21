using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinsManager : MonoBehaviour
{
    private int amountCoinsEachLevel = 10;
    private int currentCoins;

    public void OnEnable()
    {
        XpManager.OnNewLevel += AddCoins;
    }

    public void OnDisable()
    {
        XpManager.OnNewLevel -= AddCoins;
    }

    private void AddCoins()
    {
        currentCoins += amountCoinsEachLevel * XpManager.instance.CurrentLvl;
    }
}
