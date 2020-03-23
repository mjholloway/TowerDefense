using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProperties : MonoBehaviour
{
    public int hitsLeft = 5;
    // Start is called before the first frame update
    void Start()
    {
        AddBoxCollider();
    }

    private void AddBoxCollider()
    {
        Collider collider = gameObject.AddComponent<BoxCollider>();
        collider.isTrigger = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnParticleCollision(GameObject other)
    {
        hitsLeft--;

        if (hitsLeft <= 0)
        {
            Destroy(gameObject);
        }
    }
}
