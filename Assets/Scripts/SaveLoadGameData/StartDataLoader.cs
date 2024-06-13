using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartDataLoader : MonoBehaviour
{
    [SerializeField] private Authentication authentication;
    private async void Start()
    {
        // check if account data exists and load it
        var GameDataManager = new GameDataManager();
        GameDataManager.LoadAccountData();
        if (GameDataManager.accountDataForStorage != null)
        {
            GameDataManager.ApplyAccountData(); // set token in ClientAPI.Instance.token
            authentication.Login(false);

            if(GameDataManager.gameDataForStorage.data.storeData.Items.Count == 0)
            {
                UserDataManager.Instance.InitAllItems(); // Если число итемов не совпадает с серверным, то загружает на сервер новые данные
            }
        }
    }
}
