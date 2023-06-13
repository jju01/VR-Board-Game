using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

// 역할1 : Trigger1_ 추돌한 player의 위치를 앞, 뒤칸 +- 3 칸 동시 이동
// 역할2 : Trigger UI창을 비활성화
// - Player의 위치값
// - 이동할 위치 값

public class Trigger1 : MonoBehaviour
{
    // stage class 불러오기
    Stage stage;

    // TrigerMaanger 불러오기
    TriggerCanvasManager triggerCanvasManager;
    
    Vector3 goalVector;

    CharacterController cc;
    int random;

    void Start()
    {
        // stage에 (부모)stage 컴포넌트 할당
        stage = GetComponentInParent<Stage>();
        // triggerManager에 (부모할당) 컴포넌트
        triggerCanvasManager = GetComponentInParent<TriggerCanvasManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // trigger 1에 충돌한 player의 위치를 랜덤으로 이동시킨다.
    private void OnTriggerEnter(Collider other)
    {
        // Cantrigger가 false이면 충돌감지X
        if (TriggerManager.canTrigger == false)
        { return; }

        Debug.Log("OnTriggerEnter1");
        // 나의 인덱스번호(위치)에서 랜덤으로 이동할 위치 선정
        random = Random.Range(1,2);
               
        // tag : player가 trigger1과 충돌한다면?  
        if (other.tag == "Player")
        {
            // - Player Y 값 세팅(trigger box의 가운데로 )
            // trigger의 위치 변수로 할당하기
            Transform goal = transform;
            // 변경된 위치를 할당
           goalVector = goal.position;
            // player의 y값위치를 변경된 위치의 y값에 할당(박스위로 올라오도록 함.)
           goalVector.y = other.transform.position.y;

            // Player의 위치를 trigger의 위치로
           other.transform.DOMove(goalVector, 0.5f);

            // 트리거 발동 코루틴 함수 실행
           StartCoroutine("TriggerActive", other.transform);                      
        }

    }

    // player 코루틴 함수 적용
    public IEnumerator TriggerActive(Transform player)
    {
        // player의 characterController를 가져온다.
        cc = player.GetComponent<CharacterController>();
        //  강제 이동을 위해 characterController 잠시 비활성화
        cc.enabled = false;       

        // 1초 후에 밑의 함수 실행
        yield return new WaitForSeconds(1);

        // triggerCanvas를 활성화 .triggerCanvas의 위치에 현재 trigger의 위치 적용
        triggerCanvasManager.UiCanvasON(transform.position);

        // 3초후에 triggerCanvas를 비활성화
        yield return new WaitForSeconds(3);       
        triggerCanvasManager.UiCanvasOFF();

        yield return new WaitForSeconds(0.2f);

        // 나의 인덱스 번호(triger 위치) 가져오기
        int trigerPos = transform.GetSiblingIndex();
        // 나의 위치 + 랜덤이동할 위치
        int goalPos = trigerPos + random;

        // 변경된 위치 (인덱스번호) 할당하기
        Transform goal = stage.mapList[goalPos];
        // 변경된 위치를 할당
        goalVector = goal.position;
        // player의 y값위치를 변경된 위치의 y값에 할당(박스위로 올라오도록 함.)
        goalVector.y = player.transform.position.y;                

        // 변경된 위치를 player의 위치에 할당 
        player.transform.DOMove(goalVector, 0.5f * random).OnComplete(MoveDone);

    }

    void MoveDone()
    {
        //  강제 이동 후에 characterController 활성화
        cc.enabled = true;
    }

  

}
