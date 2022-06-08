using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
[CreateAssetMenu(fileName = "New Choice", menuName = "Choice")]
public class ChoiceSO : ScriptableObject
{
    [SerializeField]
    string _name;
    [SerializeField]
    string _tag;
    [SerializeField]
    GameObject _fillArea;
    [SerializeField]
    List<GameObject> _choiceAreas = new List<GameObject>();
    [SerializeField]
    string _questionText;
    [SerializeField]
    List<string> _choiceText = new List<string>();
    [SerializeField]
    List<string> _consequenceText = new List<string>();
    [SerializeField]
    AudioClip _initialClip;
    [SerializeField]
    AudioClip _madeChoiceClip;

    public string NewName => _name;
    public string NewTag => _tag;
    public GameObject FillArea => _fillArea;
    public List<GameObject> ChoiceAreas => _choiceAreas;
    public string QuestionText => _questionText;
    public List<string> ChoiceText => _choiceText;
    public List<string> ConsequenceText => _consequenceText;
    public AudioClip InitialAudioClip => _initialClip;
    public AudioClip MadeChoiceAudioClip => _madeChoiceClip;
}