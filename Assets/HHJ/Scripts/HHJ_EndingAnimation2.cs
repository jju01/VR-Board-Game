using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using Photon.Pun;
using UnityEngine.SceneManagement;
public class HHJ_EndingAnimation2 : MonoBehaviourPunCallbacks
{
    [SerializeField]
    // ����
    private Transform iceBlock;
    // ������� �� ���ΰ�
    public float iceBlockPosY;
    // �ӵ�
    public float upSpeed;

    [Space]
    [SerializeField]
    // �����׸�
    private Transform buildObj1;
    [SerializeField]
    // ����
    private Transform buildObj2;
    [SerializeField]
    // ��
    private Transform buildObj3;
    [SerializeField]
    // ����
    private Transform buildObj4;
    [SerializeField]
    // ������
    private Transform buildObj5;

    [Space]
    [SerializeField]
    // �׸� ������ ��ǥ��ġ
    private Transform buildPoint1;
    [SerializeField]
    // ������ ������ ��ǥ��ġ
    private Transform dropSpoonPos;
    [SerializeField]
    // �հ� ������ ��ġ
    private Transform dropCrownPos;
    [SerializeField]
    private Transform dropPlayerPos;

    [Space]
    // ������ �ӵ�
    public float dropSpeed;
    // ������Ʈ���� ����
    public float intervalObjY;

    // �÷��̾�
    private Transform player;
    // �÷��̾� ȸ��
    [SerializeField]
    private float rotPlayer;

    private Animator ani;

    [Space]
    [SerializeField]
    // �հ�
    private Transform crown;

    // ȸ���ӵ�
    public float rotSpeed = 50f;

    [Space]
    [SerializeField]
    private TextMeshProUGUI winnerName;
    [SerializeField]
    private Texture[] textures;


    private bool isMoveToStart = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        ani = player.GetComponent<Animator>();
        crown = GameObject.FindGameObjectWithTag("CrownObj").transform;

        // ������Ʈ ��Ȱ��ȭ
        SetActiveObj(false);

