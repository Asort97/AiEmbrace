using DG.Tweening;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public enum ReactionType
{
    Sad,
    Like,
    Heart,
    Smile
}

public class Message : MonoBehaviour
{
    public static Action<Message> OnClick;

    [Serializable]
    public struct ReactionsList
    {
        public ReactionType reactionType;
        public Sprite sprite;
    }

    [SerializeField] private List<ReactionsList> reactionsList;
    [SerializeField] private RectTransform reactionPanel;
    [SerializeField] private Image reactionImage;
    [SerializeField] private Image reactionImageOutline;

    [SerializeField] private Image bgMessage;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text messageText;
    [SerializeField] private float reactionPanelSpeed;
    private Vector3 originalReactionScale;
    private Vector3 animReactionScale;
    private bool alreadyClick;
    private bool isPlayer;

    private void Awake()
    {
        originalReactionScale = reactionImage.rectTransform.localScale;
        animReactionScale = reactionImage.rectTransform.localScale * 1.5f;
    }

    // private void OnEnable() 
    // {
    //     Message.OnClick += EnableReactionPanel;
    // }

    // private void OnDisable()
    // {
    //     Message.OnClick -= EnableReactionPanel;
    // }

    public void Init(bool isPlayer, string name, string msg)
    {
        this.isPlayer = isPlayer;

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

    public void EnableReactionPanel(Message message)
    {
        if(!isPlayer)
        {
            OnClick?.Invoke(this);

            if(message == this)
            {
                alreadyClick = !alreadyClick;

                if(alreadyClick)
                {
                    reactionPanel.DOLocalMoveY(0, reactionPanelSpeed);
                }
                else
                {
                    reactionPanel.DOLocalMoveY(-275, reactionPanelSpeed);
                }            
            }
            else
            {
                alreadyClick = false;
                reactionPanel.DOLocalMoveY(-275, reactionPanelSpeed);
            }            
        }
    }

    public void SelectRectionImage(string reactionType)
    {
        int index = reactionsList.FindIndex(x => x.reactionType.ToString() == reactionType);

        if(index != -1)
        {
            EnableReactionPanel(this);
            reactionImageOutline.gameObject.SetActive(true);

            reactionImageOutline.rectTransform.DOScale(animReactionScale, 0.2f)
                .SetEase(Ease.InOutSine)
                .OnComplete(() => 
                {
                    reactionImageOutline.rectTransform.DOScale(originalReactionScale, 0.4f)
                        .SetEase(Ease.OutBounce);
                });

            reactionImage.sprite = reactionsList[index].sprite;
        }
        else
        {
            reactionImageOutline.gameObject.SetActive(false);
        }
    }
}
