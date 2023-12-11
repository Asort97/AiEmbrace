using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatManager : MonoBehaviour
{   
    public static Action OnDrawMessage;
    [SerializeField] private Message msgPrefab;
    [SerializeField] private Message zeroMsgPrefab;
    [SerializeField] private Transform PlayerMessagesContainer;
    [SerializeField] private Transform BotMessagesContainer;
    private bool aiIsWaiting;

    private void Start()
    {
        DrawNewMessage("BOT", "Now i talk with you", false);
        aiIsWaiting = true;
    }

    public void SendMsgRequest()
    {
        if(UIManager.instance.inputFieldChat.text != "")
        {
            DrawNewMessage("You", UIManager.instance.inputFieldChat.text, true);

            Debug.Log($"Send request to AI");

            if( aiIsWaiting )
            {
                // Add XPы
                XpManager.instance.AddXp(150);
                // aiIsWaiting = false;
            }

            UIManager.instance.ClearInputFieldChat();
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
        if(isPlayer)
        {
            Message newMsg = Instantiate<Message>(msgPrefab, PlayerMessagesContainer);
            Message zeroMsg = Instantiate<Message>(zeroMsgPrefab, BotMessagesContainer);
            newMsg.Init(name, msg);
            zeroMsg.Init(name, msg);

            MsgResponce();
        }
        else
        {
            Message newMsg = Instantiate<Message>(msgPrefab, BotMessagesContainer);
            Message zeroMsg = Instantiate<Message>(zeroMsgPrefab, PlayerMessagesContainer);
            newMsg.Init(name, msg);
            zeroMsg.Init(name, msg);
        }

        OnDrawMessage?.Invoke();            
    }
}
