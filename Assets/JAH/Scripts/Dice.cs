using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���� 1 : ī�� ���� �� 1�� �� �ֻ���(�ڽĿ�����Ʈ "Basic Model") Ȱ��ȭ 

public class Dice : MonoBehaviour
{
    public GameObject Player;

    // �ֻ��� 1~6 ����Ʈ
    public List<GameObject> DiceList = new List<GameObject>();

    // �ֻ��� ����Ʈ ���� ��ȣ
    private int curDice;

    private int curPos;
    // IceCube
    public List<GameObject> IceCubeList = new List<GameObject>();


    // Update is called once per frame
    void Update()
    {
        RandomDice();
    }

    // ���� ī�� ���� �� 1�� �� �ֻ��� ��Ÿ���� �Ѵ� (�� ī�� ��ư�� ����)
    public void OrderNumber()
    { Invoke("DiceSetActive", 1); }

    // �ֻ���(�ڽĿ�����Ʈ "Basic Model") Ȱ��ȭ �Լ�
    public void DiceSetActive()
    {
        // Basic Model Ȱ��ȭ
        transform.Find("Basic Model").gameObject.SetActive(true);

        // A. VR Controller ��� ����� ���
        if (GameManager.Instance.useVRController) { /* ���߿� �Է�..*/ }
        // B.  VR Controller �̻�� ����� ���
        else
        {
            // Player �տ� ��ġ��Ų��
            transform.Find("Basic Model").gameObject.transform.position = Player.transform.position + (Player.transform.forward * 2);
        }

    }

    // �ֻ���(Basic Model)�� Ŭ��(Ray �� ���)�ϸ� �������� �ֻ��� ����Ʈ1~6 �� �ϳ��� Ȱ��ȭ 
    public void RandomDice()
    {
        // A. VR Controller ��� ����� ���
        if (GameManager.Instance.useVRController) { /* ���߿� �Է�..*/ }
        // B.  VR Controller �̻�� ����� ���
        else
        {
            // ���콺�� ������ ��
            if (Input.GetButtonDown("Fire1"))
            {
                // Ray (Mouse Point�� ��ġ�� ������ Ray�� ���.)
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                // RaycasyHit
                RaycastHit hitInfo;

                if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity))
                {
                    // �ֻ��� Basic Model�� Ŭ���ϸ�
                    if (hitInfo.transform.name == "Basic Model")
                    {
                        // 1. Basic Model ��Ȱ��ȭ
                        hitInfo.collider.gameObject.SetActive(false);
                        // 2. �ֻ��� ����Ʈ 1~6 �������� �ϳ� Ȱ��ȭ
                        curDice = Random.Range(0, DiceList.Count);
                        DiceList[curDice].SetActive(true);
                        // 3. ���õ� �ֻ��� Player ������ ��ġ
                        DiceList[curDice].gameObject.transform.position = Player.transform.position + (Player.transform.forward * 2);
                        // 4. 1�� �� ��ƼŬ ���� + ���õ� �ֻ��� �����
                        //DiceList[curDice].gameObject.SetActive(false);
                        Invoke("RandomDiceList", 1f);
                        // 5. 1�� �� ���� UI + ��ƼŬ ���� 
                        // 6. 1�ʵ� ���� UI + ��ƼŬ �����
                        // 7. Player�� �ֻ��� ���ڸ�ŭ �̵���Ų��
                        Invoke("PlayerMove", 2f);
                    }

                }
            }
        }

        
        
    }
    // ���� ���õ� �ֻ��� ������� ��
    private void RandomDiceList()
    {
        DiceList[curDice].gameObject.SetActive(false);
    }

   // Player�� �ֻ��� ���ڸ�ŭ �̵���Ų��
   private void PlayerMove()
    {
        curPos += curDice;
        IceCubeList[curPos].gameObject.GetComponent<IceCube>().IceCubeMove();
        //for (int i = 0; i < curDice; i++)
        //{
        //      IceCubeList[i].gameObject.GetComponent<IceCube>().IceCubeMove();
        //}

        print(curPos);
    }
}
