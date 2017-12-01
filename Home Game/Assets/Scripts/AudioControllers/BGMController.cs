using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMController : MonoBehaviour
{

    public static BGMController instance;

    public AudioSource DayMusic;
    public AudioSource NightMusic;
    public AudioSource CaveMusic;

    public AudioSource ActiveAudio;

    public CurrentTrack currentTrack = CurrentTrack.cave;

    public float fadeSpeed;

    public float maxVolume;

    public List<AudioSource> BGMTracks;

    public enum CurrentTrack
    {
        day,
        night,
        cave
    }

    private void Awake()
    {
        instance = this;

        BGMTracks.Add(DayMusic);
        BGMTracks.Add(NightMusic);
        BGMTracks.Add(CaveMusic);

        ChangeMusic(2);


    }

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        foreach(AudioSource audio in BGMTracks)
        {
            if(audio == ActiveAudio)
            {
                if (audio.volume < maxVolume)
                {
                    audio.volume += Time.deltaTime * fadeSpeed;
                }
            }
            else
            {
                if (audio.volume > 0)
                {
                    audio.volume -= Time.deltaTime * fadeSpeed;
                }
            }
            
        }
    }

    /// <summary>
    /// 0 = Day. 1 = Night. 2 = Cave.
    /// </summary>
    /// <param name="_track"></param>
    public void ChangeMusic(int _track)
    {
        switch (_track)
        {
            case 0:
                currentTrack = CurrentTrack.day;
                ActiveAudio = DayMusic;
                //DayMusic.volume = 0.25f;
                break;
            case 1:
                currentTrack = CurrentTrack.night;
                ActiveAudio = NightMusic;
                //NightMusic.volume = 0.25f;
                break;
            case 2:
                currentTrack = CurrentTrack.cave;
                ActiveAudio = CaveMusic;
                //CaveMusic.volume = 0.25f;
                break;

        }
    }

}
