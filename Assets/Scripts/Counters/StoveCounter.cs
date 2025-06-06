using System;
using System.Collections;
using UnityEngine;

public class StoveCounter : BaseCounter, IHasProgress
{
    // Event for progress bar
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;

    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;

    public class OnStateChangedEventArgs : EventArgs
    {
        public State state;
    }

    [SerializeField]
    private FryingRecipeSO[] fryingRecipeSOArray;

    [SerializeField]
    private BurningRecipeSO[] burningRecipeSOArray;

    // // Using coroutine
    // public void Start()
    // {
    //     StartCoroutine(HandleFryTimer());
    // }
    //
    // private IEnumerator HandleFryTimer()
    // {
    //     yield return new WaitForSeconds(1f);
    // }
    //
    //

    private float fryingTimer;
    private FryingRecipeSO fryingRecipeSO;
    private float burningTimer;
    private BurningRecipeSO burningRecipeSO;

    public enum State
    {
        Idle,
        Frying,
        Fried,
        Burn,
    }

    private State state;

    private void Start() { }

    private void Update()
    {
        if (HasKitchenObject())
        {
            switch (state)
            {
                case State.Idle:
                    break;

                case State.Frying:
                    fryingTimer += Time.deltaTime;

                    // Trigger progress bar UI
                    OnProgressChanged?.Invoke(
                        this,
                        new IHasProgress.OnProgressChangedEventArgs
                        {
                            progressNormalized = fryingTimer / fryingRecipeSO.fryingTimeMax,
                        }
                    );

                    if (fryingTimer > fryingRecipeSO.fryingTimeMax)
                    {
                        GetKitchenObject().DestroySelf();

                        KitchenObject.SpawnKitchenObject(fryingRecipeSO.output, this);

                        state = State.Fried;

                        // Start burning time
                        burningTimer = 0f;
                        burningRecipeSO = GetBurningRecipeSOWithInput(
                            GetKitchenObject().GetKitchenObjectSO()
                        );

                        state = State.Fried;

                        // Trigger animation event
                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { state = state });
                    }
                    break;
                case State.Fried:

                    burningTimer += Time.deltaTime;

                    // Trigger progress bar UI
                    OnProgressChanged?.Invoke(
                        this,
                        new IHasProgress.OnProgressChangedEventArgs
                        {
                            progressNormalized = burningTimer / burningRecipeSO.burningTimeMax,
                        }
                    );

                    if (burningTimer > burningRecipeSO.burningTimeMax)
                    {
                        GetKitchenObject().DestroySelf();

                        KitchenObject.SpawnKitchenObject(burningRecipeSO.output, this);

                        state = State.Burn;

                        // Trigger animation event
                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { state = state });

                        // Trigger progress bar UI
                        OnProgressChanged?.Invoke(
                            this,
                            new IHasProgress.OnProgressChangedEventArgs { progressNormalized = 0f }
                        );
                    }

                    break;
                case State.Burn:
                    break;
            }
            // Debug.Log(state);
        }
    }

    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            // There is no Kitchen Object here
            if (player.HasKitchenObject())
            {
                // Player is carrying something
                if (HasRecipewithInput(player.GetKitchenObject().GetKitchenObjectSO()))
                {
                    // Player carry something that can be fried then they can place here
                    player.GetKitchenObject().SetKitchenObjectParent(this);

                    fryingRecipeSO = GetFryingRecipeSOWithInput(
                        GetKitchenObject().GetKitchenObjectSO()
                    );

                    // Set current state
                    state = State.Frying;

                    fryingTimer = 0f;

                    // Trigger animation event
                    OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { state = state });

                    // Trigger progress bar UI
                    //
                    OnProgressChanged?.Invoke(
                        this,
                        new IHasProgress.OnProgressChangedEventArgs
                        {
                            progressNormalized = fryingTimer / fryingRecipeSO.fryingTimeMax,
                        }
                    );
                }
            }
            else
            {
                // Player has nothing on hand
            }
        }
        else
        {
            // There is a kitchen object here
            if (player.HasKitchenObject())
            {
                // Player is carrying something

                if (
                    player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject)
                )
                {
                    // User is holding a plate
                    // Find the food then remove it so later on can add the food on top of the plate
                    if (
                        plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO())
                    )
                    {
                        GetKitchenObject().DestroySelf();

                        state = State.Idle;
                        // Trigger animation event
                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { state = state });

                        // Trigger progress bar UI
                        OnProgressChanged?.Invoke(
                            this,
                            new IHasProgress.OnProgressChangedEventArgs { progressNormalized = 0f }
                        );
                    }
                }
            }
            else
            {
                // Player is not carrying anything
                GetKitchenObject().SetKitchenObjectParent(player);

                state = State.Idle;

                // Trigger animation event
                OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { state = state });

                // Trigger progress bar UI
                OnProgressChanged?.Invoke(
                    this,
                    new IHasProgress.OnProgressChangedEventArgs { progressNormalized = 0f }
                );
            }
        }
    }

    private bool HasRecipewithInput(KitchenObjectSO inputKitchenSO)
    {
        return GetFryingRecipeSOWithInput(inputKitchenSO) != null;
    }

    private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenSO)
    {
        FryingRecipeSO fryingRecipeSO = GetFryingRecipeSOWithInput(inputKitchenSO);

        if (fryingRecipeSO != null)
        {
            return fryingRecipeSO.output;
        }
        else
        {
            return null;
        }
    }

    private FryingRecipeSO GetFryingRecipeSOWithInput(KitchenObjectSO inputKitchenSO)
    {
        Debug.Log(inputKitchenSO);
        foreach (FryingRecipeSO fryingRecipeSO in fryingRecipeSOArray)
        {
            if (fryingRecipeSO.input == inputKitchenSO)
            {
                return fryingRecipeSO;
            }
        }
        return null;
    }

    private BurningRecipeSO GetBurningRecipeSOWithInput(KitchenObjectSO inputKitchenSO)
    {
        Debug.Log(inputKitchenSO);
        foreach (BurningRecipeSO burningRecipeSO in burningRecipeSOArray)
        {
            if (burningRecipeSO.input == inputKitchenSO)
            {
                return burningRecipeSO;
            }
        }
        return null;
    }

    public bool IsFried()
    {
        return state == State.Fried;
    }
}
