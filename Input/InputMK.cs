using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InputMK : InputDevice
{

    public InputMK(int _id) : base(_id)
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
        return false;
    }

    public override bool IsButtonTriangleDown()
    {
        return false;
    }

    public override bool IsButtonTriangleUp()
    {
        return false;
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

        float x = 0;

        if (Input.GetKey(KeyCode.D))
        {
            x += 1;
        }

        if (Input.GetKey(KeyCode.A))
        {
            x -= 1;
        }

        return x;
    }
    public override float GetLeftJoystickY()
    {

        float y = 0;

        if (Input.GetKey(KeyCode.W))
        {
            y += 1;
        }

        if (Input.GetKey(KeyCode.S))
        {
            y -= 1;
        }

        return y;
    }

    public override float GetRightJoystickX()
    {

        Vector3 mousePosition = Input.mousePosition;
        Vector3 direction = (mousePosition - new Vector3(Screen.width / 2.0f, Screen.height / 2.0f)).normalized;

        direction.z = direction.x;
        direction.x = -direction.y;
        direction.y = 0.0f;

        return direction.x;
    }

    public override float GetRightJoystickY()
    {
        Vector3 mousePosition = Input.mousePosition;
        Vector3 direction = (mousePosition - new Vector3(Screen.width / 2.0f, Screen.height / 2.0f)).normalized;

        direction.z = direction.x;
        direction.x = -direction.y;
        direction.y = 0.0f;

        return direction.y;
    }



}
