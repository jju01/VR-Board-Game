using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���� ; Ư�� �������� ������Ʈ(=�ڱ��ڽ�)�� ȸ����Ų��.

public class Rotator : MonoBehaviour
{
    // ȸ�� ����
    public Vector3 direction = new Vector3(15, 30, 45);

    // ȸ�� �ӵ�
    public float speed = 5;

    private void Start()
    {
        // �����ϰ� ȸ�� �ӷ��� �����Ѵ�. (�������� -1 ~ +1)
        speed = speed + Random.Range(-1.0f, 1.0f);
    }


    // Update is called once per frame
    void Update()
    {
        // �ڱ� �ڽ��� ������ �ӵ�(=speed)�� Ư�� ����(=direction)���� ȸ����Ų��.
        // Time.deltaTime (=frame������ �ƴ϶� �ʽð� �������� �ڵ带 ȣ��)
       // transform.M(direction * speed * Time.deltaTime);

    }
}
