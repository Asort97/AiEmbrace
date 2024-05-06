using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class StoreContent : MonoBehaviour
{
    [Serializable]
    public enum FilterState
    {
        Purchased,
        New,
        Default
    }
    
    [Serializable]
    public class Items
    {
        public ItemSO itemStore;
        public bool isBuyed;
    }
    [SerializeField] private Items[] itemsStore;
    [SerializeField] private FilterState filterState;
    [SerializeField] private Transform[] Categories;
    [SerializeField] private ItemClothes itemCellPrefab;

    [Space(5)]
    [SerializeField] public Button newClothesFiltButton;
    [SerializeField] public Button defaultClothesFiltButton;
    [SerializeField] public Button purchasedClothesFiltButton;

    private List<ItemClothes> itemClothes = new List<ItemClothes>();

    private void Start()
    {
        Init();
        ChangeFilterCategory(FilterState.New);
    }

    private void Init()
    {
        newClothesFiltButton.onClick.AddListener(() => ChangeFilterCategory(FilterState.New));
        defaultClothesFiltButton.onClick.AddListener(() => ChangeFilterCategory(FilterState.Default));
        purchasedClothesFiltButton.onClick.AddListener(() => ChangeFilterCategory(FilterState.Purchased));

        foreach (var item in itemsStore)
        {
            // foreach (var category in Categories)
            // {
                // if(category.name == item.itemStore.itemCategory.ToString())
                // {
                    ItemClothes cell = Instantiate<ItemClothes>(itemCellPrefab, Categories[0]);
                    cell.Init(item.itemStore, item.isBuyed);
                    itemClothes.Add(cell);
                    Debug.Log($"ADD to {Categories[0]} an {item}");

                    // break;
                // }
            // }            
        }
    }

    public void ChangeFilterCategory(FilterState state)
    {
        filterState = state;

        switch (filterState)
        {
            case FilterState.Default:

                foreach (var item in itemClothes)
                {
                    item.gameObject.SetActive(true);
                }

                purchasedClothesFiltButton.image.color = Color.white;
                newClothesFiltButton.image.color = Color.white;
                defaultClothesFiltButton.image.color = Color.red;

                break;

            case FilterState.New:

                foreach (var item in itemClothes)
                {
                    item.gameObject.SetActive(!item.isPurchased);
                }
                
                purchasedClothesFiltButton.image.color = Color.white;
                newClothesFiltButton.image.color = Color.red;
                defaultClothesFiltButton.image.color = Color.white;

                break;

            case FilterState.Purchased:
                
                foreach (var item in itemClothes)
                {
                    item.gameObject.SetActive(item.isPurchased);
                }

                purchasedClothesFiltButton.image.color = Color.red;
                newClothesFiltButton.image.color = Color.white;
                defaultClothesFiltButton.image.color = Color.white;

                break;
        }
    }



}
