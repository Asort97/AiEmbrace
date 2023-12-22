using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Collections;

public class CustomizationCharacter : MonoBehaviour
{
    [Serializable] public struct Clothes
    {
        public GameObject itemObject;
        public ClothesSO itemSo;
    }

    [Serializable] public class Dressed
    {
        public GameObject item;
        public ClothesSO.ClothesCategory clothesCategory;
    }

    [SerializeField] private List<Dressed> alreadyWearing;
    [SerializeField] private Clothes[] allClothes;
    [SerializeField] private GameObject TShirt;


    private void OnEnable()
    {
        ItemClothes.OnUseItem += SetNewItem;
    }

    private void OnDisable()
    {
        ItemClothes.OnUseItem -= SetNewItem;
    }

    public void SetNewItem(ClothesSO item, bool toClothe)
    {   
        foreach (Clothes clothes in allClothes)
        {
            if(clothes.itemSo == item)
            {
                clothes.itemObject.SetActive(toClothe);

                if(toClothe)
                {
                    foreach (Dressed weared in alreadyWearing)
                    {
                        if (weared.clothesCategory == item.itemCategory)
                        {
                            weared.item.SetActive(false);
                            clothes.itemObject.SetActive(true);

                            weared.clothesCategory = item.itemCategory;
                        }
                    }
                    // alreadyWearing.Add(clothes.itemObject);
                }
                else
                {
                    // alreadyWearing.Remove(clothes.itemObject);
                }
                
            }
        }
    }

}
