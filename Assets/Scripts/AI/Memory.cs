using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Memory
{
    public int gameDate;
    public int importance;
    public string description;


    public string Remember(int currentDate)
    {
        string result = "\nIt was "+(currentDate-gameDate)+" days ago. " + description;

        return result;
    }

}