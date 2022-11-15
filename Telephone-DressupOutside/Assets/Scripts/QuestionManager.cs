using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using Random = UnityEngine.Random;

//Create ClothingSpawner Object in hierarchy window. Attach this script.
public class QuestionManager : MonoBehaviour
{
	internal string[] people = new string[] { "boy", "girl" };
	internal string modelGender;
	internal string dresserGender;
	internal bool inQuestion = true;
	internal bool inResults = false;
	public GameObject resultsScreen;

	public GameObject optionButtons; //attach parent object containing all of the answer option buttons
	internal OptionButtons optionButtonsScript;

	//CLOTHING OBJECT DATA
	internal LoadTextData clothingDataLoader;
	internal List<ClothingItem> modelClothingList = new List<ClothingItem>();
	internal List<ClothingItem> modelClothing = new List<ClothingItem>();
	internal List<ClothingItem> selectedItems = new List<ClothingItem>();

	public GameObject smsController;
	internal SMSController SMSControllerScript;
	public ClothingSpawner clothingSpawner;

	void Start()
	{
		optionButtonsScript = optionButtons.GetComponent<OptionButtons>();
		clothingDataLoader = GetComponent<LoadTextData>();
		clothingDataLoader.ClothingReader();
		SMSControllerScript = smsController.GetComponent<SMSController>();
	}

	public void SetQuestion()
	{
		//there's a lot going on in this function, and each line requires the previous function to be completed. if there are issues, check first how this runs.
		StartCoroutine(PreparePeople());
			IEnumerator PreparePeople()
			{
			yield return new WaitUntil(() => clothingDataLoader.loaded);
			bool rolesSet = false; //do I need this anymore?
				//bool modelDressed = false; //do I need this anymore?
				rolesSet = SetRoles();
				yield return new WaitUntil(() => rolesSet);
				SMSControllerScript.StartSMS();
			}
		inQuestion = true;
	}

    bool SetRoles()
	{
		if (modelClothingList.Count > 0){modelClothingList.Clear();}

		modelGender = people[UnityEngine.Random.Range(0, 2)];
		Debug.Log(modelGender);
		if (modelGender == "boy")
		{
			dresserGender = "girl";
			modelClothingList = clothingDataLoader.maleClothing;
			if(modelClothingList.Count == 0) { modelClothingList = clothingDataLoader.GetMaleClothingItems(); }
			clothingSpawner.SetPeople(0, 1, modelClothingList); //(0 male = model, 1 female = dresser)
			return true;
		}
		else
		{
			dresserGender = "boy";
			modelClothingList = clothingDataLoader.femaleClothing;
			if (modelClothingList.Count == 0) { modelClothingList = clothingDataLoader.GetFemaleClothingItems(); }
			clothingSpawner.SetPeople(1, 0, modelClothingList);
			return true;
		}
	}


	internal void SetOptions()
	{
		//set list with names of items active on model with modelClothingList. choose other words at random until list count = 8
		//Generates a list of words of clothing items optionWords and sends it to OptionButtons script to populate the button texts.
		List<ClothingItem> unshuffledItems = new List<ClothingItem>();
		List<ClothingItem> listAllItems = modelClothingList;

		int randItem;
		//fills options with correct answers
		for (int i = 0; i < modelClothing.Count; i++)
		{
			unshuffledItems.Add(modelClothing[i]);
			listAllItems.Remove(modelClothing[i]);
		}

        //fills options with incorrect answers
        for (int i = modelClothing.Count; i < 8; i++)
        {
            randItem = Random.Range(0, listAllItems.Count); ;//choose random item from an genderallitemlist
            unshuffledItems.Add(listAllItems[randItem]);
			listAllItems.Remove(listAllItems[randItem]);
        }

		//shuffles a list and sends it to OptionButtonsScript to set buttons
		optionButtonsScript.SetOptionButtons(ShuffleClothingOptions(unshuffledItems));
    }

    List<ClothingItem> ShuffleClothingOptions(List<ClothingItem> unshuffledItems)
    {
		List<ClothingItem> shuffledItems = new List<ClothingItem>();

		int l = unshuffledItems.Count;
		int rand;

        for(int i=0; i<l; i++)
        {
			rand = Random.Range(0, unshuffledItems.Count);
			shuffledItems.Add(unshuffledItems[rand]);
			unshuffledItems.RemoveAt(rand);
        }
		return shuffledItems;
	}

	List<ClothingItem> GenerateDresserClothingSprites(List<ClothingItem> selectedItems)
    {
		List<ClothingItem> dresserClothingList = new List<ClothingItem>();
		if (dresserGender == "girl"){dresserClothingList = clothingDataLoader.GetFemaleClothingItems();}
        else{dresserClothingList = clothingDataLoader.GetMaleClothingItems();}

		List<ClothingItem> dresserClothingWear = new List<ClothingItem>();
		
		for (int i = 0; i < selectedItems.Count; i++)
        {
			dresserClothingWear.Add(dresserClothingList.Find(x => x.GetItemName() == selectedItems[i].GetItemName()));
		}
		return dresserClothingWear;
	}

    internal void SetSelectedItemWords(bool selected, ClothingItem selectedItem)
	{
		//Sent to SMSConainerScript

		if (selected)
		{
			selectedItems.Add(selectedItem);
		}
		else
		{
			selectedItems.Remove(selectedItem);
		}

		SMSControllerScript.EditSMSText(selectedItems);
	}

	public void SubmitAnswers()
	{
		inQuestion = false;
		SMSControllerScript.SetSentText();
	}

    internal void GoToResults()
    {
		StartCoroutine(AfterSMSSent());
		IEnumerator AfterSMSSent()
		{
			yield return new WaitForSeconds(0.7f);
			yield return new WaitUntil(() => !optionButtons.GetComponent<AudioSource>().isPlaying);
			clothingSpawner.DressDresser(GenerateDresserClothingSprites(selectedItems));
			optionButtonsScript.HighlightAnswers(modelClothing);
			resultsScreen.SetActive(true);
			SetScore();
			GameObject.Find("Questions Screen").SetActive(false);
			inResults = true;
		}
	}

	public void ResetQuestion()
	{
		inResults = false;
		modelClothingList.Clear();
		modelClothing.Clear();
		selectedItems.Clear();

		SMSControllerScript.ResetSMS();
		clothingSpawner.ResetPeople();
		optionButtonsScript.ResetQBs();
		SetQuestion();
	}

	internal void SetScore()
    {
		int score = modelClothing.Where(x => selectedItems.Contains(x)).Count();
		Debug.Log("Score: " + score + " / " + modelClothing.Count);
		GameObject.Find("Score Text").GetComponent<TextMeshProUGUI>().text = "Score: " + score + " / " + modelClothing.Count;
	}
}

