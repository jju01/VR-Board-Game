using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ��� ������ ���� �� ���� ȿ���� ��� 
public class ItemEffectSound : MonoBehaviour
{
    private AudioSource itemEffectaudio;

    public static ItemEffectSound Instance;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        itemEffectaudio = GetComponent<AudioSource>();
    }

    public void ItemSoundPlay()
    {
        itemEffectaudio.Stop();
        itemEffectaudio.Play();
    }
}
