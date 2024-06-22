using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllUserDataTemplate : MonoBehaviour
{
    [SerializeField]
    private AllUserData templateData; // ��������� ��� � ���������
    public ItemSO[] AllItems;
    public AllUserData GetTemplateData()
    {
        return templateData;
    }
}
