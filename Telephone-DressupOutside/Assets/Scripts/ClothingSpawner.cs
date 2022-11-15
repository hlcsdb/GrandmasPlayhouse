using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ClothingSpawner : MonoBehaviour
{
	public GameObject clothingImageContainer; //set to the correct dimensions as per the layer artboards (156x, 399.1y)
	public QuestionManager questionManager;
	public GameObject[] peoplePrefabs = new GameObject[2]; //prefab [male, female]
	public GameObject[] peoplePositions = new GameObject[4]; //[model container, dresser container]
	internal ModelClothingRandomizer clothingRandomizerScript;

	void Start()
	{
		clothingRandomizerScript = GetComponent<ModelClothingRandomizer>();

	}

	public void SetPeople(int model, int dresser, List<ClothingItem> modelClothingList)
	{ //sort out where model/dresser go based on male/female. also right now this is a co-ed game, no same gender. issue?
	  //(0 male = model, 1 female = dresser)
		GameObject modelPrefab = Instantiate(peoplePrefabs[model], peoplePositions[0].transform.position, Quaternion.identity);
		modelPrefab.transform.SetParent(peoplePositions[0].transform);
		modelPrefab.transform.localScale = new Vector3(1, 1, 1);
		GameObject dresserPrefab = Instantiate(peoplePrefabs[dresser], peoplePositions[1].transform.position, Quaternion.identity);
		dresserPrefab.transform.SetParent(peoplePositions[1].transform);
		dresserPrefab.transform.localScale = new Vector3(1, 1, 1);

		Debug.Log(modelClothingList.Count);
		clothingRandomizerScript.DressModel(modelClothingList); //should this be moved into the clothing spawner script?
		StartCoroutine(StartDressing());
		IEnumerator StartDressing()
        {
			yield return new WaitUntil(() => clothingRandomizerScript.modelClothingGenerated);
			questionManager.modelClothing = clothingRandomizerScript.modelClothing;
			DressModel(questionManager.modelClothing);
        }
	}

	public void DressModel(List<ClothingItem> modelClothing)
	{
		List<ClothingItem> orderedClothing = RelayerClothes(modelClothing);
		foreach (ClothingItem item in orderedClothing)
		{
			SpawnClothingLayer(item, peoplePositions[0]);
		}
		questionManager.SetOptions();
	}

	public void DressDresser(List<ClothingItem> selectedItems)
	{
		List<ClothingItem> orderedClothing = RelayerClothes(selectedItems);
		foreach (ClothingItem item in orderedClothing)
		{
			SpawnClothingLayer(item, peoplePositions[1]);
		}
		MovePeopleOnResult();
	}

	public List<ClothingItem> RelayerClothes(List<ClothingItem> clothesToWear)
	{
		List<ClothingItem> orderedItems = new List<ClothingItem>();
		orderedItems = clothesToWear.OrderBy(c => c.GetCategory()).ToList();
		return orderedItems;
    }

	public void SpawnClothingLayer(ClothingItem item, GameObject person)
	{
        GameObject clothingLayer = Instantiate(clothingImageContainer, person.transform.position, Quaternion.identity);
		clothingLayer.name = item.GetItemName();
		clothingLayer.GetComponent<Image>().sprite = item.GetSprite();
        clothingLayer.transform.SetParent(person.transform.GetChild(0));
		clothingLayer.transform.localPosition = Vector3.zero;
		clothingLayer.transform.localScale = new Vector3(1, 1, 1);
	}

	public void MovePeopleOnResult()
    {
		peoplePositions[0].transform.GetChild(0).SetParent(peoplePositions[2].transform);
		peoplePositions[2].transform.GetChild(0).localPosition = Vector3.zero;
		peoplePositions[1].transform.GetChild(0).SetParent(peoplePositions[3].transform);
		peoplePositions[3].transform.GetChild(0).localPosition = Vector3.zero;
	}

	public void ReturnPeopleToStart()
	{
		peoplePositions[2].transform.GetChild(0).SetParent(peoplePositions[0].transform);
		peoplePositions[0].transform.GetChild(0).localPosition = Vector3.zero;
		peoplePositions[3].transform.GetChild(0).SetParent(peoplePositions[1].transform);
		peoplePositions[1].transform.GetChild(0).localPosition = Vector3.zero;
	}

	public void ResetPeople()
    {
		clothingRandomizerScript.ResetClothingRandomizer();
		ReturnPeopleToStart();
		foreach (Transform child in peoplePositions[0].transform)
		{
			Destroy(child.gameObject);
		}
		foreach (Transform child in peoplePositions[1].transform)
		{
			Destroy(child.gameObject);
		}

	}

}