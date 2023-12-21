using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Collections;

public class CustomizationCharacter : MonoBehaviour
{
    [Serializable] public class Clothes
    {
        public GameObject itemObject;
        public ClothesSO itemSo;
    }   

    [SerializeField] private List<GameObject> alreadyWearing;

    [SerializeField] private Clothes[] allClothes;

    private void OnEnable()
    {
        ItemClothes.OnUseItem += SetNewItem;
    }

    private void OnDisable()
    {
        ItemClothes.OnUseItem -= SetNewItem;
    }

    public void SetNewItem(ClothesSO item)
    {   
        foreach (Clothes clothes in allClothes)
        {
            if(clothes.itemSo == item)
            {
                clothes.itemObject.SetActive(true);
                alreadyWearing.Add(clothes.itemObject);
            }
        }
    }

}
