using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���� : Player�� �� ĭ�� ���� ���� ĭ���� �̵���Ų��

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
//   // A. VR Controller ��� ����� ���
//        if (GameManager.Instance.useVRController) { /* ���߿� �Է�..*/ }
//        // B.  VR Controller �̻�� ����� ���
//        else
//{
//    // ���콺�� ������ ��
//    if (Input.GetButtonDown("Fire1"))
//    {
//        if (Player.transform.position == gameObject.transform.position + new Vector3(0, 3.45f, 0))
//        {
//            Player.transform.position = NextIceCube.transform.position + new Vector3(0, 3.45f, 0);
//        }
//    }
//}
}
