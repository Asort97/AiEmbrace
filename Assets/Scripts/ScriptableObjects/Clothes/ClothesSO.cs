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
    public string nameItem;
    public ClothesCategory itemCategory;
    public Image displayImage;
    public int price;
}
