using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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
        public ClothesSO itemStore;
        public bool isBuyed;
    }
    [SerializeField] private Items[] itemsStore;
    [SerializeField] private FilterState filterState;
    // [SerializeField] private ClothesSO[] itemsStore;
    [SerializeField] private Transform[] Categories;
    [SerializeField] private ItemClothes itemCellPrefab;

    private List<ItemClothes> itemClothes = new List<ItemClothes>();

    private void Start()
    {
        Init();
        ChangeFilterCategory(FilterState.Purchased);
    }

    private void Init()
    {
        UIManager.instance.newClothesFiltButton.onClick.AddListener(() => ChangeFilterCategory(FilterState.New));
        UIManager.instance.defaultClothesFiltButton.onClick.AddListener(() => ChangeFilterCategory(FilterState.Default));
        UIManager.instance.purchasedClothesFiltButton.onClick.AddListener(() => ChangeFilterCategory(FilterState.Purchased));

        foreach (var item in itemsStore)
        {
            foreach (var category in Categories)
            {
                if(category.name == item.itemStore.itemCategory.ToString())
                {
                    ItemClothes cell = Instantiate<ItemClothes>(itemCellPrefab, category);
                    cell.Init(item.itemStore, item.isBuyed);
                    itemClothes.Add(cell);
                    // Debug.Log($"ADD to {category.name} an {item}");

                    break;
                }
            }            
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

                break;

            case FilterState.New:

                foreach (var item in itemClothes)
                {
                    item.gameObject.SetActive(!item.isBuyed);
                }

                break;

            case FilterState.Purchased:
                
                foreach (var item in itemClothes)
                {
                    item.gameObject.SetActive(item.isBuyed);
                }

                break;
        }
    }



}
