using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// - Player�� Ʈ���ſ� ������ UICanvas Ȱ��ȭ 
// - map ����Ʈ�� �����Ѵ�.
// - player ����Ʈ�� �����Ѵ�.

public class Stage : MonoBehaviour
{
    // map ����Ʈ ��������
    public Transform[] mapList;
    
    // map ���� ������Ʈ ����Ʈ�� �������� 
   // public List<GameObject> mapOBJs = new List<GameObject>();

    
    //void Start()
    //{
    //    // ���ڽ��� ������ ��ƿ´�.
    //    int childCount = transform.childCount;
    //    // �ڽ��� ����� üũ
    //    for (int i = 0; i < childCount; i++)
    //    {
    //        // i ��°�� child�� ��ġ�� �����´�.
    //        Transform curChild =  transform.GetChild(i);
    //        // ���� �˻��� i child�� �±װ� Tirgger���
    //        if (curChild.CompareTag("Trigger"))
    //        {
    //            // ���� ������Ʈ�� mapOBJs�� ����Ʈ�� �߰��Ѵ�.
    //           // mapOBJs.Add(curChild.gameObject);
    //        }

    //    }
    //}

  

    // Update is called once per frame
    void Update()
    {
        
    }
}
