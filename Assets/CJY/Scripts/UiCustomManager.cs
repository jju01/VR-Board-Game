using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UiCustomManager : MonoBehaviour
{
    // ĳ���� ĵ����
    public GameObject customPannel;
    
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

        // �غ� �� �Ǹ� ���̵�
        
    }
  
}
