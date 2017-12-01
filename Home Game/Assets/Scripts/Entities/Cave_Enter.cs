using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cave_Enter : MonoBehaviour
{
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerController>() != null)
        {
            other.gameObject.GetComponent<PlayerController>().inCave = true;
            BGMController.instance.ChangeMusic(2);
        }
    }
}
