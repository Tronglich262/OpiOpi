using UnityEngine;

[System.Serializable]
public class CharacterData : MonoBehaviour
{
    public string characterName;   // tên
    //public Sprite Sprite;
    public int hp;
    public int attack;
    public int speed;
    public int cuteness;           // độ dễ thương

    [TextArea(3, 5)]
    public string story;           // cốt truyện

    [Header("UI Display")]
    public Sprite characterSprite;
}
