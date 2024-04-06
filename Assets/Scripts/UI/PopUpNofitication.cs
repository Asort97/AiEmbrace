using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpNofitication : MonoBehaviour
{
    public static PopUpNofitication instance;

    public enum NofStatus
    {
        SuccessPurchased,
        NotEnoughCash,
    }

    [SerializeField, TextArea(1, 4)] private string PurchasedInfo;
    [SerializeField, TextArea(1, 4)] private string NotEnoughCashInfo;

    private void Awake()
    {
        instance = this;

        ShowAutoSave();
    }

    public void ShowNofitication(string info)
    {
        UIManager.instance.ShowNofiticationPanel(info);
    }
    
    public void ShowAutoSave()
    {
        UIManager.instance.ShowAutosavePanel();
    }

    public void ShowNofitication(NofStatus status)
    {
        switch (status)
        {
            case NofStatus.SuccessPurchased:
                UIManager.instance.ShowNofiticationPanel(PurchasedInfo);
                break;
            case NofStatus.NotEnoughCash:
                UIManager.instance.ShowNofiticationPanel(NotEnoughCashInfo);
                break;
        }
    }

}
