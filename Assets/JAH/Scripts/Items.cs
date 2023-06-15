using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 역할 1: Player 와 Item이 부딪혔을 때 Item이 사라진다
// 역할 2 : Player 와 Item이 부딪혔을 때 UI 생성 (ItemManager 스크립트)
// 역할 3 : Item 4개 모두 다 부딪히면 게임 종료 (ItemManager 스크립트)

public class Items : MonoBehaviour
{
    // Item 타입 4가지로 분류
    public enum Type
    {
        A, B, C, D
    }

    public Type type;




    private void OnTriggerEnter(Collider other)
    {
        // Player와 Item이 부딪혔을 때
        if (other.tag == "Player" || other.name.Contains("Player"))
        {
           if(Dice.Instance.moveValue <=1 )
            {

            // ItemManager 데이터 가져온다
            ItemManager IM = FindObjectOfType<ItemManager>();

            // 카운트 증가
            IM.count++;

            // 아이템 (나 자신) 비활성화
            gameObject.SetActive(false);

            //UI 활성화
            switch (type)
            {
                // 만일 Item type이 A라면, GItem UI 활성화
                case Type.A: IM.UIItems[0].SetActive(true); break;
                case Type.B: IM.UIItems[1].SetActive(true); break;
                case Type.C: IM.UIItems[2].SetActive(true); break;
                case Type.D: IM.UIItems[3].SetActive(true); break;

            }

            // 만일 GItem UI 4개 다 활성화 되면 게임 종료, Ending Scene으로 전환
            if (IM.UIItems[0].activeSelf == true && IM.UIItems[1].activeSelf == true &&
                IM.UIItems[2].activeSelf == true && IM.UIItems[3].activeSelf == true ) print("GameClear");
            }
            
        }
    }
}
