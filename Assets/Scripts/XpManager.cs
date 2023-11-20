using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XpManager : MonoBehaviour
{
    public static XpManager instance;
    [SerializeField] private float amountXpToNewLvl;
    private int currentLvl = 1;
    private float levelXP;

    private void Awake()
    {
        instance = this;
    }

    public void AddXp()
    {
        levelXP += 30;

        if(amountXpToNewLvl % levelXP == 0)
        {
            currentLvl++;
        }
    }

}
