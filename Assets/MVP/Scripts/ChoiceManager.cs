using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class ChoiceManager : MonoBehaviour
{
    [SerializeField]
    GameObject choicePrefab;
    [SerializeField]
    List<ChoiceSO> choiceScriptableObjects = new List<ChoiceSO>();
    List<Choice> choices = new List<Choice>();
    int index;
    bool canContinue;
    [SerializeField]
    List<TextMeshProUGUI> text;
    Drop dropScript;
    
    public bool CanContinue
    {
        set { canContinue = value; }
    }
    public void SetText(string newText, int i)
    {
        text[i].text = newText;
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
        choices[0].Enable();
        for (int i = 1; i < choices.Count; i++)
        {
            choices[i].gameObject.SetActive(false);
        }
        
        dropScript = FindObjectOfType<Drop>().GetComponent<Drop>();
        EnableText(false);
    }

    public void UpdateActiveChoice()
    {
        if (!canContinue || index >= choiceScriptableObjects.Count) { return; }

        choices[index].gameObject.SetActive(false);
        index++;
        choices[index].gameObject.SetActive(true);
        choices[index].Enable();
        canContinue = false;
        EnableText(false);
        dropScript.SetWindowChecks();
    }
    void EnableText(bool value)
    {
        for (int i = 0; i < text.Count; i++)
        {
            text[i].enabled = value;
        }
        dropScript.ResetInk();
    }
    public void FillAreaComplete(bool enableText)
    {
        EnableText(enableText);
        dropScript.ResetInk();
        dropScript.SetWindowChecks();
    }
}
