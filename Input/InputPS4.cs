using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Buttons
    Square  = joystick button 0
    X       = joystick button 1
    Circle  = joystick button 2
    Triangle= joystick button 3
    L1      = joystick button 4
    R1      = joystick button 5
    L2      = joystick button 6
    R2      = joystick button 7
    Share	= joystick button 8
    Options = joystick button 9
    L3      = joystick button 10
    R3      = joystick button 11
    PS      = joystick button 12
    PadPress= joystick button 13

Axes:
    LeftStickX      = X-Axis
    LeftStickY      = Y-Axis (Inverted?)
    RightStickX     = 3rd Axis
    RightStickY     = 4th Axis (Inverted?)
    L2              = 5th Axis (-1.0f to 1.0f range, unpressed is -1.0f)
    R2              = 6th Axis (-1.0f to 1.0f range, unpressed is -1.0f)
    DPadX           = 7th Axis
    DPadY           = 8th Axis (Inverted?)
 * 
 * 
 * 
 * 
 * 
 */


public class InputPS4 : InputDevice
{

    public InputPS4(int _id) : base(_id)
    {

    }

    //Shoulder buttons
    public override bool IsTriggerLeft1Pressed()
    {
        return false;
    }

    public override bool IsTriggerLeft1Down()
    {
        return false;
    }

    public override bool IsTriggerLeft1Up()
    {
        return false;
    }

    public override bool IsTriggerLeft2Pressed()
    {
        return false;
    }

    public override bool IsTriggerLeft2Down()
    {
        return false;
    }

    public override bool IsTriggerLeft2Up()
    {
        return false;
    }

    public override bool IsTriggerRight1Pressed()
    {
        return false;
    }

    public override bool IsTriggerRight1Down()
    {
        return false;
    }

    public override bool IsTriggerRight1Up()
    {
        return false;
    }

    public override bool IsTriggerRight2Pressed()
    {
        return false;
    }

    public override bool IsTriggerRight2Down()
    {
        return false;
    }

    public override bool IsTriggerRight2Up()
    {
        return false;
    }

    //Cross buttons
    public override bool IsButtonUpPressed()
    {
        return false;
    }

    public override bool IsButtonUpDown()
    {
        return false;
    }

    public override bool IsButtonUpUp()
    {
        return false;
    }


    public override bool IsButtonLeftpPressed()
    {
        return false;
    }

    public override bool IsButtonLeftDown()
    {
        return false;
    }

    public override bool IsButtonLeftUp()
    {
        return false;
    }


    public override bool IsButtonDownPressed()
    {
        return false;
    }

    public override bool IsButtonDownDown()
    {
        return false;
    }

    public override bool IsButtonDownUp()
    {
        return false;
    }

    public override bool IsButtonRightPressed()
    {
        return false;
    }

    public override bool IsButtonRightDown()
    {
        return false;
    }

    public override bool IsButtonRightUp()
    {
        return false;
    }

    // buttons
    public override bool IsButtonTrianglePressed()
    {
        return Input.GetKey(KeyCode.Joystick1Button3 + GetIDShift());
    }

    public override bool IsButtonTriangleDown()
    {
        return Input.GetKeyDown(KeyCode.Joystick1Button3 + GetIDShift());
    }

    public override bool IsButtonTriangleUp()
    {
        return Input.GetKeyUp(KeyCode.Joystick1Button3 + GetIDShift());
    }


    public override bool IsButtonCirclePressed()
    {
        return Input.GetKey(KeyCode.Joystick1Button2 + GetIDShift());
    }

    public override bool IsButtonCircleDown()
    {
        return Input.GetKeyDown(KeyCode.Joystick1Button2 + GetIDShift());
    }

    public override bool IsButtonCircleUp()
    {
        return Input.GetKeyUp(KeyCode.Joystick1Button2 + GetIDShift());
    }


    public override bool IsButtonXPressed()
    {
        return Input.GetKey(KeyCode.Joystick1Button1 + GetIDShift());
    }

    public override bool IsButtonXDown()
    {
        return Input.GetKeyDown(KeyCode.Joystick1Button1 + GetIDShift());
    }

    public override bool IsButtonXUp()
    {
        return Input.GetKeyUp(KeyCode.Joystick1Button1 + GetIDShift());
    }

    public override bool IsButtonSquarePressed()
    {
        return Input.GetKey(KeyCode.Joystick1Button0 + GetIDShift());
    }

    public override bool IsButtonSquareDown()
    {
        return Input.GetKeyDown(KeyCode.Joystick1Button0 + GetIDShift());
    }

    public override bool IsButtonSquareUp()
    {
        return Input.GetKeyUp(KeyCode.Joystick1Button0 + GetIDShift());
    }


    //Joysticks
    public override float GetLeftJoystickX()
    {

        return Input.GetAxis("Axis1_J1");
    }
    public override float GetLeftJoystickY()
    {

        return Input.GetAxis("Axis2_J1")*-1;
    }

    public override float GetRightJoystickX()
    {
        return Input.GetAxis("Axis3_J1");
    }

    public override float GetRightJoystickY()
    {
        return Input.GetAxis("Axis4_J1");
    }



}
