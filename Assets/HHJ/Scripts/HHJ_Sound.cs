using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ������Ʈ�� �ε����� �Ҹ��� ���´�.

public class HHJ_Sound : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        // ���� �ε��� ��ü�� �÷��̾�
        if (other.tag == "Player") 
        {
            HHJ_SoundManager.Instance.DropPlayerSound();            
        }
        // ���� �ε��� ��ü�� �����̶��
        if(other.tag == "IceObj")
        {
            HHJ_SoundManager.Instance.UpIceSound();
        }
        // ���� �ε��� ��ü�� �հ��̶��
        if(other.tag == "CrownObj")
        {
            HHJ_SoundManager.Instance.DropCrownSound();
        }
        // ���� �ε��� ��ü�� �׸��̶��
        if(other.tag == "Objs")
        {
            HHJ_SoundManager.Instance.DropObjectSound();
        }
        // ���� �ε��� ��ü�� �������̶��
        if(other.tag == "SpoonObj")
        {
            HHJ_SoundManager.Instance.DropSpoonSound();
        }
    }
}
