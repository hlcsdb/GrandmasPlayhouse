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
	public GameObject[] peoplePositions = new GameObject[2]; //[model container, dresser container]

	int siblingIndex;

	void Start()
	{

	}

	public void SetPeople(int model, int dresser)
	{ //sort out where model/dresser go based on male/female. also right now this is a co-ed game, no same gender. issue?
	  //(0 male = model, 1 female = dresser)
		Instantiate(peoplePrefabs[model], peoplePositions[model].transform.position, Quaternion.identity);

		peoplePrefabs[0] = peoplePositions[model]; //(0,1) (male, female) 
		peoplePrefabs[0].transform.SetParent(peoplePositions[model].transform);
		peoplePrefabs[1] = peoplePositions[dresser];
		peoplePrefabs[1].transform.SetParent(peoplePositions[dresser].transform);
	}

	public void DressModel(List<ClothingItem> modelClothing)
	{
		foreach (ClothingItem item in modelClothing)
		{
			SpawnClothingLayer(item.GetSprite(), item.GetLayerNumber(), peoplePositions[0]);
		}
	}

	public void DressDresser(List<ClothingItem> selectedItems)
	{
		foreach (ClothingItem item in selectedItems)
		{
			SpawnClothingLayer(item.GetSprite(), item.GetLayerNumber(), peoplePositions[1]);
		}
	}

	public void SpawnClothingLayer(Sprite clothingItemSprite, int clothingLayerNum, GameObject person)
	{
		GameObject clothingLayer = Instantiate(clothingImageContainer, person.transform.localPosition, Quaternion.identity);
		clothingLayer.transform.SetParent(person.transform);
		transform.SetSiblingIndex(clothingLayerNum); //this may pose an issue if there's no socks
		clothingLayer.GetComponent<Image>().sprite = clothingItemSprite;
	}

	public void ClearClothingFromPeople()
	{ //this may pose an issue since clothes are now children of model and dresser GOs
		foreach (Transform child in transform)
		{
			Destroy(child.gameObject); //will probably have to fix this for lost position objects
		}
	}
}