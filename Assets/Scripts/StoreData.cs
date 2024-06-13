using System;
using System.Collections.Generic;
using System.Data.Common;


public enum ItemCategory
{
    Clothes,
    Background,
    EmotionStand,
    CharacterPreset
}    

[System.Serializable]
public class Item
{
    public string ItemId;
    public ItemCategory Category;
    public bool IsPurchased;
    public bool IsUsedDefault;

    public Item(string id, ItemCategory category, bool isPurchased, bool isUsedDefault)
    {
        ItemId = id;
        Category = category;
        IsPurchased = isPurchased;
        IsUsedDefault = isUsedDefault;
    }
}

[System.Serializable]
public class StoreData 
{
    public List<Item> Items;
}
