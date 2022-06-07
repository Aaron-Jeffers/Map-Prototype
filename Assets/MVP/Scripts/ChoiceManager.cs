using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoiceManager : MonoBehaviour
{
    [SerializeField]
    GameObject choicePrefab;
    [SerializeField]
    List<ChoiceSO> choiceScriptableObjects = new List<ChoiceSO>();
    List<Choice> choices = new List<Choice>();
    int index;
    bool canContinue;

    public bool CanContinue
    {
        set { canContinue = value; }
    }

    private void Awake()
    {
        if (!choicePrefab || choiceScriptableObjects.Count == 0) { return; }
        for (int i = 0; i < choiceScriptableObjects.Count; i++)
        {
            GameObject choice = Instantiate(choicePrefab);
            choice.transform.parent = transform;
            choice.transform.position = transform.position;
            choice.GetComponent<Choice>().SetChoiceSO = choiceScriptableObjects[i];
        }

        Choice[] newList = GetComponentsInChildren<Choice>();
        choices.AddRange(newList);

        for (int i = 1; i < choices.Count; i++)
        {
            choices[i].gameObject.SetActive(false);
        }
    }

    public void UpdateActiveChoice()
    {
        if (!canContinue || index == choiceScriptableObjects.Count) { return; }

        choices[index].gameObject.SetActive(false);
        index++;
        choices[index].gameObject.SetActive(true);
        canContinue = false;
    }
}
