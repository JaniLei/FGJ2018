using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerControls : MonoBehaviour
{
    public float speed;
    public Slider energySlider;

    Rigidbody2D rb;
    float energy, maxEnergy;
    float currentSpeed;
    int vDirection;
    bool poweringUp;
    bool fast;

	void Start ()
    {
        rb = GetComponent<Rigidbody2D>();
        maxEnergy = 100;
        energy = maxEnergy;
        energySlider.maxValue = maxEnergy;
        energySlider.value = energy;
        currentSpeed = speed;
        vDirection = 1;
    }
	
	void Update ()
    {
        if (poweringUp)
        {
            if (energySlider.value == energy)
            {
                poweringUp = false;
                currentSpeed = speed;
            }
        }
        

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            fast = true;
            currentSpeed *= 2;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            fast = false;
            currentSpeed /= 2;
        }

        if (fast && energy > 0)
            energy -= 1;

        if (energySlider.value != energy)
            UpdateEnergyUI();
    }

    void FixedUpdate()
    {
        if (!poweringUp)
        {
            float hSpeed = Input.GetAxis("Horizontal") * currentSpeed;
            if (hSpeed != 0 && Mathf.Abs(rb.velocity.y) < 0.1f)
            {
                //Vector3 newPos = transform.position + new Vector3(hSpeed, 0);
                //rb.MovePosition(newPos);
                transform.Translate(new Vector3(hSpeed, 0, 0));
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                RayCastRight();
            }
            float vInput = Input.GetAxisRaw("Horizontal");
            if (vInput != 0)
            {
                vDirection = (int)(vInput / Mathf.Abs(vInput));
            }
        }

        /*if (Mathf.Abs(transform.rotation.z ) > 10)
        {
            float clampedZ = Mathf.Clamp(transform.rotation.z, -10, 10);
            Quaternion newRot = transform.rotation;
            newRot.z = clampedZ;
            transform.Rotate(newRot.eulerAngles);
        }*/
    }

    void RayCastRight()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position + (Vector3.right * vDirection), Vector2.right * vDirection, 1);

        if (hit.collider)
        {
            if (hit.transform.gameObject.tag != "")
                Debug.Log("Hit a " + hit.transform.gameObject.tag);

            if (hit.transform.gameObject.tag == "Door")
            {
                Door hitDoor = hit.transform.GetComponent<Door>();
                if (hitDoor)
                {
                    hitDoor.activated = true;
                    //energy -= 10;
                    ChangeEnergyValue(-10);
                }
                //Destroy(hit.transform.gameObject);
            }
            if (hit.transform.gameObject.tag == "PowerStation")
            {
                poweringUp = true;
                energy = maxEnergy;
            }
        }
    }

    void ChangeEnergyValue(int amount)
    {
        energy += amount;
        currentSpeed = speed * (energy / maxEnergy);
    }

    void UpdateEnergyUI()
    {
        if (energySlider.value > energy)
            energySlider.value--;
        else if (energySlider.value < energy)
            energySlider.value++;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Elevator testElevator = other.GetComponent<Elevator>();
        if (testElevator)
        {
            Debug.Log("overlap elevator");
            testElevator.activated = true;
            //energy -= 10;
            ChangeEnergyValue(-10);
        }
        if (other.tag == "Battery" && energy != maxEnergy)
        {
            //energy += 20;
            ChangeEnergyValue(20);
            Destroy(other.gameObject);
        }
    }
}
