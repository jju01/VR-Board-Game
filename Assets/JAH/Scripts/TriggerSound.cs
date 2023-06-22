using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 트리거 닿을 때 이펙트 사운드 재생
public class TriggerSound : MonoBehaviour
{
    private AudioSource triggerEffectaudio;

    public static TriggerSound Instance;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        triggerEffectaudio = GetComponent<AudioSource>();
    }

    public void TriggereffectPlay()
    {
        triggerEffectaudio.Stop();
        triggerEffectaudio.Play();
    }
}
