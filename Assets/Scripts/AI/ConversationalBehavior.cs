using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "ConversationalBehavior", menuName = "ConversationalBehavior")]
[System.Serializable]
public class ConversationalBehavior : ScriptableObject
{
    [Tooltip("Max 21 element")]
    public List <string> conversationalBehavior = new List<string>(21);


    public string GetConversationalBehavior(int attraction)
    {
        attraction += 10;
        return conversationalBehavior[attraction];
    }

}
