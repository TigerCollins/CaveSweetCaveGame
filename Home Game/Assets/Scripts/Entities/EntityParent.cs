using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityParent : MonoBehaviour
{

    public bool collidingWithWorld = false;
    public GameObject rootObject;

    public AudioClip CollisionClip;

    private void Awake()
    {
        
    }

    // Use this for initialization
    void Start ()
    {
        CollisionClip = SFXController.instance.CollisionSound;
	}
	
	// Update is called once per frame
	void Update ()
    {
        //Color debugColour = collidingWithWorld ? Color.red : Color.white;

        //for (int i = 0; i < transform.childCount; i++)
        //{
        //    gameObject.transform.GetChild(i).gameObject.GetComponent<Renderer>().material.color = debugColour;
        //}
	}

    private void FixedUpdate()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Triggered");
        collidingWithWorld = true;
    }

    private void OnTriggerStay(Collider other)
    {
        collidingWithWorld = true;
    }

    //If this object touches an object that is sticky, it turns kinimatic and sets its parent to that object
    private void OnCollisionEnter(Collision collision)
    {

        SFXController.instance.SpawnAudioBomb(this.transform.position, CollisionClip, 0.1f);

        //Debug.Log("Basic collision");
        //Debug.Log(collision.transform.gameObject.name);
        //If the collision has at least one child
        if (collision.transform.childCount > 0)
        {
            //if the child is an entity
            if (collision.transform.GetChild(0).GetComponent<Entity>() != null)
            {
                //Debug.Log("collision with entity");
                //if the entity is set to sticky
                if (transform.GetChild(0).GetComponent<Entity>().sticky && transform.GetComponent<Rigidbody>().isKinematic == false && collision.gameObject.GetComponent<Rigidbody>().isKinematic == false)
                {
                    //Debug.Log("Stick collision");


                    //Identify parent objects
                    GameObject selfParent = this.gameObject;
                    GameObject otherParent = collision.gameObject;

                    //Create a list containing all child objects inside of both colliding parents
                    List<GameObject> allEntities = new List<GameObject>();

                    //Add all children from self parent
                    for (int i = 0; i < selfParent.transform.childCount; i++)
                    {
                        allEntities.Add(selfParent.transform.GetChild(i).gameObject);
                    }

                    //Add all children from other parent
                    for (int i = 0; i < otherParent.transform.childCount; i++)
                    {
                        allEntities.Add(otherParent.transform.GetChild(i).gameObject);
                    }

                    Vector3 averagePosition = Vector3.zero;

                    //Loop through all entities
                    foreach (GameObject entity in allEntities)
                    {
                        //disconnect parent
                        entity.transform.parent = null;
                        //Debug.Log("All entities: " + entity.name);
                        //Add to the average position 
                        averagePosition += entity.transform.position;
                    }

                    //Divid the average position
                    averagePosition /= allEntities.Count;

                    //Destroy both parent objects
                    Destroy(selfParent);
                    Destroy(otherParent);

                    //Create a new parent
                    GameObject entityParent = new GameObject();
                    entityParent.tag = "EntityParent";
                    entityParent.name = "StickyParent";
                    entityParent.transform.position = averagePosition;
                    //Spawn a rigid body for the parent
                    entityParent.AddComponent<Rigidbody>();
                    //Spawn a entityParent Script
                    entityParent.AddComponent<EntityParent>();

                    //Add all the stored child entities to the new parent
                    foreach (GameObject entity in allEntities)
                    {
                        entity.transform.parent = entityParent.transform;
                        entity.GetComponent<Entity>().RemoveSticky();
                    }

                    entityParent.GetComponent<Rigidbody>().ResetCenterOfMass();
                    entityParent.GetComponent<Rigidbody>().ResetInertiaTensor();
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        collidingWithWorld = false;
    }
}
