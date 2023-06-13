using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// - Player가 트리거에 닿으면 UICanvas 활성화 
// - map 리스트를 관리한다.
// - player 리스트를 관리한다.

public class Stage : MonoBehaviour
{
    // map 리스트 가져오기
    public Transform[] mapList;
    
    // map 게임 오브젝트 리스트로 가져오기 
   // public List<GameObject> mapOBJs = new List<GameObject>();

    
    //void Start()
    //{
    //    // 내자식의 갯수를 담아온다.
    //    int childCount = transform.childCount;
    //    // 자식의 수대로 체크
    //    for (int i = 0; i < childCount; i++)
    //    {
    //        // i 번째의 child의 위치를 가져온다.
    //        Transform curChild =  transform.GetChild(i);
    //        // 만약 검사한 i child의 태그가 Tirgger라면
    //        if (curChild.CompareTag("Trigger"))
    //        {
    //            // 게임 오브젝트를 mapOBJs의 리스트에 추가한다.
    //           // mapOBJs.Add(curChild.gameObject);
    //        }

    //    }
    //}

  

    // Update is called once per frame
    void Update()
    {
        
    }
}
