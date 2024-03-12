using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ChatManager : MonoBehaviour
{   
    public static ChatManager instance;
    public static Action<string> OnSendPromtAI;
    public static Action OnDrawMessage;
    [SerializeField] private EmotionController emotionController;
    [SerializeField] private Message msgPrefab;
    [SerializeField] private Message aiMsgPrefab;
    [SerializeField] private Transform mainContainer;
    [SerializeField] private Transform BotMessagesContainer;
    private bool aiIsWaiting;
    public AICharacterData currentAI;

    private void Awake()
    {
        instance = this;
        
        Debug.Log("ChatManager Awake");

    }

    private async void OnEnable() 
    {
        ClientAPI.OnResponcePrompt += DrawNewMessage;

        // todo: remove this bicycle and manage manager initialization
        await Task.Delay(2000);
        InitChatHistory("Misa");
    }

    private void OnDisable()
    {
        ClientAPI.OnResponcePrompt -= DrawNewMessage;
    }

    private void Start()
    {
        // currentAI = AIDataManager.instance.aiCharactersData.GetAICharacterData("Misa");

        aiIsWaiting = true;

    }

    public void InitChatHistory(string name)
    {
        /* 
         * Init chat history for AI
         * 
         */

        // clear old messages
        ClearChatHistory();

        Debug.Log("InitChatHistory");
        Debug.Log(AIDataManager.instance);
        // pick AI from data
        currentAI = AIDataManager.instance.aiCharactersData.GetAICharacterData(name);

        if(currentAI != null)
        {
            // draw all replies from chatHistory
            foreach (var reply in currentAI.chatHistory.GetReplies())
            {
                bool isPlayer = false;
                string _name = reply.name;
                if(reply.name == "player_name")
                {
                    _name = "You";
                    isPlayer = true;
                }
                if (reply.name == "character_name")
                {
                    _name = currentAI.characterName;
                }
                AddMessage(_name, reply.message, isPlayer);
            }
        }
    }

    public async void SendMsgRequest()
    {
        // Run on player message send

        if(UIManager.instance.inputFieldChat.text != "" && AIDataManager.instance.aiCharactersData.GetAICharacterData("Misa") != null)
        {
            DrawNewMessage("You", UIManager.instance.inputFieldChat.text, true);

            currentAI.chatHistory.Append(new Reply("Player", UIManager.instance.inputFieldChat.text));
            currentAI.chatHistory.Append(new Reply(currentAI.characterName, ""));

            if( aiIsWaiting )
            {
                // Add XPы
                XpManager.instance.AddXp(150);
                // aiIsWaiting = false;
            }

            UIManager.instance.ClearInputFieldChat();

            string prompt = await currentAI.GeneratePrompt();
            MessageResponse response = await ClientAPI.instance.RunLLM(prompt);
            Debug.Log(response);
            if (response.success)
            {
                currentAI.chatHistory.SetLastReply(response.text);
                DrawNewMessage(currentAI.characterName, response.text, false);
            }else
            {
                // todo: if error is GPU, show this error to user
                Debug.LogError("Error while sending prompt to AI");
            }
        }
    }

    public void MsgResponce()
    {
        // Логика отправки AI сообщения
        DrawNewMessage("BOT", "Yes", false); // тест
        
        aiIsWaiting = true;
        // DrawNewMessage(name, text);
    }
    
    public void DrawNewMessage(string name, string msg, bool isPlayer)
    {
        // Adding new message to chat history with some logic

        Debug.Log($"{isPlayer}");
        if(isPlayer)
        {
            AddMessage(name, msg, isPlayer);
            // emotionController.PlayRandomAnimation();
            // MsgResponce();
        }
        else
        {
            AddMessage(name, msg, isPlayer);
            
            AudioManager.Instance.PlayNofiticationSound();
        }

        OnDrawMessage?.Invoke();            
    }

    public void ClearChatHistory()
    {
        foreach (Transform child in mainContainer)
        {
            Destroy(child.gameObject);
        }
    }

    private void AddMessage(string name, string text, bool isPlayer)
    {
        // Just add message to chat history

        if (isPlayer)
        {
            Message newMsg = Instantiate<Message>(msgPrefab, mainContainer);
            newMsg.Init(isPlayer, name, text);
        }
        else
        {
            Message aiMsg = Instantiate<Message>(aiMsgPrefab, mainContainer);
            aiMsg.Init(isPlayer, name, text);
        }
    }
}
