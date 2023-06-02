using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorButton : MonoBehaviour
{
    Button myBtn;
    Image img;
    UiCustomManager uiCustom;
    

    // Start is called before the first frame update
    void Start()
    {
        myBtn = GetComponent<Button>();
        myBtn.onClick.AddListener(ChangeColor);

        img = GetComponent<Image>();

        uiCustom = FindObjectOfType<UiCustomManager>();
    }
     
    public void ChangeColor()
    {
        GameObject palyer = NetworkManager.Instance.myPlayer;
        palyer.GetComponent<Player>().SetColor(img.color);
    }
}
