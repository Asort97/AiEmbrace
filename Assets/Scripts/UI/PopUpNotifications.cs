using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PopUpNotifications : MonoBehaviour
{
    public static PopUpNotifications instance;
    [SerializeField] private GameObject notificationPanel;
    [SerializeField] private TMP_Text notificationText;
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
        // notificationPanel.SetActive(true); //Даем обработать ContentSizeFilter, без этого багуется
        // notificationPanel.SetActive(false);
    }

    private void Start() 
    {
        SetTextPanel("NULL");        
    }

    public void ShowNotification(string info)
    {
        notificationPanel.SetActive(true);
        SetTextPanel(info);
    }
    
    public void CloseNotification()
    {
        notificationPanel.SetActive(false);
    }
    
    public void ShowAutoSave()
    {
        UIManager.instance.ShowAutosavePanel();
    }

    public void ShowNotification(NofStatus status)
    {
        notificationPanel.SetActive(true);

        switch (status)
        {
            case NofStatus.SuccessPurchased:
                SetTextPanel(PurchasedInfo);
                break;
            case NofStatus.NotEnoughCash:
                SetTextPanel(NotEnoughCashInfo);
                break;
        }
    }

    private void SetTextPanel(string info)
    {
        notificationText.text = info;
    }

}
