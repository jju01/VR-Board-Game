using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : MonoBehaviour
{
    public GameObject whole;
    public GameObject sliced;
    public ParticleSystem brokeParticle1;
    public ParticleSystem brokeParticle2;
    public ParticleSystem brokeParticle3;
    public ParticleSystem brokeParticle4;

    public int points = 1;

    private void Awake()
    {
        brokeParticle1 = GetComponentInChildren<ParticleSystem>();
        brokeParticle2 = GetComponentInChildren<ParticleSystem>();
        brokeParticle3 = GetComponentInChildren<ParticleSystem>();
        brokeParticle4 = GetComponentInChildren<ParticleSystem>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Sword"))
        {
            MiniGameManager.Instance.IncreaseScore(points);

            whole.SetActive(false);
            sliced.SetActive(true);

            brokeParticle1.Play();
            brokeParticle2.Play();
            brokeParticle3.Play();
            brokeParticle4.Play();
        }
    }
}
