using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DragItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private RectTransform rectTransform;
    private RectTransform startRectTransform;
    //private float timeCount = 0.0f;

    [SerializeField] private Canvas canvas;
    private CanvasGroup canvasGroup;
    private DraggableItem draggable;
    private ChallengeController currSceneController;
    //private FreeplayController currSceneController;
    private AudioSource audioSource;
    private float timer = 0.0f;
    private float scaleDur = 0.3f;
    private GameObject hovertext;
    internal DisplayDraggable draggableUI;
    // private float rotateAmount = 10.0f; //Amount to rotate in degrees

    //outline colours
    int idleState = 0;
    int activeState = 1;
    int wrongState = 2;
    bool mobileClicked = false;
    int siblingIndex;

    private void Awake()
    {
        draggable = GetComponent<DisplayDraggable>().draggable;
        audioSource = GameObject.Find("Audio Source").GetComponent<AudioSource>();
        //if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Challenge"))
        //{
            currSceneController = GameObject.Find("Challenge Manager").GetComponent<ChallengeController>();
        //}
        rectTransform = GetComponent<RectTransform>();
        startRectTransform = rectTransform;
        canvasGroup = GetComponent<CanvasGroup>();
        draggableUI = GetComponent<DisplayDraggable>();
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        siblingIndex = transform.GetSiblingIndex();
    }

    public void Update(){
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        transform.SetSiblingIndex(7);
        if (!currSceneController.inSelection && !draggable.dragged && !audioSource.isPlaying && !currSceneController.inInstruction)
        {
            
            rectTransform = GetComponent<RectTransform>();
            Debug.Log("begin dragging");
            // canvasGroup.blocksRaycasts = false;
            StartCoroutine(Grow(1.2f));
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!currSceneController.inSelection && !draggable.dragged && !audioSource.isPlaying && !currSceneController.inInstruction)
        {
            rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetSiblingIndex(siblingIndex);
        if (!currSceneController.inSelection && !draggable.dragged && !currSceneController.inInstruction)
        {
            

            Debug.Log("end dragging");

            if (!draggableUI.OverlappingDropZone())
            {
                Debug.Log(" not overlapping");
                transform.localPosition = draggableUI.ThisRandomPos();
            }


            else
            {
                Debug.Log("overlapping");
                Debug.Log("thisrandindex: " + draggable.thisRandIndex + ", cur item: " + currSceneController.curItem);
                if (draggable.thisRandIndex == currSceneController.curItem)
                {
                    Debug.Log("correct item dragged");
                    canvasGroup.blocksRaycasts = true;
                    CorrectItemDropped();
                }

                else
                {
                    Debug.Log("incorrect item dragged");
                    StartCoroutine(IncorrectItemDropped());
                    currSceneController.CountItemsLayered(false);
                }
            }
        }
    }

    public void CorrectItemDropped()
    {
        //if(!GameObject.Find("mirror glass").activeSelf)
        //{
        //    GameObject.Find("mirror glass").SetActive(true);
        //}

        if (gameObject.name== "underpants"){
            GameObject.Find("towel").SetActive(false);
        }

        transform.localScale = draggable.dropSize;
        transform.localPosition = draggable.dropPos;
        draggableUI.DroppedDraggableImage();
        currSceneController.CountItemsLayered(true);
        audioSource.PlayOneShot(draggable.audioClip);
        draggable.Dragged(true);
        HideTile();
        //little particle effect
    }

    public IEnumerator IncorrectItemDropped()
    {
        draggableUI.ColourTileOutline(wrongState);
        yield return new WaitForSeconds(2.5f);
        transform.localPosition = draggableUI.ThisRandomPos();
        draggableUI.ColourTileOutline(idleState);
        StartCoroutine(Shrink(1f));

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!currSceneController.inInstruction)
        {
            draggableUI.ColourTileOutline(activeState);
        }
        if (currSceneController.inSelection)
        {
            draggableUI.ColourTileOutline(activeState);
            draggableUI.SetWord();
            //hovertext.SetActive(true);
        }
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        draggableUI.ColourTileOutline(idleState);

        if (currSceneController.inSelection)
        {
            draggableUI.HideWord();
            mobileClicked = false;
        }
    }

    private IEnumerator Grow(float maxSize)
    {
        Vector2 startScale = transform.localScale;
        Vector2 maxScale = new Vector2(maxSize, maxSize);
        do
        {
            transform.localScale = Vector3.Lerp(startScale, maxScale, timer / scaleDur);
            timer += Time.deltaTime;
            yield return null;
        }
        while (timer < scaleDur);
        timer = 0;
    }

    private IEnumerator Shrink(float minSize)
    {
        Vector2 startScale = transform.localScale;
        Vector2 minScale = new Vector2(minSize, minSize);
        do
        {
            transform.localScale = Vector3.Lerp(startScale, minScale, timer / scaleDur);
            timer += Time.deltaTime;
            yield return null;
        }
        while (timer < scaleDur);
        timer = 0;
    }

    public void HighlightCorrectItem()
    {
        //StartCoroutine(TwistDraggable());
        StartCoroutine(GrowShrinkLoop());
    }

    // public IEnumerator TwistDraggable()
    // {
    //     Debug.Log("twisting");
    //     transform.Rotate(0.0f, 0.0f, rotateAmount / 2);
    //     for (int i = 0; i < 2; i++)
    //     {
    //         if (transform.rotation.z > 0)
    //         {
    //             transform.Rotate(0.0f, 0.0f, -rotateAmount);
    //         }
    //         else if (transform.rotation.z < 0)
    //         {
    //             transform.Rotate(0.0f, 0.0f, rotateAmount);
    //         }
    //         yield return new WaitForSeconds(0.07f);
    //     }
    //     transform.Rotate(0.0f, 0.0f, -(rotateAmount / 2));
    // }

    public IEnumerator GrowShrinkLoop()
    {
        for (int i = 0; i < 2; i++)
        {
            StartCoroutine(Grow(1.2f));
            yield return new WaitForSeconds(0.5f);
            StartCoroutine(Shrink(0.5f));
            yield return new WaitForSeconds(0.3f);
            
        }
        StartCoroutine(Grow(1f));
    }

    public IEnumerator ShowLabelForSeconds(int seconds)
    {
        hovertext = gameObject.transform.GetChild(0).gameObject;
        hovertext.SetActive(true);
        yield return new WaitForSeconds(4);
        hovertext.SetActive(false);
    }

    public void HideTile()
    {
        draggable.Dragged(true);
        draggableUI.ShowHideTile();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (currSceneController.inSelection)
        {
            if (!mobileClicked)
            {
                if (!audioSource.isPlaying)
            {
                    Debug.Log("audio will start");
                    audioSource.PlayOneShot(draggable.audioClip);
                    mobileClicked = true;
                }
                else
                {
                    audioSource.Stop();
                    audioSource.PlayOneShot(draggable.audioClip);
                }
            }
              
            else
            {
                mobileClicked = false;
            }
        }
    }
}
