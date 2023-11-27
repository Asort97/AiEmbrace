using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Reply
{

    public string name;
    public string message;

    public Reply(string _name, string _message)
    {
        name = _name;
        message = _message;
    }

    public override string ToString()
    {
        if (message == "")
        {
            return name.ToString() + ":";
        }
        else
        {
            return name.ToString() + ": " + message.ToString();
        }
    }
}
