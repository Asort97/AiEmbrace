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

    public Button newClothesFiltButton;
    public Button defaultClothesFiltButton;
    public Button purchasedClothesFiltButton;

    public TMP_InputField inputFieldChat;
    public ItemClothes prevItem;
    private float diff;

    private void Awake()
    {
        instance = this;
    }

    private void OnEnable()
    {
        ItemClothes.OnShowBuyBtn += ShowBuyItemButton;
    }
    private void OnDisable()
    {
        ItemClothes.OnShowBuyBtn -= ShowBuyItemButton;
    }

    // public void SelectItem(bool isSelected, bool isBuyed, ItemClothes currentItem)
    // {
    //     if(isSelected)
    //     {

    //     }
    // }

    private void ShowBuyItemButton(bool isEnable, string name, string price, ItemClothes item)
    {
        buyButton.gameObject.SetActive(true);
        buyButtonText.text = name;

        buyButton.onClick.RemoveAllListeners();
        buyButton.onClick.AddListener(item.BuyItem);
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
}
