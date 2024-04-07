using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllUserDataTemplate : MonoBehaviour
{
    [SerializeField]
    private AllUserData templateData; // Заполните это в редакторе

    public AllUserData GetTemplateData()
    {
        return templateData;
    }
}
