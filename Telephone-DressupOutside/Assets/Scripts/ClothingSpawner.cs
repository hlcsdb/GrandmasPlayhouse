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

	int siblingIndex;

	void Start()
	{

	}

	public void SetPeople(int model, int dresser)
	{ //sort out where model/dresser go based on male/female. also right now this is a co-ed game, no same gender. issue?
	  //(0 male = model, 1 female = dresser)
		GameObject modelPrefab = Instantiate(peoplePrefabs[model], peoplePositions[0].transform.position, Quaternion.identity);
		modelPrefab.transform.SetParent(peoplePositions[0].transform);

		GameObject dresserPrefab = Instantiate(peoplePrefabs[dresser], peoplePositions[1].transform.position, Quaternion.identity);
		dresserPrefab.transform.SetParent(peoplePositions[1].transform);
	}

	public void DressModel(List<ClothingItem> modelClothing)
	{
		List<ClothingItem> orderedClothing = RelayerClothes(modelClothing);
		Debug.Log("Dressing model in spawner");
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
			//Debug.Log(item.GetSpriteFilename());
			SpawnClothingLayer(item, peoplePositions[1]);
		}
		Debug.Log("finished dressing");
		MovePeopleOnResult();
	}

	public List<ClothingItem> RelayerClothes(List<ClothingItem> clothesToWear)
	{
		List<ClothingItem> orderedItems = new List<ClothingItem>();
		orderedItems = clothesToWear.OrderBy(c => c.GetLayerNumber()).ToList();
		return orderedItems;
    }

	public void SpawnClothingLayer(ClothingItem item, GameObject person)
	{
		Sprite itemSprite = item.GetSprite();
		Debug.Log(item.GetSpriteFilename());
        //transform.GetChild(0).GetComponent<Image>().sprite = clothingItemSprite;
        GameObject clothingLayer = Instantiate(clothingImageContainer, person.transform.position, Quaternion.identity);
		clothingLayer.name = item.GetItemName();
        clothingLayer.GetComponent<Image>().sprite = itemSprite;
        clothingLayer.transform.SetParent(person.transform);
	}

	public void MovePeopleOnResult()
    {
		peoplePositions[0].transform.position = peoplePositions[2].transform.position;
		peoplePositions[1].transform.position = peoplePositions[3].transform.position;
	}


    public void ResetPeople()
    {
		foreach (Transform child in peoplePositions[0].transform)
		{
			GameObject.Destroy(child.gameObject);
		}
		foreach (Transform child in peoplePositions[1].transform)
		{
			GameObject.Destroy(child.gameObject);
		}
	}
}