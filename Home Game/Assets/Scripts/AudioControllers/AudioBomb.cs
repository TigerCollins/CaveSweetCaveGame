using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioBomb : MonoBehaviour
{
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PlayClip(AudioClip _clip, float _volume)
    {
        GetComponent<AudioSource>().volume = _volume;
        GetComponent<AudioSource>().PlayOneShot(_clip);
        Destroy(this.gameObject, 1f);
    }
}
