using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SKGG.Inputs
{

    //this script is for processing the inputs before sending them to the player controller
    //the only purpose of this script is to handle inputs and display them to the controller. Changing input settings should be handled by a different script
    public class PlayerInputs
    {

        #region Initialization

        private GeneratedPlayerControls controls;
        private Transform user;

        public PlayerInputs(Transform user)
        {
            controls = new GeneratedPlayerControls();
            this.user = user;
            InitSettings();
            HookInputs();
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
            controls.InGame.Walk.canceled += ProcessWalk; //releasing the buttons calls canceled but not performed

            controls.InGame.MoveCursorMouse.performed += ProcessMoveCursorMouse;
            controls.InGame.MoveCursorStick.performed += ProcessMoveCursorStick;

            controls.InGame.AlternateCursorMovementMode.started += ToggleAltMove;
            controls.InGame.AlternateCursorMovementMode.canceled += ToggleAltMove;

            controls.InGame.AlternateCursorSensitivityMode.started += ToggleAltSens;
            controls.InGame.AlternateCursorSensitivityMode.canceled += ToggleAltSens;

            controls.InGame.GrabRelease.performed += ProcessGrabRelease;
            controls.InGame.GrabRelease.canceled += ProcessGrabRelease;

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

        #endregion

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
            //Debug.Log($"New Walk Input: {walkInput}");
        }

        #endregion

        #region Cursor Position

        //this is something that PlayerController would have to constantly get (for moving hands), so make it a property

        private Vector2 m_cursorOffset;
        public Vector2 cursorOffset
        {
            get
            {
                if (usingMouse)
                {
                    Vector2 unclampedPos = Camera.main.ScreenToWorldPoint(cursorScreenPos);
                    //use stick control cursorOffset rather creating a local variable so that the cursor stays in the right place if the player switches to stick controls
                    m_cursorOffset = unclampedPos - (Vector2)user.position;
                    if (m_cursorOffset.magnitude > cursorRange)
                        m_cursorOffset = m_cursorOffset.normalized * cursorRange;
                }
                return m_cursorOffset;
            }
        }

        private bool usingMouse; //the cursor position property acts slightly differently for mouse and stick controls.

        public float cursorRange; //this is a property of the player rather than an input setting, so make it public

        //for mouse controls only
        private Vector2 cursorScreenPos;

        //for stick controls only
        private float cursorSens;

        private bool altIsToggle;
        private bool usingAltSens;
        private bool usingAltMove;

        private float altCursorSens;
        private float altCursorRangeMult;

        private void ProcessMoveCursorMouse(InputAction.CallbackContext obj)
        {
            usingMouse = true;
            //don't convert to world space here so that the position still works if the camera moves without the input updating
            cursorScreenPos = obj.ReadValue<Vector2>();
        }

        private void ProcessMoveCursorStick(InputAction.CallbackContext obj)
        {
            usingMouse = false;
            Vector2 stickValue = obj.ReadValue<Vector2>();
            if (usingAltMove)
            {
                if (usingAltSens)
                    stickValue *= altCursorRangeMult;
                if (stickValue.magnitude > 1)
                    stickValue.Normalize();
                m_cursorOffset = stickValue * cursorRange;
            }
            else
            {
                stickValue *= usingAltSens ? altCursorSens : cursorSens;
                m_cursorOffset += stickValue;
                if (m_cursorOffset.magnitude > cursorRange)
                    m_cursorOffset = m_cursorOffset.normalized * cursorRange;
            }
            //Debug.Log($"Stick move cursor to offset {cursorOffset}");
        }

        private void ToggleAltSens(InputAction.CallbackContext obj)
        {
            if (obj.started || !altIsToggle)
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
            //obj.performed = pressed, obj.cancelled = released

            if (toggleGrab)
            {
                if (obj.performed)
                {
                    grabbing = !grabbing;
                    (grabbing ? Grab : Release)?.Invoke(); //this is probably the weirdest line I've ever written
                }
            }
            else
            {
                if (obj.performed)
                {
                    grabbing = true;
                    Grab?.Invoke();
                    //Debug.Log("Grab");
                }
                else
                {
                    grabbing = false;
                    Release?.Invoke();
                    //Debug.Log("Release");
                }
            }
        }

        #endregion

        #region Item Action

        //this is something PlayerController only needs to know when it's triggered (to trigger an action with the held item), so make it an event

        public event Action ItemAction;

        //this doesn't need any processing so the event is directly hooked to the input in initialization

        #endregion
    }
}
