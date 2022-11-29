using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.UI;
using UnityEngine.UI;

public class ClothingContainer : MonoBehaviour
{
    //public Sprite currSprite;
    public Image containerImage;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    internal void ChangeSprite(Sprite spritename)
    {
        //currSprite = spritename;
        containerImage.sprite = spritename;
    }
}
