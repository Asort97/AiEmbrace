using UnityEngine;


[CreateAssetMenu(fileName = "New Item", menuName = "")]
public class ItemSO : ScriptableObject
{
    public Sprite displayImage;
    public Color imageColor;    
    public ItemCategory itemCategory;
    public string idItem;    
    public int price;
    public bool showDescriptionMenu;
    public bool purchasedByDefault;
    public bool usedByDefault;
}
