using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoinsDrawable : MonoBehaviour
{
    [SerializeField] private TMP_Text[] coinsText;
    [SerializeField] private TMP_Text[] crystalText;

    private void Awake() 
    {
        UpdateText(0, 0);
    }

    public void OnEnable()
    {
        CoinsManager.OnAddCash += UpdateText;
    }

    public void OnDisable()
    {
        CoinsManager.OnAddCash -= UpdateText;
    }

    private void UpdateText(int coin, int crystal)
    {
        Debug.Log($"Updating stats");

        Debug.Log($"{coin} and {crystal}");
        foreach (var _text in coinsText)
        {
            Debug.Log($"Ny");
            _text.text = coin.ToString();
        }

        foreach (var _text in crystalText)
        {
            _text.text = crystal.ToString();
        }
    }
}
