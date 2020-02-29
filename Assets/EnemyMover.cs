using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMover : MonoBehaviour
{
    [SerializeField] List<Waypoint> path;

    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(FollowPath());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator FollowPath()
    {
        print("Starting patrol...");
        foreach (Waypoint waypoint in path)
        {
            print("Visiting block " + waypoint.name);
            yield return new WaitForSeconds(1f);
            transform.position = waypoint.transform.position;
            
        }
        print("Ending Patrol...");
    }
}
