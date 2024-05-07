using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;
using UnityEditor.Experimental.GraphView;

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
        public bool isPurchased;
    }
    [SerializeField] private Items[] itemsStore;
    [SerializeField] private FilterState filterState;
    [SerializeField] private Transform[] Categories;
    [SerializeField] private ItemClothes itemCellPrefab;

    [Space(5)]
    [SerializeField] private List<Button> filterButton = new List<Button>();
    private List<ItemClothes> itemClothes = new List<ItemClothes>();

    private void Start()
    {
        Init();
        ChangeFilterCategory(FilterState.New);
    }

    private void Init()
    {
        filterButton[0].onClick.AddListener(() => ChangeFilterCategory(FilterState.Default));        
        filterButton[1].onClick.AddListener(() => ChangeFilterCategory(FilterState.New));
        filterButton[2].onClick.AddListener(() => ChangeFilterCategory(FilterState.Purchased));

        foreach (var item in itemsStore)
        {
            ItemClothes cell = Instantiate<ItemClothes>(itemCellPrefab, Categories[0]);
            cell.Init(item.itemStore, item.isPurchased);
            itemClothes.Add(cell);
            Debug.Log($"ADD to {Categories[0]} an {item}");
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

                ChangeSelectedFilterButton(filterButton[0]);
                break;

            case FilterState.New:

                foreach (var item in itemClothes)
                {
                    item.gameObject.SetActive(!item.isPurchased);
                }
                
                ChangeSelectedFilterButton(filterButton[1]);
                break;

            case FilterState.Purchased:
                
                foreach (var item in itemClothes)
                {
                    item.gameObject.SetActive(item.isPurchased);
                }

                ChangeSelectedFilterButton(filterButton[2]);
                break;
        }
    }

    private void ChangeSelectedFilterButton(Button button)
    {
        foreach (var btn in filterButton) //Сброс стилей всех кнопок
        {
            btn.image.color = Color.white;
            btn.GetComponent<Outline>().enabled = false;
            btn.transform.GetComponentInChildren<TMP_Text>().color = Color.black;
        }
        button.image.color = Color.black;
        button.GetComponent<Outline>().enabled = true;
        button.transform.GetComponentInChildren<TMP_Text>().color = Color.white;
    }

}
