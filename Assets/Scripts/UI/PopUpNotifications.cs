using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpNotifications : MonoBehaviour
{
    public static PopUpNotifications instance;

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
    }

    public void ShowNotification(string info)
    {
        UIManager.instance.ShowNotificationPanel(info);
    }
    
    public void ShowAutoSave()
    {
        UIManager.instance.ShowAutosavePanel();
    }

    public void ShowNotification(NofStatus status)
    {
        switch (status)
        {
            case NofStatus.SuccessPurchased:
                UIManager.instance.ShowNotificationPanel(PurchasedInfo);
                break;
            case NofStatus.NotEnoughCash:
                UIManager.instance.ShowNotificationPanel(NotEnoughCashInfo);
                break;
        }
    }

}
