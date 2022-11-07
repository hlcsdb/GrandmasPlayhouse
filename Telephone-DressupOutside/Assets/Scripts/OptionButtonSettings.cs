using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class OptionButtonSettings : MonoBehaviour
{
	[SerializeField]
	internal string buttonWord;
	internal TextMeshPro buttonTextField;
	internal bool isSelected;
	public Sprite[] optionButtonSprites = new Sprite[2];
	internal SMSController smsController;
	internal QuestionManager questionManager;
	private bool checkedEver;
	internal Image buttonImage;


	void Start()
	{
		buttonTextField = transform.GetChild(0).GetComponent<TextMeshPro>();
		smsController = GameObject.Find("SMS OBJECT").GetComponent<SMSController>();
		questionManager = GameObject.Find("Question Manager").GetComponent<QuestionManager>();
	}

	public void SetButton(string word)
	{
		checkedEver = false;
		buttonWord = word;
		buttonTextField.text = buttonWord;
		isSelected = false;
		GetComponent<Image>().sprite = optionButtonSprites[0];
	}

	public void ButtonClicked()
	{
		checkedEver = true;

		isSelected = !isSelected; //starts at false. first time selected, it changes to true and then the sprite changes to the second(checked)

		if (isSelected)
		{
			buttonImage.sprite = optionButtonSprites[1];
		}
		else
		{
			buttonImage.sprite = optionButtonSprites[0];
		}

		GetComponentInParent<QuestionManager>().SetSelectedItemWords(isSelected, buttonWord);
	}

	public void HighlightBox(bool correctAnswer)
	{
		if (checkedEver)
		{
			if (correctAnswer)
			{
				//change sprite color to green
				buttonImage.color = new Color(189, 243, 141, 255); //make green
			}

			else
			{
				buttonImage.color = new Color(243, 150, 141, 255); //make red
			}
		}
	}
}