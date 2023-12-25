using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterTransition : MonoBehaviour
{
    [SerializeField] private Transform characterPrefab;

    [Space(5)]

    [SerializeField] private Vector3 chatPointPosition;
    [SerializeField] private Vector3 chatPointScale;

    [Space(5)]

    [SerializeField] private Vector3 storePointPosition;
    [SerializeField] private Vector3 storePointScale;

    private void Start()
    {
        ChangePoint("toChat");
    }

    public void ChangePoint(string typeTransition)
    {
        switch (typeTransition)
        {
            case "toChat":
                characterPrefab.position = chatPointPosition;
                characterPrefab.localScale = chatPointScale;
                break;

            case "toStore":
                characterPrefab.position = storePointPosition;
                characterPrefab.localScale = storePointScale;
                break;
        }
    }
}
