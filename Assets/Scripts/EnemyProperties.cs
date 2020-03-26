using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProperties : MonoBehaviour
{
    public int hitsLeft = 5;

    [SerializeField] ParticleSystem hitParticles;
    [SerializeField] ParticleSystem deathParticlePrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnParticleCollision(GameObject other)
    {
        hitParticles.Play();
        hitsLeft--;

        if (hitsLeft <= 0)
        {
            ParticleSystem deathFx = Instantiate(deathParticlePrefab, transform.position, Quaternion.identity);
            deathFx.Play();
            Destroy(gameObject);
        }
    }
}
