using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FSM : MonoBehaviour
{
    // Get reference for the cop position and the GameObject from the treasure
    public Transform cop;
    public GameObject treasure;
    public GameObject robber;

    // Input the distance where the robber is able to steal the treasure
    public float distanceToSteal = 10.0f;

    // Store an agent
    public NavMeshAgent agent;

    private WaitForSeconds wait = new WaitForSeconds(0.05f);
    delegate IEnumerator State();
    private State state;

    // Reference to the script move from the robber
    public Moves move;
    
    IEnumerator Start()
    {
        state = Wander;
        while (enabled)
            yield return StartCoroutine(state());
    }

    IEnumerator Wander()
    {
        Debug.Log("Wander State");

        while(Vector3.Distance(cop.position, treasure.transform.position) < 5.0f)
        {
            move.Wander();
            yield return wait;
        }

        state = Approach;
    }

    IEnumerator Approach()
    {
        while(Vector3.Distance(cop.position, treasure.transform.position) > 10.0f)
        {
            move.Seek(treasure.transform.position);
            yield return wait;
        }

        if (Vector3.Distance(robber.transform.position, treasure.transform.position) <= distanceToSteal)
        {
            Object.Destroy(treasure);
            state = Hide;
        }
        else
        {
            state = Wander;
        }
    }

    IEnumerator Hide()
    {
        yield return wait;
    }
}
