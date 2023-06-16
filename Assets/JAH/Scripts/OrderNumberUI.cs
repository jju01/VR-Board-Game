using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using Hashtable = ExitGames.Client.Photon.Hashtable;

// 역할 : 카드(버튼)을 누르면 카드가 사라지고 주사위가 나온다 (이벤트 호출)

public class OrderNumberUI : MonoBehaviour
{
    // 버튼
    public Button[] cardBtns;
    // 랜덤 숫자 번호
    int rand;

    // Start is called before the first frame update
    void Start()
    {
        CardEvent();
    }

   public void CardEvent()
    {
        for (int i = 0; i < cardBtns.Length; i++)
        {
            int idx = i;
            cardBtns[i].onClick.RemoveAllListeners();
            cardBtns[i].onClick.AddListener(()=> {
                PickCard(idx);
            });            
        }
    }

    void PickCard(int idx)
    {
        // 나 자신(버튼) 사라짐
        gameObject.SetActive(false);

        while (true)
        {   
            // 랜덤 순서 뽑기
            rand = Random.Range(1, 5);

            // 내가 뽑은 순서가 CurrentRoom.CustomProperties에 없다면 
            if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey($"Order{rand}") == false)
            {
                // Player의 닉네임과 함께 순서 CurrentRoom에 저장
                Hashtable hs = new Hashtable();
                hs.Add($"Order{rand}", PhotonNetwork.LocalPlayer.NickName);                
                
                PhotonNetwork.CurrentRoom.SetCustomProperties(hs);

                print(rand);
                print(idx);
                //카드 번호 텍스트를 rand으로
                cardBtns[idx].GetComponentInChildren<TMP_Text>().text = rand.ToString();                
                break;
            }

        }            
    }
}
