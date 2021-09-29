using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{
    public NavMeshAgent agent;
    public GameObject target;

    public GameObject[] waypoints;
    int patrolWP = 0;

    public bool seek = false;
    public bool wander = false;
    public bool pursue = false;
    public bool flee = false;
    public bool patrol = false;
    public bool stop = false;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (seek) Seek();
        else if (wander) Wander();
        else if (pursue) Pursue();
        else if (flee) Flee();
        else if (patrol) Patrol();
        if (stop)
        {
            agent.ResetPath();
            stop = false;
        }
    }

    void Seek()
    {
        agent.destination = target.transform.position;
    }

    void Wander()
    {
        float radius = 0.1f;
        float offset = 3.0f;

        Vector3 localTarget = new Vector3(UnityEngine.Random.Range(-1.0f, 1.0f), 0, UnityEngine.Random.Range(-1.0f, 1.0f));
        localTarget.Normalize();
        localTarget *= radius;
        localTarget += new Vector3(0, 0, offset);

        Vector3 worldTarget = transform.TransformPoint(localTarget);
        worldTarget.y = 0.0f;

        agent.destination = worldTarget;
    }

    void Pursue()
    {
        Vector3 targetDir = target.transform.position - transform.position;
        float lookAhead = targetDir.magnitude / agent.speed;
        agent.destination = target.transform.position + target.transform.forward * lookAhead;
    }

    void Flee()
    {
        Vector3 targetDir = target.transform.position - transform.position;
        float lookAhead = targetDir.magnitude / agent.speed;
        agent.destination = -targetDir * lookAhead;
    }

    void Patrol()
    {
        if(!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            patrolWP = (patrolWP + 1) % waypoints.Length;
            agent.destination = waypoints[patrolWP].transform.position;
        }
    }

    void Hide()
    {
        Func<GameObject, float> distance = (hs) => Vector3.Distance(target.transform.position, hs.transform.position);
        GameObject hidingSpot = hidingSpots.Select(ho => (distance(ho), ho)).Min().Item2;
    }
}
