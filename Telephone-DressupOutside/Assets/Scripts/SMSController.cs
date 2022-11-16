using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using UnityEngine.UI;

//All the stuff regarding the sms texting window happens here.

public class SMSController : MonoBehaviour
{
	public GameObject activeTexting;
	public GameObject sentText;
	public GameObject IPTextBox;
	public GameObject sentTextBox;
	internal string dresserGender;
	internal QuestionManager questionManagerScript;
	internal string boyDeterminer = "thun ";
	internal string girlDeterminer = "thu ";
	internal string hakwush = "hakwush";
	internal string connector = "’i’";
	internal string smsText;
	internal string dresserDeterminer;
	internal AudioSource audioSource;
	public List<AudioClip> templateAudio; //[hakwush, thun, thu, ’i’]
	public List<AudioClip> selectedItemAudio;
	public GameObject smsAud;

	void Start()
	{
		questionManagerScript = GameObject.Find("Question Manager").GetComponent<QuestionManager>();
		audioSource = GameObject.Find("Question Buttons").GetComponent<OptionButtons>().GetComponent<AudioSource>();
	}

	internal void StartSMS()
	{
		if (questionManagerScript.dresserGender == "boy")
		{
			dresserDeterminer = boyDeterminer;
		}
		else { dresserDeterminer = girlDeterminer; }
	}


	string ConcatenateSMSText(List<ClothingItem> clothingItems)
	{
		string period = ".";
		string comma = ", ";
		string space = " ";
		string ellipses = "...";

		string sms = hakwush + space + dresserDeterminer;
		selectedItemAudio.Clear();
		if (clothingItems.Count > 0)
		{
			if (clothingItems.Count == 1)
			{
				sms += clothingItems[0].GetHulqWord() + period;
				selectedItemAudio.Add(clothingItems[0].GetClothingAudio());
			}
			else if (clothingItems.Count > 1)
			{
				for (int i = 0; i < clothingItems.Count - 1; i++)
				{
					sms += clothingItems[i].GetHulqWord() + comma;
					selectedItemAudio.Add(clothingItems[i].GetClothingAudio());
				}
				sms += connector + space + dresserDeterminer + clothingItems[clothingItems.Count-1].GetHulqWord() + period;
				selectedItemAudio.Add(clothingItems[clothingItems.Count-1].GetClothingAudio());
			}
		}

		else
		{
			sms = hakwush + space + dresserDeterminer + ellipses;
		}
		return sms;
	}

	public List<string> GetSMSContent(string content)
    {
		List<string> smsContent = content.Split(" ").ToList();
		foreach(string w in smsContent)
        {
			w.Replace(", ", "").Replace(".", "");
        }
		return smsContent;
	}

	public void PlaySMSAud()
    {
		smsAud.GetComponent<Button>().interactable = false;
		List<string> smsContent = GetSMSContent(smsText);
		StartCoroutine(PlaySMS());
		
		IEnumerator PlaySMS()
        {
			int i = 0;
			foreach (string s in smsContent)
			{
				yield return new WaitUntil(() => !audioSource.isPlaying);

				if(templateAudio.Find(a => (s.Equals(a.name) || s == a.name)))
                {
					audioSource.PlayOneShot(templateAudio[templateAudio.FindIndex(a => (s.Equals(a.name) || s == a.name))]);
				}
				    
				else
				{
					audioSource.PlayOneShot(selectedItemAudio[i]);
					i++;
				}
			}
			yield return new WaitForSeconds(2);
			questionManagerScript.GoToResults();
			gameObject.SetActive(false);
		}
		smsAud.GetComponent<Button>().interactable = true;
	}

	public void EditSMSText(List<ClothingItem> clothingWords)
	{
		smsText = ConcatenateSMSText(clothingWords).Replace("\r", "");
		activeTexting.GetComponent<TextMeshProUGUI>().text = "";
		activeTexting.GetComponent<TextMeshProUGUI>().text = smsText;
	}

	public void SetSentText()
    {
		IPTextBox.SetActive(false);
		PlaySMSAud();
		sentTextBox.SetActive(true);
		sentText.GetComponent<TextMeshProUGUI>().text = smsText;
	}

	public void ResetSMS()
    {
		IPTextBox.SetActive(true);
		sentTextBox.SetActive(false);
		activeTexting.GetComponent<TextMeshProUGUI>().text = "";
		sentText.GetComponent<TextMeshProUGUI>().text = "";
	}
}