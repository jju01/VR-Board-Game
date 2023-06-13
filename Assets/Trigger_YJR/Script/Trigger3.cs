using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

//  3. trigger3 : 칸에 닿은 player 주사위 한번 더 돌리기

public class Trigger3 : MonoBehaviour
{
    // anf
    CharacterController cc;

    TriggerCanvasManager triggerCanvasManager;
    Vector3 goalVector;

    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();
        triggerCanvasManager = GetComponentInParent<TriggerCanvasManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 만약에 trigger3에 닿았다면, 주사위 한번 더 실행 
     private void OnTriggerEnter(Collider other)
     {
        // 만약 canTrigger가 false이면, 충돌인식하지 않음.
        if (TriggerManager.canTrigger == false)
        { return; }

        //  충돌한 tag가 player이라면, 
        if (other.tag == "Player")
        {
            TriggerManager.canTrigger = false;
            // - Player Y 값 세팅(trigger box의 가운데로 )
            // trigger의 위치 변수로 할당하기
            Transform goal = transform;
            // 변경된 위치를 할당
            goalVector = goal.position;
            // player의 y값 위치를 변경된 위치의 y값에 할당(박스위로 올라오도록 하기위해.)
            goalVector.y = other.transform.position.y;
            // 변경된 위치를 player의 위치에 할당 
            other.transform.DOMove(goalVector, 0.8f);
            
            StartCoroutine("TriggerActive",other.transform);
          
            Debug.Log("ui창 활성화 비활성화");

            //Invoke("MoveDone", 7f);
            print("주사위 한번 더!");
        }

     }

    // Trigger UI창 활성화, 비활성화
    public IEnumerator TriggerActive(Transform player)
    {
        cc = player.GetComponent<CharacterController>();
        cc.enabled = false;

        yield return new WaitForSeconds(1f);

        // 현재 Trigger의 위치에 UI canvas 활성화
        triggerCanvasManager.UiCanvasON(transform.position);
        yield return new WaitForSeconds(3f);
        
        // UI canvas 비활성화.
        triggerCanvasManager.UiCanvasOFF();
        
        yield return new WaitForSeconds(0.2f);
        
        // characterController가 차례로 적용되지않고, 겹치는 경우가 있어 딜레이 시간을 준다 
        cc.enabled = true;
        yield return new WaitForSeconds(0.1f);
        
        MoveDone();
    }

    void MoveDone()
    {
        //  강제 이동 후에 characterController 활성화
        TriggerManager.canTrigger = true;

    }
}
