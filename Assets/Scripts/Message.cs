
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Message : MonoBehaviour
{
    [SerializeField] private Image bgMessage;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text messageText;

    public void Init(bool isPlayer, string name, string msg)
    {
        if(isPlayer)
        {
            bgMessage.color = new Color(Color.white.r, Color.white.g, Color.white.b, bgMessage.color.a);
            nameText.color = new Color(Color.black.r, Color.black.g, Color.black.b, bgMessage.color.a);
            messageText.color = new Color(Color.black.r, Color.black.g, Color.black.b, bgMessage.color.a);
        }
        else
        {
            bgMessage.color = new Color(Color.black.r, Color.black.g, Color.black.b, bgMessage.color.a);
            nameText.color = new Color(Color.white.r, Color.white.g, Color.white.b, bgMessage.color.a);
            messageText.color = new Color(Color.white.r, Color.white.g, Color.white.b, bgMessage.color.a);
        }

        nameText.text = name;
        messageText.text = msg;
    }
}
