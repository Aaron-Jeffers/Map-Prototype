using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[CreateAssetMenu(fileName = "New Choice", menuName = "Choice")]
public class ChoiceSO : ScriptableObject
{
    [SerializeField]
    string _name;
    [SerializeField]
    string _tag;
    [SerializeField]
    List<GameObject> _fillAreas = new List<GameObject>();
    [SerializeField]
    List<GameObject> _choiceAreas = new List<GameObject>();
    [SerializeField]
    List<string> _choiceText = new List<string>();
    [SerializeField]
    List<string> _consequenceText = new List<string>();
    [SerializeField]
    AudioClip _audioClip;

    public string NewName => _name;
    public string NewTag => _tag;
    public List<GameObject> FillAreas => _fillAreas;
    public List<GameObject> ChoiceAreas => _choiceAreas;
    public List<string> ChoiceText => _choiceText;
    public List<string> ConsequenceText => _consequenceText;
    public AudioClip Clip => _audioClip;
}