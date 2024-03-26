using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PromptPart
{
    /*
     * Репрезентация текста, как данных для составления промпта.
     * 
     */

    public string text;
    public int tokensCount;

    public PromptPart()
    {
        text = "";
        tokensCount = 0;
    }

    public PromptPart(string text, int tokensCount)
    {
        this.text = text;
        this.tokensCount = tokensCount;
    }

    /* Данный метод не очень точный, тк при сложении двух текстов, количество токенов не всегда равно сумме количества токенов в каждом тексте 
     * Эта неточность кроется в границе текстов.
     * Например, в первом тексте 1 токен и во втором 1 токен и при сложении будет 2 токена, но по факту для токенайзера это может быть 1 токен.
     * Конеретный пример: "Hello" и " World" - при сложении будет "Hello World", но токенайзер может воспринять это как 1 токен.
     * Это не приведет к потере текста, но мы сможем вставить чуть-чуть меньше текста, чем планировали.
     */
    public static PromptPart operator +(PromptPart a, PromptPart b)
    {
        return new PromptPart(a.text + b.text, a.tokensCount + b.tokensCount);
    }

}