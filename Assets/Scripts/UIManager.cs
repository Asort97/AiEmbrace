using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
using Unity.VisualScripting;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    [SerializeField] private GameObject chatMenu;
    [SerializeField] private GameObject storeMenu;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private Slider xpSlider;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private float smoothXpSlider;
    [SerializeField] private GameObject buyButton;
    // [SerializeField] private GameObject buyButton;
    public TMP_InputField inputFieldChat;
    private float diff;

    private void Awake()
    {
        instance = this;
    }

    private void OnEnable()
    {
        ItemClothes.OnSelectedItem += ShowBuyButton;
    }
    private void OnDisable()
    {
        ItemClothes.OnSelectedItem += ShowBuyButton;
    }

    public void ShowBuyButton(bool isSelected, bool isBuyed)
    {
        if(isSelected && !isBuyed)
        {
            buyButton.SetActive(true);
        }
        else if(!isSelected)
        {
            buyButton.SetActive(false);
        }
        else if(isSelected)
        {
            Debug.Log($"Wear a clothes");
        }
    }

    public void SetEnableChat(bool isEnable)
    {
        chatMenu.SetActive(isEnable);
        mainMenu.SetActive(!isEnable);
    }

    public void SetEnableStore(bool isEnable)
    {
        storeMenu.SetActive(isEnable);
        mainMenu.SetActive(!isEnable);
    }

    public void ClearInputFieldChat()
    {
        inputFieldChat.text = "";
    }

    public void UpdateXPSlider(float amount, float maxAmount, int level)
    {
        levelText.text = level.ToString();

        xpSlider.maxValue = maxAmount;
        xpSlider.value = amount;
    }

    private void RefreshXPSlider(float amount, int level)
    {
    }
}
