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
    private string itemName;
    public ClothesSO clothesSO;
    public static Action<ClothesSO> OnBuyItem;
    public static Action OnUseItem;
    public static Action<bool, bool> OnSelectedItem;
    public bool isSelected;

    public void Init(ClothesSO clothesSO)
    {
        this.clothesSO = clothesSO;
        displayImage = clothesSO.displayImage;
        itemPrice = clothesSO.price;
        itemName = clothesSO.nameItem;
    }

    private void BuyItem()
    {
        if(!isBuyed)
        {
            
        }
    }

    private void UseItem()
    {

    }

    public void OnSelect()
    {   
        isSelected =  !isSelected;
    
        OnSelectedItem?.Invoke(isSelected, isBuyed);
    }
}
