using System;
using TMPro;
using UnityEngine;

public class GameCountDownStartUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI countDownText;

    private Animator animator;
    private int previousCountdown;

    private const string NUMBER_POPUP = "NumberPopUp";

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        KichenGameManager.Instance.OnStateChanged += KitchenManager_OnStateChanged;

        Hide();
    }

    private void KitchenManager_OnStateChanged(object sender, EventArgs e)
    {
        if (KichenGameManager.Instance.IsCountDownToStartActive())
        {
            Show();
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

    private void Update()
    {
        int countdownNumber = Mathf.CeilToInt(KichenGameManager.Instance.GetCountDownToStartTime());

        if (previousCountdown != countdownNumber)
        {
            previousCountdown = countdownNumber;
            animator.SetTrigger(NUMBER_POPUP);

            SoundManager.Instance.PlayCountdonwSound();
        }

        countDownText.text = countdownNumber.ToString();
    }
}
