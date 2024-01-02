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

                        ChangeStandEmotion();

                        break;

                    case ClothesSO.ClothesCategory.Pants:

                        if(pants)
                        {
                            pants.SetActive(false);
                        }
                        pants = clothes.itemObject;
                        pants.SetActive(true);

                        ChangeStandEmotion();

                        break;

                    case ClothesSO.ClothesCategory.Background:
                        
                        backgroundColor = clothes.itemSo.backgroundColor;
                        ChangeBackgroundColor();

                        break;

                    case ClothesSO.ClothesCategory.EmotionStand:

                        emotionStand = clothes.itemSo.emotionStand;
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
