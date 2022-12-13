using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DisplayScenario : MonoBehaviour
{
    public ScenarioSetter scenarioSetter;
    public Scenario scenario;
    public GameObject scenarioObject;
    //public DraggableItem[] scenarioDraggables;
    public GameObject[] scenarioDraggableObjects;
    public TextMeshProUGUI sceneText;
    public TextMeshProUGUI scenarioName;
    public TextMeshProUGUI successText;
    public TextMeshProUGUI successTextEngl;
    public AudioClip sceneDescriptionAud;
    public AudioClip openerPhraseAud;
    public AudioClip repeaterPhraseAud; //instructions that proceed the word of every item, eg. LAY DOWN THE plate
    public AudioClip successPhraseAud;
    public AudioClip correctPhraseAud;
    public AudioClip completionPhraseAud;
    public Image backgroundImage;
    internal ChallengeController challengeController;
    private List<Vector2> startSlots;


    // Start is called before the first frame update
    void Start()
    {
        scenario = scenarioSetter.currentScenario;
    }

    void SetScenario()
    {
        backgroundImage.sprite = scenario.backgroundImage;
        //scenarioName.text = scenario.scenarioName;
        //sceneDescriptionAud = scenario.sceneDescriptionAud;
        openerPhraseAud = scenario.openerPhraseAud;
        repeaterPhraseAud = scenario.repeaterPhraseAud;
        successPhraseAud = scenario.successPhraseAud;
        completionPhraseAud = scenario.completionPhraseAud;
        correctPhraseAud = scenario.correctPhraseAud;
        challengeController = GameObject.Find("Challenge Manager").GetComponent<ChallengeController>();
    }

    public void PopulateStartSlots()
    {
        foreach(GameObject draggable in scenario.scenarioDraggableObjects)
        {
            startSlots.Add(draggable.transform.localPosition);
        }
    }

    public void ShowRepeater(string wordText)
    {
        sceneText.text = ""+ scenario.repeaterPhrase[1] + wordText + ".";
    }

    public void ShowCustomInstruction(string customInstructionText)
    {
        sceneText.text = "" + customInstructionText + ".";
    }

    public void EmptyScenarioText()
    {
        sceneText.text = "";
        successText.text = "";
        successTextEngl.text = "";
    }

    public void ShowSuccess()
    {
        sceneText.text = "";
        successText.text = scenario.successPhrase[scenario.dialect];
        if(scenario.dialect == 0)
        {
            successTextEngl.text = scenario.successPhrase[1];
        }
        else {successTextEngl.text = scenario.successPhrase[0]; }
        
    }

    public void ShowCompletionText()
    {
        sceneText.text = scenario.completionPhrase[scenario.dialect];
    }

    public void ShowScenarioName()
    {
        scenarioName.text = scenario.scenarioName;
    }

    public void BackToSelection()
    {
        ShowScenarioName();
        backgroundImage.sprite = scenario.backgroundImage;
        EmptyScenarioText();
    }
}
