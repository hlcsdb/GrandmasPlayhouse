using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class DisplayDraggable : MonoBehaviour
{
    public DraggableItem draggable;
    public TextMeshProUGUI wordString;
    public Image draggableArtwork;
    public GameObject tile;
    public Vector2 startPos;
    //internal Vector2 startRandPos;
    public Vector2 rectTransform;
    internal Vector2 randPos;
    public Vector2[] dzB = new Vector2[] { new Vector2(-290, -80), new Vector2(-290, 210), new Vector2(300, 210), new Vector2(300, -80)};
    public int randI;
    public GameObject textBackground;

    public void Start()
    {
        draggableArtwork.sprite = draggable.GetImage(0);
        draggable.startPos = transform.localPosition;
        randI = draggable.thisRandIndex;
    }

    public void SetRandPos(Vector2 rPos)
    {
        randPos = rPos;
        transform.localPosition = randPos;
    }

    public void ColourTileOutline(int state)
    {
        tile.transform.GetChild(0).GetComponent<Image>().color = draggable.tileStateOutlineColors[state];
    }

    public void SetWord()
    {
        textBackground.SetActive(true);
        wordString.text = draggable.WordString();
    }

    public void ShowHideTile()
    {
        ColourTileOutline(0);
        //tile.SetActive(false);

        if (tile.activeSelf)
        {
            tile.SetActive(false);
        }
        else
        {
            tile.SetActive(true);
        }
    }

    public void HideWord()
    {
        textBackground.SetActive(false);

        wordString.text = "";
    }

    public void DroppedDraggableImage()
    {
        draggableArtwork.sprite = draggable.GetImage(1);
    }

    public void ResetDraggableDisplay()
    {
        ColourTileOutline(0);
        draggableArtwork.sprite = draggable.GetImage(0);
        transform.localPosition = draggable.startPos;
        tile.SetActive(true);
        transform.localScale = draggable.startSize;
    }

    public Vector2 ThisRandomPos()
    {
        return randPos;
    }

    public Vector2[] GetBounds(Vector2 tp)
    {
        float halfSize = 50;

        Vector2[] b = new Vector2[4];
        b[0] = new Vector2(tp.x - halfSize, tp.y - halfSize);
        b[1] = new Vector2(tp.x - halfSize, tp.y + halfSize);
        b[2] = new Vector2(tp.x + halfSize, tp.y + halfSize);
        b[3] = new Vector2(tp.x + halfSize, tp.y - halfSize);
        return b;
    }

    public bool OverlappingDropZone()
    {
        Debug.Log("x: " + transform.localPosition.x + " y: " + transform.localPosition.y);
        Vector2[] db = GetBounds(transform.localPosition);

        for (int i = 0; i < 4; i++)
        {
            if (db[i].x < dzB[3].x && db[i].x > dzB[0].x && db[i].y < dzB[1].y && db[i].y > dzB[0].y)
            {
                return true;
            }
        }
        return false;
    }
}