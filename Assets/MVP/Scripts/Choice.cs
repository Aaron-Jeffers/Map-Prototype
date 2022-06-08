using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Choice : MonoBehaviour
{
    ChoiceSO choiceSO;
    ChoiceManager choiceManager;

    AudioSource audio;
    GameObject fillArea;
    List<GameObject> choiceAreas = new List<GameObject>();
    string questionText;
    List<string> choiceText = new List<string>();
    List<string> consequenceText = new List<string>();
    public List<bool> checkedArea = new List<bool>();
    bool checkedFill;
    float timer;
    bool startTimer;
    bool playedAudio;

    public ChoiceSO SetChoiceSO
    {
        set { choiceSO = value; Initialise(); }
    }

    void Initialise()
    {
        choiceManager = FindObjectOfType<ChoiceManager>();
        name = choiceSO.NewName;
        tag = choiceSO.NewTag;
        fillArea = Instantiate(choiceSO.FillArea);
        fillArea.transform.parent = transform;
        foreach(GameObject obj in choiceSO.ChoiceAreas)
        {
            GameObject newObj = Instantiate(obj);
            newObj.transform.parent = transform;
            choiceAreas.Add(newObj);
            checkedArea.Add(false);
            newObj.SetActive(false);
        }
        questionText = choiceSO.QuestionText;
        choiceText = choiceSO.ChoiceText;
        consequenceText = choiceSO.ConsequenceText;
        audio = GetComponent<AudioSource>();
        audio.clip = choiceSO.InitialAudioClip;
    }

    public void Enable()
    {
        for(int i = 0; i < choiceText.Count; i++)
        {
            choiceManager.SetText(choiceText[i], i);
        }
        choiceManager.SetText(questionText, 3);
        audio.Play();
    }

    private void Update()
    {
        if (startTimer) { timer += Time.deltaTime; }
        if(fillArea.GetComponent<WindowCheck>().Filled && !checkedFill)
        {
            startTimer = true;
            if (timer < 2f) return;
            checkedFill = true;
            fillArea.SetActive(false);
            foreach(GameObject obj in choiceAreas)
            {
                obj.SetActive(true);
            }
            choiceManager.FillAreaComplete(true);
            startTimer = false;
            timer = 0;
        }
        if(checkedArea[choiceAreas.Count - 1])
        {
            for (int i = 0; i < choiceAreas.Count; i++)
            {
                choiceAreas[i].GetComponent<WindowCheck>().EnableFill(true);
                choiceManager.SetText(consequenceText[i], i);
                checkedArea[i] = true;
                choiceManager.FillAreaComplete(true);
            }
        }
        if (checkedArea.All(x => x == true))
        {
            if (timer > 3f) 
            {
                timer = 0;
                startTimer = false;
                choiceManager.CanContinue = true;
                choiceManager.UpdateActiveChoice();
            }
            audio.clip = choiceSO.MadeChoiceAudioClip;
            if(!playedAudio)
            {
                audio.Play();
                playedAudio = true;
            }
            if(!audio.isPlaying)
            {
                startTimer = true;
            }
        }
        for (int i = 0; i < choiceAreas.Count; i++)
        {
            if(checkedArea[i]) continue;
            if(choiceAreas[i].GetComponent<WindowCheck>().Filled)
            {
                choiceManager.SetText(consequenceText[i], i);
                checkedArea[i] = true;
                choiceManager.FillAreaComplete(true);
            }
        }
    }
}
