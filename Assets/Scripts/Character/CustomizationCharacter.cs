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
        public ItemSO itemSo;
    }

    [SerializeField] private Animator animator;
    [SerializeField] public Clothes[] allClothes;
    [SerializeField] private GameObject tShirts;
    [SerializeField] private GameObject pants;
    [SerializeField] private int emotionStand;
    [SerializeField] private Color backgroundColor;

    private void OnEnable()
    {
        ItemClothes.OnUseItem += SetNewItem;
    }

    private void OnDisable()
    {
        ItemClothes.OnUseItem -= SetNewItem;
    }

    public void SetNewItem(ItemSO itemToWear, bool toClothe)
    {   
        foreach (Clothes clothes in allClothes)
        {
            if(clothes.itemSo == itemToWear)
            {
                switch (clothes.itemSo.itemCategory)
                {
                    case ItemSO.ClothesCategory.TShirts:

                        if(tShirts)
                        {
                            tShirts.SetActive(false);
                        }
                        tShirts = clothes.itemObject;
                        tShirts.SetActive(true);

                        ChangeStandEmotion();

                        break;

                    case ItemSO.ClothesCategory.Pants:

                        if(pants)
                        {
                            pants.SetActive(false);
                        }
                        pants = clothes.itemObject;
                        pants.SetActive(true);

                        ChangeStandEmotion();

                        break;

                    case ItemSO.ClothesCategory.Background:
                        
                        if(clothes.itemSo is RoomSO)
                        {
                            backgroundColor = ((RoomSO)clothes.itemSo).backgroundColor;
                        }
                        ChangeBackgroundColor();

                        break;

                    case ItemSO.ClothesCategory.EmotionStand:

                        if(clothes.itemSo is EmotionSO)
                        {
                            emotionStand = ((EmotionSO)clothes.itemSo).emotionStand;
                        }
                        ChangeStandEmotion();

                        break;
                }
            }
        }
    }

    private void ChangeStandEmotion()
    {
        animator.SetInteger("animation", emotionStand);
    }

    private void ChangeBackgroundColor()
    {
        Camera.main.backgroundColor = backgroundColor;
    }

}
