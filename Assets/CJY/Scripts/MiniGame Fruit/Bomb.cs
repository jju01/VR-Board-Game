using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public GameObject bomb;

    public ParticleSystem bombParticle;

    public int points = 3;

    // Start is called before the first frame update
    void Start()
    {
        bombParticle = GetComponentInChildren<ParticleSystem>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Sword"))
        {
            MiniGameManager.Instance.DecreaseScore(points);

            bomb.SetActive(false);

            bombParticle.Play();
        }
    }
}
