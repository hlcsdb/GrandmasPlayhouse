using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class OptionButtonSettings : MonoBehaviour
{
	[SerializeField]
	internal string buttonWord;
	internal TextMeshProUGUI buttonTextField;
	internal bool isSelected;
	public Sprite[] optionButtonSprites = new Sprite[5];
	internal SMSController smsController;
	internal QuestionManager questionManager;
	internal Image buttonImage;

	void Start()
	{
		
	}
	public void ActivateOptionButtons()
    {
		smsController = GameObject.Find("SMS OBJECT").GetComponent<SMSController>();
		questionManager = GameObject.Find("Question Manager").GetComponent<QuestionManager>();
		//Debug.Log(transform.GetChild(0).name);
		buttonTextField = transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
		//Debug.Log(buttonTextField.text);
		buttonImage = GetComponent<Image>();

}

public void SetButton(string word)
	{
		ActivateOptionButtons();
		buttonWord = word;
		buttonTextField.text = buttonWord;
		isSelected = false;
		buttonImage.sprite = optionButtonSprites[0];
	}

	public void ButtonClicked()
	{
		isSelected = !isSelected; //starts at false. first time selected, it changes to true and then the sprite changes to the second(checked)

		if (isSelected)
		{
			buttonImage.sprite = optionButtonSprites[1];
		}
		else
		{
			buttonImage.sprite = optionButtonSprites[0];
		}

		questionManager.SetSelectedItemWords(isSelected, buttonWord);
		Debug.Log(buttonWord + " is selected: " + isSelected);
	}

	public void HighlightBox(bool correctAnswer)
	{
		if (correctAnswer && isSelected)
		{
			Debug.Log(buttonWord + " should turn green");
			//change sprite color to green
			buttonImage.sprite = optionButtonSprites[3];
		}

		if (correctAnswer && !isSelected)
		{
			Debug.Log(buttonWord + " should turn green");
			//change sprite color to green
			buttonImage.sprite = optionButtonSprites[4];
		}

		else if (!correctAnswer && isSelected)
		{
			Debug.Log(buttonWord + " should turn red");
			buttonImage.sprite = optionButtonSprites[2]; //make re
		}
	}
}