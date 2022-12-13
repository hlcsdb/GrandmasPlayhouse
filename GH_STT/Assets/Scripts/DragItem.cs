using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DragItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IPointerUpHandler, IPointerDownHandler
{
    private RectTransform rectTransform;
    [SerializeField] private Canvas canvas;
    private CanvasGroup canvasGroup;
    private DraggableItem draggable;
    private ChallengeController currSceneController;
    private AudioSource audioSource;
    private float timer = 0.0f;
    private float scaleDur = 0.3f;
    private GameObject hovertext;
    internal DisplayDraggable draggableUI;
    int idleState = 0;
    int activeState = 1;
    int wrongState = 2;
    bool mobileClicked = false;


    private void Awake()
    {
        draggableUI = GetComponent<DisplayDraggable>();
        draggable = draggableUI.draggable;

        audioSource = GameObject.Find("Audio Source").GetComponent<AudioSource>();
        currSceneController = GameObject.Find("Challenge Manager").GetComponent<ChallengeController>();
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
    }

    public void Update(){
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (RecognizeHoverInPlay(true))
        {
            transform.SetSiblingIndex(7);
            rectTransform = GetComponent<RectTransform>();
            canvasGroup.blocksRaycasts = false;
            StartCoroutine(Grow(1.2f));
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (currSceneController.draggingAllowed)
        {
            if (RecognizeHoverInPlay(true)) { rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor; }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (RecognizeHoverInPlay(false))
        {
            StartCoroutine(ImmediatePostDrag());
            canvasGroup.blocksRaycasts = true;

            IEnumerator ImmediatePostDrag()
            {
                if (draggableUI.OverlappingDropZone())
                {
                    if (draggable.thisRandIndex == currSceneController.curItem) { CorrectItemDropped(); }

                    else
                    {
                        StartCoroutine(IncorrectItemDropped());
                        currSceneController.CountItemsLayered(false);
                    }
                }

                else { draggableUI.ReturnDraggable(); }

                yield return new WaitForSeconds(0.3f);
                if (!draggable.dragged) { StartCoroutine(Shrink(1f)); }
            }
        }
    }

    public void CorrectItemDropped()
    {
        currSceneController.CountItemsLayered(true);
        audioSource.PlayOneShot(draggable.audioClip);
        draggableUI.DroppedDraggable();
    }

    public IEnumerator IncorrectItemDropped()
    {
        draggableUI.ColourTileOutline(wrongState);
        yield return new WaitForSeconds(2.5f);
        draggableUI.ReturnDraggable();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (currSceneController.draggingAllowed) { draggableUI.ColourTileOutline(activeState); }
        if (currSceneController.inSelection) { draggableUI.ColourTileOutline(activeState); draggableUI.SetWord(); }

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (draggableUI.BackgroundColorState() != wrongState)
        {
            draggableUI.ColourTileOutline(idleState);
        }

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
        StartCoroutine(GrowShrinkLoop());
    }

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

    public void OnPointerDown(PointerEventData pointerEventData)
    {
        //Output the name of the GameObject that is being clicked
        //Debug.Log(name + "Game Object Click in Progress");
        if (currSceneController.draggingAllowed) { StartCoroutine(Grow(1.2f)); }
    }

    //Detect if clicks are no longer registering
    public void OnPointerUp(PointerEventData pointerEventData)
    {
        if (currSceneController.draggingAllowed)
        {
            transform.localScale = new Vector2(1,1);
        }
    }

    //public IEnumerator ShowLabelForSeconds(int seconds)
    //{
    //    hovertext = gameObject.transform.GetChild(0).gameObject;
    //    hovertext.SetActive(true);
    //    yield return new WaitForSeconds(seconds);
    //    hovertext.SetActive(false);
    //}

    //public void HideTile()
    //{
    //    draggable.Dragged(true);
    //    draggableUI.ShowHideTile();
    //}

    public void OnPointerClick(PointerEventData eventData)
    {
        if (currSceneController.inSelection)
        {
            if (!mobileClicked)
            {
                if (!audioSource.isPlaying)
                {
                    //Debug.Log("audio will start");
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

    public bool RecognizeHoverInPlay(bool andAudio)
    {
        if (!currSceneController.inSelection && !draggable.dragged && currSceneController.draggingAllowed)
        {
            if (andAudio)
            {
                if (!audioSource.isPlaying) { return true; }
                else { return false; }
            }
            return true;
        }
        return false;
    }
}