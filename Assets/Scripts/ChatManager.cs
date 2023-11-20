using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatManager : MonoBehaviour
{   
    public static Action OnDrawMessage;
    [SerializeField] private Message msgPrefab;
    [SerializeField] private Transform MessagesContainer;
    private bool aiIsWaiting;

    private void Start()
    {
        DrawNewMessage("Assistant", "Now i talk with you");
        aiIsWaiting = true;
    }

    public void SendMsgRequest()
    {
        if(UIManager.instance.inputFieldChat.text != "")
        {
            DrawNewMessage("You", UIManager.instance.inputFieldChat.text);

            Debug.Log($"Send request to AI");

            if( aiIsWaiting )
            {
                // Add XP
                XpManager.instance.AddXp();

                aiIsWaiting = false;
            }

            UIManager.instance.ClearInputFieldChat();
        }
    }

    public void MsgResponce()
    {
        // Логика отправки AI сообщения


        aiIsWaiting = true;
        // DrawNewMessage(name, text);
    }
    
    public void DrawNewMessage(string name, string msg)
    {
        Message newMsg = Instantiate<Message>(msgPrefab, MessagesContainer);
        newMsg.Init(name, msg);

        OnDrawMessage?.Invoke();
    }
}
