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
	internal string boyDeterminer = "tthun’ ";
	internal string girlDeterminer = "thun’ ";
	internal string smsText;
	internal string dresserDeterminer;

	void Start()
	{
		questionManagerScript = GameObject.Find("Question Manager").GetComponent<QuestionManager>();
	}

	internal void StartSMS()
	{
		if (questionManagerScript.dresserGender == "boy")
		{
			dresserDeterminer = boyDeterminer;
		}
		else { dresserDeterminer = girlDeterminer; }

	}


	string ConcatenateSMSText(List<string> clothingWords)
	{
		string sms = "hakwush " + dresserDeterminer;

		if (clothingWords.Count > 0)
		{
			if (clothingWords.Count == 1)
			{
				sms += clothingWords[0] + ".";
			}
			else if (clothingWords.Count > 1)
			{
				for (int i = 0; i < clothingWords.Count - 1; i++)
				{
					sms += clothingWords[i] + ", ";
				}
				sms += "’i’ " + dresserDeterminer + clothingWords[clothingWords.Count-1] + ".";
			}
		}
		else
		{
			sms = "hakwush " + dresserDeterminer + "...";
		}
		return sms;
	}

	public void EditSMSText(List<String> clothingWords)
	{
		smsText = ConcatenateSMSText(clothingWords).Replace("\r", "");
		activeTexting.GetComponent<TextMeshProUGUI>().text = "";
		activeTexting.GetComponent<TextMeshProUGUI>().text = smsText;
	}

	public void SetSentText()
    {
		IPTextBox.SetActive(false);
		sentTextBox.SetActive(true);
		sentText.GetComponent<TextMeshProUGUI>().text = smsText;
	}

}