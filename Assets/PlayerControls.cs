//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.3.0
//     from Assets/PlayerControls.inputactions
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

public partial class @PlayerControls : IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerControls"",
    ""maps"": [
        {
            ""name"": ""MyActionMap"",
            ""id"": ""471fd7f0-1502-4bd1-8e40-b0320d9e015c"",
            ""actions"": [
                {
                    ""name"": ""Cast"",
                    ""type"": ""Button"",
                    ""id"": ""9073d7e8-e9c5-42ce-981c-dda8795dff24"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Reel"",
                    ""type"": ""Button"",
                    ""id"": ""3cc2adf4-7579-41f9-b67e-07b198acae58"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""LockServo"",
                    ""type"": ""Button"",
                    ""id"": ""39e6ef08-d453-45f1-a7ab-5f9d4d114a00"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""UnlockServo"",
                    ""type"": ""Button"",
                    ""id"": ""c76b06f6-0192-4d45-9ecb-a0029592946b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Quit"",
                    ""type"": ""Button"",
                    ""id"": ""c6a54e77-9369-4178-b304-7d4e9b3c0916"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""GoToEndScreen"",
                    ""type"": ""Button"",
                    ""id"": ""e38d97a0-b07a-4d7c-9acd-3dd42ebec9b5"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""9a8c1ee4-639c-4c9a-bc1e-60e8af4d3edf"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Cast"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""bc803b6f-f41a-41cf-8c03-5ef8c6b031ec"",
                    ""path"": ""<XInputController>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Reel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e038a0f4-54f0-4e31-9425-f903a1794f6c"",
                    ""path"": ""<XInputController>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Reel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4f848248-8a31-4149-a3f3-4653908c3ac2"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Reel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6d377efc-462b-4eac-b0f9-89e8670f241d"",
                    ""path"": ""<Keyboard>/l"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""LockServo"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""19f080d5-8cbf-400d-b3d6-35978382e3c1"",
                    ""path"": ""<Keyboard>/o"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""UnlockServo"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a2c2be7c-d50f-4211-9542-a93ced77d4f8"",
                    ""path"": ""<Keyboard>/r"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Quit"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f7bc08ec-6ebb-4a31-9c5b-411ba66466ca"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""GoToEndScreen"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // MyActionMap
        m_MyActionMap = asset.FindActionMap("MyActionMap", throwIfNotFound: true);
        m_MyActionMap_Cast = m_MyActionMap.FindAction("Cast", throwIfNotFound: true);
        m_MyActionMap_Reel = m_MyActionMap.FindAction("Reel", throwIfNotFound: true);
        m_MyActionMap_LockServo = m_MyActionMap.FindAction("LockServo", throwIfNotFound: true);
        m_MyActionMap_UnlockServo = m_MyActionMap.FindAction("UnlockServo", throwIfNotFound: true);
        m_MyActionMap_Quit = m_MyActionMap.FindAction("Quit", throwIfNotFound: true);
        m_MyActionMap_GoToEndScreen = m_MyActionMap.FindAction("GoToEndScreen", throwIfNotFound: true);
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

    // MyActionMap
    private readonly InputActionMap m_MyActionMap;
    private IMyActionMapActions m_MyActionMapActionsCallbackInterface;
    private readonly InputAction m_MyActionMap_Cast;
    private readonly InputAction m_MyActionMap_Reel;
    private readonly InputAction m_MyActionMap_LockServo;
    private readonly InputAction m_MyActionMap_UnlockServo;
    private readonly InputAction m_MyActionMap_Quit;
    private readonly InputAction m_MyActionMap_GoToEndScreen;
    public struct MyActionMapActions
    {
        private @PlayerControls m_Wrapper;
        public MyActionMapActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Cast => m_Wrapper.m_MyActionMap_Cast;
        public InputAction @Reel => m_Wrapper.m_MyActionMap_Reel;
        public InputAction @LockServo => m_Wrapper.m_MyActionMap_LockServo;
        public InputAction @UnlockServo => m_Wrapper.m_MyActionMap_UnlockServo;
        public InputAction @Quit => m_Wrapper.m_MyActionMap_Quit;
        public InputAction @GoToEndScreen => m_Wrapper.m_MyActionMap_GoToEndScreen;
        public InputActionMap Get() { return m_Wrapper.m_MyActionMap; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(MyActionMapActions set) { return set.Get(); }
        public void SetCallbacks(IMyActionMapActions instance)
        {
            if (m_Wrapper.m_MyActionMapActionsCallbackInterface != null)
            {
                @Cast.started -= m_Wrapper.m_MyActionMapActionsCallbackInterface.OnCast;
                @Cast.performed -= m_Wrapper.m_MyActionMapActionsCallbackInterface.OnCast;
                @Cast.canceled -= m_Wrapper.m_MyActionMapActionsCallbackInterface.OnCast;
                @Reel.started -= m_Wrapper.m_MyActionMapActionsCallbackInterface.OnReel;
                @Reel.performed -= m_Wrapper.m_MyActionMapActionsCallbackInterface.OnReel;
                @Reel.canceled -= m_Wrapper.m_MyActionMapActionsCallbackInterface.OnReel;
                @LockServo.started -= m_Wrapper.m_MyActionMapActionsCallbackInterface.OnLockServo;
                @LockServo.performed -= m_Wrapper.m_MyActionMapActionsCallbackInterface.OnLockServo;
                @LockServo.canceled -= m_Wrapper.m_MyActionMapActionsCallbackInterface.OnLockServo;
                @UnlockServo.started -= m_Wrapper.m_MyActionMapActionsCallbackInterface.OnUnlockServo;
                @UnlockServo.performed -= m_Wrapper.m_MyActionMapActionsCallbackInterface.OnUnlockServo;
                @UnlockServo.canceled -= m_Wrapper.m_MyActionMapActionsCallbackInterface.OnUnlockServo;
                @Quit.started -= m_Wrapper.m_MyActionMapActionsCallbackInterface.OnQuit;
                @Quit.performed -= m_Wrapper.m_MyActionMapActionsCallbackInterface.OnQuit;
                @Quit.canceled -= m_Wrapper.m_MyActionMapActionsCallbackInterface.OnQuit;
                @GoToEndScreen.started -= m_Wrapper.m_MyActionMapActionsCallbackInterface.OnGoToEndScreen;
                @GoToEndScreen.performed -= m_Wrapper.m_MyActionMapActionsCallbackInterface.OnGoToEndScreen;
                @GoToEndScreen.canceled -= m_Wrapper.m_MyActionMapActionsCallbackInterface.OnGoToEndScreen;
            }
            m_Wrapper.m_MyActionMapActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Cast.started += instance.OnCast;
                @Cast.performed += instance.OnCast;
                @Cast.canceled += instance.OnCast;
                @Reel.started += instance.OnReel;
                @Reel.performed += instance.OnReel;
                @Reel.canceled += instance.OnReel;
                @LockServo.started += instance.OnLockServo;
                @LockServo.performed += instance.OnLockServo;
                @LockServo.canceled += instance.OnLockServo;
                @UnlockServo.started += instance.OnUnlockServo;
                @UnlockServo.performed += instance.OnUnlockServo;
                @UnlockServo.canceled += instance.OnUnlockServo;
                @Quit.started += instance.OnQuit;
                @Quit.performed += instance.OnQuit;
                @Quit.canceled += instance.OnQuit;
                @GoToEndScreen.started += instance.OnGoToEndScreen;
                @GoToEndScreen.performed += instance.OnGoToEndScreen;
                @GoToEndScreen.canceled += instance.OnGoToEndScreen;
            }
        }
    }
    public MyActionMapActions @MyActionMap => new MyActionMapActions(this);
    public interface IMyActionMapActions
    {
        void OnCast(InputAction.CallbackContext context);
        void OnReel(InputAction.CallbackContext context);
        void OnLockServo(InputAction.CallbackContext context);
        void OnUnlockServo(InputAction.CallbackContext context);
        void OnQuit(InputAction.CallbackContext context);
        void OnGoToEndScreen(InputAction.CallbackContext context);
    }
}
