using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    public bool activated;
    public float speed;

    float range, distTravelled;
    Vector3 startPos, upPos;
    bool descending;


    void Start ()
    {
        range = 2;
        startPos = transform.position;
	}
	
	void FixedUpdate ()
    {
		if (activated)
        {
            if (Mathf.Abs(distTravelled) < range)
            {
                Vector3 newPos = Vector3.up * speed;
                transform.Translate(newPos);
                distTravelled = (transform.position - startPos).sqrMagnitude;
            }
            else
            {
                activated = false;
                distTravelled = 0;
                upPos = transform.position;
                StartCoroutine(DescendElevator());
            }
        }
        if (descending)
        {
            if (distTravelled < range)
            {
                Vector3 newPos = -Vector3.up * speed;
                transform.Translate(newPos);
                distTravelled = (transform.position - upPos).sqrMagnitude;
            }
            else
            {
                descending = false;
                distTravelled = 0;
            }
        }
    }

    IEnumerator DescendElevator()
    {
        yield return new WaitForSeconds(5);
        descending = true;
    }
}
