using UnityEngine;

public class DeliveryManagerUI : MonoBehaviour
{
    [SerializeField]
    private Transform containter;

    [SerializeField]
    private Transform recipeTemplate;

    private void Awake()
    {
        recipeTemplate.gameObject.SetActive(false);
    }

    private void Start()
    {
        DeliveryManager.Instance.OnRecipeSpawned += DeliveryManager_OnRecipeSpawned;

        DeliveryManager.Instance.OnRecipeCompleted += DeliveryManager_OnRecipeCompleted;

        UpdateVisual();
    }

    private void DeliveryManager_OnRecipeSpawned(object sender, System.EventArgs e)
    {
        UpdateVisual();
    }

    private void DeliveryManager_OnRecipeCompleted(object sender, System.EventArgs e)
    {
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        foreach (Transform child in containter)
        {
            if (child == recipeTemplate)
                continue;
            Destroy(child.gameObject);
        }

        foreach (RecipeSO recipeSO in DeliveryManager.Instance.GetWaitingRecipeSOList())
        {
            Transform recipeTransform = Instantiate(recipeTemplate, containter);

            recipeTransform.gameObject.SetActive(true);
            recipeTransform.GetComponent<DeliverySingleUI>().SetRecipeSO(recipeSO);
        }
    }
}
