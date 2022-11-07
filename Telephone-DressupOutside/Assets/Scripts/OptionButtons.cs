using System;
using System.Collections;
using System.Collections.Generic;
// using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

//Attach to Toggle Game object. This game object will contain a sprite container for the button background and 
public class OptionButtons : MonoBehaviour
{
    // To use this example, attach this script to an empty GameObject.
    // Create three buttons (Create>UI>Button). Next, select your
    // empty GameObject in the Hierarchy and click and drag each of your
    // Buttons from the Hierarchy to the Your First Button, Your Second Button
    // and Your Third Button fields in the Inspector.
    // Click each Button in Play Mode to output their message to the console.
    // Note that click means press down and then release.

    //code from docs.unity3d.com/2019.1/Documentation/ScriptReference/UI.Button-onClick.html

    //Make sure to attach these Buttons in the Inspector
    public GameObject[] optionButtons = new GameObject[8];
    //public GameObject SMSField; //attach game object that contains textmeshpro component
    public GameObject QBResultsPos;

    void Start()
    {

    }

    public void SetOptionButtons(List<String> optionText)
    {
        //Sets button text and listeners
        int i = 0;
        foreach (GameObject optionButton in optionButtons)
        {
            optionButton.GetComponent<OptionButtonSettings>().SetButton(optionText[i]);
            i++;
        }
    }

    //void TaskWithParameters(string optionText)
    //{   //second child of button object is the text object
    //    TextMeshPro buttonText = transform.GetChild(0).GetComponent<TextMeshPro>();
    //    //change sprite to add checkmark /remove
    //}

    public void HighlightAnswers(List<ClothingItem> modelClothingList)
    {
        transform.localPosition = QBResultsPos.transform.localPosition;
        //foreach (GameObject optionButton in optionButtons)
        //{
        //    if (modelClothingList.Find(item => item.GetHulqWord() == optionButton.buttonWord) != null)
        //    {
                optionButtons[0].GetComponent<OptionButtonSettings>().HighlightBox(true);
        //    } //First might not be the right method
        //    else { optionButton.GetComponent<OptionButtonSettings>().HighlightBox(false); }
        //}
    }
}