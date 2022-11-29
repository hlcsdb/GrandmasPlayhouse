using UnityEngine;
using System.Collections.Generic;

public enum ScenarioType
{
    Default,
    TeaTime,
    Cake,
    Breakfast,
    Soup,
    Jewlery
}

[CreateAssetMenu(fileName = "New Scenario", menuName = "Scenarios")]

public class Scenario : ScriptableObject
{
    public GameObject scenarioObject;
    public ScenarioType scenarioType;
    public string scenarioName;
    public List<GameObject> scenarioDraggableObjects = new List<GameObject>();
    public List<DraggableItem> scenarioDraggableItems;
    public string[] sceneDescription;
    public string[] openerPhrase;

    public string[] repeaterPhrase; //instructions that proceed the word of every item, eg. LAY DOWN THE plate // if this is different for different objects in the scenario, leave it blank and specify in DisplayDraggable
    public string[] successPhrase;

    public string[] completionPhrase;

    //public AudioSource audioS;
    public AudioClip sceneDescriptionAud;
    public AudioClip openerPhraseAud;
    public AudioClip repeaterPhraseAud; //instructions that proceed the word of every item, eg. LAY DOWN THE plate
    public AudioClip successPhraseAud;
    public AudioClip completionPhraseAud;
    public AudioClip correctPhraseAud;
    public AudioClip incorrectSelectionAud;

    public int numDraggables;
    public Sprite backgroundImage;
    public int dialect;

    public List<Vector2> randSlots = new List<Vector2>();

    public void Awake()
    {
        numDraggables = scenarioDraggableItems.Count;
        randSlots = new List<Vector2> { new Vector2(-400, 118), new Vector2(-400, 3), new Vector2(-400, -112), new Vector2(-400, -227), new Vector2(-273, 118), new Vector2(-273, 3), new Vector2(-273, -112), new Vector2(-273, -227) };
        //PopulateStartSlots(numDraggables);
    }
    public void SetDialect(int currDialect)
    {
        dialect = currDialect;
        SetDialectOfDraggables(currDialect);
    }

    public void SetDialectOfDraggables(int currDialect)
    {
        //foreach(DraggableItem draggable in scenarioDraggables)
        //{
        //    draggable.SetCurrDialect(currDialect);
        //}
    }

    public void HideScenarioObject()
    {
        scenarioObject.SetActive(false);
    }

    public void ShowScenarioObject()
    {
        scenarioObject.SetActive(true);
    }

    public AudioClip GetRepeaterAudio()
    {
        return repeaterPhraseAud;
    }
}