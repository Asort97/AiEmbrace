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
    public float AmountToNextLevel = 100;
    public int CurrentLvl = 1;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        CheckLevel();
    }

    public void AddXp(float exp)
    {
        if(receivedDayXP < maxXpInDay)
        {
            if(exp > AmountToNextLevel)
            {
                float takedExp = exp;

                while (takedExp >= AmountToNextLevel)
                {
                    Debug.Log($"{takedExp} - {AmountToNextLevel}");
                    levelXP += AmountToNextLevel;
                    
                    if(levelXP >= AmountToNextLevel)
                    {
                        levelXP = 0;
                    }                
                    takedExp -= AmountToNextLevel;

                    UpgradeLevel();
                    UIManager.instance.UpdateXPSlider(takedExp, AmountToNextLevel, CurrentLvl);

                    Debug.Log($"Cur Level: {CurrentLvl}, amount to upgrade: {AmountToNextLevel}, ost: {takedExp} ");                
                }                
            }
            else
            {
                levelXP += exp;

                if (levelXP >= AmountToNextLevel)
                {
                    float takedXp = levelXP;

                    levelXP = 0;
                    Debug.Log($"{exp} and ost: {AmountToNextLevel - levelXP}");
                    levelXP += AmountToNextLevel - takedXp; 

                    UpgradeLevel();
                }

                UIManager.instance.UpdateXPSlider(levelXP, AmountToNextLevel, CurrentLvl);
            }
        }
        else
        {
            Debug.Log($"already received max XP");
        }
    }

    private void UpgradeLevel()
    {
        CurrentLvl++;
        
        CheckLevel();
    }

    private void CheckLevel()
    {
        AmountToNextLevel = 100 * CurrentLvl;
    }
}
