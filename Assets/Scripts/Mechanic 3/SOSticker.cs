using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Sticker", menuName = "Sticker")]
public class SOSticker : ScriptableObject
{
    public new string name;
    public string tag;
    //public GameObject button;
    public Sprite buttonSprite;
    public GameObject highlight;
    public string highlightTag;
    public GameObject dragSprite;
    public GameObject placeSprite;
}
