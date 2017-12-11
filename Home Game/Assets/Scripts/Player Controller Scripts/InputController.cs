using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputController : MonoBehaviour
{
    //Singlton Instance
    public static InputController instance;

    public Toggle isInvertY;

    public bool invertY;
    public bool Pause;

    public float xLookInput
    {
        get
        {
            return Input.GetAxis("Axis 4") * 10f;
        }
    }

    public float xLookInput2
    {
        get
        {
            return Input.GetAxis("Mouse X");
        }
    }
    public float yLookInput
    {
        get
        {
            return Input.GetAxis("Axis 5") *10f;
        }
    }

    public float yLookInput2
    {
        get
        {
            return Input.GetAxis("Mouse Y");
        }
    }
    public float xMoveInput
    {
        get
        {
            return Input.GetAxis("Horizontal");
        }
    }
    public float yMoveInput
    {
        get
        {
            return Input.GetAxis("Vertical");
        }
    }
    public bool jumpButtomDown
    {
        get
        {
            return Input.GetKeyDown("joystick button 0");
        }
    }

    public bool jumpButtomDown2
    {
        get
        {
            return Input.GetKeyDown(KeyCode.Space);
        }
    }

    public float grabButtonDown
    {
        get
      {
            return Input.GetAxis("Axis 10");
      }
    }

    public bool grabButtonDown2
    {
        get
        {
            return Input.GetMouseButtonDown(0);
        }
    }

    public bool glueButtonDown
    {
        get
        {
            return Input.GetKeyDown("joystick button 2");
        }
    }

    public bool glueButtonDown2
    {
        get
        {
            return Input.GetMouseButtonDown(1);
        }
    }

    public bool unstickButtonDown
    {
        get
        {
            return Input.GetKeyDown("joystick button 3");
        }
    }

    public bool unstickButtonDown2
    {
        get
        {
            return Input.GetKeyDown(KeyCode.Z);
        }
    }

    // Use this for initialization
    void Update ()
    {
        LockAndHideCursor();
        ToggleButton();

        if (Input.GetKey(KeyCode.Escape)|| Input.GetKeyDown("joystick button 7"))
        {
            Pause = true;
        }
    }

    void LockAndHideCursor()
    {
      /*  if (Pause == false)
        {
            //Locking and hiding the cursor by default
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        if (Pause == true)
        {
            //Locking and hiding the cursor by default
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        */
    }

    public void ContinueButton()
    {
        Pause = false;
    }

    public void ToggleButton()
    {
        if(isInvertY.isOn == true)
        {
            invertY = true;
        }

        if (isInvertY.isOn == false)
        {
            invertY = false;
        }
        
    }
    
}
