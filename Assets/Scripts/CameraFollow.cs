using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject cameraTarget;

	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        Vector3 newPos = Vector3.Lerp(transform.position, cameraTarget.transform.position, Time.deltaTime * 5);
        newPos.z = -10;
        transform.position = newPos;
	}
}
