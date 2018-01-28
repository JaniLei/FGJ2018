using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool activated;
    public float speed;

    float range, distTravelled;
    Vector3 startPos;

    void Start ()
    {
        startPos = transform.position;
        range = 4;
    }
	
	void Update ()
    {
        if (activated)
        {
            if (Mathf.Abs(distTravelled) < range)
            {
                Vector3 newPos = -Vector3.up * speed;
                transform.Translate(newPos);
                distTravelled = (transform.position - startPos).sqrMagnitude;
            }
            else
            {
                activated = false;
                distTravelled = 0;
            }
        }
    }
}
