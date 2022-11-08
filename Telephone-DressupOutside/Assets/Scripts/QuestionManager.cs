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
	internal List<ClothingItem> clothingOptions = new List<ClothingItem>();
	internal List<ClothingItem> modelClothing = new List<ClothingItem>();
	internal List<ClothingItem> selectedItems = new List<ClothingItem>();
	internal List<string> optionWords;
	internal List<string> selectedItemWords = new List<string>();

	public GameObject SMSController;
	internal SMSController SMSControllerScript;
	public ClothingSpawner clothingSpawner;


	void Start()
	{
		optionButtonsScript = optionButtons.GetComponent<OptionButtons>();
		clothingDataLoader = GetComponent<LoadTextData>();
		clothingDataLoader.ClothingReader();
		SMSControllerScript = SMSController.GetComponent<SMSController>();
	}

	public void SetQuestion()
	{
		
		//there's a lot going on in this function, and each line requires the previous function to be completed. if there are issues, check first how this runs.
		
		StartCoroutine(PreparePeople());
			IEnumerator PreparePeople()
			{
			bool rolesSet = false;
			bool modelDressed = false;
			rolesSet = SetRoles();
			yield return new WaitUntil(() => rolesSet);
			modelDressed = DressModel();
			yield return new WaitUntil(() => modelDressed);
			Debug.Log("finished picking clothes");
			clothingSpawner.DressModel(modelClothing);
			SMSControllerScript.StartSMS();
			}
		inQuestion = true;
	}

	bool SetRoles()
	{
		modelGender = people[UnityEngine.Random.Range(0, 2)];
		if (modelGender == "boy")
		{
			dresserGender = "girl";
			clothingSpawner.SetPeople(0, 1); //(0 male = model, 1 female = dresser)
			modelClothingList = clothingDataLoader.GetMaleClothingItems();
			//Debug.Log(modelClothingList.Count);
			dresserClothingList = clothingDataLoader.GetFemaleClothingItems();
			//Debug.Log(dresserClothingList.Count);
			return true;
		}
		else
		{
			dresserGender = "boy";
			clothingSpawner.SetPeople(1, 0);
			modelClothingList = clothingDataLoader.GetFemaleClothingItems();
			dresserClothingList = clothingDataLoader.GetMaleClothingItems();
			return true;
		}
		//return true;
	}

	bool DressModel()
	{
		//SETUP
		Debug.Log("starting dress function in question manager");
		//populate modelClothing list with random items
		List<ClothingItem> tempAllItems = modelClothingList; //duplicate list of possible options
		//Debug.Log("row 88 temp items: " + tempAllItems.Count);

		//PANTS
		ClothingItem layer1Item = GetRandomItemFromLayerList(1, tempAllItems);
		modelClothing.Add(layer1Item); //random pants //no dependencies
		tempAllItems.Remove(layer1Item);
		//Debug.Log("bottoms: "+ modelClothing[0].GetItemName());

		//FOOTWEAR
		ClothingItem randFootwear = GetRandomItemFromLayerList(2, tempAllItems); //random footwear// required additions
		tempAllItems.Remove(randFootwear);

		ClothingItem additionFootwear = CheckRequiredPairing(randFootwear, modelClothing, tempAllItems);
		if(additionFootwear != null)
        {
			modelClothing.Add(additionFootwear);
			tempAllItems.Remove(additionFootwear);
			//Debug.Log("required addition to footwear: " + modelClothing[modelClothing.Count-1].GetItemName());
		}
		modelClothing.Add(randFootwear);
		//Debug.Log("footwear: " + modelClothing[modelClothing.Count - 1].GetItemName());

		//TOP/JACKET
		ClothingItem randTop = GetRandomItemFromLayerList(3, tempAllItems); //random jacket // requires check prohibited eg shirt
		tempAllItems.Remove(randTop);

		ClothingItem additionTop = CheckRequiredPairing(randTop, modelClothing, tempAllItems);
		if (additionTop != null)
		{
			modelClothing.Add(additionTop);
			tempAllItems.Remove(additionTop);
			//Debug.Log("required addition to top: " + modelClothing[modelClothing.Count - 1].GetItemName());
		}
		modelClothing.Add(randTop);
		//Debug.Log("top: " + modelClothing[modelClothing.Count - 1].GetItemName());


		ClothingItem optionAccessory = AddOptionalAccessory(modelClothing, tempAllItems);
		if(optionAccessory != null)
        {
			modelClothing.Add(optionAccessory);
			tempAllItems.Remove(optionAccessory);
			//Debug.Log("optional accessory: " + modelClothing[modelClothing.Count - 1].GetItemName());

		}
		//the model now has all required clothing
		return true;
	}

	ClothingItem GetRandomItemFromLayerList(int layerNumber, List<ClothingItem> remainingItems)
	{
		//Debug.Log(remainingItems.Count);
		List<ClothingItem> layer = remainingItems.Where(x => x.GetLayerNumber() == layerNumber).ToList();
		int randItemIndex = UnityEngine.Random.Range(0, layer.Count);
		return layer[randItemIndex];
	}

	ClothingItem CheckRequiredPairing(ClothingItem item, List<ClothingItem> clothesSoFar, List<ClothingItem> modelClothingOptions)
	{
		//something might be wrong with this... line 15 variable
		if (item.GetRequiredPairing() != null)
		{
			//clothesSoFar.Add(modelClothingOptions.Find(c => c.GetItemName() == item.GetRequiredPairing()));
		return modelClothingOptions.Find(c => c.GetItemName() == item.GetRequiredPairing());
		}
		return null;
	}

	ClothingItem AddOptionalAccessory(List<ClothingItem> clothesSoFar, List<ClothingItem> modelClothingOptions)
	{
		//choose a random accessory
		ClothingItem randomAccessory = GetRandomItemFromLayerList(4, modelClothingOptions);
		string prohibitedPairing = randomAccessory.GetProhibitedPairing(); //get name of any prohibited pairing, eg. gloves not with tshirt
		//If there is no prohibited pairing, and we randomly choose to add that accessory, return it. 
		if (prohibitedPairing == "" && WantAccessory())
        {
			return randomAccessory;
        }

		//if there is a prohibited pairing, look for it in the list of clothes to be worn
		if (clothesSoFar.Find(c => c.GetItemName() == prohibitedPairing))
		{
			return null; //if it is, return nothing. No accessory will be added
		}

		//if the prohibited item isn't already to be worn, check to see if we want to add it
		else
		{
			if (WantAccessory())
			{
				return randomAccessory;
			}
		}
		return null;
	}


	bool WantAccessory()
	{
		int random = Random.Range(0, 2);
		if (random == 0) //allow accessory
		{
		return true;
		}
		return false;
	}

	internal void SetOptions()
	{
		//set list with names of items active on model with modelClothingList. choose other words at random until list count = 8
		//Generates a list of words of clothing items optionWords and sends it to OptionButtons script to populate the button texts.
		List<string> unshuffledItems = new List<string>();
		List<string> namesOfItems = clothingDataLoader.GetItemNames();

		string itemHulqWord;
		//fills options with correct answers
		for (int i = 0; i < modelClothing.Count; i++)
		{
			itemHulqWord = modelClothing[i].GetHulqWord();
			unshuffledItems.Add(itemHulqWord);
			namesOfItems.Remove(itemHulqWord);
		}

		int randItem;
        //fills options with incorrect answers
        //UNCOMMENT
        for (int i = modelClothing.Count; i < 8; i++)
        {
            randItem = Random.Range(0, namesOfItems.Count); ;//choose random item from an genderallitemlist
			itemHulqWord = namesOfItems[randItem];
            unshuffledItems.Add(itemHulqWord);
            namesOfItems.Remove(itemHulqWord);
        }

        //sends a shuffled list to set buttons
        
		StartCoroutine(PopulateOptionButtons());
		IEnumerator PopulateOptionButtons(){
			optionWords = ShuffleArray(unshuffledItems);
			yield return new WaitUntil(() => optionWords.Count == 8);
			optionButtonsScript.SetOptionButtons(optionWords);

		}
		Debug.Log("number of options: " + optionWords.Count);
    }

    //UNCOMMENT
    List<string> ShuffleArray(List<string> unshuffledItems)
    {
		List<string> shuffledItems = new List<string>();
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

	public void SubmitAnswers()
	{
		SMSControllerScript.SetSentText();
		clothingSpawner.DressDresser(selectedItems);
		optionButtonsScript.HighlightAnswers(modelClothingList);

	}

	internal void ResetQuestion()
	{
		clothingSpawner.ResetPeople();
		clothingOptions.Clear();
		SetOptions();
	}
}

