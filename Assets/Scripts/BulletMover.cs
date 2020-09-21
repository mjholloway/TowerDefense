using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMover : MonoBehaviour
{
    [SerializeField] float speed = 1.0f;

    GameObject target;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (target)
        {
            var targetPos = new Vector3(target.transform.position.x, target.transform.position.y + 4, target.transform.position.z);
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, targetPos, step);
        }
    }

    public void SetTarget(EnemyProperties enemy)
    {
        target = enemy.gameObject;
    }
}
