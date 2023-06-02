using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 카드 번호 선택후 다른 카드들 선택 못하게 한다

public class CardSelect : MonoBehaviour
{
    

    public void CardDeSelect()
    {
        gameObject.SetActive(false);
    }
}
