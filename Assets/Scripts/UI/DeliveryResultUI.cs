using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeliveryResultUI : MonoBehaviour
{
    [SerializeField]
    private Image backgroundImage;

    [SerializeField]
    private Image iconImage;

    [SerializeField]
    private TextMeshProUGUI message;

    [SerializeField]
    private Color failedColor;

    [SerializeField]
    private Color successColor;

    [SerializeField]
    private Sprite successSprite;

    [SerializeField]
    private Sprite failedSprite;

    private void Start()
    {
        DeliveryManager.Instance.OnRecipeSuccess += DeliveryManager_OnRecipeSuccess;
        DeliveryManager.Instance.OnRecipeFailed += DeliveryManager_OnRecipeFailed;

        gameObject.SetActive(false);
    }

    private void DeliveryManager_OnRecipeFailed(object sender, EventArgs e)
    {
        backgroundImage.color = failedColor;
        iconImage.sprite = failedSprite;
        message.text = "DELIVERY\nFAILED";

        gameObject.SetActive(true);
        StartCoroutine(HideAfterDelay());
    }

    private void DeliveryManager_OnRecipeSuccess(object sender, EventArgs e)
    {
        backgroundImage.color = successColor;
        iconImage.sprite = successSprite;
        message.text = "DELIVERY\nSUCCESS";

        gameObject.SetActive(true);
        StartCoroutine(HideAfterDelay());
    }

    private IEnumerator HideAfterDelay()
    {
        yield return new WaitForSeconds(3f);
        gameObject.SetActive(false);
    }
}
