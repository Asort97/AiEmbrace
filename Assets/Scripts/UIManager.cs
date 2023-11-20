using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    [SerializeField] private GameObject chatMenu;
    [SerializeField] private GameObject mainMenu;
    public TMP_InputField inputFieldChat;

    private void Awake()
    {
        instance = this;
    }

    public void SetEnableChat(bool isEnable)
    {
        chatMenu.SetActive(isEnable);
        mainMenu.SetActive(!isEnable);
    }

    public void ClearInputFieldChat()
    {
        inputFieldChat.text = "";
    }
}
