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
	internal bool inQuestion = false;

	public GameObject optionButtons; //attach parent object containing all of the answer option buttons
	internal OptionButtons optionButtonsScript;

	//CLOTHING OBJECT DATA
	internal LoadTextData clothingDataLoader;
	internal List<ClothingItem> modelClothingList;
	internal List<ClothingItem> dresserClothingList;
	internal List<string> namesOfItems;
	internal List<ClothingItem> clothingOptions;
	internal List<ClothingItem> modelClothing;
	internal List<ClothingItem> selectedItems;
	internal List<string> optionWords;
	internal List<string> selectedItemWords;

	public GameObject SMSController;
	internal SMSController SMSControllerScript;
	public ClothingSpawner clothingSpawner;


	void Start()
	{
		optionButtonsScript = optionButtons.GetComponent<OptionButtons>();
		clothingDataLoader = GetComponent<LoadTextData>();
		namesOfItems = clothingDataLoader.GetItemNames();
		SMSControllerScript = SMSController.GetComponent<SMSController>();
	}

	public void SetQuestion()
	{
		//there's a lot going on in this function, and each line requires the previous function to be completed. if there are issues, check first how this runs.
		SetRoles();
		DressModel();
		SetOptions();
		inQuestion = true;
		SMSControllerScript.StartSMS();
	}

	void SetRoles()
	{
		modelGender = people[UnityEngine.Random.Range(0, 2)];
		if (modelGender == "male")
		{
			dresserGender = "female";
			clothingSpawner.SetPeople(0, 1); //(0 male = model, 1 female = dresser)
		}
		else
		{
			dresserGender = "male";
			clothingSpawner.SetPeople(1, 0);
		}
	}

	void SetPeopleClothingList(string modelGender)
	{
		if (modelGender == "male")
		{
			modelClothingList = clothingDataLoader.GetMaleClothingItems();
			dresserClothingList = clothingDataLoader.GetFemaleClothingItems();
		}
		else
		{
			modelClothingList = clothingDataLoader.GetFemaleClothingItems();
			dresserClothingList = clothingDataLoader.GetMaleClothingItems();
		}
	}

	void DressModel()
	{
		//populate modelClothing list with random items
		List<ClothingItem> tempAllItems = modelClothingList; //duplicate list of possible options

		modelClothing.Add(GetRandomItemFromLayerList(1, tempAllItems)); //random top (shirt) //no dependencies
		modelClothing.Add(GetRandomItemFromLayerList(2, tempAllItems)); //random bottom (pants)//no dependencies

		ClothingItem randFootwear = GetRandomItemFromLayerList(3, tempAllItems); //random footwear// required additions
		modelClothing = CheckRequiredPairing(randFootwear, modelClothing, tempAllItems);

		ClothingItem randJacket = GetRandomItemFromLayerList(4, tempAllItems); //random jacket // requires check prohibited eg shirt
		modelClothing = CheckProhibitedConditions(randJacket, modelClothing);

		ClothingItem randAccessory = GetRandomItemFromLayerList(5, tempAllItems); //random accessory // add one if wanted and required pairing exists
		modelClothing = AddOptionalAccessory(randAccessory, modelClothing, tempAllItems);

		//the model now has all required clothing
		clothingSpawner.DressModel(modelClothing);
	}

	ClothingItem GetRandomItemFromLayerList(int layerNumber, List<ClothingItem> remainingItems)
	{
		List<ClothingItem> layer = remainingItems.Where(x => x.GetLayerNumber() == layerNumber).ToList();
		int randItemIndex = UnityEngine.Random.Range(0, layer.Count);
		return layer[randItemIndex];
	}

	List<ClothingItem> CheckRequiredPairing(ClothingItem item, List<ClothingItem> clothesSoFar, List<ClothingItem> modelClothingOptions)
	{
		//something might be wrong with this... line 15 variable
		if (item.GetRequiredPairing() != null)
		{
			clothesSoFar.Add(modelClothingOptions.Find(c => c.GetItemName() == item.GetRequiredPairing()));
		}
		return clothesSoFar;
	}

	List<ClothingItem> CheckProhibitedConditions(ClothingItem item, List<ClothingItem> clothesSoFar)
	{
		if (item.GetProhibitedPairing() != null)
		{
			clothesSoFar.Remove(clothesSoFar.Find(c => c.GetItemName() == item.GetProhibitedPairing()));
		}
		return clothesSoFar;
	}

	List<ClothingItem> AddOptionalAccessory(ClothingItem item, List<ClothingItem> clothesSoFar, List<ClothingItem> modelClothingOptions)
	{
		string requiredPairing = item.GetRequiredPairing();
		if (clothesSoFar.Find(c => c.GetItemName() == requiredPairing) != null)
		{ //if an accessory's required item is already on the model, decide if we even want it
            int random = Random.Range(0, 2);
			if (random == 0) //allow accessory
			{
				clothesSoFar.Add(clothesSoFar.Find(c => c.GetItemName() == requiredPairing));
			}
		}
		return clothesSoFar;
	}


	void SetOptions()
	{
		//set list with names of items active on model with modelClothingList. choose other words at random until list count = 8
		//Generates a list of words of clothing items optionWords and sends it to OptionButtons script to populate the button texts.
		string[] unshuffledItems = new string[8];
		List<string> tempItems = namesOfItems;

		//int randIndex;
		string itemHulqWord;
		//fills options with correct answers

		for (int i = 0; i < modelClothing.Count; i++)
		{
			itemHulqWord = modelClothing[i].GetHulqWord();
			unshuffledItems[i] = itemHulqWord;
			tempItems.Remove(itemHulqWord);
		}

        //fills options with incorrect answers
//UNCOMMENT
        //for (int i = modelClothingList.Count; i < 8; i++)
        //{
        //	int randItem = Random.Range(0, namesOfItems.Count); ;//choose random item from an genderallitemlist
        //	unshuffledItems[i] = tempItems[randItem];
        //	tempItems.Remove(randItem);
        //}

        ////sends a shuffled list to set buttons
        //ShuffleArray(unshuffledItems);
        //optionButtonsScript.SetOptionButtons(clothingOptions);
    }

	//UNCOMMENT
 //   void ShuffleArray(string[] unshuffledItems)
	//{

	//	random = new Random();
	//	unshuffledItems = arr.OrderBy(x => random.Next()).ToArray();
	//	foreach (string item in unshuffledItems)
	//	{
	//		clothingOptions.Add(item);
	//	}
	//}

	internal void SetSelectedItemWords(bool selected, string itemName)
	{
		//Sent to SMSConainerScript

		if (selected)
		{
			selectedItemWords.Add(itemName);
			selectedItems.Add(dresserClothingList.First(item => itemName == item.GetHulqWord()));
		}
		else
		{
			selectedItemWords.Remove(itemName);
			selectedItems.Remove(dresserClothingList.First(item => itemName == item.GetHulqWord()));
		}

		SMSControllerScript.EditSMSText(selectedItemWords);
	}

	internal void SubmitAnswers()
	{
		SMSControllerScript.SetSentText();
		clothingSpawner.DressDresser(selectedItems);
		optionButtonsScript.HighlightAnswers(modelClothingList);
	}

	internal void ResetQuestion()
	{
		clothingOptions.Clear();
		clothingSpawner.ClearClothingFromPeople();
	}
}

