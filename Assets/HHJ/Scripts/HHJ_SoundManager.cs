using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 오디오 관리한다.
// 플레이어가 목표지점에 도착 시 사운드 재생하기
// 왕관이 특정위치에 도달할 시 사운드 재생하기
// 왕관을 들었을 때 사운드 재생하기
// 시작할 때 사운드 재생하기
// 블럭이 떨어졌을 때 마다 사운드 재생하기
// 숟가락 떨어졌을 때 사운드 재생하기

//싱글톤으로 관리

public class HHJ_SoundManager : MonoBehaviour
{

    public static HHJ_SoundManager Instance;
    [Header("얼음올라가는 소리")]
    public AudioSource upIce;
    public AudioClip iceSound;

    [Space]
    [Header("시작사운드")]
    [SerializeField]
    private AudioSource startSound;

    [Space]
    [Header("우승자 떨어지는 소리")]
    [SerializeField]
    private AudioSource dropPlayer;
    public AudioClip playerSound;

    [Space]
    [Header("왕관 떨어지는 소리")]
    [SerializeField]
    private AudioSource dropCrown;
    public AudioClip crownSound;

    [Space]
    [Header("빙수 떨어지는 소리")]
    [SerializeField]
    private AudioSource dropObject;
    public AudioClip objSound;

    [Space]
    [Header("숟가락 떨어지는 소리")]
    [SerializeField]
    private AudioSource dropSpoon;
    public AudioClip spoonSound;

    void Start()
    {
        Instance = this;

        startSound.Play();
    }
    // 얼음 올라오는 소리
    public void UpIceSound()
    {
        upIce.PlayOneShot(iceSound);  
    }
    // 플레이어 떨어지는 소리
    public void DropPlayerSound()
    {
        dropPlayer.PlayOneShot(playerSound);
    }
    // 왕관 떨어지는 소리
    public void DropCrownSound()
    {
        dropCrown.PlayOneShot(crownSound);
    }
    // 블럭 떨어지는 소리 
    public void DropObjectSound()
    {
        dropObject.PlayOneShot(objSound);
    }
    // 숟가락 떨어지는 소리
    public void DropSpoonSound()
    {
        dropSpoon.PlayOneShot(spoonSound);
    }

}
