using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChallengeController : MonoBehaviour
{
    //Selection
    public Scenario[] allScenarios;
    //public GameObject selectionScreen;
    //public GameObject carousel;
    public int dialect = 1;

    //Scenario
    //private int selectedScenarioIndex = 0;
    public Scenario selectedScenarioSO;
    public GameObject selectedScenarioObj;
    private DisplayScenario selectedScenarioUI;
   
    public List<DraggableItem> draggables;
    public List<GameObject> draggableObjects = new List<GameObject>();
    public int numObjects;

    //private CarouselSlider carouselSliderScript;

    //UI
    internal SettingsPanelController settingsController;
    internal AudioSource audioSource;
    public GameObject gameOverScreen;
    //Gameplay
    public bool inSelection = true;
    private int numItemsDropped;
    internal int curItem = 0;
    int numErrors = 0;
    public Button sceneAudButton;
    internal bool inInstruction = true;

    
    //public AudioClip incorrectSelectionAudio;


    private void Start()
    {
        /// IF USING MULTIPLE SCENARIOS / CAROUSEL
        //carouselSliderScript = carousel.GetComponent<CarouselSlider>();

        //settingsController = GameObject.Find("Settings").GetComponent<SettingsPanelController>();
        audioSource = GameObject.Find("Audio Source").GetComponent<AudioSource>();
    }


    public void SetSelectedStart()
    {
        
        inSelection = false;
        sceneAudButton.interactable = false;

        //selectedScenarioObj = GameObject.Find("Scenarios").transform.GetChild(selectedScenarioIndex).gameObject;
        numObjects = selectedScenarioObj.transform.GetChild(0).gameObject.transform.childCount;
        selectedScenarioSO = allScenarios[0];
        selectedScenarioUI = selectedScenarioObj.GetComponent<DisplayScenario>();
        /// IF USING MULTIPLE SCENARIOS / CAROUSEL -- will need to reorder when carousel is used
        /// 
        //      selectedScenario = allScenarios[carouselSliderScript.slideNum];
        //      StartCoroutine(WaitToResetCarousel(0.5f));
        //      HideInactiveScenarios(carouselSliderScript.slideNum);
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
        List<DraggableItem> tempDraggables = selectedScenarioSO.scenarioDraggableItems;
        List<int> iArr = IndicesArray(numObjects);

        int iRand;
        for (int i = 0; i < numObjects; i++)
        {
            iRand = Random.Range(0, iArr.Count);

            draggables.Add(tempDraggables[iArr[iRand]]);
            draggableObjects.Add(selectedScenarioObj.transform.GetChild(0).gameObject.transform.GetChild(iArr[iRand]).gameObject);
            draggables[i].ThisItemIndex(i);
            draggables[i].ResetSO();
            draggableObjects[i].GetComponent<DisplayDraggable>().HideWord();
            iArr.RemoveAt(iRand);
        }
    }

    public void RandomizeDraggablePos()
    {
        List<int> iArr = IndicesArray(numObjects);
        int iRand;

        foreach (GameObject draggable in draggableObjects)
        {
            iRand = Random.Range(0, iArr.Count);
            int randSlot = iArr[iRand];
            draggable.GetComponent<DisplayDraggable>().SetRandPos(selectedScenarioSO.randSlots[randSlot]);
            iArr.RemoveAt(iRand);
        }
        StartCoroutine(InstructDragging(curItem));
    }

    public IEnumerator InstructDragging(int curItem)
    {
        yield return new WaitUntil(() => !audioSource.isPlaying);
        yield return new WaitForSeconds(2f);
        if (draggables[curItem].IsInstructionCustom())
        {
            selectedScenarioUI.ShowCustomInstruction(draggables[curItem].InstructionString());
        }
        else
        {
            selectedScenarioUI.ShowRepeater(draggables[curItem].WordString());
        }

        audioSource.PlayOneShot(draggables[curItem].draggableInstruction);
        
        yield return new WaitUntil(() => !audioSource.isPlaying);
        sceneAudButton.interactable = true;
        inInstruction = false;
    }

     public void ReplaySceneAud()
    {
        if (!inSelection)
        {
            audioSource.PlayOneShot(draggables[curItem].draggableInstruction);
        }
    }

    public void CountItemsLayered(bool correct)
    {
        if (!correct)
        {
            audioSource.PlayOneShot(selectedScenarioSO.incorrectSelectionAud);
            numErrors++;
            if (numErrors == 3)
            {
                draggableObjects[curItem].GetComponent<DragItem>().HighlightCorrectItem();
            }
        }

        else if (correct)
        {
            numErrors = 0;
            numItemsDropped++;
            curItem++;

            StartCoroutine(AudAfterCorrDrop());
        }
    }

    public IEnumerator AudAfterCorrDrop()
    {
        inInstruction = true;
        yield return new WaitForSeconds(1);
        sceneAudButton.interactable = false;
        yield return new WaitUntil(() => !audioSource.isPlaying);
        audioSource.PlayOneShot(selectedScenarioSO.repeaterPhraseAud);

        if (numItemsDropped == draggables.Count)
        {
            yield return new WaitForSeconds(1);
            yield return new WaitUntil(() => !audioSource.isPlaying);
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
        yield return new WaitUntil(() => !audioSource.isPlaying);
        audioSource.PlayOneShot(selectedScenarioSO.successPhraseAud);
        yield return new WaitWhile(() => audioSource.isPlaying);
        yield return new WaitForSeconds(1);
        ShowCompletionScreen();
    }

    public void ShowCompletionScreen()
    {
        //selectedScenarioUI.ShowCompletionText();
        gameOverScreen.SetActive(true);
        selectedScenarioObj.transform.parent.gameObject.SetActive(false);
        audioSource.PlayOneShot(selectedScenarioSO.completionPhraseAud);
        //yield return new WaitWhile(() => audioSource.isPlaying);
        ResetAllDraggableObjects();
    }

    public void SetDialect(int currDialect)
    {
        dialect = currDialect;

        foreach (DraggableItem draggableItem in draggables)
        {
            draggableItem.SetCurrDialect(currDialect);
        }
    }

    public void ResetAllDraggableObjects()
    { 
        foreach(GameObject draggableObject in draggableObjects)
        {
            draggableObject.GetComponent<DisplayDraggable>().ResetDraggableDisplay();
        }

        foreach (DraggableItem draggable in draggables)
        {
            draggable.ResetSO();
        }

        draggables.Clear();
        draggableObjects.Clear();
    }


    public void BackToSelection()
    {
        ResetAllDraggableObjects();
        inSelection = true;
        numItemsDropped = 0;
        curItem = 0;
    }


    //ONLY USED WITH CAROUSEL
    //IEnumerator WaitToResetCarousel(float secondsToWait)
    //{
    //    carouselSliderScript.ResetSlider();
    //    yield return new WaitForSeconds(secondsToWait);
    //    //selectionScreen.SetActive(false);
    //}

    ////ONLY USED WITH CAROUSEL
    //public void HideInactiveScenarios(int scenarioIndex)
    //{
    //    for(int i = 0; i < allScenarios.Length; i++)
    //    {
    //        if(i != scenarioIndex)
    //        {
    //            allScenarios[i].HideScenarioObject();
    //        }
    //    }
    //}

   
}
