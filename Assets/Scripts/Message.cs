using UnityEngine;
using TMPro;

public class Message : MonoBehaviour
{
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text messageText;

    public void Init(string name, string msg)
    {
        nameText.text = name;
        messageText.text = msg;
    }
}
