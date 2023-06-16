using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class BoardGamePlayer : MonoBehaviour
{
    [SerializeField] private Transform camPos;

    private PhotonView pv;
    
    void Start()
    {
        pv = GetComponent<PhotonView>();
    }

    public void SetCamera(Transform camTr)
    {
        camTr.SetParent(camPos);
        camTr.localPosition = Vector3.zero;
        camTr.localEulerAngles = Vector3.zero;
    }
}
