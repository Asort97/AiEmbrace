using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

[CreateAssetMenu(fileName = "ClothesSO", menuName = "")]
public class ClothesSO : ScriptableObject
{
    public enum ClothesCategory
    {
        TShirts,
        Shirts,
        Pants,
        Background,
        EmotionStand
    }

    public Color color;
    public string nameItem;
    public ClothesCategory itemCategory;
    public Sprite displayImage;
    public int emotionStand;
    public Color backgroundColor;
    public int price;
}
