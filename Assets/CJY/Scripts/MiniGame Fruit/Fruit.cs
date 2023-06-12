using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : MonoBehaviour
{
    public GameObject whole;
    public GameObject sliced;
    public ParticleSystem brokeParticle;

    private void Awake()
    {
        brokeParticle = GetComponentInChildren<ParticleSystem>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Sword"))
        {
            FindObjectOfType<MiniGameManager>().IncreaseScore();

            whole.SetActive(false);
            sliced.SetActive(true);

            brokeParticle.Play();
        }
    }
}
