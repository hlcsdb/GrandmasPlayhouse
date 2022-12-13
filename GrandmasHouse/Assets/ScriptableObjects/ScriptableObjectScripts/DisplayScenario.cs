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
    internal GameObject[] scenarioDraggableObjects;
    public TextMeshProUGUI sceneText;
    public TextMeshProUGUI scenarioName;
    public TextMeshProUGUI successText;
    public TextMeshProUGUI successTextEngl;

    public Image backgroundImage;
    internal ChallengeController challengeController;
    private List<Vector2> startSlots;
    private GameObject dzGO;

    //AUDIO
    internal AudioClip sceneDescriptionAud;
    internal AudioClip HTPexampleAud;
    internal AudioClip openerPhraseAud;
    internal AudioClip repeaterPhraseAud; //instructions that proceed the word of every item, eg. LAY DOWN THE plate
    internal AudioClip successPhraseAud;
    internal AudioClip incorrectPhraseAud;
    internal AudioClip correctPhraseAud;
    internal AudioClip completionPhraseAud;

    public AudioButtonUI HTPexampleButton;
    public AudioOnLoad openerPhraseButton;
    public AudioOnLoad completionPhraseButton;

    // Start is called before the first frame update
    void Start()
    {
        scenario = scenarioSetter.currentScenario;
        SetScenario();
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
        SetAudioButtons();

        challengeController = GameObject.Find("Challenge Manager").GetComponent<ChallengeController>();
        SetDZImage();
        
    }

    void SetAudioButtons()
    {
        openerPhraseButton.audioOnActive = openerPhraseAud;
        completionPhraseButton.audioOnActive = completionPhraseAud;
        HTPexampleButton.customAudClip = HTPexampleAud;
    }

    void SetDZImage()
    {
        dzGO = GameObject.Find("DZ Obj");
        var dzImageRectTransform = dzGO.transform as RectTransform;

        dzImageRectTransform.sizeDelta = scenario.dzRectDimensions;
        dzGO.transform.position = scenario.dzPos;
        dzGO.GetComponent<Image>().sprite = scenario.dzImage;
    }

    internal void SetScenarioDraggableObjects(GameObject scenarioDraggables)
    {

        for (int i = 0; i < transform.childCount; i++)
        {
            scenarioDraggableObjects[i] = scenarioDraggables.transform.GetChild(i).gameObject;
        }
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
