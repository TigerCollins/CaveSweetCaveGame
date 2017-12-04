using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionController : MonoBehaviour
{

    public float maxGrabDistance;
    public float reticleZoomSpeed;

    public float grabLerpSpeed;
    public float rotationSpeed;
    public GameObject grabbedObject;

    FirstPersonController firstPersonController;
    InputController inputController;
    PlayerController playerController;

    public GameObject aimingReticle;

    private void Awake()
    {
        //Link Controller Modules
        firstPersonController = GetComponent<FirstPersonController>();
        inputController = GetComponent<InputController>();
        playerController = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        //Check to see if any relevent input has been triggered
        CheckInput();

        //If you have grabbed an object, run the update code
            

        //Apply the inverse rotation to the grabbed object if you have one
        if (grabbedObject != null)
            UpdateGrabbedObject();

        //Temp dodgy scroll code
        aimingReticle.transform.position += firstPersonController.VerticalTurntable.transform.forward * Time.deltaTime * reticleZoomSpeed * Input.mouseScrollDelta.y;
    }

    void CheckInput()
    {
        //Grab and Drop Input
        if(inputController.grabButtonDown)
        {
            //If you dont have an object, try and grab one
            if (grabbedObject == null)
                TryGrab();
            //If you do have an object try and drop it
            else
                TryDrop();
        }

        if(inputController.glueButtonDown)
        {
            if(playerController.glueBerries > 0 && grabbedObject != null)
            {
                TryGlue();
            }
        }

        if (inputController.unstickButtonDown)
        {
            TryUnstick(grabbedObject);
        }


    }

    void TryGlue()
    {
        if (PlayerController.instance.inCave == false)
        {
            if (playerController.dayNightControl.currentTime > 0.25f && playerController.dayNightControl.currentTime < 0.75f)
            {
                //Enable the children colliders
                for (int i = 0; i < grabbedObject.transform.childCount; i++)
                {
                    if (grabbedObject.transform.GetChild(i).gameObject.GetComponent<Entity>().sticky)
                    {
                        grabbedObject.transform.GetChild(i).gameObject.GetComponent<Entity>().RemoveSticky();
                    }
                    else
                    {
                        grabbedObject.transform.GetChild(i).gameObject.GetComponent<Entity>().MakeSticky();
                        PlayerController.instance.glueBerries -= 1;
                    }
                }
            }
        }

        if (PlayerController.instance.inCave == true)
        {
            //Enable the children colliders
            for (int i = 0; i < grabbedObject.transform.childCount; i++)
            {
                if (grabbedObject.transform.GetChild(i).gameObject.GetComponent<Entity>().sticky)
                {
                    grabbedObject.transform.GetChild(i).gameObject.GetComponent<Entity>().RemoveSticky();
                }
                else
                {
                    grabbedObject.transform.GetChild(i).gameObject.GetComponent<Entity>().MakeSticky();
                    PlayerController.instance.glueBerries -= 1;
                }
            }
        }
    }

    void TryGrab()
    {
        //Cretaes a ray point out from the characters camera
        Ray ray = new Ray(firstPersonController.VerticalTurntable.transform.position, firstPersonController.VerticalTurntable.transform.forward);
        RaycastHit hit;
        //If the ray hits something
        if (Physics.Raycast(ray, out hit))
        {
            float distance = (hit.collider.transform.position - this.transform.position).magnitude;

            if(distance < maxGrabDistance)
            {
                //and its tagged as a block
                if (hit.collider.tag == "block" && playerController.strength > hit.collider.gameObject.GetComponent<Entity>().weight)
                {
                    //Grab the object
                    GrabObject(hit.collider.gameObject);
                }

                //If it clicks on a berry instead
                if (hit.collider.tag == "berry")
                {
                    //Grab the berry
                    hit.collider.gameObject.GetComponent<Berry>().Activate(playerController);
                }

                
                if (hit.collider.tag == "Bed")
                {
                    if (playerController.dayNightControl.currentTime < 0.25f || playerController.dayNightControl.currentTime > 0.75f)
                    {
                        Sleep();
                    }

                    if (playerController.dayNightControl.currentTime > 0.25f || playerController.dayNightControl.currentTime < 0.75f)
                    {
                        GrabObject(hit.collider.gameObject);
                    }
                }
            }

        }
    }

    void Sleep()
    {
        if(playerController.dayNightControl.currentTime  < 0.25f || playerController.dayNightControl.currentTime  > 0.75f)
        {
            playerController.dayNightControl.currentTime = 0.25f;
            Debug.Log("Sleep");
        }
        
    }

    void GrabObject(GameObject _grabbedObject)
    {
        grabbedObject = _grabbedObject.transform.parent.gameObject;
        //Set the grabbed objects parent's root object 
        grabbedObject.GetComponent<EntityParent>().rootObject = _grabbedObject;


        


        //Set the parent of the parentEntity
        grabbedObject.transform.parent = firstPersonController.VerticalTurntable.transform;
        //Disable Gravity
        grabbedObject.gameObject.GetComponent<Rigidbody>().useGravity = false;
        //Freeze Constraints
        grabbedObject.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        //Set kinematics
        grabbedObject.gameObject.GetComponent<Rigidbody>().isKinematic = false;

        //Loop through all the children
        for (int i = 0; i < grabbedObject.transform.childCount; i++)
        {
            grabbedObject.transform.GetChild(i).gameObject.GetComponent<Collider>().isTrigger = true;

            //Adjust final bounds
        }
    }

    public void TryDrop()
    {
        if(grabbedObject != null)
        {
            if (grabbedObject.GetComponent<EntityParent>().collidingWithWorld == false)
            {
                DropObject();
            }
        }
        
    }

    void DropObject()
    {
        //Detach parent from player
        grabbedObject.transform.parent = null;
        //Turn on gravity
        grabbedObject.GetComponent<Rigidbody>().useGravity = true;
        //Turn off constraints
        grabbedObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;

        //Enable the children colliders
        for (int i = 0; i < grabbedObject.transform.childCount; i++)
        {
            grabbedObject.transform.GetChild(i).gameObject.GetComponent<Collider>().isTrigger = false;
        }

        //Disassosiate grabed object
        grabbedObject = null;
    }

    void TryUnstick(GameObject _grabbedObject)
    {
        if(_grabbedObject != null)
        {
            if(_grabbedObject.transform.childCount > 1)
            {
                Unstick(_grabbedObject);
            }
        }
    }

    public void Unstick(GameObject _grabbedObject)
    {
        //When you unstick an object it stores the root object

        List<GameObject> allEntities = new List<GameObject>();
        GameObject rootEntity = _grabbedObject.GetComponent<EntityParent>().rootObject;

        //Scans over all objects creating parents and rigid bodies for all
        for (int i = 0; i < _grabbedObject.transform.childCount; i++)
        {
            //Remove the entity from the parent object
            allEntities.Add(_grabbedObject.transform.GetChild(i).gameObject);
        }

        foreach(GameObject entity in allEntities)
        {
            entity.transform.parent = null;
            entity.GetComponent<Collider>().isTrigger = false;
            entity.GetComponent<Entity>().InstantiateEntityParent();
            
            if(entity != rootEntity)
            {
                entity.GetComponent<Entity>().MakeTempSticky(0.1f);
            }
        }

        GrabObject(rootEntity);
        //Sets all but the root object to sticky
        //Sets the grabbed object to the root object
        //Disabled the collider for the grabbed object as usual
        //Hope that all the other free falling objects stick together as they were
        //If you have to make the sticky state of the newly stickyified objects only last a few frames
    }

    public void UnstickGlobal(GameObject _grabbedObject)
    {
        //When you unstick an object it stores the root object

        List<GameObject> allEntities = new List<GameObject>();
        GameObject rootEntity = _grabbedObject.GetComponent<EntityParent>().rootObject;

        //Scans over all objects creating parents and rigid bodies for all
        for (int i = 0; i < _grabbedObject.transform.childCount; i++)
        {
            //Remove the entity from the parent object
            allEntities.Add(_grabbedObject.transform.GetChild(i).gameObject);
        }

        foreach (GameObject entity in allEntities)
        {
            entity.transform.parent = null;
            entity.GetComponent<Collider>().isTrigger = false;
            entity.GetComponent<Entity>().InstantiateEntityParent();
        }
    }

    void UpdateGrabbedObject()
    {
        //Adjust the rotation of the cube based on the characters first person controller
        grabbedObject.transform.RotateAround(grabbedObject.transform.position, firstPersonController.VerticalTurntable.transform.right, inputController.yLookInput * firstPersonController.yCameraSensitivity * Time.deltaTime * -1 * (inputController.invertY ? -1 : 1));

        //Moving the object towards the reticle
        grabbedObject.transform.position = Vector3.Lerp(grabbedObject.transform.position, firstPersonController.AimingReticle.transform.position, grabLerpSpeed);

        //myQuaternion 
        if (Input.GetKey(KeyCode.Q))
        {
            grabbedObject.transform.RotateAround(grabbedObject.transform.position, firstPersonController.HorizontalTurntable.transform.up, rotationSpeed);
        }

        //myQuaternion 
        if (Input.GetKey(KeyCode.E))
        {
            grabbedObject.transform.RotateAround(grabbedObject.transform.position, firstPersonController.HorizontalTurntable.transform.up, -rotationSpeed);
        }

        //myQuaternion 
        if (Input.GetKey(KeyCode.R))
        {
            grabbedObject.transform.RotateAround(grabbedObject.transform.position, firstPersonController.VerticalTurntable.transform.right, rotationSpeed);
        }

        //myQuaternion 
        if (Input.GetKey(KeyCode.F))
        {
            grabbedObject.transform.RotateAround(grabbedObject.transform.position, firstPersonController.VerticalTurntable.transform.right, -rotationSpeed);
        }

        //myQuaternion 
        if (Input.GetKey(KeyCode.Alpha1))
        {
            grabbedObject.transform.RotateAround(grabbedObject.transform.position, firstPersonController.VerticalTurntable.transform.forward, rotationSpeed);
        }

        //myQuaternion 
        if (Input.GetKey(KeyCode.Alpha3))
        {
            grabbedObject.transform.RotateAround(grabbedObject.transform.position, firstPersonController.VerticalTurntable.transform.forward, -rotationSpeed);
        }
    }

    void InitializeRigidBody(GameObject _object)
    {
        _object.AddComponent<Rigidbody>();
    }

    public void WipeAllStructures()
    {
        Debug.Log("wiped all structures");

        List<GameObject> allParentObjects = new List<GameObject>();

        foreach(GameObject parent in GameObject.FindGameObjectsWithTag("EntityParent"))
        {
            Entity en = parent.GetComponentInChildren<Entity>();
            if (en != null && en.inHome)
            {
                continue;
            }
            allParentObjects.Add(parent);
            
        }

        foreach(GameObject parent in allParentObjects)
        {
            if(parent.GetComponent<Rigidbody>().isKinematic == false)
            {

                UnstickGlobal(parent);
            }
        }
    }
}
