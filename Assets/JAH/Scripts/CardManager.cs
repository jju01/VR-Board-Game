using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

// 역할 1: cards(나 자신)의 자식오브젝트 Button(1~4)을 클릭했을 때 cards(나 자신) 비활성화
// 역할 2: 랜덤 순서 배정
// 역할 3: order numbers의 button 활성화 ( button의 text를 랜덤 순서로)
// (주사위는 order numbers의 button을 클릭했을 때 생성하기)


public class CardManager : MonoBehaviour
{
    // 카드 버튼
    public Button[] cardBtns;
    // 랜덤 숫자 번호
    int rand;
    // order numbers의 카드 버튼
    public Button[] orderBtns;

    // 카드 버튼을 선택할 때 이벤트 호출
    public void CardEvent()
    {
        for (int i = 0; i < cardBtns.Length; i++)
        {
            int idx = i;
            cardBtns[i].onClick.RemoveAllListeners();
            //cardBtns[i].onClick.AddListener(PickCard(int idx));




        }
    }

    void PickCard(int idx)
    {
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
                //ordernumbers 텍스트를 rand으로
                orderBtns[idx].GetComponentInChildren<TMP_Text>().text = rand.ToString();
                break;
            }

            // card 버튼 비활성화
            cardBtns[idx].gameObject.SetActive(false);
        
        }



    }
}
