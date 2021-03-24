
using System.Collections;
using System.Threading;
using UnityEngine;






public class Controls : MonoBehaviour
{
    public float health = 100.0f;
    public float rotationSpeed = 100.0F;
    public float walkingSpeed = 2.0f;
    public float runningSpeed = 7.0f;
    public float acceleration = 0.2f;
    public int cellsCollected = 0;
    public static float clipLength = 3;

    private Animator anim;
    private CharacterController characterController;
    private bool canPickUp = false;
    private float currentSpeed = 0;   
    private GameObject objectToPickUp;
    
    void Start()
    {
        anim = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Item")
        {
            canPickUp = true;
            objectToPickUp = other.gameObject.transform.parent.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        canPickUp = false;
        
    }

    // Update is called once per frame

    bool CheckIfDead()
    {
        if (health <= 0)
        {
            anim.SetTrigger("isDead");
            return true;
        }
        return false;
    }

    void PickUpItem()
    {
        if (canPickUp == true)
        {
            if (Input.GetKeyDown("e"))
            {
               
                anim.SetTrigger("isCollecting");
                Destroy(objectToPickUp, 1.5f);
                cellsCollected++;
                Debug.Log("Picked Up!");
                Debug.Log("Number of cells collected: " +
                  cellsCollected.ToString());
                canPickUp = false;
                health -= 35;
            }
        }
    }


    void Movement()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("picking"))
        {
            return;
        }
        float rotation = Input.GetAxis("Horizontal") * rotationSpeed;
        rotation *= Time.deltaTime;
        transform.Rotate(0, rotation, 0);

        float trigger = Input.GetAxis("Vertical");
        if (trigger > 0)
        {

            anim.SetBool("isWalking", true);

            if (Input.GetKey(KeyCode.LeftShift))
            {
                anim.SetBool("isRunning", true);
                if (currentSpeed < runningSpeed) currentSpeed += acceleration;

            }

            else
            {
                anim.SetBool("isRunning", false);
                currentSpeed = walkingSpeed;
            }
        }

        else
        {
            currentSpeed = 0;
            anim.SetBool("isRunning", false);
            anim.SetBool("isWalking", false);
        }

        Vector3 forward = transform.TransformDirection(Vector3.forward);
        characterController.SimpleMove(forward * currentSpeed);
    }


    void Update()
    {
        //computing movement

        if (!CheckIfDead())
        {
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("idle")) PickUpItem();
            Movement();
        }
        
    }

        

    
       
}
