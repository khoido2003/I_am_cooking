using System;
using System.Collections.Generic;
using UnityEngine;

public class PlateCompleteVisual : MonoBehaviour
{
    [SerializeField]
    private PlateKitchenObject plateKitchenObject;

    [SerializeField]
    private List<KitchenObjectSO_GameObject> kitchenObjectSO_GameObjectList;

    [Serializable]
    public struct KitchenObjectSO_GameObject
    {
        public KitchenObjectSO kitchenObjectSO;
        public GameObject gameObject;
    }

    private void Start()
    {
        plateKitchenObject.OnIngerdientAdded += PlateKitchenObject_OnIngredientAdded;

        foreach (
            KitchenObjectSO_GameObject kitchenObjectSO_GameObject in kitchenObjectSO_GameObjectList
        )
        {
            kitchenObjectSO_GameObject.gameObject.SetActive(false);
        }
    }

    private void PlateKitchenObject_OnIngredientAdded(
        object sender,
        PlateKitchenObject.OnIngerdientAddedEventArgs e
    )
    {
        foreach (
            KitchenObjectSO_GameObject kitchenObjectSO_GameObject in kitchenObjectSO_GameObjectList
        )
        {
            if (kitchenObjectSO_GameObject.kitchenObjectSO == e.kitchenObjectSO)
            {
                kitchenObjectSO_GameObject.gameObject.SetActive(true);
            }
        }
    }
}
