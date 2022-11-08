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
        for (int i = 0; i < optionButtons.Length; i++)
        {
            optionButtons[i].GetComponent<OptionButtonSettings>().SetButton(optionText[i]);
        }
    }

    //void TaskWithParameters(string optionText)
    //{   //second child of button object is the text object
    //    TextMeshPro buttonText = transform.GetChild(0).GetComponent<TextMeshPro>();
    //    //change sprite to add checkmark /remove
    //}

    public void HighlightAnswers(List<ClothingItem> modelClothingList)
    {
        Debug.Log("strt highlighting answers");

        transform.position = QBResultsPos.transform.position;
        foreach (GameObject optionButton in optionButtons)
        {
            OptionButtonSettings buttonSetting = optionButton.GetComponent<OptionButtonSettings>();
            Debug.Log(buttonSetting.buttonWord);
            if (modelClothingList.Find(item => item.GetHulqWord() == buttonSetting.buttonWord))
            {
                Debug.Log("correct option: " + buttonSetting.buttonWord);
                buttonSetting.HighlightBox(false);
            }
            
            else
            {
                Debug.Log("incorrect option" + buttonSetting.buttonWord);
                buttonSetting.HighlightBox(true);
            }
        }
    }
}