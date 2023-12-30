using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class ItemClothes : MonoBehaviour
{
    [SerializeField] private Image displayImage;
    public bool isBuyed;
    private int itemPrice;
    public string itemName;
    public ClothesSO clothesSO;
    public static Action<int> OnBuyItem;
    public static Action<bool, string, string, ItemClothes> OnShowBuyBtn;
    public static Action<ClothesSO, bool> OnUseItem;
    public static Action<ItemClothes> OnSelectedItem;
    public bool isSelected;

    public void Init(ClothesSO clothesSO, bool isBuyed)
    {
        this.clothesSO = clothesSO;
        
        displayImage.sprite = clothesSO.displayImage;
        displayImage.color = clothesSO.color;

        itemPrice = clothesSO.price;
        itemName = clothesSO.nameItem;
        
        this.isBuyed = isBuyed;
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

            isBuyed = true;
        }
    }

    private void ShowBuyButton()
    {
        OnShowBuyBtn?.Invoke(isSelected, itemName, itemPrice.ToString(), this);
    }

    private void UseItem(bool isEnable)
    {
        OnUseItem?.Invoke(clothesSO, isEnable);
    }

    public void OnSelect()
    {   
        isSelected =  !isSelected;
        
        if(isBuyed)
        {
            UseItem(isSelected);
        }
        else
        {
            ShowBuyButton();
        }

        OnSelectedItem?.Invoke(this);
    }
}
