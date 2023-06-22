using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 오브젝트에 부딪히면 소리가 나온다.

public class HHJ_Sound : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        // 만약 부딪힌 물체가 플레이어
        if (other.tag == "Player") 
        {
            HHJ_SoundManager.Instance.DropPlayerSound();            
        }
        // 만일 부딪힌 물체가 얼음이라면
        if(other.tag == "IceObj")
        {
            HHJ_SoundManager.Instance.UpIceSound();
        }
        // 만일 부딪힌 물체가 왕관이라면
        if(other.tag == "CrownObj")
        {
            HHJ_SoundManager.Instance.DropCrownSound();
        }
        // 만일 부딪힌 물체가 그릇이라면
        if(other.tag == "Objs")
        {
            HHJ_SoundManager.Instance.DropObjectSound();
        }
        // 만일 부딪힌 물체가 숟가락이라면
        if(other.tag == "SpoonObj")
        {
            HHJ_SoundManager.Instance.DropSpoonSound();
        }
    }
}
