using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProperties : MonoBehaviour
{
    public int hitsLeft = 5;

    [SerializeField] ParticleSystem hitParticles;
    [SerializeField] ParticleSystem deathParticlePrefab;
    [SerializeField] AudioClip hitSfx;
    [SerializeField] AudioClip deathSoundFx;

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
        GetComponent<AudioSource>().PlayOneShot(hitSfx);

        if (hitsLeft == 0)
        {
            ProcessEnemyDeath();
        }
    }

    public void ProcessEnemyDeath()
    {
        AudioSource.PlayClipAtPoint(deathSoundFx, new Vector3(23.5f, 80f, -64.1f));
        ParticleSystem deathFx = Instantiate(deathParticlePrefab, transform.position, Quaternion.identity);
        deathFx.Play();
        Destroy(deathFx.gameObject, deathFx.main.duration);
        Destroy(gameObject);
    }
}
