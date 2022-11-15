using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OptionButtonSettings : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	[SerializeField]
	internal AudioSource buttonAudioSource;
	internal Image buttonImage;
	public Sprite[] optionButtonSprites = new Sprite[5];
	internal TextMeshProUGUI buttonTextField;

	internal SMSController smsController;
	internal QuestionManager questionManager;

	internal bool isSelected;
	internal ClothingItem clothingItem;
	internal string clothingWord;
	internal string clothingEngl;

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
		buttonAudioSource = gameObject.GetComponentInParent(typeof(AudioSource)) as AudioSource;
	}

	public void SetButton(ClothingItem clothingButtonOption)
	{
		isSelected = false;
		ActivateOptionButtons();
		clothingItem = clothingButtonOption;
		clothingWord = clothingButtonOption.GetHulqWord();
		buttonImage.sprite = optionButtonSprites[0];
		buttonTextField.text = clothingWord;
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
        if (questionManager.inQuestion || questionManager.inResults)
        {
			buttonAudioSource.PlayOneShot(clothingItem.GetClothingAudio());
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
        if ((questionManager.inQuestion || questionManager.inResults) && buttonAudioSource.isPlaying)
        {
			buttonAudioSource.Stop();
		}
	}

	public void ButtonClicked()
	{
		isSelected = !isSelected; //starts at false. first time selected, it changes to true and then the sprite changes to the second(checked)
        if (questionManager.inQuestion)
        {
			if (isSelected)
			{
				buttonImage.sprite = optionButtonSprites[1];
			}
			else
			{
				buttonImage.sprite = optionButtonSprites[0];
			}

		questionManager.SetSelectedItemWords(isSelected, clothingItem);
		Debug.Log(clothingWord + " is selected: " + isSelected);
        }
	}

	public void HighlightBox(bool correctAnswer)
	{
		if (correctAnswer) //change sprite color to green
		{
            if (isSelected) 
            {
				Debug.Log(clothingWord + " should turn green");
				buttonImage.sprite = optionButtonSprites[3];
            }
            else if (!isSelected)
            {
				Debug.Log(clothingWord + " should turn green");
				buttonImage.sprite = optionButtonSprites[4];
			}
		}

		else if (!correctAnswer && isSelected) //change sprite color to red
		{
			Debug.Log(clothingWord + " should turn red");
			buttonImage.sprite = optionButtonSprites[2]; 
		}
	}
}