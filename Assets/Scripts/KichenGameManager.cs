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

    public event EventHandler OnStateChanged;

    public static KichenGameManager Instance { get; private set; }

    private State state;
    private float waitingToStartTimer = 1f;
    private float countDownToStartTimer = 3f;
    private float gamePlayingToStartTimer = 10f;

    private void Awake()
    {
        state = State.WaitingToStart;

        Instance = this;
    }

    private void Update()
    {
        switch (state)
        {
            case State.WaitingToStart:
                waitingToStartTimer -= Time.deltaTime;
                if (waitingToStartTimer < 0f)
                {
                    state = State.CountdownToStart;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;

            case State.CountdownToStart:

                countDownToStartTimer -= Time.deltaTime;
                if (countDownToStartTimer < 0f)
                {
                    state = State.GamePlaying;

                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;

            case State.GamePlaying:
                gamePlayingToStartTimer -= Time.deltaTime;
                if (gamePlayingToStartTimer < 0f)
                {
                    state = State.GameOver;

                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;

            case State.GameOver:

                break;
        }
        Debug.Log(state);
    }

    public bool IsGamePlaying()
    {
        return state == State.GamePlaying;
    }

    public bool IsCountDownToStartActive()
    {
        return state == State.CountdownToStart;
    }

    public float GetCountDownToStartTime() {
        return countDownToStartTimer;
    }
}
