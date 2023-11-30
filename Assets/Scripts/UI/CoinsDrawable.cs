using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoinsDrawable : MonoBehaviour
{
    [SerializeField] private TMP_Text coinsText;
    [SerializeField] private TMP_Text crystalText;

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
        coinsText.text = coin.ToString();
        crystalText.text = crystal.ToString();
    }
}
