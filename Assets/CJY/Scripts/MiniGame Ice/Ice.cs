using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ice : MonoBehaviour
{
    public bool isture;

    // Start is called before the first frame update
    void Start()
    {
        isture = true;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnTriggerEnter(Collider other)
    {
        // ������ Ʈ���� ���� ����
        //if (other.gameObject.CompareTag("Trigger"))
        //{
        //    // Ȱ��ȭ �϶�
        //    if (isture)
        //    {
        //        // ī��Ʈ
        //        FindObjectOfType<MiniGameIce>().count++;
        //    }
        //    // ��Ȱ��ȭ
        //    isture = false;
        //}

    }
}