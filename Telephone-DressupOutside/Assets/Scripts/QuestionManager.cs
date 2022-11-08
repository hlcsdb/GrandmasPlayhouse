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
	public GameObject resultsScreen;

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
	internal List<ClothingItem> optionItems = new List<ClothingItem>();
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

    internal void SetSelectedItems(bool isSelected, ClothingItem clothingItem)
    {
        throw new NotImplementedException();
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
        //UNCOMMENT
        for (int i = modelClothing.Count; i < 8; i++)
        {
            randItem = Random.Range(0, listAllItems.Count); ;//choose random item from an genderallitemlist
            unshuffledItems.Add(listAllItems[randItem]);
			listAllItems.Remove(listAllItems[randItem]);
        }

        //sends a shuffled list to set buttons
        
		StartCoroutine(PopulateOptionButtons());

		IEnumerator PopulateOptionButtons(){
			optionItems = ShuffleClothingOptions(unshuffledItems);
			yield return new WaitUntil(() => optionItems.Count == 8);
			optionButtonsScript.SetOptionButtons(optionItems);

		}
		Debug.Log("number of options: " + optionItems.Count);
    }

    //UNCOMMENT
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
		SMSControllerScript.SetSentText();
	}

    internal void GoToResults()
    {
		StartCoroutine(AfterSMSSent());
		IEnumerator AfterSMSSent()
		{
			yield return new WaitForSeconds(0.7f);
			yield return new WaitUntil(() => !optionButtons.GetComponent<AudioSource>().isPlaying);
			clothingSpawner.DressDresser(selectedItems);
			optionButtonsScript.HighlightAnswers(modelClothing);
			resultsScreen.SetActive(true);
			GameObject.Find("Questions Screen").SetActive(false);
		}
	}

	internal void ResetQuestion()
	{
		clothingSpawner.ResetPeople();
		clothingOptions.Clear();
		SetOptions();
	}
}

