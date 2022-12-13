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
    public string[] titleName = new string[2];
    public string[] openerPhrase = new string[2];
    public Vector2[] DZB = new Vector2[] { new Vector2(80, -180), new Vector2(80, 95), new Vector2(300, 95), new Vector2(300, -180) };

    public string[] repeaterPhrase; //instructions that proceed the word of every item, eg. LAY DOWN THE plate // if this is different for different objects in the scenario, leave it blank and specify in DisplayDraggable
    public string[] successPhrase;

    public string[] completionPhrase;

    //public AudioSource audioS;
    public AudioClip titleAud;
    public AudioClip openerPhraseAud;
    public AudioClip repeaterPhraseAud; //instructions that proceed the word of every item, eg. LAY DOWN THE plate - IF CUSTOM TO A TILE.
    public AudioClip successPhraseAud;
    public AudioClip completionPhraseAud;
    public AudioClip correctPhraseAud;
    public AudioClip incorrectSelectionAud;

    public Sprite homeImage;
    public int numDraggables;
    public Sprite backgroundImage;
    public int dialect;
    public string playSceneName;
    public Sprite dzImage;
    public Vector2 dzPos;
    public Vector2 dzRectDimensions;



    //set in scenario scriptable object and clear list here.
    public List<Vector2> randSlots = new List<Vector2> { new Vector2(-400, 118), new Vector2(-400, 3), new Vector2(-400, -112), new Vector2(-400, -227), new Vector2(-273, 118), new Vector2(-273, 3), new Vector2(-273, -112), new Vector2(-273, -227) };

    public void Awake()
    {
        numDraggables = scenarioDraggableItems.Count;
        SetObjectsScenarioParam();
        //PopulateStartSlots(numDraggables);
    }

    internal AudioClip GetTitleAudioClip()
    {
        return titleAud;
    }

    void SetObjectsScenarioParam()
    {
        foreach(DraggableItem draggableSO in scenarioDraggableItems)
        {
            draggableSO.dzB = DZB;
        }
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