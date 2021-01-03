// GENERATED AUTOMATICALLY FROM 'Assets/ZB_Inputs/Controls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @Controls : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @Controls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""Controls"",
    ""maps"": [
        {
            ""name"": ""Player"",
            ""id"": ""074f7ae4-ea86-4416-8930-3f7330be00e6"",
            ""actions"": [
                {
                    ""name"": ""Move Camera"",
                    ""type"": ""Value"",
                    ""id"": ""5e0c7640-9c6a-47c7-81a9-4cdf03ec962b"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""WASD"",
                    ""id"": ""eb61ff46-dc31-4139-a946-73d433f53b54"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move Camera"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""25ee5d6f-cdfc-4394-b84e-3c9dc173411e"",
                    ""path"": ""W"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyBoard & Mouse "",
                    ""action"": ""Move Camera"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""464dbafe-4db2-45fd-82bf-460d9916b3f3"",
                    ""path"": ""S"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyBoard & Mouse "",
                    ""action"": ""Move Camera"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""26fb9eaa-50b5-4d7c-87e6-68a6aa4436fe"",
                    ""path"": ""A"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyBoard & Mouse "",
                    ""action"": ""Move Camera"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""627a8a72-c08f-4a23-8e1d-97c090398946"",
                    ""path"": ""D"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyBoard & Mouse "",
                    ""action"": ""Move Camera"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Arrow Keys"",
                    ""id"": ""d71cd1f6-7f56-445c-a190-1406947452ea"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move Camera"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""15290353-5398-402d-8467-a4d596aa9e60"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyBoard & Mouse "",
                    ""action"": ""Move Camera"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""94b5bd58-b9e3-4e69-a865-d0ff2e6df19a"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyBoard & Mouse "",
                    ""action"": ""Move Camera"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""80b956b7-ff8b-49e4-b17e-44a1f028fc2b"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyBoard & Mouse "",
                    ""action"": ""Move Camera"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""4cbd6f6f-68b7-4d16-abd6-d1b555cfd50f"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyBoard & Mouse "",
                    ""action"": ""Move Camera"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""KeyBoard & Mouse "",
            ""bindingGroup"": ""KeyBoard & Mouse "",
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
            ""name"": ""XBOX Controller"",
            ""bindingGroup"": ""XBOX Controller"",
            ""devices"": [
                {
                    ""devicePath"": ""<XInputController>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Player
        m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
        m_Player_MoveCamera = m_Player.FindAction("Move Camera", throwIfNotFound: true);
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

    // Player
    private readonly InputActionMap m_Player;
    private IPlayerActions m_PlayerActionsCallbackInterface;
    private readonly InputAction m_Player_MoveCamera;
    public struct PlayerActions
    {
        private @Controls m_Wrapper;
        public PlayerActions(@Controls wrapper) { m_Wrapper = wrapper; }
        public InputAction @MoveCamera => m_Wrapper.m_Player_MoveCamera;
        public InputActionMap Get() { return m_Wrapper.m_Player; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerActions instance)
        {
            if (m_Wrapper.m_PlayerActionsCallbackInterface != null)
            {
                @MoveCamera.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMoveCamera;
                @MoveCamera.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMoveCamera;
                @MoveCamera.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMoveCamera;
            }
            m_Wrapper.m_PlayerActionsCallbackInterface = instance;
            if (instance != null)
            {
                @MoveCamera.started += instance.OnMoveCamera;
                @MoveCamera.performed += instance.OnMoveCamera;
                @MoveCamera.canceled += instance.OnMoveCamera;
            }
        }
    }
    public PlayerActions @Player => new PlayerActions(this);
    private int m_KeyBoardMouseSchemeIndex = -1;
    public InputControlScheme KeyBoardMouseScheme
    {
        get
        {
            if (m_KeyBoardMouseSchemeIndex == -1) m_KeyBoardMouseSchemeIndex = asset.FindControlSchemeIndex("KeyBoard & Mouse ");
            return asset.controlSchemes[m_KeyBoardMouseSchemeIndex];
        }
    }
    private int m_XBOXControllerSchemeIndex = -1;
    public InputControlScheme XBOXControllerScheme
    {
        get
        {
            if (m_XBOXControllerSchemeIndex == -1) m_XBOXControllerSchemeIndex = asset.FindControlSchemeIndex("XBOX Controller");
            return asset.controlSchemes[m_XBOXControllerSchemeIndex];
        }
    }
    public interface IPlayerActions
    {
        void OnMoveCamera(InputAction.CallbackContext context);
    }
}
