using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 역할 1 : 카드 선택 후 1초 뒤 주사위(자식오브젝트 "Basic Model") 활성화 

public class Dice : MonoBehaviour
{
    public GameObject Player;

    // 주사위 1~6 리스트
    public List<GameObject> DiceList = new List<GameObject>();

    // 주사위 리스트 랜덤 번호
    private int curDice;

    private int curPos;
    // IceCube
    public List<GameObject> IceCubeList = new List<GameObject>();


    // Update is called once per frame
    void Update()
    {
        RandomDice();
    }

    // 순서 카드 선택 후 1초 뒤 주사위 나타나게 한다 (각 카드 버튼에 연결)
    public void OrderNumber()
    { Invoke("DiceSetActive", 1); }

    // 주사위(자식오브젝트 "Basic Model") 활성화 함수
    public void DiceSetActive()
    {
        // Basic Model 활성화
        transform.Find("Basic Model").gameObject.SetActive(true);

        // A. VR Controller 사용 모드인 경우
        if (GameManager.Instance.useVRController) { /* 나중에 입력..*/ }
        // B.  VR Controller 미사용 모드인 경우
        else
        {
            // Player 앞에 위치시킨다
            transform.Find("Basic Model").gameObject.transform.position = Player.transform.position + (Player.transform.forward * 2);
        }

    }

    // 주사위(Basic Model)을 클릭(Ray 를 쏘기)하면 랜덤으로 주사위 리스트1~6 중 하나가 활성화 
    public void RandomDice()
    {
        // A. VR Controller 사용 모드인 경우
        if (GameManager.Instance.useVRController) { /* 나중에 입력..*/ }
        // B.  VR Controller 미사용 모드인 경우
        else
        {
            // 마우스를 눌렀을 때
            if (Input.GetButtonDown("Fire1"))
            {
                // Ray (Mouse Point가 위치한 곳으로 Ray를 쏜다.)
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                // RaycasyHit
                RaycastHit hitInfo;

                if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity))
                {
                    // 주사위 Basic Model을 클릭하면
                    if (hitInfo.transform.name == "Basic Model")
                    {
                        // 1. Basic Model 비활성화
                        hitInfo.collider.gameObject.SetActive(false);
                        // 2. 주사위 리스트 1~6 랜덤으로 하나 활성화
                        curDice = Random.Range(0, DiceList.Count);
                        DiceList[curDice].SetActive(true);
                        // 3. 선택된 주사위 Player 앞으로 위치
                        DiceList[curDice].gameObject.transform.position = Player.transform.position + (Player.transform.forward * 2);
                        // 4. 1초 뒤 파티클 생성 + 선택된 주사위 사라짐
                        //DiceList[curDice].gameObject.SetActive(false);
                        Invoke("RandomDiceList", 1f);
                        // 5. 1초 뒤 숫자 UI + 파티클 생성 
                        // 6. 1초뒤 숫자 UI + 파티클 사라짐
                        // 7. Player를 주사위 숫자만큼 이동시킨다
                        Invoke("PlayerMove", 2f);
                    }

                }
            }
        }

        
        
    }
    // 랜덤 선택된 주사위 사라지게 함
    private void RandomDiceList()
    {
        DiceList[curDice].gameObject.SetActive(false);
    }

   // Player를 주사위 숫자만큼 이동시킨다
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
