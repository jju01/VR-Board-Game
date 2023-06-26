using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandUI : MonoBehaviour
{
    public GameObject iteminventory;


    void Update()
    {
        if (gameObject.activeSelf == true)
        {
            StartCoroutine(IslandUIactive());
            iteminventory.gameObject.SetActive(false);
        }
        else iteminventory.gameObject.SetActive(true);

    }
    IEnumerator IslandUIactive()
    {
        yield return new WaitForSeconds(30);
        gameObject.SetActive(false);
    }
}
