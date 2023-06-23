using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TriggerNoticeManager : MonoBehaviour
{
    public GameObject triggerNotice1;
    public TMP_Text txtTriggerNotice1_Player;
    
    public GameObject triggerNotice2;
    public TMP_Text txtTriggerNotice2_Player;
    public TMP_Text txtTriggerNotice2_Target;
    
    public GameObject triggerNotice3;
    public TMP_Text txtTriggerNotice3_Player;

    // Trigger �ߵ�! UI
    public GameObject triggerui;
    public void SetTriggerNotice1(string playerName)
    {
        txtTriggerNotice1_Player.text = playerName;
        triggerNotice1.SetActive(true);
        
        CancelInvoke();
        Invoke("ResetTriggerNotice", 2.5f);
    }
    
    public void SetTriggerNotice2(string playerName, string targetName)
    {
        txtTriggerNotice2_Player.text = playerName;
        txtTriggerNotice2_Target.text = playerName;
        triggerNotice2.SetActive(true);
        
        CancelInvoke();
        Invoke("ResetTriggerNotice", 2.5f);
    }
    
    public void SetTriggerNotice3(string playerName)
    {
        txtTriggerNotice3_Player.text = playerName;
        triggerNotice3.SetActive(true);
        
        CancelInvoke();
        Invoke("ResetTriggerNotice", 2.5f);
    }

    private void ResetTriggerNotice()
    {
        triggerNotice1.SetActive(false);
        triggerNotice2.SetActive(false);
        triggerNotice3.SetActive(false);
    }



    public void TriggerNotice(string playerName, Trigger.Type type)
    {

       StartCoroutine("TriggerAnim2(Trigger.Type type)");
    }

    IEnumerator TriggerAnim2(Trigger.Type type)
    {
        // ItemManager ������ �����´�
        ItemManager IM = FindObjectOfType<ItemManager>();

        //4Trigger_1~3 ���� UI Ȱ��ȭ
        switch (type)
        {

            case Trigger.Type.A:
                // �ذ� �̹��� 
                triggerui.SetActive(true);
                // triggerui2
                IM.TriggerUI2[0].SetActive(true);
                // 3�� �� �������
                yield return new WaitForSeconds(3.0f);
                triggerui.SetActive(false);
                IM.TriggerUI2[0].SetActive(false);
                break;
            case Trigger.Type.B:
                // �ذ� �̹��� 
                triggerui.SetActive(true);
                // triggerui2
                IM.TriggerUI2[1].SetActive(true);
                // 3�� �� �������
                yield return new WaitForSeconds(3.0f);
                triggerui.SetActive(false);
                IM.TriggerUI2[1].SetActive(false);
                break;
            case Trigger.Type.C:
                // �ذ� �̹��� 
                triggerui.SetActive(true);
                // triggerui2
                IM.TriggerUI2[2].SetActive(true);
                // 3�� �� �������
                yield return new WaitForSeconds(3.0f);
                triggerui.SetActive(false);
                IM.TriggerUI2[2].SetActive(false);
                break;
        }
    }
}
