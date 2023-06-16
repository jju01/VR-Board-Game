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
    //  - Bezier 커브의 기준점 3가지 (p0,p1,p2)
    private Vector3 p0, p1, p2;
    //  - 곡선의 거리 (Far)
    public float far = 5f;
    //  - 곡선의 높낮이 (down)
    public float down = -2f;
    //  - 곡선의 점의 개수
    public int dotCount = 2;
    //  - 충돌한 지점까지 그려지는 곡선들의 점의 개수
    private int count = 0;


    [Space]
    //  - Marker Target
    public GameObject Target;
    //  - Teleport Marker
    public GameObject marker;
    // 순간이동 지점에서 Y축으로 약간의 여유분 두고 이동
    public float offsetY = 1.5f;
    // - Unavailable Area
    public GameObject[] unavailableAreaArray;

    [Space]
    // LineRenderer 색상
    public Color startColor = Color.white;
    public Color endColor = Color.white;

    [Space]
    //  - 충돌체크를 위한 RigidBody
    private Rigidbody rb;
    //  - 잡을 수 있는 범위
    public float catchRadius = 0.5f;
    //  - 잡은 물체가 뭐야?
    private GameObject catchObj;
    [Space]
    //  - 손의 위치(Left? or Right?)
    public PlayerHand m_hand;
    //어느 손인지에 따라 OVR 컨트롤러 다르게 설정
    OVRInput.Controller controllerTouch;

    // Start is called before the first frame update
    void Start()
    {
        // Play하면, 나한테 붙어있는 Rigidbody 가져온다.
        rb = GetComponent<Rigidbody>();
        // 왼쪽 손일 때, 오른쪽 손일때에 따라 OVR Touch 방향 설정
        if (VRManager.Instance.useVRController)
        {
            switch (m_hand)
            {
                case PlayerHand.LEFT: controllerTouch = OVRInput.Controller.LTouch; break;
                case PlayerHand.RIGHT: controllerTouch = OVRInput.Controller.RTouch; break;
            }
        }
        // LineRenderer 컴포넌트 가져와서 초기화한다.
        lr = GetComponent<LineRenderer>();
        // LineRenderer 색상 초기화
        lr.startColor = startColor;
        lr.endColor = endColor;
        // LineRenderer 굵기 초기화
        lr.startWidth = 0.01f;


        // LineRenderer는 꺼진 상태로 시작
        lr.enabled = false;
        // Telepoint Marker는 꺼진 상태로 시작
        //DeactiveMarker();
    }

    // Update is called once per frame
    void Update()
    {
        //잡기,놓기
        HandController();
        //LR 활성화
        lr.enabled = this;
        // 위치 갱신
        PositionWithHand();
        //텔레포트 라인 그리기
        DrawCurve();

    }

    //Controller 혹은 테스트일때 물건을 잡고 놓는 함수
    private void HandController()
    {
        // A. VR Controller 사용
        if (VRManager.Instance.useVRController)
        {
            if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, controllerTouch) && catchObj == null)
            {
                CatchObj();
                //LR 활성화
                lr.enabled = this;
            }
            if (catchObj != null)
            {
                // 위치 갱신
                PositionWithHand();
                //텔레포트 라인 그리기
                DrawCurve();

            }
            if (OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger, controllerTouch) && catchObj != null)
            {
                DropObj();
                // LR 비활성화
                lr.enabled = false;
                // 텔레포트 마커 비활성화
                DeactiveMarker();
            }
        }
        // B. VR Controller 미사용 -- TEST
        else
        {
            // 1. 내가 마우스 오른쪽 버튼을 누른 순간, catchObject 비어있다면 잡기
            if (Input.GetMouseButtonDown(1) && catchObj == null)
            {
                CatchObj();
                //LR 활성화
                lr.enabled = this;
            }
            if (catchObj != null)
            {
                // 위치 갱신
                PositionWithHand();
                //텔레포트 라인 그리기
                DrawCurve();

            }
            // 2. 내가 마우스 오른쪽 버튼을 뗀 순간, catchObject 있다면 놓기
            if (Input.GetMouseButtonUp(1) && catchObj != null && marker.activeSelf == true)
            {
                DropObj();
                // LR 비활성화
                lr.enabled = false;
                // 텔레포트 마커 비활성화
                DeactiveMarker();
            }
        }
    }


    // 1. 잡기
    // Hand를 기준으로 catchRadius 만큼의 반지름을 가진 범위 안에 
    // 뮬체가 들어와 있는 경우, 그 물체를 내 손의 자식으로 놓는다.
    private void CatchObj()
    {
        //catchRadius만큼의 범위 안에 있는 몰체를 모두 검색해 가장 가까운 물체를 잡는다.
        // - 검색 기준 위치 = 마커
        Vector3 position = marker.transform.position;
        // - 얼마만큼의 반지름(= 범위)를 가진 Sphere를 만들지
        float radius = catchRadius;
        // - 특정 Laywe (아이템레이어)
        int layerMask = 1 << LayerMask.NameToLayer("Item");
        // - 검색한 녀석들(=배열형태)을 모두 담는다
        Collider[] hits = Physics.OverlapSphere(position, radius, layerMask);

        //1. 가장 내손에서 가까운 녀석의 index를 담을 변수
        int selectedIndex = -1;
        // 방어코드 == 검색한 녀셕이 1개 이상이라면
        if (hits != null && hits.Length > 0)
        {
            // 검색된 녀석들 중에서 가장 가까이 있는 녀석을 내 손에 자식으로 담는다.
            //2. 부딪힌 녀석이 있다면, 0번째 녀석의 거리부터 검사시작
            selectedIndex = 0;
            //3. 부딪힌 녀석들 중에 가장 짧은 거리의 녀석 선택할 수 있게 반복 검사
            for (int i = 0; i < hits.Length; i++)
            {
                //4.비교를 통해 짧은 녀석의 index를 구한다.
                // - 현재 녀석의 거리 (손- 부딪힌 물체)
                float currentDis = Vector3.Distance(marker.transform.position, hits[selectedIndex].transform.position);
                // - 다음 녀셕의 거리
                float nextDis = Vector3.Distance(marker.transform.position, hits[i].transform.position);
                // a. 현재 선택된 녀석과, 다음 부딪힌 녀석을 비교한다.
                if (currentDis > nextDis)
                {
                    // b. 그 중 거리를 비교해서 더 작은 녀석을 selectedIndex에 넣어준다.
                    selectedIndex = i;
                }

            }

        }
        //만일,selectedIndex에 들어가 있는 값이 -1이 아니라면?
        //-> 충돌한 녀석이 있었다면?
        if (selectedIndex != -1)
        {
            catchObj = hits[selectedIndex].gameObject;
            // 2. 부모-자식 : 캐치오브젝트에 들어가 있는 물체를 손의 자식으로 설정한다.
            catchObj.transform.parent = Target.transform;
            // +타겟의 위치로 맞춰줌
            catchObj.transform.position = Target.transform.position;
            // +부모가 바라보는 방향과 똑같은 방향으로
            catchObj.transform.rotation = transform.rotation;


        }
    }

    private void DropObj()
    {
        // 잡은 물체를 커브끝으로 이동시킨다.
        catchObj.transform.position = marker.transform.position;
        // 잡은 물체의 Rotation 값의 X,Z를 0으로만든다.
        Quaternion catchObjRot = new Quaternion(0, catchObj.transform.rotation.y, 0, catchObj.transform.rotation.w);
        catchObj.transform.rotation = catchObjRot;
        //잡은 물체 부모를 없애준다.
        catchObj.transform.parent = null;
        //2. 잡은 물체 놓아준다 
        catchObj = null;

    }
    private void PositionWithHand()
    {
        // 1. 시작 위치 : PlayerHand의 위치
        p0 = hand.position;
        // 2. 중간 위치 : [시작 위치] 기준으로 Hand가 바라보는 방향으로 far만큼 떨어진 위치
        p1 = p0 + hand.forward * far;
        // 3. 끝 위치   : [중간위치] 기준으로 Hand가 바라보는 방향으로 far의 1/2만큼 떨어지고,
        //                아래로 down만큼 내려간 위치
        p2 = p1 + hand.forward * far * 0.5f + Vector3.up * down;
    }
    // 텔레포트 라인 그리기
    private void DrawCurve()
    {
        // 곡선을 그리기 위해서 알아보기 쉽도록 변수의 이름을 바꿔서 진행
        Vector3 start = p0;
        Vector3 center = p1;
        Vector3 end = p2;

        // 1. 충돌한 위치와 방향을 구하기 위해서 점의 이전 위치
        Vector3 prePos = Vector3.zero;
        // 2. 충돌한 지점의 점의 개수는 0부터 +1 씩 카운트
        count = 0;

        // Bezier곡선 이용해서 dotCount만큼의 점들을 선으로 연결
        for (int i = 0; i < dotCount; i++)
        {
            // 1. t 의 값을 구한다.
            float t = i / (float)dotCount;
            // 2. t 의 위치를 구한다. => Bezier함수 사용
            Vector3 tPos = Bezier(start, center, end, t);

            // 3. 곡선의 진행방향에서 충돌이 일어났는지 체크
            // 3-1. 부딪힌 경우, 
            if (i > 0 && IsHit(prePos, tPos) == true)
            {
                //  - 부딪힌 곳까지만 점을 그리고 더이상 그리지 않는다.
                lr.positionCount = count;
                return;
            }
            // 3-2. 부딪히지 않은 경우
            else
            {
                //  - 곡선의 다음 점이 그려질 위치에, LR 점 추가
                AddPointToLineRenderer(tPos);
            }

            // 4. 이전 위치를, 현재 위치로 갱신
            prePos = tPos;
        }

        // 5. 모두 부딪힘 없이 그려진 경우, LR이 가진 총 PositionCount 는 count 개수와 같음.
        lr.positionCount = count;
    }

    // Bezier 함수 : T 의 위치를 알려주는 역할
    private Vector3 Bezier(Vector3 p0, Vector3 p1, Vector3 p2, float t)
    {
        // p0 - p1 거리 사이의 선형보간 t 위치 구하기
        Vector3 p0p1 = Vector3.Lerp(p0, p1, t);
        // p1 - p2 거리 사이의 선형보간 t 위치 구하기
        Vector3 p1p2 = Vector3.Lerp(p1, p2, t);
        // p0p1 - p1p2 거리 사이의 선형보간 t 위치 구하기
        Vector3 tPos = Vector3.Lerp(p0p1, p1p2, t);
        // 해당 결과값 반환
        return tPos;
    }
    // 점(pre)과 점(cur) 사이의 충돌이 일어났는지 체크해주는 함수
    private bool IsHit(Vector3 prePos, Vector3 pos)
    {
        //  - 방향 (prePos가 pos를 바라보는 방향) & 정규화
        Vector3 direction = (pos - prePos).normalized;
        //  - 거리 (prePos와 pos 사이의 거리)
        float distance = Vector3.Distance(pos, prePos);
        // 1. Ray를 만듭니다.(위치&방향)
        Ray ray = new Ray(prePos, direction);
        // 2. RayCastHit 충돌 그릇을 만듭니다.
        RaycastHit hitInfo = new RaycastHit();
        // 3. Ray를 발사합니다. (다음 점까지의 거리만큼)
        // 4-A. 만일 충돌한 경우..
        if (Physics.Raycast(ray, out hitInfo, distance))
        {
            //  - 충돌한 지점에 LR 점 추가한다.
            AddPointToLineRenderer(hitInfo.point);

            // ---- 충돌한 녀석이 만일 Unavailable Area가 아니라면
            if (hitInfo.transform.tag != "Unavailable Area")
            {
                //  - 충돌한 지점에다 Teleport Marker 가져다 두면 좋겠다.
                ActiveMarker(hitInfo);
            }
            //  - 충돌했다고 반환해서 알려준다.
            return true;

        }
        // 4-B. 충돌하지 않은 경우
        else
        {
            //  - TeleportMarker 비활성화..
            DeactiveMarker();
            //  - 충돌하지 않았다고 반환해서 알려준다.
            return false;
        }
    }

    // 곡선의 다음 점을 그릴 위치에 LR 포인트 추가
    // - 현재 위치(tPos)
    private void AddPointToLineRenderer(Vector3 pos)
    {
        // 만일 충돌했을 때, count(=실제 그려지는 점 개수)가 LR PoisionCount보다 많다면
        if (count >= lr.positionCount)
        {
            // LR positionCount와 count +1 의 개수를 맞춰준다.
            lr.positionCount = count + 1;
        }

        // 실제 충돌한 곳에 위치를 LR에 추가해서 그려준다.
        lr.SetPosition(count, pos);
        // 포인트에 추가한 이후에 count +1 을 해줘서 다음 점의 위치로 넘어가도록 한다.
        count++;
    }


    // 텔레포트 마커 활성화
    private void ActiveMarker(RaycastHit hitInfo)
    {

        // 만일,마커가 비활성화상태인 경우에만 마커를 활성화한다.
        if (marker.gameObject.activeSelf == false)
        {
            marker.gameObject.SetActive(true);
        }

        // 마커를 충돌한 지점으로 이동시킨다.
        marker.transform.position = hitInfo.point;
        // 마커의 방향을 맞춘다.(Z축 == normal)
        marker.transform.forward = hitInfo.normal;
    }

    // 텔레포트 마커 비활성화
    private void DeactiveMarker()
    {
        // 만일, 마커가 활성화 상태인 경우에만 마커를 비활성화한다.
        if (marker.gameObject.activeSelf == true)
        {
            marker.gameObject.SetActive(false);
        }
    }



}
