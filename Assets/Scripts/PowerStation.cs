using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerStation : MonoBehaviour
{
    public Sprite onSprite;
    [HideInInspector]public bool active;
    

	void Start ()
    {
        active = false;
	}

    public void DeActivate()
    {
        GetComponent<SpriteRenderer>().sprite = onSprite;
        active = true;
    }
}
