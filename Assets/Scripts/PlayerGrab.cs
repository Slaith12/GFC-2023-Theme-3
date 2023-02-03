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
    public float rotationOffset { get => GetRotationOffset(); }
    [SerializeField] float m_torqueStrength = 10;
    public float torqueStrength { get => m_torqueStrength; }
    [SerializeField] float m_lookAheadTime = 0.1f;
    public float lookAheadTime { get => m_lookAheadTime; }

    [SerializeField] Transform firstHand; //i am considering making the hand objects un-parented in Awake to make sure they don't move weirdly with the player
    [SerializeField] Transform firstHandDefaultPos;
    [SerializeField] Transform secondHand;
    [SerializeField] Transform secondHandDefaultPos;
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
        MoveHands();
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

    private void MoveHands()
    {
        if (grabbingObject)
        {
            if (interpTime > 0f)
            {
                interpTime -= Time.deltaTime;
                firstHand.position = Vector2.Lerp(currentGrabbed.firstHandPosition, firstHandDefaultPos.position, interpTime / handTravelTime);
                if(currentGrabbed.isTwoHanded)
                    secondHand.position = Vector2.Lerp(currentGrabbed.secondHandPosition, secondHandDefaultPos.position, interpTime / handTravelTime);
            }
            else
            {
                firstHand.position = currentGrabbed.firstHandPosition;
                if (currentGrabbed.isTwoHanded)
                    secondHand.position = currentGrabbed.secondHandPosition;
            }
        }
        else
        {
            if (interpTime > 0f)
            {
                interpTime -= Time.deltaTime;
                firstHand.position = Vector2.Lerp(firstHandDefaultPos.position, currentGrabbed.firstHandPosition, interpTime / handTravelTime);
                if(currentGrabbed.isTwoHanded)
                {
                    secondHand.position = Vector2.Lerp(secondHandDefaultPos.position, currentGrabbed.secondHandPosition, interpTime / handTravelTime);
                }
            }
            else
            {
                firstHand.position = firstHandDefaultPos.position;
                secondHand.position = secondHandDefaultPos.position;
            }
        }
    }

    private float GetRotationOffset()
    {
        Vector2 posOffset = targetLocation - (Vector2)firstHandDefaultPos.position;
        Vector2 horizontal = Vector2.right;
        if (posOffset.x < 0)
            horizontal = Vector2.left;
        return Vector2.SignedAngle(horizontal, posOffset);
    }

    private Vector2 GetCursorPos()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

}
