using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerControls : MonoBehaviour
{
    public float speed;
    public Slider energySlider;
    public Image img1, img2, img3, bg;
    public GameObject spr;


    Rigidbody2D rb;
    Animator anim;
    float energy, maxEnergy;
    float currentSpeed;
    int vDirection;
    bool poweringUp;
    bool fast;
    PowerStation hitStation;

	void Start ()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
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
                currentSpeed = 0;
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

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
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
                if (img1.gameObject.activeInHierarchy)
                {
                    img1.gameObject.SetActive(false);
                    bg.gameObject.SetActive(false);
                    //img2.gameObject.SetActive(true);
                }
                else if (img2.gameObject.activeInHierarchy)
                {
                    img2.gameObject.SetActive(false);
                    img3.gameObject.SetActive(true);
                }
                else if (img3.gameObject.activeInHierarchy)
                {
                    Application.LoadLevel(Application.loadedLevel);
                }
                //anim.Play("robo_arm_anim");
                RayCastRight();
            }
            float vInput = Input.GetAxisRaw("Horizontal");
            if (vInput != 0)
            {
                vDirection = (int)(vInput / Mathf.Abs(vInput));
                spr.transform.localScale = new Vector3(vDirection, 1, 1);
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
        RaycastHit2D hit = Physics2D.Raycast(transform.position + ((Vector3.right * vDirection)/2), Vector2.right * vDirection, 1);

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
                hitStation = hit.transform.GetComponent<PowerStation>();
                if (hitStation)
                {
                    if (!hitStation.active)
                    {
                        StartCoroutine(ActivatePowerStation());
                    }
                }
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
    
    IEnumerator ActivatePowerStation()
    {
        anim.SetTrigger("Arm");
        yield return new WaitForSeconds(.5f);

        poweringUp = true;
        energy = 0;
        hitStation.DeActivate();
        hitStation.GetComponent<BoxCollider2D>().enabled = false;
        hitStation = null;
        
        StartCoroutine(EndGame());
    }

    IEnumerator EndGame()
    {
        yield return new WaitForSeconds(3);

        bg.gameObject.SetActive(true);
        img2.gameObject.SetActive(true);
    }
}
