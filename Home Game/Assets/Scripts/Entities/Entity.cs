using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{

    public bool sticky;

    Color stickyColour;
    Color normalColour;

    public float weight;
    public float stickTimer;

    public bool Burnable;
    public float BurnTime;


    private void Awake()
    {
        if(transform.parent != null)
        {
            if(transform.parent.tag != "EntityParent")
            {
                InstantiateEntityParent();
            }
        }
        else
        {
            InstantiateEntityParent();
        }

        stickyColour = Color.blue;
        normalColour = GetComponent<Renderer>().material.color;

        this.transform.parent.gameObject.GetComponent<Rigidbody>().isKinematic = true;
    }

    public void InstantiateEntityParent()
    {
        GameObject entityParent = new GameObject();
        entityParent.tag = "EntityParent";
        entityParent.name = "EntityParent";
        entityParent.transform.position = this.gameObject.transform.position;

        //if the original object has a rigid body, delete it
        if(GetComponent<Rigidbody>() != null)
        {
            Destroy(GetComponent<Rigidbody>());
        }

        //Spawn a rigid body for the parent
        entityParent.AddComponent<Rigidbody>();

        //Spawn a entityParent Script
        entityParent.AddComponent<EntityParent>();


        this.transform.parent = entityParent.transform;
    }

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		if(stickTimer > 0)
        {
            stickTimer -= Time.deltaTime;

            if(stickTimer < 0)
            {
                RemoveSticky();
            }
        }
	}

    public void MakeSticky()
    {
        sticky = true;
        GetComponent<MeshRenderer>().material.color = stickyColour;
    }

    public void MakeTempSticky(float _time)
    {
        sticky = true;
        stickTimer = _time;
    }

    public void RemoveSticky()
    {
        sticky = false;
        GetComponent<MeshRenderer>().material.color = normalColour;
    }

    ////If this object touches an object that is sticky, it turns kinimatic and sets its parent to that object
    //private void OnCollisionEnter(Collision collision)
    //{
    //    Debug.Log("Basic collision");
    //    if(collision.gameObject.GetComponent<Entity>() != null)
    //    {
    //        Debug.Log("collision with entity");
    //        if (collision.gameObject.GetComponent<Entity>().sticky)
    //        {
    //            if(this.gameObject.GetComponent<Rigidbody>().isKinematic == false  && collision.gameObject.GetComponent<Rigidbody>().isKinematic == false)
    //            {
    //                Debug.Log("Stick collision");

    //                //Identify parent objects
    //                GameObject selfParent = transform.parent.gameObject;
    //                GameObject otherParent = collision.transform.parent.gameObject;

    //                //Create a list containing all child objects inside of both colliding parents
    //                List<GameObject> allEntities = new List<GameObject>();

    //                //Add all children from self parent
    //                for (int i = 0; i < selfParent.transform.childCount; i++)
    //                {
    //                    allEntities.Add(selfParent.transform.GetChild(i).gameObject);
    //                }

    //                //Add all children from other parent
    //                for (int i = 0; i < otherParent.transform.childCount; i++)
    //                {
    //                    allEntities.Add(otherParent.transform.GetChild(i).gameObject);
    //                }

    //                //Loop through all entities
    //                foreach (GameObject entity in allEntities)
    //                {
    //                    //disconnect parent
    //                    entity.transform.parent = null;
    //                }

    //                //Destroy both parent objects
    //                Destroy(selfParent);
    //                Destroy(otherParent);

    //                //Create a new parent
    //                GameObject entityParent = new GameObject();
    //                entityParent.tag = "EntityParent";
    //                entityParent.name = "EntityParent";
    //                entityParent.transform.position = this.gameObject.transform.position;
    //                //Spawn a rigid body for the parent
    //                entityParent.AddComponent<Rigidbody>();
    //                //Spawn a entityParent Script
    //                entityParent.AddComponent<EntityParent>();

    //                //Add all the stored child entities to the new parent
    //                foreach (GameObject entity in allEntities)
    //                {
    //                    entity.transform.parent = entityParent.transform;
    //                }

    //                entityParent.GetComponent<Rigidbody>().ResetCenterOfMass();
    //                entityParent.GetComponent<Rigidbody>().ResetInertiaTensor();
    //            }
                
    //        }
    //    }
    //}
}
