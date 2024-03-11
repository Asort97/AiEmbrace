using UnityEngine;

[CreateAssetMenu(fileName = "ClothesSO", menuName = "")]
public class ItemSO : ScriptableObject
{
    public enum ClothesCategory
    {
        TShirts,
        Shirts,
        Pants,
        Background,
        EmotionStand,
        CharacterPreset
    }

    public Color imageColor;
    public string nameItem;
    public ClothesCategory itemCategory;
    public Sprite displayImage;
    public int price;
}
