using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//All the stuff regarding the sms texting window happens here.

public class SMSController : MonoBehaviour
{
	public GameObject activeTexting;
	public GameObject sentText;
	public GameObject IPTextBox;
	public GameObject sentTextBox;
	internal string dresserGender;
	internal QuestionManager questionManagerScript;
	internal string maleDeterminer = "tthun’ ";
	internal string femaleDeterminer = "thun’ ";
	internal string SMS;
	internal string dresserDeterminer;

	void Start()
	{
		questionManagerScript = GameObject.Find("QuestionManager").GetComponent<QuestionManager>();
	}

	internal void StartSMS()
	{
		if (questionManagerScript.dresserGender == "male")
		{
			dresserDeterminer = maleDeterminer;
		}
		else { dresserDeterminer = femaleDeterminer; }

		SMS = "hakwush " + dresserDeterminer + "...";
	}


	void ConcatenateSMSText(List<string> clothingWords)
	{
		if (clothingWords.Count > 0)
		{
			if (clothingWords.Count == 1)
			{
				SMS += clothingWords[0] + ".";
			}
			else
			{
				for (int i = 0; i < clothingWords.Count - 1; i++)
				{
					SMS += clothingWords[i] + ",";
				}
				SMS += "’i’ " + dresserDeterminer + clothingWords[clothingWords.Count] + ".";
			}
		}
		else
		{
			SMS += "...";
		}
	}

	public void EditSMSText(List<String> clothingWords)
	{
		ConcatenateSMSText(clothingWords);
		activeTexting.GetComponent<TextMeshPro>().text = SMS;
	}

	public void SetSentText()
    {
		IPTextBox.SetActive(false);
		sentTextBox.SetActive(true);
		sentText.GetComponent<TextMeshPro>().text = SMS;

	}

}