using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// UI Canvas ����

public class TriggerCanvasManager : MonoBehaviour
{

    // trigger cavas UI 
    public GameObject triggerCanvas;
    // ui canvas y �� ����
    public float UIyVectorSetting = 5f;

    // TriggerUIcanvas�� Ų��. 
    public void UiCanvasON(Vector3 pos)
    {
        // uiCanvas y�� ����
        pos.y += UIyVectorSetting;
        // UI Canvas�� �ܺ� ��ġ �� �Ҵ�
        triggerCanvas.transform.position = pos;

        // UICanvas ��Ȱ��ȭ Ȱ��ȭ
        triggerCanvas.gameObject.SetActive(false);
        triggerCanvas.gameObject.SetActive(true);

    }
    public void UiCanvasOFF()
    {
        triggerCanvas.gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
