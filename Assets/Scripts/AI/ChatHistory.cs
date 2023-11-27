using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
[System.Serializable]
public class ChatHisoty
{
    [Tooltip("The words of the participants in the chat")]
    [SerializeField]
    private List<Reply> replies;

    // фрагменты общения (каждый фрагмент это индекс в истории, когда этот фрагмент закончился)
    // когда игрок подходит к ИИ и общается а потом выходит из чата - это отдельный фрагмент
    [SerializeField]
    private List<int> chatFragments = new();

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
        string result = "";
        foreach (var i in replies)
        {
            if (result != "")
            {
                result += "\n";
            }
            result += i.ToString();
        }
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
