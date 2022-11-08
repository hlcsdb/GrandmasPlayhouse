using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClothingItem : MonoBehaviour
{

	//can't modify without setters or access without getters
	internal string sprite_filename;
	[SerializeField]
	private string file_gender;
	[SerializeField]
	private string item_name;
	private string clothing_gender;
	private string layer;
	private string category;
	private string requires;
	private string not_with;
	private string audio_filename;
	private string word_hulq;

  //  public void Start()
  //  {
		//Debug.Log("made a clothing item");
  //  }
    //SETTERS - only set within code
    internal void SetSpriteFilename(string spriteFilename)
	{
		sprite_filename = spriteFilename;

	}

	internal void SetFileGender(string fileGender)
	{
		file_gender = fileGender;
	}

	internal void SetItemName(string itemName)
	{
		item_name = itemName;
	}

	internal void SetClothingGender(string clothingGender)
	{
		clothing_gender = clothingGender;
	}

	internal void SetLayerNumber(string clothingLayer)
	{
		layer = clothingLayer;
	}

	internal void SetClothingCategory(string clothingCategory)
	{
		category = clothingCategory;
	}

	internal void SetRequiredPairing(string requiresItem)
	{
		requires = requiresItem;
	}

	internal void SetProhibitedPairing(string notWithItem)
	{
		not_with = notWithItem;
	}
	internal void SetAudioFilename(string audioFilename)
	{
		audio_filename = audioFilename;
	}

	internal void SetHulqWord(string hulqWord)
	{
		word_hulq = hulqWord;
	}

	// GETTERS - can view/get in inspector or code
	public string GetSpriteFilename()
	{
		return sprite_filename;
	}

	//Return the image associated with the image filename... return sprite??
	public Sprite GetSprite()
	{
		return Resources.Load<Sprite>(GetSpriteFilename());
	}

	public string GetFileGender()
	{
		return file_gender;
	}

	public string GetItemName()
	{
		return item_name;
	}

	public int GetLayerNumber()
	{
		return int.Parse(layer);
	}
	public string GetRequiredPairing()
	{
		return requires;
	}

	public string GetProhibitedPairing()
	{
		return not_with;
	}

	public string GetAudioFilename()
	{
		return audio_filename;
	}

	public AudioClip GetClothingAudio()
	{
		return Resources.Load<AudioClip>(GetAudioFilename());
	}

	public string GetHulqWord()
	{
		return word_hulq;
	}

	public string GetClothingGender()
	{
		return clothing_gender;
	}

	public string GetCategory()
	{
		return category;
	}
}

