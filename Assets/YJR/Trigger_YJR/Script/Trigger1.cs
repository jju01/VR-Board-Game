using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

// ����1 : Trigger1_ �ߵ��� player�� ��ġ�� ��, ��ĭ +- 3 ĭ ���� �̵�
// ����2 : Trigger UIâ�� ��Ȱ��ȭ
// - Player�� ��ġ��
// - �̵��� ��ġ ��

public class Trigger1 : MonoBehaviour
{
    // stage class �ҷ�����
    Stage stage;

    // TrigerMaanger �ҷ�����
    TriggerCanvasManager triggerCanvasManager;
    
    Vector3 goalVector;

    CharacterController cc;
    int random;

    void Start()
    {
        // stage�� (�θ�)stage ������Ʈ �Ҵ�
        stage = GetComponentInParent<Stage>();
        // triggerManager�� (�θ��Ҵ�) ������Ʈ
        triggerCanvasManager = GetComponentInParent<TriggerCanvasManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // trigger 1�� �浹�� player�� ��ġ�� �������� �̵���Ų��.
    private void OnTriggerEnter(Collider other)
    {
        // Cantrigger�� false�̸� �浹����X
        if (TriggerManager.canTrigger == false)
        { return; }

        Debug.Log("OnTriggerEnter1");
        // ���� �ε�����ȣ(��ġ)���� �������� �̵��� ��ġ ����
        random = Random.Range(1,2);
               
        // tag : player�� trigger1�� �浹�Ѵٸ�?  
        if (other.tag == "Player")
        {
            // - Player Y �� ����(trigger box�� ����� )
            // trigger�� ��ġ ������ �Ҵ��ϱ�
            Transform goal = transform;
            // ����� ��ġ�� �Ҵ�
           goalVector = goal.position;
            // player�� y����ġ�� ����� ��ġ�� y���� �Ҵ�(�ڽ����� �ö������ ��.)
           goalVector.y = other.transform.position.y;

            // Player�� ��ġ�� trigger�� ��ġ��
           other.transform.DOMove(goalVector, 0.5f);

            // Ʈ���� �ߵ� �ڷ�ƾ �Լ� ����
           StartCoroutine("TriggerActive", other.transform);                      
        }

    }

    // player �ڷ�ƾ �Լ� ����
    public IEnumerator TriggerActive(Transform player)
    {
        // player�� characterController�� �����´�.
        cc = player.GetComponent<CharacterController>();
        //  ���� �̵��� ���� characterController ��� ��Ȱ��ȭ
        cc.enabled = false;       

        // 1�� �Ŀ� ���� �Լ� ����
        yield return new WaitForSeconds(1);

        // triggerCanvas�� Ȱ��ȭ .triggerCanvas�� ��ġ�� ���� trigger�� ��ġ ����
        triggerCanvasManager.UiCanvasON(transform.position);

        // 3���Ŀ� triggerCanvas�� ��Ȱ��ȭ
        yield return new WaitForSeconds(3);       
        triggerCanvasManager.UiCanvasOFF();

        yield return new WaitForSeconds(0.2f);

        // ���� �ε��� ��ȣ(triger ��ġ) ��������
        int trigerPos = transform.GetSiblingIndex();
        // ���� ��ġ + �����̵��� ��ġ
        int goalPos = trigerPos + random;

        // ����� ��ġ (�ε�����ȣ) �Ҵ��ϱ�
        Transform goal = stage.mapList[goalPos];
        // ����� ��ġ�� �Ҵ�
        goalVector = goal.position;
        // player�� y����ġ�� ����� ��ġ�� y���� �Ҵ�(�ڽ����� �ö������ ��.)
        goalVector.y = player.transform.position.y;                

        // ����� ��ġ�� player�� ��ġ�� �Ҵ� 
        player.transform.DOMove(goalVector, 0.5f * random).OnComplete(MoveDone);

    }

    void MoveDone()
    {
        //  ���� �̵� �Ŀ� characterController Ȱ��ȭ
        cc.enabled = true;
    }

  

}
