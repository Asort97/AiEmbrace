using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

[Serializable]
public class AICharacterDataWrapper
{
    public List<AICharacterDataPair> aiCharacterDataList = new List<AICharacterDataPair>();

    public AICharacterData GetAICharacterData(string name)
    {
        // пытаемся достать персонажа из списка, иначе возвращаем null
        if (aiCharacterDataList.Where(x => x.name == name).FirstOrDefault() == null)
        {
            return null;
        }
        return aiCharacterDataList.Where(x => x.name == name).FirstOrDefault().aICharacterData;
    }

    public void SetAICharacterData(string name, AICharacterData aICharacterData)
    {
        AICharacterDataPair pair = aiCharacterDataList.Where(x => x.name == name).FirstOrDefault();
        if (pair != null)
        {
            pair.aICharacterData = aICharacterData;
            Debug.Log("pair.aICharacterData = " + pair.aICharacterData);

        }
        else
        {
            Debug.Log("pair == null");
            aiCharacterDataList.Add(new AICharacterDataPair() { name = name, aICharacterData = aICharacterData });
        }
    }
}

[Serializable]
public class AICharacterDataPair
{
    public string name;
    public AICharacterData aICharacterData;
}

[Serializable]
public class AIDataManager : MonoBehaviour
{
    // Содержит актуальные данные ИИ персонажей
    /*
     * todo: сейчас класс AICharacterData используется для каждого нпц отдельно и хранит в себе все данные, которые нужны для общения с LLM моделью
     * нужно сделать так, чтобы данные хранились в одном месте и были доступны для всех нпц
     * для этого нужно:
     * -1. создать класс AIDataManager, который будет хранить все данные ИИ персонажей
     * -2. добавить в него хранилище данных для каждого персонажа
     * 3. заменить все обращения к данным в классе AICharacterData на обращения к данным в AIDataManager
     * 4. заменить AICharacterData на ключ, по которому можно получить данные из AIDataManager
     * 5. организовать AIDataManager в приложении следующим образом: создать его в GameManager, а затем передавать во все нужные места
    */
    public static AIDataManager instance;

    [SerializeField]
    public AICharacterDataWrapper aiCharactersData = new AICharacterDataWrapper();

    public List<AIChatConversationalMemoryPrefab> examplesOfConversationalMemory;
    public ConversationalBehavior attractionBehavior;

    private void Start()
    {
        instance = this;

        DontDestroyOnLoad(gameObject);
    }

    // функция для установки имени игрока
    // замена Player на PlayerName
    public void ConfirmPlayerName(string playerName)
    {
        // для каждого персонажа в списке
        foreach (var data in aiCharactersData.aiCharacterDataList)
        {
            // для каждой реплики заменяем имя игрока и его упоминания на имя, которое ввел игрок
            foreach (var reply in data.aICharacterData.chatHistory.GetReplies())
            {
                reply.name = reply.name.Replace("Player", playerName);
                reply.message = reply.message.Replace("Player", playerName);
            }
        }
    }

    public void UpdateCharactersChatFromDialog(List<string> charactersNames, DialogPro dialog, bool needToExtendList)
    {
        if (needToExtendList)
        {
            // добавляем с писок имен все имена, которые встречаются в диалоге
            foreach (var name in dialog.GetNames())
            {
                if (!charactersNames.Contains(name))
                {
                    charactersNames.Add(name);
                }
            }
        }

        foreach (var characterName in charactersNames)
        {
            // получаем данные персонажа
            AICharacterData data = aiCharactersData.GetAICharacterData(characterName);
            if (data == null)
            {
                // если такого персонажа нет, то пропускаем
                Debug.LogWarning("UpdateChatFromDialog: data == null for character: " + characterName);
                continue;
            }

            foreach (var phrases in dialog.phrases)
            {
                if (phrases.character.name == "")
                {
                    // skip narrator todo: replace with narrator name or something
                    continue;
                }
                foreach (var phrase in phrases.lines)
                {
                    // добавляем реплику в историю чата с персонажем
                    data.chatHistory.Append(new Reply(phrases.character.name, phrase));
                }
            }
        }
    }

    // todo: добавить методы для сохранения и загрузки данных
}
