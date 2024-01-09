using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoinsDrawable : MonoBehaviour
{
    [SerializeField] private TMP_Text[] coinsText;
    [SerializeField] private TMP_Text[] crystalText;

    public void OnEnable()
    {
        Debug.Log($"dfdfd");
        CoinsManager.OnAddCash += UpdateText;
    }

    public void OnDisable()
    {
        CoinsManager.OnAddCash -= UpdateText;
    }

    private void UpdateText(int coin, int crystal)
    {
        Debug.Log($"Updating stats");

        foreach (var text in coinsText)
        {
            text.text = coin.ToString();
        }

        foreach (var text in crystalText)
        {
            text.text = crystal.ToString();
        }
    }
}
