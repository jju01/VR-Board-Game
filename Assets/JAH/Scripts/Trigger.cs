using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

// ���� 1 : Player�� �ε����� �� ��ƼŬ ���
// ���� 2 : ȿ���� ��� ->TriggerAudio ��ũ��Ʈ ��������
// ���� 3 : UI ����
// >>  Trigger �ߵ�!
// >>  �� Trigger�� ���� ����(3��) -- > ItemManager ��ũ��Ʈ����! 
public class Trigger : MonoBehaviour
{
    // ��ƼŬ
    public ParticleSystem triggerparticle;
    // Trigger �ߵ�! UI
    public GameObject triggerui;


    public enum Type
    {
        A, B, C
    }

    public Type type;

    private void Start()
    {

        triggerui.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Player�� Item�� �ε����� ��
        if (other.tag == "Player" || other.name.Contains("Player"))
        {
            PhotonView pv = other.GetComponent<PhotonView>();
            if (pv == null || pv.IsMine == false)
                return;


            if (GameManager.Instance.MyDice.moveValue <= 1)
            {
            
                triggerparticle.transform.position = gameObject.transform.position;
                // ��ƼŬ ��� 
                triggerparticle.Stop();
                triggerparticle.Play();

                // ����� ���

                // triggerui �ִϸ��̼� ����
                StartCoroutine(TriggerAnim());
            }
        }
    }

    IEnumerator TriggerAnim ()
    {
        // 1) triggerui�� Ȱ��ȭ�Ѵ�.
        // Trigger�ߵ�! UI Ȱ��ȭ
        // >> ��ġ: ���ڽ�, ����: �÷��̾�������
        triggerui.SetActive(true);
        triggerui.transform.position = gameObject.transform.position + Vector3.forward * 2f;
        transform.LookAt(GameManager.Instance.MyPlayer.transform);

        // 2) 2�� ��ٸ�
        yield return new WaitForSeconds(2.0f);

        // 3) triggerui�� ��Ȱ��ȭ�Ѵ�.
        triggerui.SetActive(false);

        // ItemManager ������ �����´�
        ItemManager IM = FindObjectOfType<ItemManager>();

        //4) Trigger_1~3 ���� UI Ȱ��ȭ
        switch (type)
        {
            // ���� Item type�� A���, GItem UI Ȱ��ȭ + �÷��̾� ������ ���� ����..
            case Type.A:
                IM.TriggerUI[0].SetActive(true);
                IM.TriggerUI[0].transform.position = gameObject.transform.position + Vector3.forward * 2f;
                IM.TriggerUI[0].transform.LookAt(GameManager.Instance.MyPlayer.transform);
                yield return new WaitForSeconds(2.0f);
                IM.TriggerUI[0].SetActive(false);
                break;
            case Type.B:
                IM.TriggerUI[1].SetActive(true);
                IM.TriggerUI[1].transform.position = gameObject.transform.position + Vector3.forward * 2f;
                IM.TriggerUI[1].transform.LookAt(GameManager.Instance.MyPlayer.transform);
                yield return new WaitForSeconds(2.0f);
                IM.TriggerUI[1].SetActive(false);
                break;
            case Type.C:
                IM.TriggerUI[2].SetActive(true);
                IM.TriggerUI[2].transform.position = gameObject.transform.position + Vector3.forward * 2f;
                IM.TriggerUI[2].transform.LookAt(GameManager.Instance.MyPlayer.transform);
                yield return new WaitForSeconds(2.0f);
                IM.TriggerUI[2].SetActive(false);
                break;
        }
    }
}
