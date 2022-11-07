using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LoadTextData : MonoBehaviour
{
    //from DAYS OF THE WEEK Game
    //THIS IS BEING USED IN THE TELEPHONE CLOTHING GAME

    //Attach this script to the GameManager object in the hierarchy window

    public TextAsset clothingData; //import csv to assets [as of Nov 5, called telephone_ClothingObjects.csv], convert to TextAsset, drop the csv as a text asset into this var slot in the CSVReader script in ClothingSpawner object.
    public int numItems; //set in inspector to 23 as of Nov 5

    [SerializeField]
    private List<ClothingItem> clothingItems; //holds the list of clothing data as clothing objects
    private List<ClothingItem> maleClothingItems;
    private List<ClothingItem> femaleClothingItems;
    private List<String> itemNames;

    internal bool loaded = false;

    void Start()
    {
        ClothingReader();
        SetMaleClothingItems();
        SetFemaleClothingItems();
    }

    public void ClothingReader()
    {
        string[] data = clothingData.text.Split(new string[] { ",", "\n" }, System.StringSplitOptions.None);
        //Debug.Log(data.Length);
        int numCols = data.Length / numItems;

        ClothingItem[] items = new ClothingItem[numItems];

        for (int i = 1; i < numItems; i++) //starting at i = 1 to skip first row of textasset. this should work...?
        {
            clothingItems.Add(gameObject.AddComponent<ClothingItem>());
            clothingItems[i].SetSpriteFilename("TelephoneClothingImages/" + data[numItems * i]);
            clothingItems[i].SetFileGender(data[numItems * i + 1]);
            clothingItems[i].SetItemName(data[numItems * i + 2]);
            clothingItems[i].SetClothingGender(data[numItems * i + 3]);
            clothingItems[i].SetLayerNumber(data[numItems * i + 4]);
            clothingItems[i].SetClothingCategory(data[numItems * i + 5]);
            clothingItems[i].SetRequiredPairing(data[numItems * i + 6]);
            clothingItems[i].SetProhibitedPairing(data[numItems * i + 7]);
            clothingItems[i].SetAudioFilename("TelephoneClothingAudio/" + data[numItems * i + 8]);
            clothingItems[i].SetHulqWord(data[numItems * i + 9]);

            Debug.Log(clothingItems[i].GetItemName());
        }
        Debug.Log(clothingItems.Count);
        clothingItems = items.ToList();
        loaded = true;
    }

    // internal List<ClothingItem>GetAllClothingItems(){
    //     if(clothingItems.Count == 0) {Debug.Log("clothing items not loaded"); return null;}
    //     else{return clothingItems();}
    // }

    internal void SetMaleClothingItems()
    {
        if (clothingItems.Count != 0)
        {
            for (int i = 0; i < clothingItems.Count; i++)
            {
                if (clothingItems[i].GetFileGender() == "male")
                {
                    maleClothingItems.Add(clothingItems[i]);
                }
            }
            Debug.Log("clothing items not loaded");
        }
    }

    internal void SetFemaleClothingItems()
    {
        if (clothingItems.Count != 0)
        {
            for (int i = 0; i < clothingItems.Count; i++)
            {
                if (clothingItems[i].GetFileGender() == "female")
                {
                    femaleClothingItems.Add(clothingItems[i]);
                }
            }
        }
    }

    internal List<ClothingItem> GetFemaleClothingItems()
    {
        return femaleClothingItems;
    }

    internal List<ClothingItem> GetMaleClothingItems()
    {
        return maleClothingItems;
    }

    internal void SetItemNames()
    {
        foreach (ClothingItem item in maleClothingItems)
        {
            itemNames.Add(item.GetHulqWord());
        }
    }

    internal List<String> GetItemNames()
    {
        return itemNames;
    }
}