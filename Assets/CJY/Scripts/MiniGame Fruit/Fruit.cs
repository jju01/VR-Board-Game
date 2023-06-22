using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : MonoBehaviour
{
    public GameObject whole;
    public GameObject sliced;
    public ParticleSystem[] brokeParticle;

    public int points = 1;

    private void Awake()
    {
        brokeParticle = GetComponentsInChildren<ParticleSystem>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Sword"))
        {
            MiniGameManager.Instance.IncreaseScore(points);

            whole.SetActive(false);
            sliced.SetActive(true);
            
            foreach(ParticleSystem particle in brokeParticle)
                particle.Play();

            //for(int i = 0; i < brokeParticle.Length; i++)
            //    brokeParticle[i].Play();
        }
    }
}
