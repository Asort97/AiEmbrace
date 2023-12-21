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
    [SerializeField] private GameObject[] allMenu;
    [SerializeField] private GameObject chatMenu;
    [SerializeField] private GameObject storeMenu;
    [SerializeField] private GameObject clothesMenu;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private Slider xpSlider;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private float smoothXpSlider;
    [SerializeField] private Button buyButton;
    [SerializeField] private TMP_Text buyButtonText;
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
        ItemClothes.OnSelectedItem -= ShowBuyButton;
    }

    public void ShowBuyButton(bool isSelected, bool isBuyed, ItemClothes currentItem)
    {
        if(isSelected && !isBuyed)
        {
            buyButton.gameObject.SetActive(true);
            buyButtonText.text = currentItem.itemName;
            buyButton.onClick.AddListener(currentItem.BuyItem);
        }
        else if(!isSelected)
        {
            buyButton.gameObject.SetActive(false);
            buyButtonText.text = "";
            buyButton.onClick.RemoveAllListeners();
        }
        
        else if(isSelected && isBuyed)
        {
            currentItem.UseItem();

            Debug.Log($"Wear a clothes");

            buyButtonText.text = "";
            buyButton.onClick.RemoveAllListeners();
        }
    }

    public void SetEnableChat(bool isEnable)
    {
        chatMenu.SetActive(isEnable);
        mainMenu.SetActive(!isEnable);
    }

    public void SetEnableStore(bool isEnable)
    {
        foreach (GameObject item in allMenu)
        {
            item.SetActive(false);
        }
        storeMenu.SetActive(isEnable);
        mainMenu.SetActive(!isEnable);
    }

    public void SetEnableMenu(GameObject menu)
    {
        foreach (GameObject item in allMenu)
        {
            item.SetActive(false);
        }

        menu.SetActive(true);
    }

    public void SetEnableClother(bool isEnable)
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
