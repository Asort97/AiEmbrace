using UnityEngine;

[CreateAssetMenu(fileName = "CharacterSO", menuName = "")]
public class CharacterSO : ItemSO
{
    public string characterName;
    [TextArea(5,10)]public string characterDescription;
}
