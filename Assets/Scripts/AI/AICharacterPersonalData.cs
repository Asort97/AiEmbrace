using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AICharacterPersonalData
{
    /*
     * All character data that user can change.
     */

    public ChatHistory chatHistory = new ChatHistory();
    // Чат разбивается на фрагменты. Если какая-то часть текста обработалась (из нее были выделены воспоминания), то это считается одним фрагментом чата.
    // todo: directly connect with chat history
    public int oldestUnprocessedChatFragmentIndex = 0;

    [Range(-10, 10)]
    public int attractionLevel = 0;

    // the character's acquired memories during gameplay
    public MemoriesManager eventsMemories = new MemoriesManager();

    public MemoriesManager conversationalMemories = new MemoriesManager();

    public List<PromptPart> factsAboutPlayer;


    public void ChangeAttraction(int value)
    {
        attractionLevel = Mathf.Clamp(attractionLevel + value, -10, 10);
    }

}
