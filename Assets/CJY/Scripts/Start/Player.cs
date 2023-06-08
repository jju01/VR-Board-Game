using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class Player : MonoBehaviourPunCallbacks
{
    public static Player Instance;

    [SerializeField]
    private TextMeshProUGUI nameText;

    PhotonView pv;
    MeshRenderer characterRenderer;
    public GameObject readycheck;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        pv = GetComponent<PhotonView>();
        characterRenderer = GetComponentInChildren<MeshRenderer>();
        SetName();
    }

    private void SetName()
    {
        nameText.text = pv.Owner.NickName;
    }

    public void SetColor(Color color)
    {
        pv.RPC("ChangeColor", RpcTarget.All, color.r, color.g, color.b);
    }

    [PunRPC]
    public void ChangeColor(float r, float g, float b)
    {
        characterRenderer.material.color = new Color(r, g, b);
    }

    // 레디하면 옆에 체크 표시 뜨는 함수
    public void ReadyChecktrue()
    {
        readycheck.SetActive(true);
    }
}
