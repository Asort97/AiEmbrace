using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Collections;

public class CustomizationCharacter : MonoBehaviour
{
    public static CustomizationCharacter instance;
    [Serializable] public struct Clothes
    {
        public GameObject itemObject;
        public ItemSO itemSo;
    }

    [SerializeField] private Animator animator;
    [SerializeField] public Clothes[] allClothes;
    [SerializeField] private Clothes tShirt;
    [SerializeField] private Clothes pants;
    [SerializeField] private Clothes room;
    [SerializeField] private Clothes animationStand;
    // [SerializeField] private int emotionStand;
    // [SerializeField] private Color backgroundColor;
    private ItemSO previousClothes;
    private ItemSO previousRoom;
    private ItemSO previousAnim;

    private void Awake()
    {
        instance = this;        
    }

    private void Start()
    {
        previousClothes = tShirt.itemSo;
        previousRoom = room.itemSo;
        previousAnim = animationStand.itemSo;        
    }

    private void OnEnable()
    {
        ItemClothes.OnUseItem += SetNewItem;
        ItemClothes.OnSelectedItem += PreviewItem;
    }

    private void OnDisable()
    {
        ItemClothes.OnUseItem -= SetNewItem;
        ItemClothes.OnSelectedItem -= PreviewItem;
    }

    private void PreviewItem(ItemClothes itemClothes)
    {
        SetNewItem(itemClothes.clothesSO, true, true);
    }
    
    public void DisablePreviewItems()
    {
        SetNewItem(previousClothes, true, true);
        SetNewItem(previousAnim, true, true);
        SetNewItem(previousRoom, true, true);
    }

    public void SetNewItem(ItemSO itemToWear, bool toClothe, bool isPreview)
    {           
        foreach (Clothes clothes in allClothes)
        {
            if(clothes.itemSo == itemToWear)
            {
                switch (clothes.itemSo.itemCategory)
                {
                    case ItemSO.ClothesCategory.TShirts:

                        if(!isPreview)
                        {
                            previousClothes = tShirt.itemSo;
                        }

                        if(tShirt.itemObject != null)
                        {
                            tShirt.itemObject.SetActive(false);
                        }

                        tShirt = clothes;
                        tShirt.itemObject.SetActive(true);

                        ChangeStandEmotion();

                        break;

                    case ItemSO.ClothesCategory.Pants:
                        
                        if(!isPreview)
                        {
                            previousClothes = pants.itemSo;
                        }

                        if(pants.itemObject != null)
                        {
                            pants.itemObject.SetActive(false);
                        }

                        pants = clothes;
                        pants.itemObject.SetActive(true);

                        ChangeStandEmotion();

                        break;

                    case ItemSO.ClothesCategory.Background:

                        if(!isPreview)
                        {
                            previousRoom = room.itemSo;
                        }

                        room = clothes;

                        ChangeBackgroundColor();

                        break;

                    case ItemSO.ClothesCategory.EmotionStand:

                        if(!isPreview)
                        {
                            previousAnim = animationStand.itemSo;
                        }

                        animationStand = clothes;

                        ChangeStandEmotion();

                        break;
                }
            }
        }
    }

    private void ChangeStandEmotion()
    {
        animator.SetInteger("animation", ((EmotionSO)animationStand.itemSo).emotionStand);
    }

    private void ChangeBackgroundColor()
    {
        Camera.main.backgroundColor = ((RoomSO)room.itemSo).backgroundColor;
    }

}
