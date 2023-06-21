using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ����� �����Ѵ�.
// �÷��̾ ��ǥ������ ���� �� ���� ����ϱ�
// �հ��� Ư����ġ�� ������ �� ���� ����ϱ�
// �հ��� ����� �� ���� ����ϱ�
// ������ �� ���� ����ϱ�
// ���� �������� �� ���� ���� ����ϱ�
// ������ �������� �� ���� ����ϱ�

//�̱������� ����

public class HHJ_SoundManager : MonoBehaviour
{

    public static HHJ_SoundManager Instance;
    [Header("�����ö󰡴� �Ҹ�")]
    public AudioSource upIce;
    public AudioClip iceSound;

    [Space]
    [Header("���ۻ���")]
    [SerializeField]
    private AudioSource startSound;

    [Space]
    [Header("����� �������� �Ҹ�")]
    [SerializeField]
    private AudioSource dropPlayer;
    public AudioClip playerSound;

    [Space]
    [Header("�հ� �������� �Ҹ�")]
    [SerializeField]
    private AudioSource dropCrown;
    public AudioClip crownSound;

    [Space]
    [Header("���� �������� �Ҹ�")]
    [SerializeField]
    private AudioSource dropObject;
    public AudioClip objSound;

    [Space]
    [Header("������ �������� �Ҹ�")]
    [SerializeField]
    private AudioSource dropSpoon;
    public AudioClip spoonSound;

    void Start()
    {
        Instance = this;

        startSound.Play();
    }
    // ���� �ö���� �Ҹ�
    public void UpIceSound()
    {
        upIce.PlayOneShot(iceSound);  
    }
    // �÷��̾� �������� �Ҹ�
    public void DropPlayerSound()
    {
        dropPlayer.PlayOneShot(playerSound);
    }
    // �հ� �������� �Ҹ�
    public void DropCrownSound()
    {
        dropCrown.PlayOneShot(crownSound);
    }
    // �� �������� �Ҹ� 
    public void DropObjectSound()
    {
        dropObject.PlayOneShot(objSound);
    }
    // ������ �������� �Ҹ�
    public void DropSpoonSound()
    {
        dropSpoon.PlayOneShot(spoonSound);
    }

}
