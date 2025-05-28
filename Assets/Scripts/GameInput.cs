using System;
using UnityEngine;

public class GameInput : MonoBehaviour
{
    private PlayerInputAction playerInputAction;

    // Trigger event when interact with T
    public event EventHandler OnInteraction;

    public event EventHandler OnInteractionAlternate;

    private void Awake()
    {
        playerInputAction = new PlayerInputAction();
        playerInputAction.Player.Enable();

        playerInputAction.Player.Interact.performed += Interact_performed;
        playerInputAction.Player.InteractAlternate.performed += InteractAlternate_performed;
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
