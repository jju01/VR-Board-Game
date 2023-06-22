using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Ʈ���� ���� �� ����Ʈ ���� ���
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
