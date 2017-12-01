using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cave_Exit : MonoBehaviour
{
    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.GetComponent<PlayerController>() != null)
        {
            other.gameObject.GetComponent<PlayerController>().inCave = false;

            if(PlayerController.instance.dayNightControl.currentTime > 0.25f && PlayerController.instance.dayNightControl.currentTime < 0.75f)
            {
                BGMController.instance.ChangeMusic(0);
            }
            else
            {
                BGMController.instance.ChangeMusic(1);
            }
        }
    }

}
