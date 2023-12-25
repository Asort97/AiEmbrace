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

    [SerializeField] private Clothes[] allClothes;

    private GameObject tShirts;
    private GameObject pants;

    private void OnEnable()
    {
        ItemClothes.OnUseItem += SetNewItem;
    }

    private void OnDisable()
    {
        ItemClothes.OnUseItem -= SetNewItem;
    }

    public void SetNewItem(ClothesSO itemToWear, bool toClothe)
    {   
        foreach (Clothes clothes in allClothes)
        {
            if(clothes.itemSo == itemToWear)
            {
                switch (clothes.itemSo.itemCategory)
                {
                    case ClothesSO.ClothesCategory.TShirts:

                        if(tShirts)
                        {
                            tShirts.SetActive(false);
                        }
                        tShirts = clothes.itemObject;
                        tShirts.SetActive(true);

                        break;

                    case ClothesSO.ClothesCategory.Pants:

                        if(pants)
                        {
                            pants.SetActive(false);
                        }
                        pants = clothes.itemObject;

                        pants.SetActive(true);
                        break;
                }
            }
        }
    }

}
