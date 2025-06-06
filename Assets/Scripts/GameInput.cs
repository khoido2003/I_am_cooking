using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    private const string PLAYER_PREFS_BINDINGS = "InputBindings";

    public static GameInput Instance { get; private set; }

    public enum Binding
    {
        Move_Up,
        Move_Down,
        Move_Left,
        Move_Right,
        Interact,
        InteractAlt,
        Pause,
        Gamepad_Interact,
        Gamepad_InteractAlt,
        Gamepad_Pause,
    }

    private PlayerInputAction playerInputAction;

    // Trigger event when interact with T
    public event EventHandler OnInteraction;

    public event EventHandler OnInteractionAlternate;

    public event EventHandler OnPauseAction;

    public event EventHandler OnBindingRebind;

    private void Awake()
    {
        Instance = this;

        playerInputAction = new PlayerInputAction();

        // If User has changed the keymap then load it back
        if (PlayerPrefs.HasKey(PLAYER_PREFS_BINDINGS))
        {
            playerInputAction.LoadBindingOverridesFromJson(
                PlayerPrefs.GetString(PLAYER_PREFS_BINDINGS)
            );
        }

        playerInputAction.Player.Enable();

        playerInputAction.Player.Interact.performed += Interact_performed;
        playerInputAction.Player.InteractAlternate.performed += InteractAlternate_performed;

        playerInputAction.Player.Pause.performed += Pause_performed;
    }

    private void OnDestroy()
    {
        // UnSubscribe Event
        playerInputAction.Player.Interact.performed += Interact_performed;
        playerInputAction.Player.InteractAlternate.performed += InteractAlternate_performed;
        playerInputAction.Player.Pause.performed += Pause_performed;

        playerInputAction.Dispose();
    }

    private void Pause_performed(InputAction.CallbackContext context)
    {
        OnPauseAction?.Invoke(this, EventArgs.Empty);
    }

    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        // Trigger interaction event
        // Debug.Log("Interact action performed");
        OnInteraction?.Invoke(this, EventArgs.Empty);
    }

    private void InteractAlternate_performed(
        UnityEngine.InputSystem.InputAction.CallbackContext obj
    )
    {
        // Trigger interaction event
        // Debug.Log("Interact alternate action performed");
        OnInteractionAlternate?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetMovementVectorNormalized()
    {
        Vector2 inputVector = playerInputAction.Player.Move.ReadValue<Vector2>();

        // Legacy way to handle input
        /*Vector2 inputVector = new Vector2(0, 0);*/
        /*if (Input.GetKey(KeyCode.W))*/
        /*{*/
        /*    inputVector.y -= 1;*/
        /*}*/
        /*if (Input.GetKey(KeyCode.S))*/
        /*{*/
        /*    inputVector.y += 1;*/
        /*}*/
        /*if (Input.GetKey(KeyCode.A))*/
        /*{*/
        /*    inputVector.x += 1;*/
        /*}*/
        /*if (Input.GetKey(KeyCode.D))*/
        /*{*/
        /*    inputVector.x -= 1;*/
        /*}*/

        inputVector = inputVector.normalized;

        return inputVector;
    }

    public string GetBindingText(Binding binding)
    {
        switch (binding)
        {
            default:
            case Binding.Interact:
                return playerInputAction.Player.Interact.bindings[0].ToDisplayString();

            case Binding.InteractAlt:
                return playerInputAction.Player.InteractAlternate.bindings[0].ToDisplayString();

            case Binding.Pause:
                return playerInputAction.Player.Pause.bindings[0].ToDisplayString();

            case Binding.Move_Up:
                return playerInputAction.Player.Move.bindings[1].ToDisplayString();

            case Binding.Move_Down:

                return playerInputAction.Player.Move.bindings[2].ToDisplayString();

            case Binding.Move_Right:

                return playerInputAction.Player.Move.bindings[4].ToDisplayString();

            case Binding.Move_Left:

                return playerInputAction.Player.Move.bindings[3].ToDisplayString();

            case Binding.Gamepad_Interact:

                return playerInputAction.Player.Interact.bindings[1].ToDisplayString();

            case Binding.Gamepad_InteractAlt:

                return playerInputAction.Player.InteractAlternate.bindings[1].ToDisplayString();

            case Binding.Gamepad_Pause:
                return playerInputAction.Player.Pause.bindings[1].ToDisplayString();
        }
    }

    public void RebindingKeyMap(Binding binding, Action onActionRebound)
    {
        playerInputAction.Player.Disable();

        InputAction inputAction;
        int bindingIndex;

        switch (binding)
        {
            default:
            case Binding.Move_Up:
                inputAction = playerInputAction.Player.Move;
                bindingIndex = 1;
                break;

            case Binding.Move_Down:
                inputAction = playerInputAction.Player.Move;
                bindingIndex = 2;
                break;

            case Binding.Move_Right:
                inputAction = playerInputAction.Player.Move;
                bindingIndex = 4;
                break;

            case Binding.Move_Left:
                inputAction = playerInputAction.Player.Move;
                bindingIndex = 3;
                break;
            case Binding.Interact:
                inputAction = playerInputAction.Player.Interact;
                bindingIndex = 0;
                break;

            case Binding.InteractAlt:
                inputAction = playerInputAction.Player.InteractAlternate;
                bindingIndex = 0;
                break;

            case Binding.Pause:
                inputAction = playerInputAction.Player.Pause;
                bindingIndex = 0;
                break;

            case Binding.Gamepad_Interact:
                inputAction = playerInputAction.Player.Interact;
                bindingIndex = 1;
                break;

            case Binding.Gamepad_InteractAlt:
                inputAction = playerInputAction.Player.InteractAlternate;
                bindingIndex = 1;
                break;

            case Binding.Gamepad_Pause:
                inputAction = playerInputAction.Player.Pause;
                bindingIndex = 1;
                break;
        }

        inputAction
            .PerformInteractiveRebinding(bindingIndex)
            .OnComplete(
                (callback) =>
                {
                    callback.Dispose();
                    playerInputAction.Player.Enable();
                    onActionRebound();

                    playerInputAction.SaveBindingOverridesAsJson();
                    PlayerPrefs.SetString(
                        PLAYER_PREFS_BINDINGS,
                        playerInputAction.SaveBindingOverridesAsJson()
                    );
                    PlayerPrefs.Save();

                    OnBindingRebind?.Invoke(this, EventArgs.Empty);
                }
            )
            .Start();
    }
}
