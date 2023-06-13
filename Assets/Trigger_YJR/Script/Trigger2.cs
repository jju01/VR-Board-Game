using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

//  2. trigger2 : ĭ�� ���� player�� ������ player�� �������� 1���� ��ġ ���� �ٲٱ� / player ���� ������ġ �ٲٱ�.
//  - �� ��ġ�� �ٲٰ� ����, ���� Trigger�� ���浹���� �ʵ��� �ؾ��Ѵ�. 

public class Trigger2 : MonoBehaviour
{
    // Player ����Ʈ ��������
    public Transform[] playerList;
    // �浹�� other �� characterController ��������
    CharacterController otherCc;
    // playerList�� ��� player�� characterController ��������
    CharacterController plCc;
    // Ʈ���� �Ŵ��� ��������
    TriggerCanvasManager triggerCanvasManager;
    // Ʈ������ ��ġ��
    Vector3 goalVector;

    private Transform changedPlayer;

    void Start()
    {
        otherCc = GetComponent<CharacterController>();
        plCc = GetComponent<CharacterController>();
        triggerCanvasManager = GetComponentInParent<TriggerCanvasManager>();
    }

    // Trigger2�� Player�� �浹�Ѵٸ� �浹 player�� ������ ������ player�� ��ġ�� �ٲ۴�. 
    private void OnTriggerEnter(Collider other)
    {   
        // ���� canTrigger�� false�̸�, �浹�ν����� ����. 
        if (TriggerManager.canTrigger == false)
        { return; }
        // ���� �浹�� ����� ��ġ�� �ٲ� Player���, �浹�ν����� ����.
        if(other.transform == changedPlayer)
        { return; }

        Debug.Log("OnTriggerEnter2");

        // tag : player�� trigger1�� �浹�Ѵٸ�?  
        if (other.tag == "Player")
        {
            // - Player Y �� ����(trigger box�� ����� )
            // trigger�� ��ġ ������ �Ҵ��ϱ�
            Transform goal = transform;
            // ����� ��ġ�� �Ҵ�
            goalVector = goal.position;
            // player�� y�� ��ġ�� ����� ��ġ�� y���� �Ҵ�(�ڽ����� �ö������ �ϱ�����.)
            goalVector.y = other.transform.position.y;
            // ����� ��ġ�� player�� ��ġ�� �Ҵ� 
            other.transform.DOMove(goalVector, 0.8f).OnComplete(MoveDone);


            // player�� characterController�� �����´�.
            otherCc = other.GetComponent<CharacterController>();
            //  ���� �̵��� ���� characterController ��� ��Ȱ��ȭ
            otherCc.enabled = false;
            // Player�� �浹�ϸ� canTrigger�� false�� ����.
            TriggerManager.canTrigger = false;

            // UI CanvasŰ��, player ��ġ ���� �ٲٱ� 
            StartCoroutine("TriggerActive",other.transform);  
        }   

    }

    // UI Canvas Ȱ��ȭ
    public IEnumerator TriggerActive(Transform player)
    {

        // 1�� �Ŀ� ���� �Լ� ����
        yield return new WaitForSeconds(1);

        // triggerCanvas�� Ȱ��ȭ .triggerCanvas�� ��ġ�� ���� trigger�� ��ġ ����
        triggerCanvasManager.UiCanvasON(transform.position);

        // 3���Ŀ� triggerCanvas�� ��Ȱ��ȭ
        yield return new WaitForSeconds(3);
        triggerCanvasManager.UiCanvasOFF();

        // 2�� ��ٸ���
        yield return new WaitForSeconds(0.2f);

        // �˻� �Ŀ� other�� �����ϰ�, ������ 
        while (true)
        {
            // ��ġ�� �ٲ� player�� �������� ���Ѵ�.
            int changPos = Random.Range(0, playerList.Length);

            // other�� transform�� PlayerList�� ��ġ�� �ʴ´ٸ�(�浹 other����)
            if (player.transform != playerList[changPos])
            {
                // playerList�� �� Player�� CharacterController ��Ȱ��ȭ
                plCc = playerList[changPos].GetComponent<CharacterController>();
                plCc.enabled = false;

                // plcc�� ��ġ�� changePlayer�� ��ġ�� �־��ش�. 
                changedPlayer = plCc.transform;

                // PlayerList�� ��ġ�� other�� ��ġ�� �α�
                player.transform.DOMove(playerList[changPos].position, 0.5f);
                // other�� ��ġ�� PlayerList�� ��ġ�� �α� (�������� ������,  true�� ���� )
                playerList[changPos].DOMove(player.transform.position, 0.6f).OnComplete(MoveDone);
                break;
            }
        }
    }

    void MoveDone()
    {
        //  ���� �̵� �Ŀ� characterController Ȱ��ȭ
        otherCc.enabled = true;
        plCc.enabled = true;

        TriggerManager.canTrigger = true;
    }




}
