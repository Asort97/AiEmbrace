using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

public class StoreContent : MonoBehaviour
{
    [Serializable]
    public enum FilterState
    {
        Purchased,
        New,
        Default
    }
    [SerializeField] private ItemCategory itemsCategory;
    [SerializeField] private FilterState filterState;
    [SerializeField] private Transform parent;
    [SerializeField] private ItemClothes itemCellPrefab;

    [Space(5)]
    [SerializeField] private List<Button> filterButton = new List<Button>();
    private List<ItemClothes> filterItems = new List<ItemClothes>();

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

        foreach (var item in UserDataManager.Instance.data.storeData.Items)
        {
            if(item.Category == itemsCategory)
            {
                ItemClothes cell = Instantiate<ItemClothes>(itemCellPrefab, parent);
                cell.Init(item.ItemId, item.IsPurchased, item.IsUsedDefault);
                filterItems.Add(cell);
                Debug.Log($"ADD to {parent} an {item}");
            }
        }
    }

    public void ChangeFilterCategory(FilterState state)
    {
        filterState = state;

        switch (filterState)
        {
            case FilterState.Default:
                foreach (var item in filterItems)
                {
                    item.gameObject.SetActive(true);
                }

                ChangeSelectedFilterButton(filterButton[0]);
                break;

            case FilterState.New:

                foreach (var item in filterItems)
                {
                    item.gameObject.SetActive(!item.isPurchased);
                }
                
                ChangeSelectedFilterButton(filterButton[1]);
                break;

            case FilterState.Purchased:
                
                foreach (var item in filterItems)
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
