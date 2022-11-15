using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ModelClothingRandomizer : MonoBehaviour
{
	internal List<ClothingItem> modelClothing = new List<ClothingItem>();
	internal bool modelClothingGenerated = false;

	public void DressModel(List<ClothingItem> modelClothingList)
	{
		if (modelClothing.Count > 0) { modelClothingList.Clear(); }
		Debug.Log("num clothes in model clothing list" + modelClothingList.Count);
		//SETUP
		//Debug.Log("starting dress function in question manager");
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
		if (additionFootwear != null)
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
		if (optionAccessory != null)
		{
			modelClothing.Add(optionAccessory);
			tempAllItems.Remove(optionAccessory);
			//Debug.Log("optional accessory: " + modelClothing[modelClothing.Count - 1].GetItemName());

		}
		//the model now has all required clothing
		modelClothingGenerated = true;
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

	public void ResetClothingRandomizer()
    {
		modelClothing.Clear();
		modelClothingGenerated = false;
	}
}
