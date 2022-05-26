using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
[CreateAssetMenu(fileName = "New Sticker", menuName = "Sticker")]
public class SOSticker : ScriptableObject
{
    [SerializeField]
    string _name;
    [SerializeField]
    string _tag;
    //public GameObject button;
    [SerializeField]
    Sprite _buttonSprite;
    [SerializeField]
    GameObject _highlight;
    [SerializeField]
    string _highlightTag;
    [SerializeField]
    GameObject _dragSprite;
    [SerializeField]
    GameObject _placeSprite;
    [SerializeField]
    AudioClip _audioClip;

    public string NewName => _name;
    public  string NewTag => _tag;
    public Sprite ButtonSprite => _buttonSprite;
    public GameObject Highlight => _highlight;
    public string HighlightTag => _highlightTag;
    public GameObject DragSprite => _dragSprite;
    public GameObject PlaceSprite => _placeSprite;
    public AudioClip Clip => _audioClip;
}
