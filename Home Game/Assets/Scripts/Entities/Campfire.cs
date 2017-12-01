using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Campfire : MonoBehaviour
{

    public float currentFireTime;
    public float maxFireTime;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        UpdateFireTime();
	}

    void UpdateFireTime()
    {
        if(currentFireTime > 0)
        {
            currentFireTime -= Time.deltaTime;
        }
        else
        {

        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        
        if(collision.gameObject.GetComponent<EntityParent>() != null)
        {
            Debug.Log("Collided with fire");
            GameObject parentObject = collision.gameObject;

            if(parentObject.transform.childCount > 1)
            {
                //Get all burnable chield objects
                List<GameObject> burnableChildren = new List<GameObject>();
                List<GameObject> allEntities = new List<GameObject>();
                for (int i = 0; i < parentObject.transform.childCount; i++)
                {
                    allEntities.Add(parentObject.transform.GetChild(i).gameObject);
                    if(parentObject.transform.GetChild(i).gameObject.GetComponent<Entity>().Burnable == true)
                    {
                        burnableChildren.Add(parentObject.transform.GetChild(i).gameObject);
                    }
                }

                foreach(GameObject child in burnableChildren)
                {
                    foreach (GameObject entity in allEntities)
                    {
                        entity.transform.parent = null;
                        entity.GetComponent<Collider>().isTrigger = false;
                        entity.GetComponent<Entity>().InstantiateEntityParent();

                        //Disable all burnable objects so they dont stick to other objects again
                        if(burnableChildren.Contains(entity))
                        {
                            Destroy(entity);
                            //entity.GetComponent<Collider>().enabled = false;
                        }
                        else
                        {
                            entity.GetComponent<Entity>().MakeTempSticky(0.1f);
                        }
                            
                    }
                }
            }
            else
            {
                GameObject childObject = parentObject.transform.GetChild(0).gameObject;
                if(childObject.GetComponent<Entity>().Burnable)
                {
                    currentFireTime += childObject.GetComponent<Entity>().BurnTime;
                    Destroy(parentObject);
                }
                
            }
        }
    }
}
