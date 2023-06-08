using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// ���� :  ī�޶� ���� World Canvas UI�� �����Ҽ� �ֵ��� ���ش�.
//         Graphic RayCaster�� �߻��Ѵ�.
// - Graphic Raycaster (Canvas�� �پ��ִ�)
// - UI �浹ü�� ���� �׸�
// - Canvas ���� ������ ���콺 ������ ��ġ �� ����
// - ������ ���콺 Ŀ�� ������ ����� ī�޶�
// - LineRenderer

// ���� : Ray ������ ����(Line)�� CrossHair ������ ������ �д�.
// - CrossHair
// - ũ�Ⱑ ������ �� �ֵ��� ���� (����ũ�� + �Ÿ� * ������)
//     -> ������ 
//     -> ���� ũ��

// + ������ ������ ����Ͽ� �ּ�ó�� �Ͽ���.



public class VRGraphicRayCaster : BaseInputModule
{
    // - Graphic Raycaster (Canvas�� �پ��ִ�)
    public GraphicRaycaster graphicRaycaster;
    // - UI �浹ü�� ���� �׸�
    public List<RaycastResult> raycastResults;
    // - Canvas ���� ������ ���콺 ������ ��ġ �� ����
    private PointerEventData pointerEventData;
    // - ������ ���콺 Ŀ�� ������ ����� ī�޶�
    public Camera cam;
    // - LineRenderer
    private LineRenderer lr;
    // - LR�׸� ���� ����
    public float lineDis = 1.0f;
    [Space]
    // - CrossHair
    public Transform crossHair;
    //     -> ������ 
    public float adjustSize = 0.5f;
    //     -> ���� ũ��
    private Vector3 originSize;

    // Start is called before the first frame update
    protected override void Start()
    {

        // pointerEventData �ʱ�ȭ
        pointerEventData = new PointerEventData(null);
        // pointerEventData ������ ���콺 ������ ��ġ�� ȭ�� ���߾�����
        pointerEventData.position = new Vector2(cam.pixelWidth * 0.5f, cam.pixelHeight * 0.5f);
        // UI �浹ü�� ���� �׸� ����Ʈ �ʱ�ȭ
        raycastResults = new List<RaycastResult>();
        // LR �ʱ�ȭ : ī�޶��� �θ� �پ��ִ�(Hand) ���� �پ��ִ� LineRenderer ������ 
        lr = cam.GetComponentInParent<LineRenderer>();

        // crossHair �� ���� ������ ���� 
        originSize = crossHair.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        GraphicRayCast();
    }

    private void GraphicRayCast()
    {

        // pointerEventData ������ ���콺 ������ ��ġ�� ȭ�� ���߾�����
        pointerEventData.position = new Vector2(cam.pixelWidth * 0.5f, cam.pixelHeight * 0.5f);


        //1. Ray�� ����� ���ÿ� �浹��Ű��, �浹�� ������ ����Ʈ�� ��´�.
        // - Ray�� ��𿡼� �� ��(Ư�� ĵ����)
        // - Ray�� ��� �� �� (������ ���콺 ��ġ = pointerEventData.position)
        // - Ray�� ���� �ε��� ������ ���� �׸� (raycastResults)
        graphicRaycaster.Raycast(pointerEventData, raycastResults);

        // 2. �浹�� ��ü(UI)�� �ִٸ�?
        if (raycastResults.Count > 0)
        {
            //   a. �浹�� ��ü�� ��� Ž���Ѵ�.
            for (int i = 0; i < raycastResults.Count; i++)
            {
                //   b. �� �浹 ��ü���� Mouse Hovering �̺�Ʈ ����
                Debug.Log(raycastResults[i].gameObject.name);
                HandlePointerExitAndEnter(pointerEventData, raycastResults[i].gameObject);
                //   c. �浹�� ���¿��� Ư�� Input ��ư�� ������ 
                if (Input.GetKeyDown(KeyCode.Space) || OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch))
                {                    
                    //     - �ش� UI�� Ŭ�� �̺�Ʈ�� �����Ѵ�.
                    ExecuteEvents.Execute(
                        raycastResults[i].gameObject,   // Event ������ ��ü
                        new BaseEventData(eventSystem), // Event �����ϴ� ������
                        ExecuteEvents.submitHandler     // �ش� Event ����(����)
                        );

                    if (raycastResults[i].gameObject.GetComponent<Button>())
                        return;
                }
            }
            //   d. �浹�� ������ LR �׷��ֱ�
            //DrawLine(raycastResults[0].distance);

            //   e. �浹�� ��ġ�� crossHair�� ������ �д�.
            //SetCrossHairPosition(raycastResults[0].distance);

        }
        // 3. �浹�� ��ü(UI)�� ���ٸ�?
        else
        {
            //  a. Mouse Hovering�̺�Ʈ ����. (= ȣ���� ���)
            HandlePointerExitAndEnter(pointerEventData, null);
            //  b. �⺻ Ray ���̸�ŭ LR �׷��ֱ�.
            // DrawLine(lineDis);
            //  c. Line�� ������ ��ġ�� crossHair�� ������ �д�.
            // SetCrossHairPosition(lineDis);
        }

        // 4. �浹�� ������ ��Ƴ��� ����Ʈ Reset ���ش�.
        raycastResults.Clear();


    }

    private void SetCrossHairPosition(float distance)
    {
        //    ��ġ : cam�� ��ġ + cam�� ���ϴ� ���� * �浹�� ��ü�� ������ �Ÿ� or ������ ������ �Ÿ�
        Vector3 crossHairPos = cam.transform.position + cam.transform.forward * distance;
        crossHair.position = crossHairPos;
        //    ũ�� : ���� ũ�� * �Ÿ�* ������
        float dis = Vector3.Distance(crossHairPos, Camera.main.transform.position);
        crossHair.localScale = originSize * dis * adjustSize;
        //    ���� : �׻� ��(���� ī�޶�)�� �ٶ󺸰� �ֵ���
        Vector3 dir = crossHairPos - Camera.main.transform.position;
        crossHair.forward = dir.normalized;
    }


    //���� �������׸��� �Լ�
    private void DrawLine(float distance)
    {
        // ������ ù��° �� ��ġ ����(���� ����Ʈ) : ����ġ
        lr.SetPosition(0, cam.transform.position);
        // ������ �ι�° �� ��ġ ����(�� ����Ʈ) : ����ġ + �� ���� *�Ÿ�(distance)
        lr.SetPosition(1, cam.transform.position + cam.transform.forward * distance);
    }


    #region BaseInputModule ��ӹ޾Ƽ� �ʼ������� �����ؾ��ϴ� �Լ�
    public override void Process() { }
    #endregion
}
