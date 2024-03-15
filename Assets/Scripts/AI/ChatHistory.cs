using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DrawnChatFragment
{
    public int tokens;
    public string messages;
}



[System.Serializable]
public class ChatHistory
{
    [Tooltip("The words of the participants in the chat")]
    [SerializeField]
    public List<Reply> replies;

    // фрагменты общения (каждый фрагмент это индекс в истории, когда этот фрагмент закончился)
    // когда игрок подходит к ИИ и общается а потом выходит из чата - это отдельный фрагмент
    [SerializeField]
    public List<int> chatFragments = new();

    public void Append(Reply nr)
    {
        replies.Add(nr);
    }

    public Reply LastReply()
    {
        return replies.Last();
    }

    public Reply GetReply(int index)
    {
        return replies[index];
    }

    public int Size()
    {
        return replies.Count;
    }

    public List<Reply> GetReplies()
    {
        return replies;
    }

    public void SetLastReply(string message)
    {
        replies.Last().message = message;
    }

    public string Draw()
    {
        return Draw(999999);
    }

    // todo: можно возвращать DrawnChatFragment
    public string Draw(int tokenLimit)
    {
        int newLineTokens = 1;

        int currentTokens = 0;

        string result = "";

        var i = replies.Count - 1;

        // отрисовываем чат с конца, если упираемся в лимит, то останавливаемся
        while (i >= 0)
        {
            Reply reply = replies[i];

            // выходим, если превысили количество токенов
            if (currentTokens + newLineTokens + reply.tokens > tokenLimit)
            {
                break;
            }
            else
            {
                currentTokens += newLineTokens + reply.tokens;
            }

            // вставляем текст сообщения в начало, тк идем реверсивно
            result = reply.ToString() + "\n" + result;
            i--;
        }

        // todo: можно удалить ту часть, которая уже есть в воспоминаниях 
        // todo: можно исключать те воспоминания, которые есть в чате

        return result;
    }

    public string DrawChatFragment(int fragmentIndex)
    {
        int startReplyIndex;
        if (fragmentIndex == 0)
        {
            startReplyIndex = 0;
        }
        else
        {
            startReplyIndex = chatFragments[fragmentIndex - 1] + 1;
        }
        int endReplyIndex = chatFragments[fragmentIndex];

        string result = "";
        for (int i = startReplyIndex; i <= endReplyIndex; i++)
        {
            if (result != "")
            {
                result += "\n";
            }
            result += replies[i].ToString();
        }
        return result;
    }

    public bool MarkCurrentChatAsEnded()
    {
        // todo: по идее, всегда false (chatFragments пустой), нужно проверить
        if (chatFragments.Any() && (chatFragments.Last() == replies.Count || replies.Count == 0))
        {
            return false;
        }
        else
        {
            chatFragments.Add(replies.Count - 1);
            return true;
        }
    }

}
