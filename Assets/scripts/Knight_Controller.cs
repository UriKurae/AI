using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight_Controller : MonoBehaviour
{
    public GameObject target;
   
    float freq = 0.0f;

    private bool isSeeking = false;
    public float stopDistance = 2.0f;

    public float turnSpeed = 5.0f;
    public float maxTurnSpeed = 5.0f;
    public float maxVelocity = 20.0f;
    public float acceleration = 5.0f;
    public float movSpeed = 5.0f;
    public float maxSpeed = 5.0f;
    public float turnAcceleration = 5.0f;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.S)) isSeeking = !isSeeking;

        if (isSeeking)
        {
            freq += Time.deltaTime;
            if (freq > 0.5)
            {
                freq -= 0.5f;

            }
            //Seek(target);
            //Flee(target);
            SteeringSeek(target);
        }

       

    }

    void Flee(GameObject target)
    {
        Vector3 direction = this.transform.position - target.transform.position;
        direction.y = 0.0f;

        Vector3 movement = direction.normalized * maxVelocity;
        float angle = Mathf.Rad2Deg * Mathf.Atan2(movement.x, movement.z);
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.up);

        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * turnSpeed);
        transform.position += transform.forward.normalized * maxVelocity * Time.deltaTime;

        Debug.Log("I am seeking");
    }

    void Seek(GameObject target)
    {
        Vector3 direction = target.transform.position - this.transform.position;
        direction.y = 0.0f;

        Vector3 movement = direction.normalized * maxVelocity;
        float angle = Mathf.Rad2Deg * Mathf.Atan2(movement.x, movement.z);
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.up);

        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * turnSpeed);
        transform.position += transform.forward.normalized * maxVelocity * Time.deltaTime;

        Debug.Log("I am seeking");
    }

    void SteeringSeek(GameObject target)
    {
        if (Vector3.Distance(target.transform.position, transform.position) < stopDistance) return;

        Vector3 direction = target.transform.position - this.transform.position;
        direction.y = 0.0f;

        Vector3 movement = direction.normalized * acceleration;
        float angle = Mathf.Rad2Deg * Mathf.Atan2(movement.x, movement.z);
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.up);

        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * turnSpeed);
        transform.position += transform.forward.normalized * movSpeed * Time.deltaTime;

        turnSpeed += turnAcceleration * Time.deltaTime;
        turnSpeed = Mathf.Min(turnSpeed, maxTurnSpeed);
        movSpeed += acceleration * Time.deltaTime;
        movSpeed = Mathf.Min(movSpeed, maxSpeed);

        

        Debug.Log("I am seeking");

  
    }
}
