using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class Player : MonoBehaviourPunCallbacks
{
    public static Player Instance;

    [SerializeField]
    private TextMeshProUGUI nameText;

    PhotonView pv;
    [SerializeField]
    SkinnedMeshRenderer characterRenderer;
    public GameObject readycheck;

    public Texture[] textures;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        pv = GetComponent<PhotonView>();
        SetName();
    }

    private void SetName()
    {
        nameText.text = pv.Owner.NickName;
    }

    public void SetColor(int i)
    {
        foreach (Photon.Realtime.Player p in PhotonNetwork.CurrentRoom.Players.Values)
        {
            if (p.CustomProperties.ContainsKey("avatar"))
            {
                int pi = (int)p.CustomProperties["avatar"];
                if (i == pi)
                {
                    return;
                }
            }
            print("선택됨");
        }

        pv.RPC("ChangeColor", RpcTarget.All, i);

        Hashtable CustomProperties = new Hashtable();

        CustomProperties.Add($"avatar", i);

        PhotonNetwork.SetPlayerCustomProperties(CustomProperties);
    }

    [PunRPC]
    public void ChangeColor(int i)
    {
        characterRenderer.material.mainTexture = textures[i];
    }

    // 레디하면 옆에 체크 표시 뜨는 함수
    public void ReadyChecktrue()
    {
        pv.RPC("ReadyIcorn", RpcTarget.All);
    }


    [PunRPC]
    public void ReadyIcorn()
    {
        readycheck.SetActive(true);
    }
}
