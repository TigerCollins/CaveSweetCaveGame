using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonController : MonoBehaviour
{
    public GameObject HorizontalTurntable;
    public GameObject VerticalTurntable;
    public GameObject AimingReticle;

    public float VerticleLookTracker;

    public float minAngle;
    public float maxAngle;

    public float xCameraSensitivity;
    public float yCameraSensitivity;

    public float moveSpeed;
    public float jumpForce;

    public bool onGround;
    public float groundDistance;

    InputController inputController;

    public float StepDistance;
    public float StepTracker;

    private void Awake()
    {
        //Finding input controller on object
        inputController = GetComponent<InputController>();
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {



        //Horizontal Camera Rotation
        HorizontalTurntable.transform.localRotation *= Quaternion.Euler(0, inputController.xLookInput * xCameraSensitivity * Time.deltaTime, 0);

        //If the next step is gonna be over the cap, do not proceed. PS: i am god
        if (VerticleLookTracker + (inputController.yLookInput * yCameraSensitivity * Time.deltaTime * (inputController.invertY ? -1 : 1)) < minAngle || VerticleLookTracker + (inputController.yLookInput * yCameraSensitivity * Time.deltaTime * (inputController.invertY ? -1 : 1)) > maxAngle)
        {
            //Do nothing
        }
        else
        {
            //Vertical Camera Rotation
            VerticalTurntable.transform.localRotation *= Quaternion.Euler(inputController.yLookInput * yCameraSensitivity * Time.deltaTime * (inputController.invertY ? -1 : 1), 0, 0);
            //Tracking verticle turntable looking
            VerticleLookTracker += inputController.yLookInput * yCameraSensitivity * Time.deltaTime * (inputController.invertY ? -1 : 1);
        }

        //Calculate Movement Vector
        Vector3 movementVector = (HorizontalTurntable.transform.forward * inputController.yMoveInput) + (HorizontalTurntable.transform.right * inputController.xMoveInput);
        //Normalizing the movement vector and apply to object
        transform.position += movementVector.normalized * moveSpeed * Time.deltaTime;

        //Character Jumping
        if (inputController.jumpButtomDown && onGround)
            GetComponent<Rigidbody>().AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

        //Character Footsteps
        if (movementVector.magnitude > 0)
        {
            StepTracker += Time.deltaTime;

            if (StepTracker > StepDistance)
            {
                PlayerController.instance.GetComponent<AudioSource>().Play();
                StepTracker = 0;
                //play noise reset tracker;
            }
        }

        //Ground checking
        Ray downRay = new Ray(HorizontalTurntable.transform.position, HorizontalTurntable.gameObject.transform.up * -1);
        RaycastHit hit = new RaycastHit();

        if (Physics.Raycast(downRay, out hit, groundDistance))
        {
            onGround = true;
        }
        else
        {
            onGround = false;
        }
    }


}
