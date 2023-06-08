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
        // 얼음이 트리거 존에 들어가면
        //if (other.gameObject.CompareTag("Trigger"))
        //{
        //    // 활성화 일때
        //    if (isture)
        //    {
        //        // 카운트
        //        FindObjectOfType<MiniGameIce>().count++;
        //    }
        //    // 비활성화
        //    isture = false;
        //}

    }
}