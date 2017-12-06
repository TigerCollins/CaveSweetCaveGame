﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using UnityStandardAssets.ImageEffects;



public class InteractionController : MonoBehaviour
    {
        public static InteractionController instance;
        public bool HoldingItem = false;

        public float maxGrabDistance;
        public float reticleZoomSpeed;

        public float grabLerpSpeed;
        public float rotationSpeed;
        public GameObject grabbedObject;
        public GameObject ObjectLookedAt;

        FirstPersonController firstPersonController;
        InputController inputController;
        PlayerController playerController;

        public GameObject aimingReticle;

    public float MaxScrollDistance;
    public float MinScrollDistance;

    public GameObject OpenHandUI;
    public GameObject ClosedHandUI;
    public GameObject SleepUI;

    public float InvertValue;

        private void Awake()
        {
            //Link Controller Modules
            firstPersonController = GetComponent<FirstPersonController>();
            inputController = GetComponent<InputController>();
            playerController = GetComponent<PlayerController>();
        }

        // Update is called once per frame
        public void Update()
        {
        print(Input.GetAxis("Axis 9") > 0);
        print(Input.GetAxis("Axis 5") > 0);
        //Check to see if any relevent input has been triggered
        CheckInput();

            if (grabbedObject != null)
        {
            DepthOfField.instance.focalTransform = null;
            DepthOfField.instance.focalDistance01 = 14.82f;
        }


        //If you have grabbed an object, run the update code
        Ray ray = new Ray(firstPersonController.VerticalTurntable.transform.position, firstPersonController.VerticalTurntable.transform.forward);
            RaycastHit hit;
        //If the ray hits something
        if (Physics.Raycast(ray, out hit))
        {
            float distance = (hit.collider.transform.position - this.transform.position).magnitude;
            if (distance < maxGrabDistance)
            {
                if (hit.collider.transform.tag == "block")
                {
                    OpenHandUI.SetActive(true);
                    ClosedHandUI.SetActive(false);
                }
                else
                {
                    OpenHandUI.SetActive(false);
                    ClosedHandUI.SetActive(false);
                }
                if (hit.collider.tag == "Bed")
                {
                    if (playerController.dayNightControl.currentTime < 0.25f || playerController.dayNightControl.currentTime > 0.75f)
                    {
                        SleepUI.SetActive(true);
                        OpenHandUI.SetActive(false);
                        ClosedHandUI.SetActive(false);
                    }

                    if (playerController.dayNightControl.currentTime > 0.25f && playerController.dayNightControl.currentTime < 0.75f)
                    {
                        SleepUI.SetActive(false);
                        OpenHandUI.SetActive(true);
                        ClosedHandUI.SetActive(false);
                    }
                }
            }
            else
            {
                OpenHandUI.SetActive(false);
                ClosedHandUI.SetActive(false);
                SleepUI.SetActive(false);
            }
        }
        if(grabbedObject != null)
        {
            OpenHandUI.SetActive(false);
            ClosedHandUI.SetActive(true);
        }

            //Apply the inverse rotation to the grabbed object if you have one
            if (grabbedObject != null)
            {
                UpdateGrabbedObject();
                HoldingItem = true;
            }

            if (grabbedObject == null)
            {
                HoldingItem = false;
            }

        // Controller Reticle Zoom
        if ((Input.GetAxis("Axis 9") > 0) && (Input.GetAxis("Axis 5") > 0 && Vector3.Distance(aimingReticle.transform.position, transform.position) < MinScrollDistance))
        {
            aimingReticle.transform.position += firstPersonController.VerticalTurntable.transform.forward * Time.deltaTime * reticleZoomSpeed * -1;
        }

        if ((Input.GetAxis("Axis 9") > 0) && (Input.GetAxis("Axis 5") < 0 && Vector3.Distance(aimingReticle.transform.position, transform.position) > MaxScrollDistance))
        {
            aimingReticle.transform.position += firstPersonController.VerticalTurntable.transform.forward * Time.deltaTime * reticleZoomSpeed * 1;
        }

        //Keyboard Reticle Zoom
        if (Input.mouseScrollDelta.y > 0 && Vector3.Distance(aimingReticle.transform.position, transform.position) < MinScrollDistance)
        {
            aimingReticle.transform.position += firstPersonController.VerticalTurntable.transform.forward * Time.deltaTime * reticleZoomSpeed * Input.mouseScrollDelta.y;
        }

        if (Input.mouseScrollDelta.y < 0 && Vector3.Distance(aimingReticle.transform.position, transform.position) > MaxScrollDistance)
        {
            aimingReticle.transform.position += firstPersonController.VerticalTurntable.transform.forward * Time.deltaTime * reticleZoomSpeed * Input.mouseScrollDelta.y;
        }

        }



        public void CheckInput()
        {
            //Grab and Drop Input
            if (inputController.grabButtonDown > 0)
            {
                //If you dont have an object, try and grab one
                if (grabbedObject == null)
                    TryGrab();
                //If you do have an object try and drop it
                else
                    TryDrop();
            }

            if (inputController.glueButtonDown)
            {
                if (playerController.glueBerries > 0 && grabbedObject != null)
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
            //Creates a ray point out from the characters camera
            Ray ray = new Ray(firstPersonController.VerticalTurntable.transform.position, firstPersonController.VerticalTurntable.transform.forward);
            RaycastHit hit;
            //If the ray hits something
            if (Physics.Raycast(ray, out hit))
            {
                float distance = (hit.collider.transform.position - this.transform.position).magnitude;

                if (distance < maxGrabDistance)
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

                        if (playerController.dayNightControl.currentTime > 0.25f && playerController.dayNightControl.currentTime < 0.75f)
                        {
                            GrabObject(hit.collider.gameObject);
                        }
                    }
                }

            }
        }

        void Sleep()
        {
            if (playerController.dayNightControl.currentTime < 0.25f || playerController.dayNightControl.currentTime > 0.75f)
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


            DepthOfField.instance.focalTransform = grabbedObject.transform;
             


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
            if (grabbedObject != null)
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

        public void TryUnstick(GameObject _grabbedObject)
        {
        if (_grabbedObject != null)
            {
                if (_grabbedObject.transform.childCount > 1)
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

            foreach (GameObject entity in allEntities)
            {
                entity.transform.parent = null;
                entity.GetComponent<Collider>().isTrigger = false;
                entity.GetComponent<Entity>().InstantiateEntityParent();

                if (entity != rootEntity)
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
            if ((Input.GetAxis("Axis 9")>0) && (Input.GetAxis("Horizontal")>0))
            {
                grabbedObject.transform.RotateAround(grabbedObject.transform.position, firstPersonController.HorizontalTurntable.transform.up, rotationSpeed);
            }

            //myQuaternion 
            if ((Input.GetAxis("Axis 9") > 0) && (Input.GetAxis("Horizontal") < 0))
        {
                grabbedObject.transform.RotateAround(grabbedObject.transform.position, firstPersonController.HorizontalTurntable.transform.up, -rotationSpeed);
            }

            //myQuaternion 
            if (Input.GetKey(KeyCode.R) || Input.GetKey(KeyCode.Keypad8) || (Input.GetAxis("Axis 9") > 0) && (Input.GetAxis("Vertical") < 0))
            {
                grabbedObject.transform.RotateAround(grabbedObject.transform.position, firstPersonController.VerticalTurntable.transform.right, rotationSpeed);
            }

            //myQuaternion 
            if (Input.GetKey(KeyCode.F) || Input.GetKey(KeyCode.Keypad2) || (Input.GetAxis("Axis 9") > 0) && (Input.GetAxis("Vertical") > 0))
        {
                grabbedObject.transform.RotateAround(grabbedObject.transform.position, firstPersonController.VerticalTurntable.transform.right, -rotationSpeed);
            }

            //myQuaternion 
            if (Input.GetKey(KeyCode.Alpha1) || Input.GetKey(KeyCode.Keypad7))
            {
                grabbedObject.transform.RotateAround(grabbedObject.transform.position, firstPersonController.VerticalTurntable.transform.forward, rotationSpeed);
            }

            //myQuaternion 
            if (Input.GetKey(KeyCode.Alpha3) || Input.GetKey(KeyCode.Keypad9))
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

            foreach (GameObject parent in GameObject.FindGameObjectsWithTag("EntityParent"))
            {
                Entity en = parent.GetComponentInChildren<Entity>();
                if (en != null && en.inHome)
                {
                    continue;
                }
                allParentObjects.Add(parent);

            }

            foreach (GameObject parent in allParentObjects)
            {
                if (parent.GetComponent<Rigidbody>().isKinematic == false)
                {

                    UnstickGlobal(parent);
                }
            }
        }
    }


