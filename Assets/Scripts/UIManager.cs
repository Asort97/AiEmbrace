using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    [SerializeField] private GameObject[] allMenu;
    [SerializeField] private Slider xpSlider;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private GameObject leavePanel;
    [SerializeField] private GameObject autosavePanel;
    [SerializeField] private Button buyButton;
    [SerializeField] private Button useButton;
    [SerializeField] private TMP_Text buyButtonText;
    [SerializeField] private TMP_Text useButtonText;
    [SerializeField] private TMP_Text nicknameProfile;
    [SerializeField] private Image profileAvatar;
    [SerializeField] private GameObject changeNickPanel;
    [SerializeField] private TMP_InputField changeNickField;
    [SerializeField] private GameObject nofiticationPanel;
    [SerializeField] private TMP_Text nofiticationText;

    public TMP_InputField inputFieldChat;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        nicknameProfile.text = UserDataManager.Instance.data.userData.userNickname;
    }

    private void OnEnable()
    {
        ItemClothes.OnShowBuyBtn += ShowBuyItemButton;
        ItemClothes.OnShowUseBtn += ShowUseItemButton;
    }
    private void OnDisable()
    {
        ItemClothes.OnShowBuyBtn -= ShowBuyItemButton;
        ItemClothes.OnShowUseBtn -= ShowUseItemButton;
    }

    private void ShowBuyItemButton(bool isEnable, string name, string price, ItemClothes item)
    {
        useButton.gameObject.SetActive(false);
        buyButton.gameObject.SetActive(true);

        buyButtonText.text = name;

        buyButton.onClick.RemoveAllListeners();
        buyButton.onClick.AddListener(item.BuyItem);
    }

    private void ShowUseItemButton(string name, ItemClothes item)
    {
        buyButton.gameObject.SetActive(false);
        useButton.gameObject.SetActive(true);

        useButtonText.text = name;

        useButton.onClick.RemoveAllListeners();
        useButton.onClick.AddListener(item.UseItem);
    }

    public void SetEnableMenu(GameObject menu)
    {
        foreach (GameObject item in allMenu)
        {
            item.SetActive(false);
        }

        menu.SetActive(true);
    }

    public void SetEnableLeavePanel()
    {
        leavePanel.SetActive(!leavePanel.activeSelf);
    }

    public void ClearInputFieldChat()
    {
        inputFieldChat.text = "";
    }

    public void UpdateXPSlider(float amount, float maxAmount, int level)
    {
        levelText.text = level.ToString();

        xpSlider.maxValue = maxAmount;
        xpSlider.value = amount;
    }

    public void ShowNotificationPanel(string info)
    {
        nofiticationPanel.SetActive(true);
        nofiticationText.text = info;
    }
    
    public void ShowAutosavePanel()
    {
        autosavePanel.SetActive(true);
        autosavePanel.transform.DOLocalMoveX(375, 0.5f).OnComplete(()=>StartCoroutine(waitForPanel()));

        IEnumerator waitForPanel()
        {
            yield return new WaitForSeconds(5f);
            autosavePanel.transform.DOLocalMoveX(740, 0.5f).OnComplete(()=>autosavePanel.SetActive(false));
        }
    }
    
    public void SetEnableInputChat(bool enabled)
    {
        inputFieldChat.gameObject.SetActive(enabled);
    }

    public void CloseNotification()
    {
        nofiticationPanel.SetActive(false);
    }
    
    public void SetEnableChangeNickname(bool enabled)
    {
        changeNickPanel.SetActive(enabled);
    }

    public async void ApplyNewNickname()
    {
        // Debug.Log(changeNickField.text);
        if(changeNickField.text.Length >= 3)
        {
            UserDataManager.Instance.data.userData.userNickname = changeNickField.text; // устанавливаем ник в clientAPI
            var GameDataManager = new GameDataManager();
            await GameDataManager.SaveGameData(); // сохраняем ник в облаке
            nicknameProfile.text = changeNickField.text; // обновляем ник в UI

            changeNickPanel.SetActive(false);
        }
        else
        {
            PopUpNotifications.instance.ShowNotification("Too short!");
        }
    }

    public void ChangePlayerAvatar()
    {
        string path = UnityEditor.EditorUtility.OpenFilePanel("Select Image", "", "png,jpg,jpeg");

        if (!string.IsNullOrEmpty(path))
        {
            // Читаем байты изображения
            byte[] imageData = File.ReadAllBytes(path);

            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(imageData);

            profileAvatar.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        }
    }

    public async void LeaveAccount()
    {
        var GameDataManager = new GameDataManager();
        await GameDataManager.SaveGameData(); // save game data before logout
        GameDataManager.ClearAccountData();
        var result = await ClientAPI.Instance.Logout();
        if (result)
        {
            // todo: очистить все данные игры, чтобы новый вход не показывал старые данные
            SceneManager.LoadScene("LoginScene");
        }
        else
        {
            Debug.LogError("Logout attempt failed");
        }
    }

    public void CloseUseButton()
    {
        useButton.gameObject.SetActive(false);
        buyButton.gameObject.SetActive(false);

        CustomizationCharacter.instance.DisablePreviewItems();
        SetEnableInputChat(true);
    }
}
