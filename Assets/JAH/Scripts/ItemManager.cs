using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

// 역할 1:  Item UI 관리
// 역할 2 : Item 과 Player가 부딪혔을 때 카운트 + 1
//              >> 카운트가 4가 되면 GameOver

// 역할 3: 트리거_1~3 ui 리스트로 관리

public class ItemManager : MonoBehaviour
{
    // Item UI 리스트
    public GameObject[] UIItems;

    // Trigger UI 리스트
    public GameObject[] TriggerUI;


    // Player 와 Item이 부딪혔을 때 카운트 
    public int count = 0;

    public void RefreshItemState()
    {
        for (int i = 0; i < UIItems.Length; i++)
        {
            string itemType = $"itemType_{i}";

            var playerProps = PhotonNetwork.LocalPlayer.CustomProperties; 
            int itemCount = 0;
            if (playerProps.ContainsKey(itemType))
                itemCount = (int)playerProps[itemType];

            UIItems[i].SetActive(itemCount != 0);
        }
    }
}
    
