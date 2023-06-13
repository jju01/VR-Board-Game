using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

//  3. trigger3 : ĭ�� ���� player �ֻ��� �ѹ� �� ������

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

    // ���࿡ trigger3�� ��Ҵٸ�, �ֻ��� �ѹ� �� ���� 
     private void OnTriggerEnter(Collider other)
     {
        // ���� canTrigger�� false�̸�, �浹�ν����� ����.
        if (TriggerManager.canTrigger == false)
        { return; }

        //  �浹�� tag�� player�̶��, 
        if (other.tag == "Player")
        {
            TriggerManager.canTrigger = false;
            // - Player Y �� ����(trigger box�� ����� )
            // trigger�� ��ġ ������ �Ҵ��ϱ�
            Transform goal = transform;
            // ����� ��ġ�� �Ҵ�
            goalVector = goal.position;
            // player�� y�� ��ġ�� ����� ��ġ�� y���� �Ҵ�(�ڽ����� �ö������ �ϱ�����.)
            goalVector.y = other.transform.position.y;
            // ����� ��ġ�� player�� ��ġ�� �Ҵ� 
            other.transform.DOMove(goalVector, 0.8f);
            
            StartCoroutine("TriggerActive",other.transform);
          
            Debug.Log("uiâ Ȱ��ȭ ��Ȱ��ȭ");

            //Invoke("MoveDone", 7f);
            print("�ֻ��� �ѹ� ��!");
        }

     }

    // Trigger UIâ Ȱ��ȭ, ��Ȱ��ȭ
    public IEnumerator TriggerActive(Transform player)
    {
        cc = player.GetComponent<CharacterController>();
        cc.enabled = false;

        yield return new WaitForSeconds(1f);

        // ���� Trigger�� ��ġ�� UI canvas Ȱ��ȭ
        triggerCanvasManager.UiCanvasON(transform.position);
        yield return new WaitForSeconds(3f);
        
        // UI canvas ��Ȱ��ȭ.
        triggerCanvasManager.UiCanvasOFF();
        
        yield return new WaitForSeconds(0.2f);
        
        // characterController�� ���ʷ� ��������ʰ�, ��ġ�� ��찡 �־� ������ �ð��� �ش� 
        cc.enabled = true;
        yield return new WaitForSeconds(0.1f);
        
        MoveDone();
    }

    void MoveDone()
    {
        //  ���� �̵� �Ŀ� characterController Ȱ��ȭ
        TriggerManager.canTrigger = true;

    }
}
