using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UiCustomManager : MonoBehaviour
{
    // 캐릭터 캔버스
    public GameObject customPannel;
    // 사용법 패널
    public GameObject howtousePanel;
    
    // Start is called before the first frame update
    void Start()
    {
 

    }

   public void OncClickCustom()
    {
        customPannel.gameObject.SetActive(true);
    }

    public void OnClickClosed()
    {
        customPannel.gameObject.SetActive(false);
    }

    public void Ready()
    {

        // 준비 다 되면 씬이동
        
    }
  
    public void HowToUse()
    {
        howtousePanel.gameObject.SetActive(true);
    }

    public void EndButton()
    {
        howtousePanel.gameObject.SetActive(false);
    }
}
