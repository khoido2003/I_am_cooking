using System;
using UnityEngine;

public class PlatesCounter : BaseCounter
{
    private float spawnPlateTimer;
    private float spawnPlateTimerMax = 4f;

    [SerializeField]
    private KitchenObjectSO plateKitchenObjectSO;

    private int plateSpawnedAmount;
    private int plateSpawnedAmountMax = 4;

    public event EventHandler OnPlateSpawned;
    public event EventHandler OnPlateRemoved;

    private void Update()
    {
        spawnPlateTimer += Time.deltaTime;

        if (spawnPlateTimer > spawnPlateTimerMax)
        {
            spawnPlateTimer = 0f;

            if (plateSpawnedAmount < plateSpawnedAmountMax)
            {
                plateSpawnedAmount++;

                OnPlateSpawned?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public override void Interact(Player player)
    {
        if (!player.HasKitchenObject())
        {
            // Player is empty handed
            if (plateSpawnedAmount > 0)
            {
                // There is at least 1 plate here
                plateSpawnedAmount--;

                KitchenObject.SpawnKitchenObject(plateKitchenObjectSO, player);


                OnPlateRemoved?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
