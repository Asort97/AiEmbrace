using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

[CreateAssetMenu(fileName = "ClothesSO", menuName = "")]
public class ClothesSO : ScriptableObject
{
    public enum ClothesCategory
    {
        TShirts,
        Shirts,
        Tops,
        Hoodies,
        Jackets,
        Jeans,
        Pants,
        Skirts,
        Shorts,
        Short,
        Long,
        Sneakers,
    }
    public Color color;
    public string nameItem;
    public ClothesCategory itemCategory;
    public Sprite displayImage;
    public int price;
}
