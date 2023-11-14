using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatAutoScroller : MonoBehaviour
{
	[SerializeField] private ScrollRect scrollRect;
    [SerializeField] private float smoothSpeed;

	private void Start()
	{
        StartCoroutine(AutoScrollChat());
    }

    private void OnEnable()
    {
        ChatManager.OnDrawMessage += OnChatMessage;        
    }

    private void OnDisable()
    {
        ChatManager.OnDrawMessage -= OnChatMessage;        
    }

	private void OnChatMessage()
	{
		StartCoroutine(AutoScrollChat());
	}
    
	private IEnumerator AutoScrollChat()
	{
		yield return new WaitForEndOfFrame();

        float targetVerticalPosition = 0f;

        while (Mathf.Abs(scrollRect.verticalNormalizedPosition - targetVerticalPosition) > 0.001f)
        {
            scrollRect.verticalNormalizedPosition = Mathf.Lerp(scrollRect.verticalNormalizedPosition, targetVerticalPosition, smoothSpeed * Time.deltaTime);
            yield return null;
        }

        scrollRect.verticalNormalizedPosition = targetVerticalPosition;
	}
}
