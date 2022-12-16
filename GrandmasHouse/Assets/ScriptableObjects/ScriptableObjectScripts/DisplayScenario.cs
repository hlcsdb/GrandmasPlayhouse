using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DisplayScenario : MonoBehaviour
{
    public ScenarioSetter scenarioSetter;
    public Scenario scenario;
    public Transform draggableContainer;

    //public GameObject scenariosInHierarchy;

    public TextMeshProUGUI sceneText;
    public TextMeshProUGUI scenarioName;
    public TextMeshProUGUI successText;
    public TextMeshProUGUI successTextEngl;

    public Image backgroundImage;
    internal ChallengeController challengeController;
    private List<Vector2> startSlots;
    public GameObject dzGO;

    //AUDIO
    internal AudioClip sceneDescriptionAud;
    internal AudioClip openerPhraseAud;
    internal AudioClip repeaterPhraseAud; //instructions that proceed the word of every item, eg. LAY DOWN THE plate
    internal AudioClip successPhraseAud;
    internal AudioClip incorrectPhraseAud;
    internal AudioClip correctPhraseAud;
    internal AudioClip completionPhraseAud;

    public AudioButtonUI HTPexampleButton;
    public AudioOnLoad openerPhraseButton;
    public AudioOnLoad completionPhraseButton;

    public List<Vector2> vocabPositions = new List<Vector2>() { new Vector2(-220,46), new Vector2(-60,46), new Vector2(100,46), new Vector2(260,46), new Vector2(-220, -74), new Vector2( -60, - 74), new Vector2(100,-74), new Vector2(260,-74) };
    // Start is called before the first frame update
    void Start()
    {
        scenario = scenarioSetter.currentScenario;
        SetScenario();
    }

    internal void SetScenario()
    {
        
        challengeController = GameObject.Find("Challenge Manager").GetComponent<ChallengeController>();
        openerPhraseAud = scenario.openerPhraseAud;
        repeaterPhraseAud = scenario.repeaterPhraseAud;
        successPhraseAud = scenario.successPhraseAud;
        completionPhraseAud = scenario.completionPhraseAud;
        correctPhraseAud = scenario.correctPhraseAud;
        backgroundImage.sprite = scenario.backgroundImage;
        SetAudioButtons();
        SpawnDraggables();
        SetDZImage();
    }

    internal void SpawnDraggables()
    {
        ResetDraggableSOs();
        Vector2 scale = new Vector2(1, 1);
        int i = 0;
        foreach(GameObject draggable in scenario.scenarioDraggableObjects)
        {
            GameObject ourDraggable = Instantiate(draggable);
            challengeController.draggableObjects.Add(ourDraggable);
            ourDraggable.transform.SetParent(draggableContainer);
            ourDraggable.transform.localPosition = vocabPositions[i];
            ourDraggable.transform.localScale = scale;
            i++;
        }
    }

    internal void ResetDraggableSOs()
    {
        foreach (DraggableItem draggable in scenario.scenarioDraggableItems)
        {
            draggable.ResetSO();
        }
    }

    void SetAudioButtons()
    {
        //Debug.Log("setting aud buttons");
        openerPhraseButton.SetAudioclip(openerPhraseAud);
        completionPhraseButton.audioOnActive = completionPhraseAud;
        HTPexampleButton.SetAudioClip(scenario.htpExampleAud);
    }

    void SetDZImage()
    {
        var dzImageRectTransform = dzGO.transform as RectTransform;

        dzImageRectTransform.sizeDelta = scenario.dzRectDimensions;
        dzGO.transform.localPosition = scenario.dzPos;
        dzGO.GetComponent<Image>().sprite = scenario.dzImage;
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
        EmptyScenarioText();
    }
}
