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
    internal List<ClothingItem> clothingItems = new List<ClothingItem>(); //holds the list of clothing data as clothing objects
    internal List<ClothingItem> maleClothing = new List<ClothingItem>();
    internal List<ClothingItem> femaleClothing = new List<ClothingItem>();

    internal bool loaded = false;

    void Start()
    {
        //ClothingReader();
    }

    public void ClothingReader()
    {
        string[] data = clothingData.text.Split(new string[] { ",", "\n" }, System.StringSplitOptions.None);
        //Debug.Log(data.Length);
        int numCols = data.Length / numItems;
        //Debug.Log("num items:" + numItems + ";  num cols: " + numCols);
        //Debug.Log("first item: " + data[numCols * 1]);
        //ClothingItem[] items = new ClothingItem[numItems];

        for (int i = 0; i < numItems; i++) //starting at i = 0 because the index of the list needs to start at 0
        {
            clothingItems.Add(gameObject.AddComponent<ClothingItem>());
            clothingItems[i].SetSpriteFilename("TelephoneClothingImages/" + data[numCols * (i+1)]);
            clothingItems[i].SetFileGender(data[numCols * (i+1) + 1]);
            clothingItems[i].SetItemName(data[numCols * (i + 1) + 2]);
            clothingItems[i].SetClothingGender(data[numCols * (i + 1) + 3]);
            clothingItems[i].SetLayerNumber(data[numCols * (i + 1) + 4]);
            clothingItems[i].SetClothingCategory(data[numCols * (i + 1) + 5]);
            clothingItems[i].SetRequiredPairing(data[numCols * (i + 1) + 6]);
            clothingItems[i].SetProhibitedPairing(data[numCols * (i + 1) + 7]);
            clothingItems[i].SetAudioFilename("TelephoneClothingAudio/" + data[numCols * (i + 1) + 8]);
            clothingItems[i].SetHulqWord(data[numCols * (i + 1) + 9]);

            //Debug.Log(clothingItems[i].GetItemName());
        }

        StartCoroutine(LoadByGender());
        IEnumerator LoadByGender(){
            yield return new WaitUntil(() => clothingItems.Count == numItems);
            maleClothing = GetMaleClothingItems();
            femaleClothing = GetFemaleClothingItems();
            yield return new WaitUntil(() => (maleClothing.Count + femaleClothing.Count) == numItems);
            loaded = true;
        }
        //Debug.Log(clothingItems.Count);
        //clothingItems = items.ToList();
    }

    // internal List<ClothingItem>GetAllClothingItems(){
    //     if(clothingItems.Count == 0) {Debug.Log("clothing items not loaded"); return null;}
    //     else{return clothingItems();}
    // }

    internal List<ClothingItem> GetMaleClothingItems()
    {
        List<ClothingItem> maleClothingItems = new List<ClothingItem>();
        List<ClothingItem> tempAllItems = clothingItems;
        //Debug.Log("All items in male: " + tempAllItems.Count);

        if (tempAllItems.Count != 0)
        {
            //Debug.Log(tempAllItems[0].GetItemName());
            for (int i = 0; i < tempAllItems.Count; i++)
            {
                if (tempAllItems[i].GetFileGender() == "boy")
                {
                    maleClothingItems.Add(tempAllItems[i]);
                }
            }
            //Debug.Log("clothing items not loaded");
        }
        return maleClothingItems;
    }

    internal List<ClothingItem> GetFemaleClothingItems()
    {
        List<ClothingItem> femaleClothingItems = new List<ClothingItem>();
        List<ClothingItem> tempAllItems = clothingItems;

        //Debug.Log("All items in female: " + tempAllItems.Count);

        if (tempAllItems.Count != 0)
        {
            //Debug.Log(tempAllItems[0].GetItemName());
            for (int i = 0; i < tempAllItems.Count; i++)
            {
                if (tempAllItems[i].GetFileGender() == "girl")
                {
                    femaleClothingItems.Add(tempAllItems[i]);
                }
            }
        }
        return femaleClothingItems;
    }

  
    internal List<string> GetItemNames()
    {
        List<ClothingItem> clothingItems = GetFemaleClothingItems();
        List<string> itemNames = new List<string>();
        foreach (ClothingItem item in clothingItems)
        {
            itemNames.Add(item.GetHulqWord());
        }
        return itemNames;
    }
}