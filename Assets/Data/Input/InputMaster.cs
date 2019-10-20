// GENERATED AUTOMATICALLY FROM 'Assets/Data/Input/InputMaster.inputactions'

using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class InputMaster : IInputActionCollection
{
    private InputActionAsset asset;
    public InputMaster()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""InputMaster"",
    ""maps"": [
        {
            ""name"": ""Gameplay"",
            ""id"": ""e523de6a-ece7-4128-8f2a-3bbdc8368693"",
            ""actions"": [
                {
                    ""name"": ""CameraMovement"",
                    ""type"": ""Value"",
                    ""id"": ""e61bc419-01f8-4506-b2cd-d2450570a855"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Zoom"",
                    ""type"": ""Value"",
                    ""id"": ""7bd11967-ff11-4c11-afdf-8631a7f85270"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Focus"",
                    ""type"": ""Button"",
                    ""id"": ""14c6de3c-4cfd-4ac2-8590-35ee70db2665"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""arrows"",
                    ""id"": ""a34a2a03-1070-44a8-9365-7872cc656945"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""CameraMovement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""372a671a-0492-4a43-9e12-139a7863bb19"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""CameraMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""da60724d-4ec4-4916-8501-fe91e6711bb6"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""CameraMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""f0a1b029-550d-4395-8517-e2d5dd688485"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""CameraMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""27cf7fe6-f6d8-4e19-9658-48275a91244c"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""CameraMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""1D Axis"",
                    ""id"": ""8c175a84-271c-4dae-b1fc-8d70a55258b6"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Zoom"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""1D Axis"",
                    ""id"": ""30419a67-0f5c-4269-a41c-350dde1e13d5"",
                    ""path"": ""1DAxis(whichSideWins=1)"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Zoom"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""bc650a9b-d1c0-4c22-b220-44122a5bd503"",
                    ""path"": ""<Mouse>/scroll/y"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""Zoom"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""ff09ddad-bcd5-450a-a450-890b57c0ea9a"",
                    ""path"": ""<Mouse>/scroll/y"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""Zoom"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""796ee980-0028-47c2-866e-d389b8482e95"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""Focus"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Keyboard and Mouse"",
            ""bindingGroup"": ""Keyboard and Mouse"",
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
        }
    ]
}");
        // Gameplay
        m_Gameplay = asset.GetActionMap("Gameplay");
        m_Gameplay_CameraMovement = m_Gameplay.GetAction("CameraMovement");
        m_Gameplay_Zoom = m_Gameplay.GetAction("Zoom");
        m_Gameplay_Focus = m_Gameplay.GetAction("Focus");
    }

    ~InputMaster()
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

    // Gameplay
    private readonly InputActionMap m_Gameplay;
    private IGameplayActions m_GameplayActionsCallbackInterface;
    private readonly InputAction m_Gameplay_CameraMovement;
    private readonly InputAction m_Gameplay_Zoom;
    private readonly InputAction m_Gameplay_Focus;
    public struct GameplayActions
    {
        private InputMaster m_Wrapper;
        public GameplayActions(InputMaster wrapper) { m_Wrapper = wrapper; }
        public InputAction @CameraMovement => m_Wrapper.m_Gameplay_CameraMovement;
        public InputAction @Zoom => m_Wrapper.m_Gameplay_Zoom;
        public InputAction @Focus => m_Wrapper.m_Gameplay_Focus;
        public InputActionMap Get() { return m_Wrapper.m_Gameplay; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(GameplayActions set) { return set.Get(); }
        public void SetCallbacks(IGameplayActions instance)
        {
            if (m_Wrapper.m_GameplayActionsCallbackInterface != null)
            {
                CameraMovement.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnCameraMovement;
                CameraMovement.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnCameraMovement;
                CameraMovement.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnCameraMovement;
                Zoom.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnZoom;
                Zoom.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnZoom;
                Zoom.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnZoom;
                Focus.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnFocus;
                Focus.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnFocus;
                Focus.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnFocus;
            }
            m_Wrapper.m_GameplayActionsCallbackInterface = instance;
            if (instance != null)
            {
                CameraMovement.started += instance.OnCameraMovement;
                CameraMovement.performed += instance.OnCameraMovement;
                CameraMovement.canceled += instance.OnCameraMovement;
                Zoom.started += instance.OnZoom;
                Zoom.performed += instance.OnZoom;
                Zoom.canceled += instance.OnZoom;
                Focus.started += instance.OnFocus;
                Focus.performed += instance.OnFocus;
                Focus.canceled += instance.OnFocus;
            }
        }
    }
    public GameplayActions @Gameplay => new GameplayActions(this);
    private int m_KeyboardandMouseSchemeIndex = -1;
    public InputControlScheme KeyboardandMouseScheme
    {
        get
        {
            if (m_KeyboardandMouseSchemeIndex == -1) m_KeyboardandMouseSchemeIndex = asset.GetControlSchemeIndex("Keyboard and Mouse");
            return asset.controlSchemes[m_KeyboardandMouseSchemeIndex];
        }
    }
    public interface IGameplayActions
    {
        void OnCameraMovement(InputAction.CallbackContext context);
        void OnZoom(InputAction.CallbackContext context);
        void OnFocus(InputAction.CallbackContext context);
    }
}
