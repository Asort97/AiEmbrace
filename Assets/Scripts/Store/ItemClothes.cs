using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.PlayerLoop;
using Microsoft.Unity.VisualStudio.Editor;

public class ItemClothes : MonoBehaviour
{
    [SerializeField] private Image displayImage;
    [SerializeField] private bool isBuyed;
    private int itemPrice;
    public string itemName;
    public ClothesSO clothesSO;
    public static Action<int> OnBuyItem;
    public static Action<ClothesSO> OnUseItem;
    public static Action<bool, bool, ItemClothes> OnSelectedItem;
    public bool isSelected;

    public void Init(ClothesSO clothesSO)
    {
        this.clothesSO = clothesSO;
        displayImage = clothesSO.displayImage;
        itemPrice = clothesSO.price;
        itemName = clothesSO.nameItem;
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

    public void UseItem()
    {
        OnUseItem?.Invoke(clothesSO);
    }

    public void OnSelect()
    {   
        isSelected =  !isSelected;
    
        OnSelectedItem?.Invoke(isSelected, isBuyed, this);
    }
}
