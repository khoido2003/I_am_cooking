using System;
using TMPro;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI recipeDeliveredText;

    private void Start()
    {
        KichenGameManager.Instance.OnStateChanged += KitchenManager_OnStateChanged;

        Hide();
    }

    private void KitchenManager_OnStateChanged(object sender, EventArgs e)
    {
        if (KichenGameManager.Instance.IsGameOver())
        {
            Show();

            recipeDeliveredText.text = DeliveryManager
                .Instance.GetSuccessfulRecipeAmount()
                .ToString();
        }
        else
        {
            Hide();
        }
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    private void Update() { }
}
