using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : MonoBehaviour
{
    public GameObject whole;
    public GameObject sliced;
    public ParticleSystem brokeParticle;

    public int points = 1;

    private void Awake()
    {
        brokeParticle = GetComponentInChildren<ParticleSystem>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Sword"))
        {
            MiniGameManager.Instance.IncreaseScore(points);

            whole.SetActive(false);
            sliced.SetActive(true);

            brokeParticle.Play();
        }
    }
}
