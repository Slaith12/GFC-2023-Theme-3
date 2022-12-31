using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO: Limit cursor position to a certain range around the player
//TODO: Add controller support for GetCursorPos(). One implementation with a persistent cursor moved around normally by the joystick, and a second implementation where the cursor position = the position of the joystick (this allows fast mouse-like movement without having absurdly high sensitivity)
public class PlayerGrab : MonoBehaviour, IGrabber
{
    public Vector2 targetLocation => GetCursorPos();
    [field: Header("Grab Attributes")]
    [SerializeField] float m_followStrength = 150; //these m variables are here so that they are capitalized in the editor
    public float followStrength { get => m_followStrength; }
    [SerializeField] float m_lookAheadTime = 0.1f;
    public float lookAheadTime { get => m_lookAheadTime; }

    [SerializeField] Transform firstHand;
    [SerializeField] Transform firstHandDefaultPos;
    [SerializeField] float handTravelTime;

    private GrabbableObject currentGrabbed;
    private bool grabbingObject;
    private float interpTime;

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
        if (grabbingObject)
        {
            if (interpTime > 0f)
            {
                interpTime -= Time.deltaTime;
                firstHand.position = Vector2.Lerp(currentGrabbed.firstHandPosition, firstHandDefaultPos.position, interpTime / handTravelTime);
            }
            else
            {
                firstHand.position = currentGrabbed.firstHandPosition;
            }
        }
        else
        {
            if(interpTime > 0f)
            {
                interpTime -= Time.deltaTime;
                firstHand.position = Vector2.Lerp(firstHandDefaultPos.position, currentGrabbed.firstHandPosition, interpTime / handTravelTime);
            }
            else
            {
                firstHand.position = firstHandDefaultPos.position;
            }
        }
    }

    private void GrabSelectedObject()
    {
        Vector2 cursorPos = GetCursorPos();
        Collider2D[] hits = Physics2D.OverlapPointAll(cursorPos);
        foreach (Collider2D hit in hits)
        {
            GrabbableObject grabbable = hit.GetComponentInParent<GrabbableObject>();
            if (grabbable == null)
                continue;
            grabbable.Grab(this, cursorPos);
            grabbingObject = true;
            currentGrabbed = grabbable;
            interpTime = handTravelTime;
            return;
        }
    }

    private void ReleaseCurrentObject()
    {
        if (!grabbingObject)
            return;
        currentGrabbed.Release();
        grabbingObject = false;
        interpTime = handTravelTime;
        return;
    }

    private Vector2 GetCursorPos()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

}
