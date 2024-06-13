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
    private bool isUsed;
    private Outline outline;
    public bool isPurchased;
    public string itemId;
    public ItemSO itemSO;
    public static Action<int> OnBuyItem;
    public static Action<bool, string, string, ItemClothes> OnShowBuyBtn;
    public static Action<string, ItemClothes> OnShowUseBtn;
    public static Action<ItemSO, bool, bool> OnUseItem;
    public static Action<ItemClothes> OnSelectedItem;
    public bool isSelected;

    public void Init(string id, bool isPurchased, bool isUsed)
    {
        outline = GetComponent<Outline>();

        itemSO = UserDataManager.Instance.GetItemSOById(id);

        if(itemSO)
        {
            this.isPurchased = isPurchased;
            this.isUsed =  isUsed;        

            displayImage.sprite = itemSO.displayImage;
            displayImage.color = itemSO.imageColor;
            priceText.text = itemSO.price.ToString();

            itemPrice = itemSO.price;
            itemId = itemSO.idItem;
            
            if(isPurchased)
            {
                priceText.text = "";
                priceImage.sprite = purchasedIcon;
            }
            if(isUsed)
            {
                outline.enabled = true;
            }
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

    public async void BuyItem()
    {
        if(!isPurchased && CoinsManager.instance.CheckEnoughCoins(itemPrice))
        {
            Debug.Log($"Buyed {itemId}");

            OnBuyItem?.Invoke(itemPrice);
            
            PopUpNotifications.instance.ShowNotification(PopUpNotifications.NofStatus.SuccessPurchased);
            
            priceText.text = "";
            priceImage.sprite = purchasedIcon;

            UserDataManager.Instance.data.storeData.Items[UserDataManager.Instance.GetIndexItemById(itemId)].IsPurchased = true;
            isPurchased = true;

            var GameDataManager = new GameDataManager();
            await GameDataManager.SaveGameData(); 
        }
        else if(!CoinsManager.instance.CheckEnoughCoins(itemPrice))
        {
            PopUpNotifications.instance.ShowNotification(PopUpNotifications.NofStatus.NotEnoughCash);
        }
    }

    private void ShowBuyButton()
    {
        OnShowBuyBtn?.Invoke(isSelected, itemId, itemPrice.ToString(), this);
    }

    private void ShowUseButton()
    {
        OnShowUseBtn?.Invoke(itemId, this);
    }

    public void UseItem()
    {
        Debug.Log($"On");
        OnUseItem?.Invoke(itemSO, isSelected, false);
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

        if(itemSO.showDescriptionMenu)
        {
            PopUpNotifications.instance.ShowNotification(((CharacterSO)itemSO).characterDescription);
        }

        OnSelectedItem?.Invoke(this);
    }
}
