using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

//  2. trigger2 : 칸에 닿은 player와 나머지 player중 랜덤으로 1명의 위치 서로 바꾸기 / player 전부 랜덤위치 바꾸기.
//  - 단 위치를 바꾸고 난후, 같은 Trigger에 재충돌하지 않도록 해야한다. 

public class Trigger2 : MonoBehaviour
{
    // Player 리스트 가져오기
    public Transform[] playerList;
    // 충돌한 other 의 characterController 가져오기
    CharacterController otherCc;
    // playerList에 담긴 player의 characterController 가져오기
    CharacterController plCc;
    // 트리거 매니저 가져오기
    TriggerCanvasManager triggerCanvasManager;
    // 트리거의 위치값
    Vector3 goalVector;

    private Transform changedPlayer;

    void Start()
    {
        otherCc = GetComponent<CharacterController>();
        plCc = GetComponent<CharacterController>();
        triggerCanvasManager = GetComponentInParent<TriggerCanvasManager>();
    }

    // Trigger2에 Player가 충돌한다면 충돌 player를 제외한 나머지 player의 위치와 바꾼다. 
    private void OnTriggerEnter(Collider other)
    {   
        // 만약 canTrigger가 false이면, 충돌인식하지 않음. 
        if (TriggerManager.canTrigger == false)
        { return; }
        // 만약 충돌한 사람이 위치가 바뀐 Player라면, 충돌인식하지 않음.
        if(other.transform == changedPlayer)
        { return; }

        Debug.Log("OnTriggerEnter2");

        // tag : player가 trigger1과 충돌한다면?  
        if (other.tag == "Player")
        {
            // - Player Y 값 세팅(trigger box의 가운데로 )
            // trigger의 위치 변수로 할당하기
            Transform goal = transform;
            // 변경된 위치를 할당
            goalVector = goal.position;
            // player의 y값 위치를 변경된 위치의 y값에 할당(박스위로 올라오도록 하기위해.)
            goalVector.y = other.transform.position.y;
            // 변경된 위치를 player의 위치에 할당 
            other.transform.DOMove(goalVector, 0.8f).OnComplete(MoveDone);


            // player의 characterController를 가져온다.
            otherCc = other.GetComponent<CharacterController>();
            //  강제 이동을 위해 characterController 잠시 비활성화
            otherCc.enabled = false;
            // Player가 충돌하면 canTrigger를 false로 변경.
            TriggerManager.canTrigger = false;

            // UI Canvas키고, player 위치 서로 바꾸기 
            StartCoroutine("TriggerActive",other.transform);  
        }   

    }

    // UI Canvas 활성화
    public IEnumerator TriggerActive(Transform player)
    {

        // 1초 후에 밑의 함수 실행
        yield return new WaitForSeconds(1);

        // triggerCanvas를 활성화 .triggerCanvas의 위치에 현재 trigger의 위치 적용
        triggerCanvasManager.UiCanvasON(transform.position);

        // 3초후에 triggerCanvas를 비활성화
        yield return new WaitForSeconds(3);
        triggerCanvasManager.UiCanvasOFF();

        // 2초 기다리기
        yield return new WaitForSeconds(0.2f);

        // 검사 후에 other은 제외하고, 나머지 
        while (true)
        {
            // 위치를 바꿀 player를 랜덤으로 정한다.
            int changPos = Random.Range(0, playerList.Length);

            // other의 transform과 PlayerList가 일치지 않는다면(충돌 other제외)
            if (player.transform != playerList[changPos])
            {
                // playerList에 들어간 Player의 CharacterController 비활성화
                plCc = playerList[changPos].GetComponent<CharacterController>();
                plCc.enabled = false;

                // plcc의 위치를 changePlayer의 위치에 넣어준다. 
                changedPlayer = plCc.transform;

                // PlayerList의 위치를 other의 위치에 두기
                player.transform.DOMove(playerList[changPos].position, 0.5f);
                // other의 위치를 PlayerList의 위치에 두기 (움직임이 끝나고,  true로 변경 )
                playerList[changPos].DOMove(player.transform.position, 0.6f).OnComplete(MoveDone);
                break;
            }
        }
    }

    void MoveDone()
    {
        //  강제 이동 후에 characterController 활성화
        otherCc.enabled = true;
        plCc.enabled = true;

        TriggerManager.canTrigger = true;
    }




}
