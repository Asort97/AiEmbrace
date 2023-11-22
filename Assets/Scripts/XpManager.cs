using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XpManager : MonoBehaviour
{
    public static XpManager instance;
    public static Action OnNewLevel;
    [SerializeField] private float maxXpInDay = 750f;
    private float receivedDayXP;
    private float levelXP;
    public float AmountToNextLevel;
    public int CurrentLvl = 1;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        UpdateExpToNextLevel();
    }

    public void AddXp()
    {
        if(receivedDayXP < maxXpInDay)
        {
            levelXP += 250;
            receivedDayXP += 250;

            if(AmountToNextLevel % levelXP == 0)
            {
                OnNewLevel?.Invoke();
                CurrentLvl++;
            }
            
            UpdateExpToNextLevel();
            UIManager.instance.UpdateXPSlider(levelXP, CurrentLvl);            
        }
        else
        {
            Debug.Log($"already received max XP");
        }
    }

    private void UpdateExpToNextLevel()
    {
        AmountToNextLevel = 100 * CurrentLvl;
        UIManager.instance.UpdateMaxValueXPSlider(AmountToNextLevel);
    }
}
