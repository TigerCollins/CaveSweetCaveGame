using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaveBounds : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void CheckInterior()
    {

    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    Debug.Log("trigger Enter");
    //    //if its colliding the the cave bounds
    //    if (other.gameObject.tag == "block")
    //    {
    //        PlayerController.instance.score += other.gameObject.GetComponent<Entity>().score;
    //    }
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    Debug.Log("Trigger Exit");
    //    //if its colliding the the cave bounds
    //    if (other.gameObject.tag == "block")
    //    {
    //        PlayerController.instance.score -= other.gameObject.GetComponent<Entity>().score;
    //    }
    //}

}
