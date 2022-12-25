using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO: Limit cursor position to a certain range around the player
//TODO: Add controller support for GetCursorPos(). One implementation with a persistent cursor moved around normally by the joystick, and a second implementation where the cursor position = the position of the joystick (this allows fast mouse-like movement without having absurdly high sensitivity)
public class PlayerGrab : MonoBehaviour, IGrabber
{
    public Vector2 targetLocation => GetCursorPos();

    private GrabbableObject currentGrabbed;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            GrabSelectedObject();
        }
        if(Input.GetMouseButtonUp(0))
        {
            ReleaseCurrentObject();
        }
    }

    private void GrabSelectedObject()
    {
        Vector2 cursorPos = GetCursorPos();
        Collider2D[] hits = Physics2D.OverlapPointAll(cursorPos);
        foreach (Collider2D hit in hits)
        {
            GrabbableObject grabbable = hit.GetComponent<GrabbableObject>();
            if (grabbable == null)
                continue;
            grabbable.Grab(this, cursorPos);
            currentGrabbed = grabbable;
            return;
        }
    }

    private void ReleaseCurrentObject()
    {
        if (currentGrabbed == null)
            return;
        currentGrabbed.Release();
        return;
    }

    private Vector2 GetCursorPos()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

}
