using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardSelector : MonoBehaviour
{
    [SerializeField] private Sprite[] cardSprites;
    private Image img;

    public void SetCard(int idx)
    {
        if(img == null)
            img = GetComponent<Image>();
        
        img.sprite = cardSprites[idx];
    }
}
