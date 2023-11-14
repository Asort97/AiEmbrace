using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatManager : MonoBehaviour
{   
    public static Action OnDrawMessage;
    [SerializeField] private Message msgPrefab;
    [SerializeField] private Transform MessagesContainer;

    private void Start()
    {
        DrawNewMessage("Assistant", "Now i talk with you");
        DrawNewMessage("Assistant", "Now i talk with you");
        DrawNewMessage("Assistant", "Now i talk with you");
        DrawNewMessage("Assistant", "Now i talk with you");
        DrawNewMessage("Assistant", "Now i talk with you");
        DrawNewMessage("Assistant", "Now i talk with you");
        DrawNewMessage("Assistant", "Now i talk with you");
        DrawNewMessage("Assistant", "Now i talk with you");
        DrawNewMessage("Assistant", "Now i talk with you");
        DrawNewMessage("Assistant", "Now i talk with you");
        DrawNewMessage("Assistant", "Now i talk with you");

        DrawNewMessage("Assistant", "Now i talk with you");
        DrawNewMessage("Assistant", "Now i talk with you");
        DrawNewMessage("Assistant", "Now i talk with you");
        DrawNewMessage("Assistant", "Now i talk with you");
        DrawNewMessage("Assistant", "Now i talk with you");
        DrawNewMessage("Assistant", "Now i talk with you");
        DrawNewMessage("Assistant", "Now i talk with you");
        DrawNewMessage("Assistant", "Now i talk with you");
        DrawNewMessage("Assistant", "Now i talk with you");
        DrawNewMessage("Assistant", "Now i talk with you");
        DrawNewMessage("Assistant", "Now i talk with you");
        DrawNewMessage("Assistant", "Now i talk with you");
        DrawNewMessage("Assistant", "Now i talk with you");
        DrawNewMessage("Assistant", "Now i talk with you");
        DrawNewMessage("Assistant", "Now i talk with you");
        DrawNewMessage("Assistant", "Now i talk with you");

        StartCoroutine(sss());
    }
    

    IEnumerator sss()
    {
        yield return new WaitForSeconds(5f);


        DrawNewMessage("!!!!!!", "11111111111111111111");
    }

    public void DrawNewMessage(string name, string msg)
    {
        Message newMsg = Instantiate<Message>(msgPrefab, MessagesContainer);
        newMsg.Init(name, msg);

        OnDrawMessage?.Invoke();
    }
}
