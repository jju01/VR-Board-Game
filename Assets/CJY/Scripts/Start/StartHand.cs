using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum PlayerHand
{
    LEFT, RIGHT
}
public class StartHand : MonoBehaviour
{
    //  - LineRenderer
    private LineRenderer lr;
    //  - PlayerHand
    public Transform hand;
    //  - Bezier Ŀ���� ������ 3���� (p0,p1,p2)
    private Vector3 p0, p1, p2;
    //  - ��� �Ÿ� (Far)
    public float far = 5f;
    //  - ��� ������ (down)
    public float down = -2f;
    //  - ��� ���� ����
    public int dotCount = 2;
    //  - �浹�� �������� �׷����� ����� ���� ����
    private int count = 0;


    [Space]
    //  - Marker Target
    public GameObject Target;
    //  - Teleport Marker
    public GameObject marker;
    // �����̵� �������� Y������ �ణ�� ������ �ΰ� �̵�
    public float offsetY = 1.5f;
    // - Unavailable Area
    public GameObject[] unavailableAreaArray;

    [Space]
    // LineRenderer ����
    public Color startColor = Color.white;
    public Color endColor = Color.white;

    [Space]
    //  - �浹üũ�� ���� RigidBody
    private Rigidbody rb;
    //  - ���� �� �ִ� ����
    public float catchRadius = 0.5f;
    //  - ���� ��ü�� ����?
    private GameObject catchObj;
    [Space]
    //  - ���� ��ġ(Left? or Right?)
    public PlayerHand m_hand;
    //��� �������� ���� OVR ��Ʈ�ѷ� �ٸ��� ����
    OVRInput.Controller controllerTouch;

    // Start is called before the first frame update
    void Start()
    {
        // Play�ϸ�, ������ �پ��ִ� Rigidbody �����´�.
        rb = GetComponent<Rigidbody>();
        // ���� ���� ��, ������ ���϶��� ���� OVR Touch ���� ����
        if (VRManager.Instance.useVRController)
        {
            switch (m_hand)
            {
                case PlayerHand.LEFT: controllerTouch = OVRInput.Controller.LTouch; break;
                case PlayerHand.RIGHT: controllerTouch = OVRInput.Controller.RTouch; break;
            }
        }
        // LineRenderer ������Ʈ �����ͼ� �ʱ�ȭ�Ѵ�.
        lr = GetComponent<LineRenderer>();
        // LineRenderer ���� �ʱ�ȭ
        lr.startColor = startColor;
        lr.endColor = endColor;
        // LineRenderer ���� �ʱ�ȭ
        lr.startWidth = 0.01f;


        // LineRenderer�� ���� ���·� ����
        lr.enabled = false;
        // Telepoint Marker�� ���� ���·� ����
        //DeactiveMarker();
    }

    // Update is called once per frame
    void Update()
    {
        //���,����
        HandController();
        //LR Ȱ��ȭ
        lr.enabled = this;
        // ��ġ ����
        PositionWithHand();
        //�ڷ���Ʈ ���� �׸���
        DrawCurve();

    }

