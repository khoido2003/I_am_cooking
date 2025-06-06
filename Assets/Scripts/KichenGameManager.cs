using System;
using UnityEngine;

public class KichenGameManager : MonoBehaviour
{
    private enum State
    {
        WaitingToStart,
        CountdownToStart,
        GamePlaying,
        GameOver,
    }

    public event EventHandler OnGamePaused;
    public event EventHandler OnGameUnPaused;

    public event EventHandler OnStateChanged;

    public static KichenGameManager Instance { get; private set; }

    private State state;

    // private float waitingToStartTimer = 1f;
    private float countDownToStartTimer = 3f;
    private float gamePlayingTimer;
    private float gamePlayingTimerMax = 10f;

    private bool isGamePause = false;

    private void Awake()
    {
        state = State.WaitingToStart;

        Instance = this;
    }

    private void Start()
    {
        GameInput.Instance.OnPauseAction += GameInput_OnPauseAction;
        GameInput.Instance.OnInteraction += GameInput_OnIteractionAction;
    }

    private void GameInput_OnIteractionAction(object sender, EventArgs e)
    {
        if (state == State.WaitingToStart)
        {
            state = State.CountdownToStart;

            OnStateChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    private void GameInput_OnPauseAction(object sender, EventArgs e)
    {
        TogglePauseGame();
    }

    public void TogglePauseGame()
    {
        isGamePause = !isGamePause;

        if (isGamePause)
        {
            Time.timeScale = 0f;
            OnGamePaused?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            OnGameUnPaused?.Invoke(this, EventArgs.Empty);
            Time.timeScale = 1f;
        }
    }

    private void Update()
    {
        switch (state)
        {
            case State.WaitingToStart:
                // waitingToStartTimer -= Time.deltaTime;
                // if (waitingToStartTimer < 0f)
                // {
                //     state = State.CountdownToStart;
                //     OnStateChanged?.Invoke(this, EventArgs.Empty);
                // }
                break;

            case State.CountdownToStart:

                countDownToStartTimer -= Time.deltaTime;
                if (countDownToStartTimer < 0f)
                {
                    state = State.GamePlaying;
                    gamePlayingTimer = gamePlayingTimerMax;

                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;

            case State.GamePlaying:
                gamePlayingTimer -= Time.deltaTime;
                if (gamePlayingTimer < 0f)
                {
                    state = State.GameOver;

                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;

            case State.GameOver:

                break;
        }
    }

    public bool IsGamePlaying()
    {
        return state == State.GamePlaying;
    }

    public bool IsCountDownToStartActive()
    {
        return state == State.CountdownToStart;
    }

    public float GetCountDownToStartTime()
    {
        return countDownToStartTimer;
    }

    public bool IsGameOver()
    {
        return state == State.GameOver;
    }

    public float GetPlayingTimeNormalized()
    {
        return 1 - (gamePlayingTimer / gamePlayingTimerMax);
    }
}
