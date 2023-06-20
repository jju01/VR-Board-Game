using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

// 역할 1 : Player와 부딪혔을 때 파티클 재생
// 역할 2 : 효과음 재생 ->TriggerAudio 스크립트 가져오기
// 역할 3 : UI 띄우기
// >>  Trigger 발동!
// >>  각 Trigger에 대한 설명(3개) -- > ItemManager 스크립트에서! 
public class Trigger : MonoBehaviour
{
    // 파티클
    public ParticleSystem triggerparticle;
    // Trigger 발동! UI
    public GameObject triggerui;


    public enum Type
    {
        A, B, C
    }

    public Type type;

    private void Start()
    {

        triggerui.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Player와 Item이 부딪혔을 때
        if (other.tag == "Player" || other.name.Contains("Player"))
        {
            PhotonView pv = other.GetComponent<PhotonView>();
            if (pv == null || pv.IsMine == false)
                return;


            if (GameManager.Instance.MyDice.moveValue <= 1)
            {
            
                triggerparticle.transform.position = gameObject.transform.position;
                // 파티클 재생 
                triggerparticle.Stop();
                triggerparticle.Play();

                // 오디오 재생

                // triggerui 애니메이션 실행
                StartCoroutine(TriggerAnim());
            }
        }
    }

    IEnumerator TriggerAnim ()
    {
        // 1) triggerui를 활성화한다.
        // Trigger발동! UI 활성화
        // >> 위치: 나자신, 방향: 플레이어쪽으로
        triggerui.SetActive(true);
        triggerui.transform.position = gameObject.transform.position + Vector3.forward * 2f;
        transform.LookAt(GameManager.Instance.MyPlayer.transform);

        // 2) 2초 기다림
        yield return new WaitForSeconds(2.0f);

        // 3) triggerui를 비활성화한다.
        triggerui.SetActive(false);

        // ItemManager 데이터 가져온다
        ItemManager IM = FindObjectOfType<ItemManager>();

        //4) Trigger_1~3 설명 UI 활성화
        switch (type)
        {
            // 만일 Item type이 A라면, GItem UI 활성화 + 플레이어 쪽으로 방향 설정..
            case Type.A:
                IM.TriggerUI[0].SetActive(true);
                IM.TriggerUI[0].transform.position = gameObject.transform.position + Vector3.forward * 2f;
                IM.TriggerUI[0].transform.LookAt(GameManager.Instance.MyPlayer.transform);
                yield return new WaitForSeconds(2.0f);
                IM.TriggerUI[0].SetActive(false);
                break;
            case Type.B:
                IM.TriggerUI[1].SetActive(true);
                IM.TriggerUI[1].transform.position = gameObject.transform.position + Vector3.forward * 2f;
                IM.TriggerUI[1].transform.LookAt(GameManager.Instance.MyPlayer.transform);
                yield return new WaitForSeconds(2.0f);
                IM.TriggerUI[1].SetActive(false);
                break;
            case Type.C:
                IM.TriggerUI[2].SetActive(true);
                IM.TriggerUI[2].transform.position = gameObject.transform.position + Vector3.forward * 2f;
                IM.TriggerUI[2].transform.LookAt(GameManager.Instance.MyPlayer.transform);
                yield return new WaitForSeconds(2.0f);
                IM.TriggerUI[2].SetActive(false);
                break;
        }
    }
}
