using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//this script is for processing the inputs before sending them to the player controller
//the only purpose of this script is to handle inputs and display them to the controller. Changing input settings should be handled by a different script
public class PlayerInputs
{
    private GeneratedPlayerControls controls;
    private Transform user;

    public PlayerInputs(Transform user)
    {
        controls = new GeneratedPlayerControls();
        this.user = user;
        InitSettings();
        HookInputs();
        Enable();
    }

    //ideally this should read from a settings file but it's gonna be hardcoded for now.
    public void InitSettings()
    {
        walkDeadzoneMin = 0.1f;
        walkDeadzoneMax = 0.9f;
        cursorSens = 3f;
        altIsToggle = true;
        usingAltSens = false;
        usingAltMove = false;
        altCursorSens = 1f;
        altCursorRangeMult = 0.5f;
    }

    private void HookInputs()
    {
        controls.InGame.Walk.performed += ProcessWalk;

        controls.InGame.MoveCursorMouse.performed += ProcessMoveCursorMouse;
        controls.InGame.MoveCursorStick.performed += ProcessMoveCursorStick;
        controls.InGame.AlternateCursorMovementMode.started += ToggleAltMove;
        controls.InGame.AlternateCursorMovementMode.canceled += ToggleAltMove;
        controls.InGame.AlternateCursorSensitivityMode.started += ToggleAltSens;
        controls.InGame.AlternateCursorSensitivityMode.canceled += ToggleAltSens;

        controls.InGame.GrabRelease.performed += ProcessGrabRelease;

        controls.InGame.ItemAction.performed += delegate { ItemAction?.Invoke(); }; //this input doesn't need any processing
    }

    public void Enable()
    {
        controls.InGame.Enable(); 
    }

    public void Disable()
    {
        controls.InGame.Disable();
    }

    #region Walking

    //this is something that PlayerController would have to constantly get (for movement), so make it a property

    public float walkInput { get; private set; }

    private float walkDeadzoneMin;
    private float walkDeadzoneMax;

    private void ProcessWalk(InputAction.CallbackContext obj)
    {
        walkInput = obj.ReadValue<float>();

        if (Mathf.Abs(walkInput) >= walkDeadzoneMax)
            walkInput = Mathf.Sign(walkInput); //Mathf.Sign returns 1 or -1

        if (Mathf.Abs(walkInput) <= walkDeadzoneMin)
            walkInput = 0;
    }

    #endregion

    #region Cursor Position

    //this is something that PlayerController would have to constantly get (for moving hands), so make it a property

    public Vector2 cursorOffset { get; private set; }

    public float cursorRange = 10; //this is a property of the player rather than an input setting, so make it public

    //for stick controls only
    private float cursorSens;

    private bool altIsToggle;
    private bool usingAltSens;
    private bool usingAltMove;

    private float altCursorSens;
    private float altCursorRangeMult;

    private void ProcessMoveCursorMouse(InputAction.CallbackContext obj)
    {
        Vector2 unclampedPos = Camera.main.ScreenToWorldPoint(obj.ReadValue<Vector2>());
        cursorOffset = unclampedPos - (Vector2)user.position;
        if (cursorOffset.magnitude > cursorRange)
            cursorOffset = cursorOffset.normalized * cursorRange;
    }

    private void ProcessMoveCursorStick(InputAction.CallbackContext obj)
    {
        Vector2 stickValue = obj.ReadValue<Vector2>();
        if(usingAltMove)
        {
            if (usingAltSens)
                stickValue *= altCursorRangeMult;
            if (stickValue.magnitude > 1)
                stickValue.Normalize();
            cursorOffset = stickValue * cursorRange;
        }
        else
        {
            stickValue *= usingAltSens ? altCursorSens : cursorSens;
            cursorOffset += stickValue;
            if (cursorOffset.magnitude > cursorRange)
                cursorOffset = cursorOffset.normalized * cursorRange;
        }
    }

    private void ToggleAltSens(InputAction.CallbackContext obj)
    {
        if(obj.started || !altIsToggle)
            usingAltSens = !usingAltSens;
    }

    private void ToggleAltMove(InputAction.CallbackContext obj)
    {
        if (obj.started || !altIsToggle)
            usingAltMove = !usingAltMove;
    }

    #endregion

    #region Grab/Release

    //this is something PlayerController only needs to know when it's triggered (to grab/release an item), so make it an event

    public event Action Grab;
    public event Action Release;

    private bool toggleGrab;
    private bool grabbing;

    private void ProcessGrabRelease(InputAction.CallbackContext obj)
    {
        if(toggleGrab)
        {
            if(obj.started)
            {
                grabbing = !grabbing;
                (grabbing ? Grab : Release)?.Invoke(); //this is probably the weirdest line I've ever written
            }
        }
        else
        {
            if(obj.started)
            {
                grabbing = true;
                Grab?.Invoke();
            }
            else
            {
                grabbing = false;
                Release?.Invoke();
            }
        }
    }

    #endregion

    #region Item Action

    //this is something PlayerController only needs to know when it's triggered (to trigger an action with the held item), so make it an event

    public event Action ItemAction;

    //this doesn't need any processing so the event is directly hooked to the input

    #endregion
}
