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
    [SerializeField] private TMP_Text levelText;
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
        float difference = xpSlider.maxValue - xpSlider.value;

        if(difference >= amount)
        {
            xpSlider.DOValue(amount, smoothXpSlider);
        }
        else
        {
            Debug.Log($"big diff {difference} and {amount - difference}");
            xpSlider.DOValue(xpSlider.value + difference, smoothXpSlider).OnComplete(() => RefreshXPSlider(amount - difference));
        }

        levelText.text = level.ToString();
    }

    private void RefreshXPSlider(float amount)
    {
        xpSlider.value = 0f;
        xpSlider.DOValue(xpSlider.value + amount, smoothXpSlider);
    }

    public void UpdateMaxValueXPSlider(float value)
    {
        xpSlider.maxValue = value;
    }
}
