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
    [SerializeField] private Clothes room;
    [SerializeField] private Clothes animationStand;
    [SerializeField] private Clothes characterPreset;

    // [SerializeField] private int emotionStand;
    // [SerializeField] private Color backgroundColor;
    private ItemSO previousClothes;
    private ItemSO previousRoom;
    private ItemSO previousAnim;
    private ItemSO previousCharacter;

    private void Awake()
    {
        instance = this;        
    }

    private void Start()
    {
        previousClothes = tShirt.itemSo;
        previousRoom = room.itemSo;
        previousAnim = animationStand.itemSo;
        previousCharacter = characterPreset.itemSo;        
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
        SetNewItem(itemClothes.itemSO, true, true);
    }
    
    public void DisablePreviewItems()
    {
        SetNewItem(previousClothes, true, true);
        SetNewItem(previousAnim, true, true);
        SetNewItem(previousRoom, true, true);
        SetNewItem(previousCharacter, true, true);
    }

    public void SetNewItem(ItemSO itemToWear, bool toClothe, bool isPreview)
    {           
        foreach (Clothes clothes in allClothes)
        {
            if(clothes.itemSo == itemToWear)
            {
                switch (clothes.itemSo.itemCategory)
                {
                    case ItemCategory.Clothes:

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

                    // case ItemCategory.Pants:
                        
                    //     if(!isPreview)
                    //     {
                    //         previousClothes = pants.itemSo;
                    //     }

                    //     if(pants.itemObject != null)
                    //     {
                    //         pants.itemObject.SetActive(false);
                    //     }

                    //     pants = clothes;
                    //     pants.itemObject.SetActive(true);

                    //     ChangeStandEmotion();

                    //     break;

                    case ItemCategory.Background:

                        if(!isPreview)
                        {
                            previousRoom = room.itemSo;
                        }

                        room = clothes;

                        ChangeBackgroundColor();

                        break;

                    case ItemCategory.EmotionStand:

                        if(!isPreview)
                        {
                            previousAnim = animationStand.itemSo;
                        }

                        animationStand = clothes;

                        ChangeStandEmotion();

                        break;

                    case ItemCategory.CharacterPreset:

                        if(!isPreview)
                        {
                            previousCharacter = characterPreset.itemSo;
                        }

                        characterPreset = clothes;

                        ChangeCharacterPreset();

                        break;
                }
            }
        }
    }

    private void ChangeStandEmotion()
    {
        animator.SetInteger("animation", ((EmotionSO)animationStand.itemSo).emotionStand);
    }

    private void ChangeCharacterPreset()
    {
        Debug.Log($"Finding new CHAR");
        var data = AIDataManager.Instance.aiCharactersData.GetAICharacterData(((CharacterSO)characterPreset.itemSo).characterName);
        var personal = UserDataManager.Instance.data.charactersData.GetAICharacterData(((CharacterSO)characterPreset.itemSo).characterName);

        ChatManager.instance.currentAI = new AICharacter(data, personal);
    }

    private void ChangeBackgroundColor()
    {
        Camera.main.backgroundColor = ((RoomSO)room.itemSo).backgroundColor;
    }

}
