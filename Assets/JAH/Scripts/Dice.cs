using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Random = UnityEngine.Random;

// 역할 1: 주사위를 굴린다
// 역할 2: 주사위 수만큼 Player를 이동시킨다 (IceCube 스크립트)

public class Dice : MonoBehaviour
{
    public static Dice Instance { get; private set; }

    public GameObject player;
    public GameObject ovrcamera;

    // 주사위 리스트 (Basic Model, 1~6)
    public List<GameObject> DiceList = new List<GameObject>();

    public IceCube firstCube;

    // IceCube
    public IceCube Icecube { get; set; }

    // 주사위 번호
    private int curDice;

    // 주사위 이동
    public bool isMoving  = false;

    // 주사위 이동 값(= curDice +1)
    public int moveValue { get; private set; }

    private void Awake()
    {
        Instance = this;
        Icecube = firstCube;
    }



   // 순서 카드 한번 더 누르면 실행됨(CardBtn 이벤트)
    public void OrderNumber()
    { Invoke("DiceSetActive", 1); }


    // 주사위(Basic Model 활성화)
    public void DiceSetActive()
    {

        transform.Find("Basic Model").gameObject.SetActive(true);

        // VR Controller 사용 모드인 경우 
        if (GameManager.Instance.useVRController)
         { // OVRCamera(CenterEyeAnchor) 앞에 주사위 위치시킴
           transform.Find("Basic Model").gameObject.transform.position = ovrcamera.transform.position + (ovrcamera.transform.forward * 2);
         }
        else
        {
            // Player 앞에 주사위 위치시킴
            transform.Find("Basic Model").gameObject.transform.position = player.transform.position + (player.transform.forward * 2);
        }
            

 
    }

    // 1. 주사위 나왔을 때 Ray를 쏘고 ,
    // 2. 주사위 결과가 나오고
    // 3. 주사위 수만큼 움직이게 하는 함수
    public void RandomDice()
    {

              
                        // 2. 1~6 주사위 랜덤 활성화
                        curDice = Random.Range(0, DiceList.Count);
                        moveValue = curDice+1;

                        DiceList[curDice].SetActive(true);

                        // VR Controller 사용 모드인 경우 
                        if (GameManager.Instance.useVRController)
                          { // OVRCamera(CenterEyeAnchor) 앞에 주사위 위치시킴
                             DiceList[curDice].gameObject.transform.position = ovrcamera.transform.position + (ovrcamera.transform.forward * 2);
                          }
                        else // VR Controller 미사용 모드인 경우
                        {
                            //  Player 앞에 위치시킴 
                            DiceList[curDice].gameObject.transform.position = player.transform.position + (player.transform.forward * 2);
                        }
                       
                        // 4. 1초 뒤 랜덤 주사위 비활성화 
                        Invoke("RandomDiceList", 1f);
                        // 5. 1초 뒤 숫자 UI  + 파티클 + 효과음 재생
                        // 6. 1초 뒤 숫자 UI  + 파티클 + 효과음 비활성화

                        // 7. 2초 뒤 Player를 주사위 수만큼 이동시킴 
                        Invoke("MoveToNext", 2f);



        
        
    }
    // 랜덤 주사위 비활성화 
    private void RandomDiceList()
    {
        DiceList[curDice].gameObject.SetActive(false);
    }

   // Player를 주사위 숫자만큼 이동
   private void MoveToNext()
    {
        if (isMoving || (moveValue <= 0))
            return;

        isMoving = true;
        Icecube.GetNext(MoveTo);
    }


    public void MoveTo(IceCube target)
    {
        Icecube = target;
        Vector3 goalPos = target.transform.position;
        goalPos.y = player.transform.position.y;

        // player를 이동시킨다
        player.transform.DOMove(goalPos, 0.8f).OnComplete(MoveDone);
        player.transform.DOLookAt(goalPos, 0.5f);
    }

    private void MoveDone()
    {
        moveValue--;
        isMoving = false;

        if (moveValue > 0)
            MoveToNext();
        else
            DiceSetActive();


    }
    }


