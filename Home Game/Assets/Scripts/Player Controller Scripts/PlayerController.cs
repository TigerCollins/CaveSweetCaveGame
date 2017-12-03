using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    #region Variables
    //Singlton Instance
    public static PlayerController instance;

    //Variables
    public float strengthBoostTimer;
    public float baseStrength;
    public float strength;
    public float glueBerries;
    public float strengthBoost;

    public bool inCave;

    public Text GlueBerryCounter;

    //Module Controller Objects
    public InputController inputController;
    public InteractionController interactionController;
    public FirstPersonController firstPersonController;
    public DayAndNightControl dayNightControl;
    #endregion

    private void Awake()
    {
        //Singlton Initialization
        instance = this;

        //Setting up control moduel references
        inputController = GetComponent<InputController>();
        interactionController = GetComponent<InteractionController>();
        firstPersonController = GetComponent<FirstPersonController>();
    }

    void Start()
    {
        //Setting strength to base default
        strength = baseStrength;
    }

    // Update is called once per frame
    void Update()
    {
        //If you have a strength boost, run the update code
        if (strengthBoostTimer > 0)
            UpdateStrengthBoost();

        //Updates the glueberry counter
        if (glueBerries > 3)
        {
            GlueBerryCounter.text = "∞";
        }

        else
        {
            GlueBerryCounter.text = glueBerries.ToString();
        }
    }

    public void BoostStrength(float _boostAmount, float _boostTime)
    {
        strength += _boostAmount;
        strengthBoostTimer = _boostTime;
    }

    void UpdateStrengthBoost()
    {
        strengthBoostTimer -= Time.deltaTime;

        if (strengthBoostTimer <= 0)
        {
            strength = baseStrength;
            strengthBoostTimer = 0;
        }
    }

    public IEnumerator Sleep()
    {
        
        yield return null;
    }

}
