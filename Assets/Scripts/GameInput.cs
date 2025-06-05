using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance { get; private set; }

    private PlayerInputAction playerInputAction;

    // Trigger event when interact with T
    public event EventHandler OnInteraction;

    public event EventHandler OnInteractionAlternate;

    public event EventHandler OnPauseAction;

    private void Awake()
    {
        Instance = this;

        playerInputAction = new PlayerInputAction();
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
}
