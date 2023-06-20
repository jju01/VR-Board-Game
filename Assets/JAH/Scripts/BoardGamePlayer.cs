using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class BoardGamePlayer : MonoBehaviour
{
    [SerializeField] private Transform camPos;
    [SerializeField] private SkinnedMeshRenderer characterRenderer;
    [SerializeField] private Texture[] textures;
    
    private PhotonView pv;
    
    void Start()
    {
        pv = GetComponent<PhotonView>();
        SetAvatar();
    }

    private void SetAvatar()
    {
        var playerProperties = pv.Owner.CustomProperties;
        if (playerProperties.ContainsKey("avatar"))
        {
            int pi = (int)playerProperties["avatar"];
            if(textures.Length > pi)
                characterRenderer.material.mainTexture = textures[pi];
        }
    }

    public void SetCamera(Transform camTr)
    {
        camTr.SetParent(camPos);
        camTr.localPosition = Vector3.zero;
        camTr.localEulerAngles = Vector3.zero;
    }


    public void SetPosition(Vector3 pos)
    {
        pv.RPC("RecvPosition", RpcTarget.All, pos);
    }

    [PunRPC]
    public void RecvPosition(Vector3 pos)
    {
        transform.position = pos;
    }
}