    //Controller Ȥ�� �׽�Ʈ�϶� ������ ��� ���� �Լ�
    private void HandController()
    {
        // A. VR Controller ���
        if (VRManager.Instance.useVRController)
        {
            if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, controllerTouch) && catchObj == null)
            {
                CatchObj();
                //LR Ȱ��ȭ
                lr.enabled = this;
            }
            if (catchObj != null)
            {
                // ��ġ ����
                PositionWithHand();
                //�ڷ���Ʈ ���� �׸���
                DrawCurve();

            }
            if (OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger, controllerTouch) && catchObj != null)
            {
                DropObj();
                // LR ��Ȱ��ȭ
                lr.enabled = false;
                // �ڷ���Ʈ ��Ŀ ��Ȱ��ȭ
                DeactiveMarker();
            }
        }
        // B. VR Controller �̻�� -- TEST
        else
        {
            // 1. ���� ���콺 ������ ��ư�� ���� ����, catchObject ����ִٸ� ���
            if (Input.GetMouseButtonDown(1) && catchObj == null)
            {
                CatchObj();
                //LR Ȱ��ȭ
                lr.enabled = this;
            }
            if (catchObj != null)
            {
                // ��ġ ����
                PositionWithHand();
                //�ڷ���Ʈ ���� �׸���
                DrawCurve();

            }
            // 2. ���� ���콺 ������ ��ư�� �� ����, catchObject �ִٸ� ����
            if (Input.GetMouseButtonUp(1) && catchObj != null && marker.activeSelf == true)
            {
                DropObj();
                // LR ��Ȱ��ȭ
                lr.enabled = false;
                // �ڷ���Ʈ ��Ŀ ��Ȱ��ȭ
                DeactiveMarker();
            }
        }
    }


    // 1. ���
    // Hand�� �������� catchRadius ��ŭ�� �������� ���� ���� �ȿ� 
    // ��ü�� ���� �ִ� ���, �� ��ü�� �� ���� �ڽ����� ���´�.
    private void CatchObj()
    {
        //catchRadius��ŭ�� ���� �ȿ� �ִ� ��ü�� ��� �˻��� ���� ����� ��ü�� ��´�.
        // - �˻� ���� ��ġ = ��Ŀ
        Vector3 position = marker.transform.position;
        // - �󸶸�ŭ�� ������(= ����)�� ���� Sphere�� ������
        float radius = catchRadius;
        // - Ư�� Laywe (�����۷��̾�)
        int layerMask = 1 << LayerMask.NameToLayer("Item");
        // - �˻��� �༮��(=�迭����)�� ��� ��´�
        Collider[] hits = Physics.OverlapSphere(position, radius, layerMask);

        //1. ���� ���տ��� ����� �༮�� index�� ���� ����
        int selectedIndex = -1;
        // ����ڵ� == �˻��� ����� 1�� �̻��̶��
        if (hits != null && hits.Length > 0)
        {
            // �˻��� �༮�� �߿��� ���� ������ �ִ� �༮�� �� �տ� �ڽ����� ��´�.
            //2. �ε��� �༮�� �ִٸ�, 0��° �༮�� �Ÿ����� �˻����
            selectedIndex = 0;
            //3. �ε��� �༮�� �߿� ���� ª�� �Ÿ��� �༮ ������ �� �ְ� �ݺ� �˻�
            for (int i = 0; i < hits.Length; i++)
            {
                //4.�񱳸� ���� ª�� �༮�� index�� ���Ѵ�.
                // - ���� �༮�� �Ÿ� (��- �ε��� ��ü)
                float currentDis = Vector3.Distance(marker.transform.position, hits[selectedIndex].transform.position);
                // - ���� ����� �Ÿ�
                float nextDis = Vector3.Distance(marker.transform.position, hits[i].transform.position);
                // a. ���� ���õ� �༮��, ���� �ε��� �༮�� ���Ѵ�.
                if (currentDis > nextDis)
                {
                    // b. �� �� �Ÿ��� ���ؼ� �� ���� �༮�� selectedIndex�� �־��ش�.
                    selectedIndex = i;
                }

            }

        }
        //����,selectedIndex�� �� �ִ� ���� -1�� �ƴ϶��?
        //-> �浹�� �༮�� �־��ٸ�?
        if (selectedIndex != -1)
        {
            catchObj = hits[selectedIndex].gameObject;
            // 2. �θ�-�ڽ� : ĳġ������Ʈ�� �� �ִ� ��ü�� ���� �ڽ����� �����Ѵ�.
            catchObj.transform.parent = Target.transform;
            // +Ÿ���� ��ġ�� ������
            catchObj.transform.position = Target.transform.position;
            // +�θ� �ٶ󺸴� ����� �Ȱ��� ��������
            catchObj.transform.rotation = transform.rotation;


        }
    }

    private void DropObj()
    {
        // ���� ��ü�� Ŀ�곡���� �̵���Ų��.
        catchObj.transform.position = marker.transform.position;
        // ���� ��ü�� Rotation ���� X,Z�� 0���θ����.
        Quaternion catchObjRot = new Quaternion(0, catchObj.transform.rotation.y, 0, catchObj.transform.rotation.w);
        catchObj.transform.rotation = catchObjRot;
        //���� ��ü �θ� �����ش�.
        catchObj.transform.parent = null;
        //2. ���� ��ü �����ش� 
        catchObj = null;

    }
    private void PositionWithHand()
    {
        // 1. ���� ��ġ : PlayerHand�� ��ġ
        p0 = hand.position;
        // 2. �߰� ��ġ : [���� ��ġ] �������� Hand�� �ٶ󺸴� �������� far��ŭ ������ ��ġ
        p1 = p0 + hand.forward * far;
        // 3. �� ��ġ   : [�߰���ġ] �������� Hand�� �ٶ󺸴� �������� far�� 1/2��ŭ ��������,
        //                �Ʒ��� down��ŭ ������ ��ġ
        p2 = p1 + hand.forward * far * 0.5f + Vector3.up * down;
    }
    // �ڷ���Ʈ ���� �׸���
    private void DrawCurve()
    {
        // ��� �׸��� ���ؼ� �˾ƺ��� ������ ������ �̸��� �ٲ㼭 ����
        Vector3 start = p0;
        Vector3 center = p1;
        Vector3 end = p2;

        // 1. �浹�� ��ġ�� ������ ���ϱ� ���ؼ� ���� ���� ��ġ
        Vector3 prePos = Vector3.zero;
        // 2. �浹�� ������ ���� ������ 0���� +1 �� ī��Ʈ
        count = 0;

        // Bezier� �̿��ؼ� dotCount��ŭ�� ������ ������ ����
        for (int i = 0; i < dotCount; i++)
        {
            // 1. t �� ���� ���Ѵ�.
            float t = i / (float)dotCount;
            // 2. t �� ��ġ�� ���Ѵ�. => Bezier�Լ� ���
            Vector3 tPos = Bezier(start, center, end, t);

            // 3. ��� ������⿡�� �浹�� �Ͼ���� üũ
            // 3-1. �ε��� ���, 
            if (i > 0 && IsHit(prePos, tPos) == true)
            {
                //  - �ε��� �������� ���� �׸��� ���̻� �׸��� �ʴ´�.
                lr.positionCount = count;
                return;
            }
            // 3-2. �ε����� ���� ���
            else
            {
                //  - ��� ���� ���� �׷��� ��ġ��, LR �� �߰�
                AddPointToLineRenderer(tPos);
            }

            // 4. ���� ��ġ��, ���� ��ġ�� ����
            prePos = tPos;
        }

        // 5. ��� �ε��� ���� �׷��� ���, LR�� ���� �� PositionCount �� count ������ ����.
        lr.positionCount = count;
    }

    // Bezier �Լ� : T �� ��ġ�� �˷��ִ� ����
    private Vector3 Bezier(Vector3 p0, Vector3 p1, Vector3 p2, float t)
    {
        // p0 - p1 �Ÿ� ������ �������� t ��ġ ���ϱ�
        Vector3 p0p1 = Vector3.Lerp(p0, p1, t);
        // p1 - p2 �Ÿ� ������ �������� t ��ġ ���ϱ�
        Vector3 p1p2 = Vector3.Lerp(p1, p2, t);
        // p0p1 - p1p2 �Ÿ� ������ �������� t ��ġ ���ϱ�
        Vector3 tPos = Vector3.Lerp(p0p1, p1p2, t);
        // �ش� ����� ��ȯ
        return tPos;
    }
    // ��(pre)�� ��(cur) ������ �浹�� �Ͼ���� üũ���ִ� �Լ�
    private bool IsHit(Vector3 prePos, Vector3 pos)
    {
        //  - ���� (prePos�� pos�� �ٶ󺸴� ����) & ����ȭ
        Vector3 direction = (pos - prePos).normalized;
        //  - �Ÿ� (prePos�� pos ������ �Ÿ�)
        float distance = Vector3.Distance(pos, prePos);
        // 1. Ray�� ����ϴ�.(��ġ&����)
        Ray ray = new Ray(prePos, direction);
        // 2. RayCastHit �浹 �׸��� ����ϴ�.
        RaycastHit hitInfo = new RaycastHit();
        // 3. Ray�� �߻��մϴ�. (���� �������� �Ÿ���ŭ)
        // 4-A. ���� �浹�� ���..
        if (Physics.Raycast(ray, out hitInfo, distance))
        {
            //  - �浹�� ������ LR �� �߰��Ѵ�.
            AddPointToLineRenderer(hitInfo.point);

            // ---- �浹�� �༮�� ���� Unavailable Area�� �ƴ϶��
            if (hitInfo.transform.tag != "Unavailable Area")
            {
                //  - �浹�� �������� Teleport Marker ������ �θ� ���ڴ�.
                ActiveMarker(hitInfo);
            }
            //  - �浹�ߴٰ� ��ȯ�ؼ� �˷��ش�.
            return true;

        }
        // 4-B. �浹���� ���� ���
        else
        {
            //  - TeleportMarker ��Ȱ��ȭ..
            DeactiveMarker();
            //  - �浹���� �ʾҴٰ� ��ȯ�ؼ� �˷��ش�.
            return false;
        }
    }

    // ��� ���� ���� �׸� ��ġ�� LR ����Ʈ �߰�
    // - ���� ��ġ(tPos)
    private void AddPointToLineRenderer(Vector3 pos)
    {
        // ���� �浹���� ��, count(=���� �׷����� �� ����)�� LR PoisionCount���� ���ٸ�
        if (count >= lr.positionCount)
        {
            // LR positionCount�� count +1 �� ������ �����ش�.
            lr.positionCount = count + 1;
        }

        // ���� �浹�� ���� ��ġ�� LR�� �߰��ؼ� �׷��ش�.
        lr.SetPosition(count, pos);
        // ����Ʈ�� �߰��� ���Ŀ� count +1 �� ���༭ ���� ���� ��ġ�� �Ѿ���� �Ѵ�.
        count++;
    }


    // �ڷ���Ʈ ��Ŀ Ȱ��ȭ
    private void ActiveMarker(RaycastHit hitInfo)
    {

        // ����,��Ŀ�� ��Ȱ��ȭ������ ��쿡�� ��Ŀ�� Ȱ��ȭ�Ѵ�.
        if (marker.gameObject.activeSelf == false)
        {
            marker.gameObject.SetActive(true);
        }

        // ��Ŀ�� �浹�� �������� �̵���Ų��.
        marker.transform.position = hitInfo.point;
        // ��Ŀ�� ������ �����.(Z�� == normal)
        marker.transform.forward = hitInfo.normal;
    }

    // �ڷ���Ʈ ��Ŀ ��Ȱ��ȭ
    private void DeactiveMarker()
    {
        // ����, ��Ŀ�� Ȱ��ȭ ������ ��쿡�� ��Ŀ�� ��Ȱ��ȭ�Ѵ�.
        if (marker.gameObject.activeSelf == true)
        {
            marker.gameObject.SetActive(false);
        }
    }



}
