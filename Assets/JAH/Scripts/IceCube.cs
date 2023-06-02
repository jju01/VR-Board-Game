using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 역할 : Player가 내 칸에 오면 다음 칸으로 이동시킨다

public class IceCube : MonoBehaviour
{
    public GameObject Player;
    public GameObject NextIceCube;


   

    public  void  IceCubeMove()
    {
        Debug.Log(transform.name);
        Player.transform.position = NextIceCube.transform.position + new Vector3(0, 3.45f, 0);
        
        //if (Player.transform.position == gameObject.transform.position + new Vector3(0, 3.45f, 0))
        //{
            
        //}
    }
//   // A. VR Controller 사용 모드인 경우
//        if (GameManager.Instance.useVRController) { /* 나중에 입력..*/ }
//        // B.  VR Controller 미사용 모드인 경우
//        else
//{
//    // 마우스를 눌렀을 때
//    if (Input.GetButtonDown("Fire1"))
//    {
//        if (Player.transform.position == gameObject.transform.position + new Vector3(0, 3.45f, 0))
//        {
//            Player.transform.position = NextIceCube.transform.position + new Vector3(0, 3.45f, 0);
//        }
//    }
//}
}
