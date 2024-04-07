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
    public bool isBuyed;
    private int itemPrice;
    public string itemName;
    public ItemSO clothesSO;
    public static Action<int> OnBuyItem;
    public static Action<bool, string, string, ItemClothes> OnShowBuyBtn;
    public static Action<string, ItemClothes> OnShowUseBtn;
    public static Action<ItemSO, bool, bool> OnUseItem;
    public static Action<ItemClothes> OnSelectedItem;
    public bool isSelected;

    public void Init(ItemSO clothesSO, bool isBuyed)
    {
        this.clothesSO = clothesSO;
        
        displayImage.sprite = clothesSO.displayImage;
        displayImage.color = clothesSO.imageColor;
        priceText.text = clothesSO.price.ToString();

        itemPrice = clothesSO.price;
        itemName = clothesSO.nameItem;
        
        this.isBuyed = isBuyed;

        if(isBuyed)
        {
            priceText.text = "purchased";
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
        }
    }

    public void BuyItem()
    {
        if(!isBuyed && CoinsManager.instance.CheckEnoughCoins(itemPrice))
        {
            Debug.Log($"Buyed {itemName}");

            OnBuyItem?.Invoke(itemPrice);
            
            PopUpNotifications.instance.ShowNotification(PopUpNotifications.NofStatus.SuccessPurchased);
            
            priceText.text = "purchased";
            isBuyed = true;
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
        OnUseItem?.Invoke(clothesSO, isSelected, false);
    }

    public void OnSelect()
    {   
        isSelected =  !isSelected;
        
        if(isBuyed)
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
