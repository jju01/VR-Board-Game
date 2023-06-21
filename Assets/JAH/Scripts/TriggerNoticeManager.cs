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
}
