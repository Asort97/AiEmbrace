using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using UnityEngine;

[System.Serializable]
public class AICharacterData
{
    /*
     * AI Character Template.
     * Contain base data for character.
     * 
     */
    public string characterName; 
    
    [TextArea(5, 20)]
    public string characterPersonality;

    public List<string> actions;
    [Tooltip("Лимит токенов на воспоминания для вставки в промпт. Ограничивает количество воспоминаний.")]
    public int eventsMemoryTokenLimit = 50;
    [Tooltip("Лимит токенов на воспоминания для вставки в промпт. Ограничивает количество воспоминаний.")]
    public int conversationalMemoryTokenLimit = 500;
    [TextArea(5, 10)]
    public string clothingDescription;

    // default memories of character from past
    public MemoriesManager eventsMemories = new MemoriesManager();
}
