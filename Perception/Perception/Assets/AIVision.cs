using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIVision : MonoBehaviour
{
	public Camera frustum;
	public LayerMask mask;
	public Animator animator;
	public GameObject[] zombies;
	private GameObject target;
	private bool isSeeking = false;

	private void Start()
	{
		zombies = GameObject.FindGameObjectsWithTag("Zombie");
	}

    float freq = 0.0f;

	public float stopDistance = 2.0f;

	public float turnSpeed = 5.0f;
	public float maxTurnSpeed = 5.0f;
	public float maxVelocity = 20.0f;
	public float acceleration = 5.0f;
	public float movSpeed = 5.0f;
	public float maxSpeed = 5.0f;
	public float turnAcceleration = 5.0f;

	void Update()
	{
		freq += Time.deltaTime;
		if (freq < 0.5f)
		{
			freq = 0.0f;
			Sigh();

			if(isSeeking)
            {
				ApplySeek(target);
				animator.SetBool("Run Forward", true);
			}
			else
            {
				animator.SetBool("Run Forward", false);
			}	
        }
	}

	void Sigh()
    {
		Collider[] colliders = Physics.OverlapSphere(transform.position, frustum.farClipPlane, mask);
		Plane[] planes = GeometryUtility.CalculateFrustumPlanes(frustum);

		foreach (Collider col in colliders)
		{
			if (col.gameObject != gameObject && GeometryUtility.TestPlanesAABB(planes, col.bounds))
			{
				RaycastHit hit;
				Ray ray = new Ray();
				ray.origin = transform.position;
				ray.direction = (col.transform.position - transform.position).normalized;
				ray.origin = ray.GetPoint(frustum.nearClipPlane);

				if (Physics.Raycast(ray, out hit, frustum.farClipPlane, mask))
					if (hit.collider.gameObject.CompareTag("Chicken"))
					{
						Debug.Log("Found chicken");
						for (int i = 0; i < zombies.Length; ++i)
						{
							zombies[i].SendMessage("IsSeeking", hit.collider.gameObject);
						}
					}
			}
		}
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
	}

	void ApplySeek(GameObject target)
    {
		Vector3 direction = target.transform.position - this.transform.position;
		direction.y = 0.0f;

		Vector3 movement = direction.normalized * maxVelocity;
		float angle = Mathf.Rad2Deg * Mathf.Atan2(movement.x, movement.z);
		Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.up);

		transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * turnSpeed);
		transform.position += transform.forward.normalized * maxVelocity * Time.deltaTime;

		animator.SetBool("Run Forward", true);
	}

	void IsSeeking(GameObject obj)
    {
		isSeeking = true;
		target = obj;
    }
}
