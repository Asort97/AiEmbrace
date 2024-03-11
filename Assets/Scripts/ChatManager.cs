using System;
using System.Collections;
using System.Collections.Generic;
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
    }

    private void OnEnable() 
    {
        ClientAPI.OnResponcePrompt += DrawNewMessage;
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

    public async void SendMsgRequest()
    {
        if(UIManager.instance.inputFieldChat.text != "" && AIDataManager.instance.aiCharactersData.GetAICharacterData("Misa") != null)
        {
            DrawNewMessage("You", UIManager.instance.inputFieldChat.text, true);

            currentAI.chatHistory.Append(new Reply("Player", UIManager.instance.inputFieldChat.text));
            currentAI.chatHistory.Append(new Reply(currentAI.characterName, ""));

            var prompt = await currentAI.GeneratePrompt();

            if( aiIsWaiting )
            {
                // Add XPы
                XpManager.instance.AddXp(150);
                // aiIsWaiting = false;
            }

            UIManager.instance.ClearInputFieldChat();

            OnSendPromtAI?.Invoke(prompt);
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
        Debug.Log($"{isPlayer}");
        if(isPlayer)
        {
            Message newMsg = Instantiate<Message>(msgPrefab, mainContainer);
            newMsg.Init(isPlayer, name, msg);
            // emotionController.PlayRandomAnimation();

            // MsgResponce();
        }
        else
        {
            Message aiMsg = Instantiate<Message>(aiMsgPrefab, mainContainer);
            aiMsg.Init(isPlayer, name, msg);
            
            AudioManager.Instance.PlayNofiticationSound();
        }

        OnDrawMessage?.Invoke();            
    }
}
