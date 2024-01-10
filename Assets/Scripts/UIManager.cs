using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    [SerializeField] private GameObject[] allMenu;
    [SerializeField] private Slider xpSlider;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private Button buyButton;
    [SerializeField] private Button useButton;
    [SerializeField] private TMP_Text buyButtonText;
    [SerializeField] private TMP_Text useButtonText;

    [SerializeField] private GameObject nofiticationPanel;
    [SerializeField] private TMP_Text nofiticationText;

    public TMP_InputField inputFieldChat;

    private void Awake()
    {
        instance = this;
    }

    private void OnEnable()
    {
        ItemClothes.OnShowBuyBtn += ShowBuyItemButton;
        ItemClothes.OnShowUseBtn += ShowUseItemButton;
    }
    private void OnDisable()
    {
        ItemClothes.OnShowBuyBtn -= ShowBuyItemButton;
        ItemClothes.OnShowUseBtn -= ShowUseItemButton;
    }

    private void ShowBuyItemButton(bool isEnable, string name, string price, ItemClothes item)
    {
        useButton.gameObject.SetActive(false);
        buyButton.gameObject.SetActive(true);

        buyButtonText.text = name;

        buyButton.onClick.RemoveAllListeners();
        buyButton.onClick.AddListener(item.BuyItem);
    }

    private void ShowUseItemButton(string name, ItemClothes item)
    {
        buyButton.gameObject.SetActive(false);
        useButton.gameObject.SetActive(true);

        useButtonText.text = name;

        useButton.onClick.RemoveAllListeners();
        useButton.onClick.AddListener(item.UseItem);
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

    public void ShowNofiticationPanel(string info)
    {
        nofiticationPanel.SetActive(true);
        nofiticationText.text = info;
    }

    public void CloseNofitication()
    {
        nofiticationPanel.SetActive(false);
    }
    
    public void CloseUseButton()
    {
        useButton.gameObject.SetActive(false);
    }
    
    public void CloseBuyButton()
    {
        buyButton.gameObject.SetActive(false);
    }
}
