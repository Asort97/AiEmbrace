using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XpManager : MonoBehaviour
{
    public static XpManager instance;
    public static Action OnNewLevel;

    [SerializeField] private float maxXpInDay = 750f;
    [SerializeField] private float amountXpToNewLvl;
    private float receivedDayXP;
    public int CurrentLvl = 1;
    private float levelXP;

    private void Awake()
    {
        instance = this;
    }

    public void AddXp()
    {
        if(receivedDayXP < maxXpInDay)
        {
            levelXP += 30;
            receivedDayXP += 30;

            if(amountXpToNewLvl % levelXP == 0)
            {
                OnNewLevel?.Invoke();
                CurrentLvl++;
            }

            UIManager.instance.UpdateXPSlider(levelXP, CurrentLvl);            
        }
        else
        {
            Debug.Log($"already received max XP");
        }
    }

}
