using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 재료 아이템 먹을 때 마다 효과음 재생 
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
