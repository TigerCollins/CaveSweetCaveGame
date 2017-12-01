using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXController : MonoBehaviour {

    public static SFXController instance;
    public GameObject AudioBombPrefab;

    public AudioClip CollisionSound;

    private void Awake()
    {
        instance = this;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SpawnAudioBomb(Vector3 _position, AudioClip _clip, float _volume)
    {
        GameObject newAudioBomb = Instantiate(AudioBombPrefab);
        newAudioBomb.transform.position = _position;
        newAudioBomb.GetComponent<AudioBomb>().PlayClip(_clip, _volume);
    }
}
