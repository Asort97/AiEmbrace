using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    [SerializeField] private GameObject chatMenu;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private Slider xpSlider;
    [SerializeField] private Text levelText;
    [SerializeField] private float smoothXpSlider;
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

    public void UpdateXPSlider(float amount, int level)
    {
        xpSlider.DOValue(amount, smoothXpSlider, false);

        levelText.text = level.ToString();
    }
}
