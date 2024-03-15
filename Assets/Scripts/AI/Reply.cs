using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Reply
{

    public string name;
    public string message;
    public int tokens;

    public Reply(string _name, string _message, int _tokens)
    {
        name = _name;
        message = _message;
        tokens = _tokens;
    }

    public override string ToString()
    {
        string separator = "####";
        string BeforeMessage = "";
        string AfterMessage = "";

        if (string.IsNullOrEmpty(this.message))
        {
            return $"{separator}{this.name}\n{BeforeMessage}";
        }
        else
        {
            return $"{separator}{this.name}\n{BeforeMessage}{this.message}{AfterMessage}";
        }
    }
}
