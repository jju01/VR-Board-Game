using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Player(OVRCamera)�� �� ��ġ�� ���� ���� �����ϴ� ȭ��ǥ ������Ʈ Ȱ��ȭ
// Ŭ���� ������ Player �̵�

public class Direction : MonoBehaviour
{
    public GameObject Player;
    public GameObject NextIceCube_1;
    public GameObject NextIceCube_2;

    // Update is called once per frame
    void Update()
    {
        // Player(OVRCamera)�� �� ��ġ�� ���� ���� �����ϴ� ȭ��ǥ ������Ʈ Ȱ��ȭ
        if (Player.transform.position == transform.position + new Vector3(0, 3.45f, 0))
        {

        }
    }
}
