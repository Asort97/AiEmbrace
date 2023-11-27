using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "ConversationalMemoryExample", menuName = "ConversationalMemoryExample")]
public class AIChatConversationalMemoryPrefab :ScriptableObject
{
    [HideInInspector]
    public static string chatPrefix = "Dialogue Fragment:";
    [HideInInspector]
    public static string importancePrefix = "Importance:";
    [HideInInspector]
    public static string memoryDescriptionPrefix = "Memory:";

    // Start is called before the first frame update
    [Range(1,10)]
    public int importance;
    [TextArea(4,10)]
    public string memoryDescription;
    public ChatHisoty chatHisoty;

}
