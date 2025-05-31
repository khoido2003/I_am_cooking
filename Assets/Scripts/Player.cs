using System;
using UnityEngine;

public class Player : MonoBehaviour, IKitchentObjectParent
{
    public static Player Instance { get; private set; }

    public event EventHandler OnPickedSomething;

    // Event to select counter
    public event EventHandler<OnSelectCounterChangedEventArgs> OnSelectedCounterChanged;

    public class OnSelectCounterChangedEventArgs : EventArgs
    {
        public BaseCounter selectedCounter;
    }

    [SerializeField]
    private float moveSpeed = 7f;

    [SerializeField]
    private GameInput gameInput;

    [SerializeField]
    private LayerMask counterLayerMask;

    private KitchenObject kitchenObject;

    [SerializeField]
    private Transform kitchenObjectHoldPoint;

    private bool isWalking = false;
    private Vector3 lastInteractDir;
    private BaseCounter selectedCounter;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Subscribe to Event triggered from GameInput
        gameInput.OnInteraction += GameInput_OnInteraction;
        gameInput.OnInteractionAlternate += GameInput_OnInteractionAlternate;
    }

    private void GameInput_OnInteraction(object sender, System.EventArgs e)
    {
        // If not in playing mode then can not interact
        if (!KichenGameManager.Instance.IsGamePlaying())
        {
            return;
        }

        if (selectedCounter != null)
        {
            selectedCounter.Interact(this);
            // Debug.Log("Found something here");
        }
    }

    private void GameInput_OnInteractionAlternate(object sender, System.EventArgs e)
    {
        // If not in playing mode then can not interact
        if (!KichenGameManager.Instance.IsGamePlaying())
        {
            return;
        }

        if (selectedCounter != null)
        {
            selectedCounter.InteractAlternate(this);
            // Debug.Log("Found something here");
        }
    }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one player instance");
        }
        Instance = this;
    }

    // Update is called once per frame
    private void Update()
    {
        HandleMovement();
        HandleInteraction();
    }

    public bool IsWalking()
    {
        return isWalking;
    }

    private void HandleInteraction()
    {
        float interactDistance = 2f;

        Vector2 inputVector = gameInput.GetMovementVectorNormalized();

        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        if (moveDir != Vector3.zero)
        {
            lastInteractDir = moveDir;
        }

        RaycastHit raycastHit;
        if (
            Physics.Raycast(
                transform.position,
                lastInteractDir,
                out raycastHit,
                interactDistance,
                counterLayerMask
            )
        )
        {
            BaseCounter baseCounter = null;
            if (raycastHit.transform.TryGetComponent(out baseCounter))
            {
                // Debug.Log("ClearCounter found on hit object: " + raycastHit.transform.name);
            }
            else if (
                raycastHit.transform.parent != null
                && raycastHit.transform.parent.TryGetComponent(out baseCounter)
            )
            {
                // Debug.Log("ClearCounter found on parent: " + raycastHit.transform.parent.name);
            }
            else
            {
                // Debug.Log(
                //     "No ClearCounter component on " + raycastHit.transform.name + " or its parent"
                // );
            }

            if (baseCounter != null)
            {
                if (baseCounter != selectedCounter)
                {
                    SetSelectedCounter(baseCounter);
                }
            }
            else
            {
                SetSelectedCounter(null);
            }
        }
        else
        {
            SetSelectedCounter(null);
            // Debug.Log("Raycast missed");
        }
    }

    private void HandleMovement()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();

        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        float playerRadius = .7f;
        float playerHeight = 2f;
        float moveDistance = moveSpeed * Time.deltaTime;

        // Check collision
        bool canMove = !Physics.CapsuleCast(
            transform.position,
            transform.position + Vector3.up * playerHeight,
            playerRadius,
            moveDir,
            moveDistance
        );

        // Handle move diagnonally
        if (!canMove)
        {
            // Can not move towards moveDir

            // Attempt only x movement
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
            canMove =
                moveDir.x != 0
                && !Physics.CapsuleCast(
                    transform.position,
                    transform.position + Vector3.up * playerHeight,
                    playerRadius,
                    moveDirX,
                    moveDistance
                );

            if (canMove)
            {
                // Can move only on the moveDirX
                moveDir = moveDirX;
            }
            else
            {
                // Can not move only on the X

                // Attempt only Z movement
                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;

                canMove =
                    moveDir.z != 0
                    && !Physics.CapsuleCast(
                        transform.position,
                        transform.position + Vector3.up * playerHeight,
                        playerRadius,
                        moveDirZ,
                        moveDistance
                    );

                if (canMove)
                {
                    // Can move only on the z
                    moveDir = moveDirZ;
                }
                else
                {
                    // Can not move in any direction
                }
            }
        }

        if (canMove)
        {
            // Move player
            transform.position += moveDir * moveDistance;
        }

        // Change the face of character to the move direction
        float rotateSpeed = 10f;
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);

        // check if player walking or not
        isWalking = moveDir != Vector3.zero;
    }

    private void SetSelectedCounter(BaseCounter selectedCounter)
    {
        this.selectedCounter = selectedCounter;

        // Trigger event to selected counter
        OnSelectedCounterChanged?.Invoke(
            this,
            new OnSelectCounterChangedEventArgs { selectedCounter = selectedCounter }
        );
    }

    // Helper method to convert LayerMask to readable string
    private string LayerMaskToString(LayerMask layerMask)
    {
        int mask = layerMask.value;
        System.Text.StringBuilder result = new System.Text.StringBuilder();
        for (int i = 0; i < 32; i++)
        {
            if ((mask & (1 << i)) != 0)
            {
                result.Append(LayerMask.LayerToName(i)).Append(" ");
            }
        }
        return result.Length > 0 ? result.ToString() : "None";
    }

    public Transform GetKitchenObjectFollowTransform()
    {
        return kitchenObjectHoldPoint;
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;

        if (kitchenObject != null)
        {
            OnPickedSomething?.Invoke(this, EventArgs.Empty);
        }
    }

    public KitchenObject GetKitchenObject()
    {
        return kitchenObject;
    }

    public void ClearKitchenObject()
    {
        kitchenObject = null;
    }

    public bool HasKitchenObject()
    {
        return kitchenObject != null;
    }
}
