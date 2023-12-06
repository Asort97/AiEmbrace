using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreContent : MonoBehaviour
{
    [SerializeField] private ClothesSO[] itemsStore;
    [SerializeField] private Transform[] Categories;
    [SerializeField] private ItemClothes itemCellPrefab;

    private void Init()
    {
        foreach (var item in itemsStore)
        {
            foreach (var category in Categories)
            {
                if(category.name == item.itemCategory.ToString())
                {
                    ItemClothes cell = Instantiate<ItemClothes>(itemCellPrefab, category);
                    cell.Init(item);

                    // Debug.Log($"ADD to {category.name} an {item}");

                    break;
                }
            }            
        }
    }

    private void Start()
    {
        Init();
    }
}
