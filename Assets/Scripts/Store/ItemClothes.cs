using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

public class ItemClothes : MonoBehaviour
{
    [SerializeField] private Image displayImage;
    [SerializeField] private TMP_Text priceText;
    [SerializeField] private Image priceImage;
    [SerializeField] private Sprite purchasedIcon;
    private int itemPrice;
    private Outline outline;
    public bool isPurchased;
    
    public string itemName;
    public ItemSO clothesSO;
    public static Action<int> OnBuyItem;
    public static Action<bool, string, string, ItemClothes> OnShowBuyBtn;
    public static Action<string, ItemClothes> OnShowUseBtn;
    public static Action<ItemSO, bool, bool> OnUseItem;
    public static Action<ItemClothes> OnSelectedItem;
    public bool isSelected;

    private void Start() 
    {
        outline = GetComponent<Outline>();    
    }

    public void Init(ItemSO clothesSO, bool isPurchased)
    {
        this.clothesSO = clothesSO;
        
        displayImage.sprite = clothesSO.displayImage;
        displayImage.color = clothesSO.imageColor;
        priceText.text = clothesSO.price.ToString();

        itemPrice = clothesSO.price;
        itemName = clothesSO.nameItem;
        
        this.isPurchased = isPurchased;

        if(isPurchased)
        {
            priceText.text = "";
            priceImage.sprite = purchasedIcon;
        }
    }

    public void OnEnable()
    {
        ItemClothes.OnSelectedItem += CheckThisItem;
    }
    public void OnDisable()
    {
        ItemClothes.OnSelectedItem -= CheckThisItem;
    }

    private void CheckThisItem(ItemClothes item)
    {
        if(item != this)
        {
            isSelected = false;
            outline.enabled = false;
        }
    }

    public void BuyItem()
    {
        if(!isPurchased && CoinsManager.instance.CheckEnoughCoins(itemPrice))
        {
            Debug.Log($"Buyed {itemName}");

            OnBuyItem?.Invoke(itemPrice);
            
            PopUpNotifications.instance.ShowNotification(PopUpNotifications.NofStatus.SuccessPurchased);
            
            priceText.text = "";
            priceImage.sprite = purchasedIcon;

            isPurchased = true;
        }
        else if(!CoinsManager.instance.CheckEnoughCoins(itemPrice))
        {
            PopUpNotifications.instance.ShowNotification(PopUpNotifications.NofStatus.NotEnoughCash);
        }
    }

    private void ShowBuyButton()
    {
        OnShowBuyBtn?.Invoke(isSelected, itemName, itemPrice.ToString(), this);
    }

    private void ShowUseButton()
    {
        OnShowUseBtn?.Invoke(itemName, this);
    }

    public void UseItem()
    {
        Debug.Log($"On");
        OnUseItem?.Invoke(clothesSO, isSelected, false);
        outline.enabled = isSelected;
    }

    public void OnSelect()
    {   
        isSelected =  !isSelected;

        if(isPurchased)
        {
            ShowUseButton();
        }
        else
        {
            ShowBuyButton();
        }

        if(clothesSO.showDescriptionMenu)
        {
            PopUpNotifications.instance.ShowNotification(((CharacterSO)clothesSO).characterDescription);
        }

        OnSelectedItem?.Invoke(this);
    }
}
