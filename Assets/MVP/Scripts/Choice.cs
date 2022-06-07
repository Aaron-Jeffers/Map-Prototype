using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Choice : MonoBehaviour
{
    [SerializeField]
    ChoiceSO choiceSO;
    ChoiceManager choiceManager;

    public ChoiceSO SetChoiceSO
    {
        set { choiceSO = value; }
    }

    
}
