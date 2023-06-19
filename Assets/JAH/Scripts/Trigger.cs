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
                // ItemManager ������ �����´�
                ItemManager IM = FindObjectOfType<ItemManager>();

                triggerparticle.transform.position = gameObject.transform.position + Vector3.down * 1.5f;
                // ��ƼŬ ��� 
                triggerparticle.Stop();
                triggerparticle.Play();

                // ����� ���


                // Trigger�ߵ�! UI Ȱ��ȭ
                // >> ��ġ: ���ڽ�, ����: �÷��̾�������
                triggerui.SetActive(true);
                triggerui.transform.position = gameObject. transform.position + Vector3.down * 1.5f;
                transform.LookAt(GameManager.Instance.MyPlayer.transform);

                //Trigger_1~3 ���� UI Ȱ��ȭ
                switch (type)
                {
                    // ���� Item type�� A���, GItem UI Ȱ��ȭ + �÷��̾� ������ ���� ����..
                    case Type.A: IM.TriggerUI[0].SetActive(true); 
                        break;
                    case Type.B: IM.TriggerUI[1].SetActive(true); break;
                    case Type.C: IM.TriggerUI[2].SetActive(true); break;
                   

                }
            }
        }
    }
}
