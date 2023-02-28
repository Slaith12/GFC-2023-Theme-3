//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.4.4
//     from Assets/Inputs/Player Controls.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @GeneratedPlayerControls : IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @GeneratedPlayerControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""Player Controls"",
    ""maps"": [
        {
            ""name"": ""InGame"",
            ""id"": ""315f1efc-ac7c-4b2f-88dc-8ec3812ccc5c"",
            ""actions"": [
                {
                    ""name"": ""Walk"",
                    ""type"": ""Value"",
                    ""id"": ""28017efa-c3c2-4da5-b504-d2e8531ae7bf"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Move Cursor (Mouse)"",
                    ""type"": ""Value"",
                    ""id"": ""9d093422-ec4f-447c-8409-09b1ed7f0f2d"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Move Cursor (Stick)"",
                    ""type"": ""Value"",
                    ""id"": ""8823337b-ef1d-459a-a755-755d62eefffc"",
                    ""expectedControlType"": ""Stick"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Grab/Release"",
                    ""type"": ""Button"",
                    ""id"": ""94f0f0f8-9eda-4af3-9441-9f60b2214571"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Action"",
                    ""type"": ""Button"",
                    ""id"": ""83382d85-a46e-45ce-9bda-35b5508c278d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""Keyboard A/D"",
                    ""id"": ""328c1dc3-e511-47a2-a4f3-92fef4a75ffb"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Walk"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""de007cfe-b1bf-4917-9733-a27f0453053a"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Mouse & Keyboard"",
                    ""action"": ""Walk"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""2b2b5a6d-ed31-4bd6-b0c3-549e9a563eda"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Mouse & Keyboard"",
                    ""action"": ""Walk"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Controller Left Stick"",
                    ""id"": ""d9038c5f-5a49-41d0-bc5a-9f73ddf25450"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Walk"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""c7e2cd31-811e-40ce-9bae-bb56996bb629"",
                    ""path"": ""<Gamepad>/leftStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""Walk"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""8d89eb19-da62-4e23-8d14-d127172b6516"",
                    ""path"": ""<Gamepad>/leftStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""Walk"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Keyboard Left/Right Arrow"",
                    ""id"": ""ea15e7e5-114e-44ce-af04-481cb83735c5"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Walk"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""5312e743-62ec-47ac-957f-8b814bee194d"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Mouse & Keyboard"",
                    ""action"": ""Walk"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""0d8df53f-9f02-4896-b175-c44d2f3aa2ff"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Mouse & Keyboard"",
                    ""action"": ""Walk"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Controller Left/Right DPad"",
                    ""id"": ""c529da0e-fbba-4965-9be5-6ac8ac8fd116"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Walk"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""d69df3bb-8a58-4cf8-960b-ab631d9127ab"",
                    ""path"": ""<Gamepad>/dpad/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""Walk"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""6c078aa7-14c6-44fc-9982-d23bb6b7a322"",
                    ""path"": ""<Gamepad>/dpad/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""Walk"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""ad289559-6403-4b8d-8e9e-a91541c498fd"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Mouse & Keyboard"",
                    ""action"": ""Move Cursor (Mouse)"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0e54d7e4-5a07-4b36-8dd5-5ddadcf4cc14"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""Move Cursor (Stick)"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0fc81761-87a4-4a4c-9343-f5455ca95fee"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""Grab/Release"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6c846164-96bf-4266-bf0b-10abeaa3b47a"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Mouse & Keyboard"",
                    ""action"": ""Grab/Release"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b0126ba1-adca-4b99-9802-274fc49333a3"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Mouse & Keyboard"",
                    ""action"": ""Action"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8d2304b7-995a-4e5d-972f-c2a2fb25c829"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""Action"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Mouse & Keyboard"",
            ""bindingGroup"": ""Mouse & Keyboard"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Controller"",
            ""bindingGroup"": ""Controller"",
            ""devices"": [
                {
                    ""devicePath"": ""<Gamepad>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // InGame
        m_InGame = asset.FindActionMap("InGame", throwIfNotFound: true);
        m_InGame_Walk = m_InGame.FindAction("Walk", throwIfNotFound: true);
        m_InGame_MoveCursorMouse = m_InGame.FindAction("Move Cursor (Mouse)", throwIfNotFound: true);
        m_InGame_MoveCursorStick = m_InGame.FindAction("Move Cursor (Stick)", throwIfNotFound: true);
        m_InGame_GrabRelease = m_InGame.FindAction("Grab/Release", throwIfNotFound: true);
        m_InGame_Action = m_InGame.FindAction("Action", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }
    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }
    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // InGame
    private readonly InputActionMap m_InGame;
    private IInGameActions m_InGameActionsCallbackInterface;
    private readonly InputAction m_InGame_Walk;
    private readonly InputAction m_InGame_MoveCursorMouse;
    private readonly InputAction m_InGame_MoveCursorStick;
    private readonly InputAction m_InGame_GrabRelease;
    private readonly InputAction m_InGame_Action;
    public struct InGameActions
    {
        private @GeneratedPlayerControls m_Wrapper;
        public InGameActions(@GeneratedPlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Walk => m_Wrapper.m_InGame_Walk;
        public InputAction @MoveCursorMouse => m_Wrapper.m_InGame_MoveCursorMouse;
        public InputAction @MoveCursorStick => m_Wrapper.m_InGame_MoveCursorStick;
        public InputAction @GrabRelease => m_Wrapper.m_InGame_GrabRelease;
        public InputAction @Action => m_Wrapper.m_InGame_Action;
        public InputActionMap Get() { return m_Wrapper.m_InGame; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(InGameActions set) { return set.Get(); }
        public void SetCallbacks(IInGameActions instance)
        {
            if (m_Wrapper.m_InGameActionsCallbackInterface != null)
            {
                @Walk.started -= m_Wrapper.m_InGameActionsCallbackInterface.OnWalk;
                @Walk.performed -= m_Wrapper.m_InGameActionsCallbackInterface.OnWalk;
                @Walk.canceled -= m_Wrapper.m_InGameActionsCallbackInterface.OnWalk;
                @MoveCursorMouse.started -= m_Wrapper.m_InGameActionsCallbackInterface.OnMoveCursorMouse;
                @MoveCursorMouse.performed -= m_Wrapper.m_InGameActionsCallbackInterface.OnMoveCursorMouse;
                @MoveCursorMouse.canceled -= m_Wrapper.m_InGameActionsCallbackInterface.OnMoveCursorMouse;
                @MoveCursorStick.started -= m_Wrapper.m_InGameActionsCallbackInterface.OnMoveCursorStick;
                @MoveCursorStick.performed -= m_Wrapper.m_InGameActionsCallbackInterface.OnMoveCursorStick;
                @MoveCursorStick.canceled -= m_Wrapper.m_InGameActionsCallbackInterface.OnMoveCursorStick;
                @GrabRelease.started -= m_Wrapper.m_InGameActionsCallbackInterface.OnGrabRelease;
                @GrabRelease.performed -= m_Wrapper.m_InGameActionsCallbackInterface.OnGrabRelease;
                @GrabRelease.canceled -= m_Wrapper.m_InGameActionsCallbackInterface.OnGrabRelease;
                @Action.started -= m_Wrapper.m_InGameActionsCallbackInterface.OnAction;
                @Action.performed -= m_Wrapper.m_InGameActionsCallbackInterface.OnAction;
                @Action.canceled -= m_Wrapper.m_InGameActionsCallbackInterface.OnAction;
            }
            m_Wrapper.m_InGameActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Walk.started += instance.OnWalk;
                @Walk.performed += instance.OnWalk;
                @Walk.canceled += instance.OnWalk;
                @MoveCursorMouse.started += instance.OnMoveCursorMouse;
                @MoveCursorMouse.performed += instance.OnMoveCursorMouse;
                @MoveCursorMouse.canceled += instance.OnMoveCursorMouse;
                @MoveCursorStick.started += instance.OnMoveCursorStick;
                @MoveCursorStick.performed += instance.OnMoveCursorStick;
                @MoveCursorStick.canceled += instance.OnMoveCursorStick;
                @GrabRelease.started += instance.OnGrabRelease;
                @GrabRelease.performed += instance.OnGrabRelease;
                @GrabRelease.canceled += instance.OnGrabRelease;
                @Action.started += instance.OnAction;
                @Action.performed += instance.OnAction;
                @Action.canceled += instance.OnAction;
            }
        }
    }
    public InGameActions @InGame => new InGameActions(this);
    private int m_MouseKeyboardSchemeIndex = -1;
    public InputControlScheme MouseKeyboardScheme
    {
        get
        {
            if (m_MouseKeyboardSchemeIndex == -1) m_MouseKeyboardSchemeIndex = asset.FindControlSchemeIndex("Mouse & Keyboard");
            return asset.controlSchemes[m_MouseKeyboardSchemeIndex];
        }
    }
    private int m_ControllerSchemeIndex = -1;
    public InputControlScheme ControllerScheme
    {
        get
        {
            if (m_ControllerSchemeIndex == -1) m_ControllerSchemeIndex = asset.FindControlSchemeIndex("Controller");
            return asset.controlSchemes[m_ControllerSchemeIndex];
        }
    }
    public interface IInGameActions
    {
        void OnWalk(InputAction.CallbackContext context);
        void OnMoveCursorMouse(InputAction.CallbackContext context);
        void OnMoveCursorStick(InputAction.CallbackContext context);
        void OnGrabRelease(InputAction.CallbackContext context);
        void OnAction(InputAction.CallbackContext context);
    }
}