        PlayerName();
    }

    private void Update()
    {
        // �ִϸ��̼� ���
        StartCoroutine(EndingPlay());

        // ���� �ִϸ��̼��� ���� ����Ǿ��ٸ� ���� �ð� �� ���� �̵��Ѵ�.
        if ((ani.GetCurrentAnimatorStateInfo(0).IsName("metarig|Victory") &&
        ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f))
        {
            Invoke("MoveStartScene", 5f);
        }
    }

    // ������ ������ �Ʒ��� �ö�´�.
    private void UpIceBlock()
    {
        var posX = iceBlock.transform.position.x;
        var posZ = iceBlock.transform.position.z;
        var posIce = new Vector3(posX, iceBlockPosY, posZ);

        iceBlock.position = Vector3.Lerp(iceBlock.position, posIce, upSpeed * Time.deltaTime);
    }

    // ����� �÷��̾��� ĳ���Ͱ� ������ �Ʒ��� ������
    private void DropWinner()
    {
        player.gameObject.SetActive(true);
        var speed = dropSpeed * 100f;

        player.position = Vector3.MoveTowards(player.position, dropPlayerPos.position, speed * Time.deltaTime);
    }

    // �հ��� ȸ���ϸ鼭 ������
    private void DropCrown()
    {
        crown.gameObject.SetActive(true);
        // �հ� ȸ���Ѵ�.
        crown.Rotate(new Vector3(0, 0.1f, 0) * rotSpeed);

        var posY = 10f;

        // ���� �ִϸ��̼��� ������ġ �̻� ����Ǿ��ٸ�
        if ((ani.GetCurrentAnimatorStateInfo(0).IsName("metarig|Victory") &&
            ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.2f &&
            ani.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f))
        {
            // �հ��� ����Ʈ����.
            crown.position = Vector3.Lerp(crown.position, dropCrownPos.position, dropSpeed * Time.deltaTime);
        }

        // ���� �ִϸ��̼��� ������ġ�̻� ����Ǿ��ٸ�
        if ((ani.GetCurrentAnimatorStateInfo(0).IsName("metarig|Victory") &&
            ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.78f &&
            ani.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f))
        {
            ani.transform.DOLocalRotate(new Vector3(0, 135, 0), 1.5f).SetEase(Ease.Linear);

            // �հ��� ���� ���� �ø���.
            crown.position = Vector3.Lerp(crown.position,
                new Vector3(dropCrownPos.position.x, dropCrownPos.position.y + posY, dropCrownPos.position.z),
                dropSpeed * Time.deltaTime);
        }
        // ���� �ִϸ��̼��� ������ġ�̻� ����Ǿ��ٸ�
        if ((ani.GetCurrentAnimatorStateInfo(0).IsName("metarig|Victory") &&
        ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.97f &&
        ani.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f))
        {
            // �հ��� ���� ���� �ø���.
            crown.position = Vector3.Lerp(crown.position,
          new Vector3(dropCrownPos.position.x, dropCrownPos.position.y + posY * 2f, dropCrownPos.position.z),
          dropSpeed * Time.deltaTime);
        }
    }

    private void Build()
    {
        StartCoroutine(Building());
    }
    // �����ױ�
    IEnumerator Building()
    {
        buildObj1.gameObject.SetActive(true);
        buildObj2.gameObject.SetActive(true);
        buildObj3.gameObject.SetActive(true);
        buildObj4.gameObject.SetActive(true);

        var speed = dropSpeed * 100f;

        // �����׸� ������ ��ġ�� ��ǥ����
        buildObj1.position = Vector3.MoveTowards(buildObj1.position, buildPoint1.position, speed * Time.deltaTime);
        yield return new WaitForSeconds(0.5f);
        // ���� ������ ��ġ�� �����׸� ����
        buildObj2.position = Vector3.MoveTowards(buildObj2.position,
            new Vector3(buildObj1.position.x, buildObj1.position.y + intervalObjY,
            buildObj1.position.z), speed * Time.deltaTime);
        yield return new WaitForSeconds(0.5f);
        // �� ������ ������ ��������
        buildObj3.position = Vector3.MoveTowards(buildObj3.position,
            new Vector3(buildObj2.position.x, buildObj2.position.y + intervalObjY,
            buildObj2.position.z), speed * Time.deltaTime);
        yield return new WaitForSeconds(0.5f);
        // ���� ������ ������ �� ����
        buildObj4.position = Vector3.MoveTowards(buildObj4.position,
            new Vector3(buildObj3.position.x, buildObj3.position.y + intervalObjY,
            buildObj3.position.z), speed * Time.deltaTime);
        yield return new WaitForSeconds(1f);
        DropSpoon();
    }
    // ������ ������
    private void DropSpoon()
    {
        buildObj5.gameObject.SetActive(true);

        var speed = dropSpeed * 100f;
        // ������ ������ ������ ��ǥ����
        buildObj5.position = Vector3.MoveTowards(buildObj5.position, dropSpoonPos.position, speed * Time.deltaTime);
    }

    // �÷��̾� �ִϸ��̼� ���
    private void AnimationPlay()
    {
        ani.SetBool("isStart", true);
    }

    // ������Ʈ Ȱ��ȭ ����
    private void SetActiveObj(bool active)
    {
        player.gameObject.SetActive(active);
        crown.gameObject.SetActive(active);
        buildObj1.gameObject.SetActive(active);
        buildObj2.gameObject.SetActive(active);
        buildObj3.gameObject.SetActive(active);
        buildObj4.gameObject.SetActive(active);
        buildObj5.gameObject.SetActive(active);
    }

    // ����� �̸� ǥ��
    private void PlayerName()
    {
        Photon.Realtime.Player winner = (Photon.Realtime.Player)PhotonNetwork.CurrentRoom.CustomProperties["GameWinner"];
        winnerName.text = winner.NickName;

        int avatarIdx = (int)winner.CustomProperties["avatar"];
        SetWinner(avatarIdx);
    }
    public void SetWinner(int textureIdx)
    {
        Transform meshObj = player.transform.FindChildRecursive("RetopoFlow");
        SkinnedMeshRenderer mesh = meshObj.GetComponent<SkinnedMeshRenderer>();
        mesh.materials[0].mainTexture = textures[textureIdx];
    }
    // ��ŸƮ������ �̵��Ѵ�
    public void MoveStartScene()
    {
        if (isMoveToStart)
            return;

        isMoveToStart = true;

        // ��������
        PhotonNetwork.LeaveRoom();
    }

    IEnumerator EndingPlay()
    {
        yield return new WaitForSeconds(2f);
        UpIceBlock();
        yield return new WaitForSeconds(1f);
        Build();
        yield return new WaitForSeconds(2f);
        DropWinner();
        yield return new WaitForSeconds(3f);
        DropCrown();
        AnimationPlay();
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();

        // ��ŸƮ������ �̵��Ѵ�. 
        PhotonNetwork.LoadLevel("CJY 1");
    }

}
