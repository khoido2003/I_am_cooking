using UnityEngine;

public class GameInput : MonoBehaviour
{
    private PlayerInputAction playerInputAction;

    private void Awake()
    {
        playerInputAction = new PlayerInputAction();
        playerInputAction.Player.Enable();
    
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
