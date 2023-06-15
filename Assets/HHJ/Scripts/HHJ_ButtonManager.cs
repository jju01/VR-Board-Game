using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Realtime;
using Photon.Pun;

public class HHJ_ButtonManager : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private Texture[] textures;

    public TextMeshProUGUI playerName;


    
    // 버튼을 누르면 해당 플레이어 생성
    public void OnCilckPlayer1()
    {
        SetWinner("plaeyr1", 0);
    }

    public void OnCilckPlayer2()
    {
        SetWinner("plqye2", 1);
    }

    public void OnCilckPlayer3()
    {
        SetWinner("pae3", 2);
    }

    public void OnCilckPlayer4()
    {
        SetWinner("pdkl4", 3);
    }

    public void SetWinner(string name, int textureIdx)
    {
        playerName.text = name;

        Transform meshObj = player.transform.FindChildRecursive("RetopoFlow");
        SkinnedMeshRenderer mesh = meshObj.GetComponent<SkinnedMeshRenderer>();
        mesh.materials[0].mainTexture = textures[textureIdx];
    }
}
