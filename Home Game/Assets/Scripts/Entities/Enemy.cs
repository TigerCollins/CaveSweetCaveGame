using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{

    public float speed;

    PlayerController player;

    public float upwardForce;
    public float outwardForce;

    public AudioClip EnemyExplosionClip;

    public bool CanMove = true;
    public bool isHome = false;

    private void Awake()
    {
        player = PlayerController.instance;
    }

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        UpdateMovement();
        if(PlayerController.instance.inCave==true)
        {
            CanMove = true;
        }

        else
        {
            CanMove = true;
        }

  if(PlayerController.instance.dayNightControl.currentTime > 0.25f && PlayerController.instance.dayNightControl.currentTime < 0.75f)
        {
            Destroy(this.gameObject);
        }
	}

    void UpdateMovement()
    {
        transform.position = Vector3.MoveTowards(transform.position, PlayerController.instance.transform.position, Time.deltaTime * speed);
        //transform.Translate((PlayerController.instance.transform.position - gameObject.transform.position).normalized * Time.deltaTime * speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            Debug.Log("Collided with player");
            Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
            Vector3 explosionForce = (PlayerController.instance.transform.position - gameObject.transform.position).normalized * outwardForce;
            explosionForce.y = upwardForce;

            rb.AddForce(explosionForce, ForceMode.Impulse);

            other.gameObject.GetComponent<InteractionController>().TryDrop();

            SFXController.instance.SpawnAudioBomb(this.transform.position, EnemyExplosionClip, 1);

            Destroy(this.gameObject);
        }

        else if (other.gameObject.tag == "Home")
        {
            isHome = true;
            Debug.Log("Collided with home");

            SFXController.instance.SpawnAudioBomb(this.transform.position, EnemyExplosionClip, 1);

            Destroy(this.gameObject);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Home")
        {
            isHome = true;
            Debug.Log("Collided with home");

            SFXController.instance.SpawnAudioBomb(this.transform.position, EnemyExplosionClip, 1);

            Destroy(this.gameObject);
        }
    }
}
