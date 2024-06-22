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
        foreach (var item in UserDataManager.Instance.data.storeData.Items) // Пробегаемся по загруженным данным чтобы определить что одето на персонаже
        {
            if(item.IsUsed)
            {
                SetNewItem(UserDataManager.Instance.GetItemSOById(item.ItemId), true, false);
            }
        }
        
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

    public async void SetNewItem(ItemSO itemToWear, bool toClothe, bool isPreview)
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
                            UserDataManager.Instance.data.storeData.Items[UserDataManager.Instance.GetIndexItemById(tShirt.itemSo.idItem)].IsUsed = false;
                            tShirt.itemObject.SetActive(false);
                        }

                        tShirt = clothes;
                        tShirt.itemObject.SetActive(true);
                        UserDataManager.Instance.data.storeData.Items[UserDataManager.Instance.GetIndexItemById(clothes.itemSo.idItem)].IsUsed = true;

                        ChangeStandEmotion();

                        break;

                    case ItemCategory.Background:

                        if(!isPreview)
                        {
                            previousRoom = room.itemSo;
                        }

                        UserDataManager.Instance.data.storeData.Items[UserDataManager.Instance.GetIndexItemById(room.itemSo.idItem)].IsUsed = false;

                        room = clothes;

                        UserDataManager.Instance.data.storeData.Items[UserDataManager.Instance.GetIndexItemById(clothes.itemSo.idItem)].IsUsed = true;

                        ChangeBackgroundColor();

                        break;

                    case ItemCategory.EmotionStand:

                        if(!isPreview)
                        {
                            previousAnim = animationStand.itemSo;
                        }

                        UserDataManager.Instance.data.storeData.Items[UserDataManager.Instance.GetIndexItemById(animationStand.itemSo.idItem)].IsUsed = false;

                        animationStand = clothes;

                        UserDataManager.Instance.data.storeData.Items[UserDataManager.Instance.GetIndexItemById(clothes.itemSo.idItem)].IsUsed = true;

                        ChangeStandEmotion();

                        break;

                    case ItemCategory.CharacterPreset:

                        if(!isPreview)
                        {
                            previousCharacter = characterPreset.itemSo;
                        }

                        UserDataManager.Instance.data.storeData.Items[UserDataManager.Instance.GetIndexItemById(characterPreset.itemSo.idItem)].IsUsed = false;
                        
                        characterPreset = clothes;

                        UserDataManager.Instance.data.storeData.Items[UserDataManager.Instance.GetIndexItemById(clothes.itemSo.idItem)].IsUsed = true;

                        ChangeCharacterPreset();

                        break;
                }
            }
        }

        var GameDataManager = new GameDataManager();
        await GameDataManager.SaveGameData(); 
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
