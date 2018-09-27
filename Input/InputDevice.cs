using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InputDevice
{

    public enum DEVICES { PS4, MOUSE_KEYBOARD }


    public InputDevice(int _id)
    {
        m_Id = _id;
    }

    protected int m_Id  = 1;
    
    //Shoulder buttons
    public abstract bool IsTriggerLeft1Pressed();
    public abstract bool IsTriggerLeft1Down();
    public abstract bool IsTriggerLeft1Up();

    public abstract bool IsTriggerLeft2Pressed();
    public abstract bool IsTriggerLeft2Down();
    public abstract bool IsTriggerLeft2Up();

    public abstract bool IsTriggerRight1Pressed();
    public abstract bool IsTriggerRight1Down();
    public abstract bool IsTriggerRight1Up();

    public abstract bool IsTriggerRight2Pressed();
    public abstract bool IsTriggerRight2Down();
    public abstract bool IsTriggerRight2Up();

    //Cross buttons
    public abstract bool IsButtonUpPressed();
    public abstract bool IsButtonUpDown();
    public abstract bool IsButtonUpUp();

    public abstract bool IsButtonLeftpPressed();
    public abstract bool IsButtonLeftDown();
    public abstract bool IsButtonLeftUp();

    public abstract bool IsButtonDownPressed();
    public abstract bool IsButtonDownDown();
    public abstract bool IsButtonDownUp();

    public abstract bool IsButtonRightPressed();
    public abstract bool IsButtonRightDown();
    public abstract bool IsButtonRightUp();

    // buttons
    public abstract bool IsButtonTrianglePressed();
    public abstract bool IsButtonTriangleDown();
    public abstract bool IsButtonTriangleUp();

    public abstract bool IsButtonCirclePressed();
    public abstract bool IsButtonCircleDown();
    public abstract bool IsButtonCircleUp();

    public abstract bool IsButtonXPressed();
    public abstract bool IsButtonXDown();
    public abstract bool IsButtonXUp();

    public abstract bool IsButtonSquarePressed();
    public abstract bool IsButtonSquareDown();
    public abstract bool IsButtonSquareUp();

    //Joysticks
    public abstract float GetLeftJoystickX();
    public abstract float GetLeftJoystickY();

    public abstract float GetRightJoystickX();
    public abstract float GetRightJoystickY();


    protected virtual int GetIDShift()
    {
        return m_Id * 20;
    }


    public static InputDevice CreateDeviceInput(DEVICES _device, int _ID = 0)
    {
        switch (_device)
        {
            case DEVICES.MOUSE_KEYBOARD:
                {
                    return new InputMK(_ID);
                }
            case DEVICES.PS4:
                {
                    return new InputPS4(_ID);
                }
        }

        return null;
    }

}
