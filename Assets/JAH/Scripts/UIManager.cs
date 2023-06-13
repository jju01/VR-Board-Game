using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 역할 : 자식 오브젝트 버튼1,2 클릭했을 때 각각 이벤트 호출

public class UIManager : MonoBehaviour
{
   
    public Button[] DirBtn;


    public void OpenSelectPopup(Action<int> selectCallback)
    {
        gameObject.SetActive(true);
        for (int i = 0; i < DirBtn.Length; i++)
        {
            int idx = i;
            DirBtn[i].onClick.RemoveAllListeners();
            DirBtn[i].onClick.AddListener(() =>
            {
                selectCallback(idx);
                gameObject.SetActive(false);
            });
        }
    }
}
