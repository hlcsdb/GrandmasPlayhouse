using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ChallengeController : MonoBehaviour
{
    //Selection
    public ScenarioSetter scenarioSetter;
    public int dialect = 1;
    public Scenario selectedScenarioSO;
    private DisplayScenario selectedScenarioUI;
    public Transform draggableContainer;
    public List<DraggableItem> draggables;
    public List<GameObject> draggableObjects = new List<GameObject>();
    public int numObjects;
    internal DraggableItem activeSO;
    internal AudioSource audioSource;
    public GameObject gameOverScreen;
    //Gameplay
    public bool inSelection = true;
    private int numItemsDropped;
    internal int curItem = 0;
    int numErrors = 0;
    public GameObject sceneAudButton;
    internal bool inInstruction = true;
    internal bool HighlightCorrectItem = false;
    internal bool draggingAllowed = false;
    public GameObject stars;

    private void Start()
    {
        audioSource = GameObject.Find("Audio Source").GetComponent<AudioSource>();
    }

    public void SetSelectedStart()
    {
        inSelection = false;
        selectedScenarioSO = scenarioSetter.currentScenario;
        selectedScenarioUI = GameObject.Find("Canvas").GetComponent<DisplayScenario>();

        numObjects = selectedScenarioSO.scenarioDraggableObjects.Count;

        Debug.Log(numObjects);
        selectedScenarioUI.EmptyScenarioText();

        SetDraggableOrder();
        RandomizeDraggablePos();
    }

    public List<int> IndicesArray(int length)
    {
        List<int> draggableIndices = new List<int>();
        for (int i = 0; i < length; i++)
        {
            draggableIndices.Add(i);
        }
        return draggableIndices;
    }

     public void SetDraggableOrder()
     {
        List<DraggableItem> tempDraggables = new List<DraggableItem>(selectedScenarioSO.scenarioDraggableItems);
        List<int> iArr = IndicesArray(numObjects);

        if (draggables.Count > 0 || draggables != null)
        {
            draggables.Clear();
        }

        int iRand;
        for (int i = 0; i < numObjects; i++)
        {
            iRand = Random.Range(0, iArr.Count);

            draggables.Add(tempDraggables[iArr[iRand]]);
            draggables[i].ThisItemIndex(i);
            draggables[i].ResetSO();
            iArr.RemoveAt(iRand);
        }
    }

    public void RandomizeDraggablePos()
    {
        List<int> iArr = IndicesArray(numObjects);
        int iRand;

        
        Debug.Log("num objects randomizing pos: " + draggableObjects.Count);
        foreach (GameObject draggable in draggableObjects)
        {
            iRand = Random.Range(0, iArr.Count);
            Vector2 randSlot = selectedScenarioSO.randSlots[iArr[iRand]];
            Debug.Log(randSlot.x);
            draggable.GetComponent<DisplayDraggable>().SetRandPos(randSlot);
            iArr.RemoveAt(iRand);
        }
        StartCoroutine(InstructDragging(curItem));
    }

    public IEnumerator InstructDragging(int curItem)
    {
        FadeAllTiles(true);
        yield return new WaitUntil(() => !audioSource.isPlaying);

        sceneAudButton.SetActive(false);

        yield return new WaitForSeconds(2f);
        if (draggables[curItem].IsInstructionCustom())
        {
            selectedScenarioUI.ShowCustomInstruction(draggables[curItem].InstructionString());
        }
        else
        {
            selectedScenarioUI.ShowRepeater(draggables[curItem].WordString());
        }
        
        PlayInstructionAud();
        sceneAudButton.SetActive(true);
        Debug.Log(draggables[curItem].name);
        yield return new WaitUntil(() => !audioSource.isPlaying);
        sceneAudButton.SetActive(true);
        inInstruction = false;
        FadeAllTiles(false);
    }

    internal void FadeAllTiles(bool fade)
    {
        foreach (GameObject tile in draggableObjects)
        {
            tile.GetComponent<DisplayDraggable>().FadeTileImage(fade);
        }
    }

    public void PlayInstructionAud()
    {
        if (audioSource.isPlaying) { audioSource.Stop(); Debug.Log("was playing"); }
        if (!inSelection)
        {
            StartCoroutine(PairInstruction());
            IEnumerator PairInstruction()
            {
                audioSource.PlayOneShot(selectedScenarioSO.GetRepeaterAudio());
                yield return new WaitUntil(() => !audioSource.isPlaying);
                audioSource.PlayOneShot(draggables[curItem].GetAudio());
                yield return new WaitUntil(() => !audioSource.isPlaying);
                draggingAllowed = true;
            }
        }
    }

    public void CountItemsLayered(bool correct)
    {
        draggingAllowed = false;
        FadeAllTiles(true);
        if (!correct)
        {
            audioSource.PlayOneShot(selectedScenarioSO.incorrectSelectionAud);
            numErrors++;
            if (numErrors == 3)
            {
                draggableObjects[curItem].GetComponent<DragItem>().HighlightCorrectItem();
            }
            draggingAllowed = true;
            FadeAllTiles(false);
        }

        else if (correct)
        {
            numErrors = 0;
            numItemsDropped++;

            Debug.Log(draggableObjects[curItem].transform.localPosition);
            InstantiateStars(draggables[curItem].dropPos, 0.3f);

            curItem++;
            StartCoroutine(AudAfterCorrDrop());
        }
    }

    //only used for testing purposes
    public void TriggerStar()
    {
        InstantiateStars(new Vector2(300,300), 0);
    }

    public void InstantiateStars(Vector2 starPosition, float delaySeconds)
    {
        activeSO = draggables[curItem];
        StartCoroutine(InstantiateOnDelay());
        IEnumerator InstantiateOnDelay()
        {
            yield return new WaitForSeconds(delaySeconds);
            //Instantiate(stars, starPosition, Quaternion.identity);
            Instantiate(stars);
        }
    }

    public IEnumerator AudAfterCorrDrop()
    {
        //inInstruction = true;
        yield return new WaitForSeconds(1);
        yield return new WaitUntil(() => !audioSource.isPlaying);
        //if (selectedScenarioSO.repeaterPhraseAud)
        //{
        //    audioSource.PlayOneShot(selectedScenarioSO.repeaterPhraseAud);
        //    yield return new WaitUntil(() => !audioSource.isPlaying);
            audioSource.PlayOneShot(selectedScenarioSO.correctPhraseAud);
        //}
        yield return new WaitUntil(() => !audioSource.isPlaying);
        if (numItemsDropped == draggables.Count)
        {
            yield return new WaitForSeconds(1);
            yield return new WaitWhile(() => audioSource.isPlaying);
            StartCoroutine(Success());
        }
        else
        {
            StartCoroutine(InstructDragging(curItem));
        }
    }

    public IEnumerator Success()
    {
        new WaitForSeconds(2);
        selectedScenarioUI.ShowSuccess();
        //big particle effect
        yield return new WaitWhile(() => audioSource.isPlaying);
        audioSource.PlayOneShot(selectedScenarioSO.successPhraseAud);
        yield return new WaitWhile(() => audioSource.isPlaying);
        yield return new WaitForSeconds(1);
        StartCoroutine(ShowCompletionScreen());
    }

    public IEnumerator ShowCompletionScreen()
    {
        //selectedScenarioUI.ShowCompletionText();
        gameOverScreen.SetActive(true);
        DestroyDraggables();
        yield return new WaitWhile(() => audioSource.isPlaying);
        //audioSource.PlayOneShot(selectedScenarioSO.completionPhraseAud);
    }

    void DestroyDraggables()
    {
        foreach(GameObject draggable in draggableObjects)
        {
            Destroy(draggable);
        }
        draggableObjects.Clear();
    }

    void DestroyFeedback() {
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Feedback");

        for (var i = 0; i < gameObjects.Length; i++)
        {
            Destroy(gameObjects[i]);
        }
    }


    public void SetDialect(int currDialect)
    {
        dialect = currDialect;

        foreach (DraggableItem draggableItem in draggables)
        {
            draggableItem.SetCurrDialect(currDialect);
        }
    }


    public void Replay()
    {
        StopAllCoroutines();
        DestroyFeedback();
        DestroyDraggables();
        numItemsDropped = 0;
        curItem = 0;
        selectedScenarioUI.SpawnDraggables();
        StartCoroutine(PositionNewDraggables());

        IEnumerator PositionNewDraggables()
        {
            yield return new WaitUntil(()=>draggableContainer.transform.childCount == 0);
            SetSelectedStart();
        }
        
    }
}
