using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameLogic : MonoBehaviour
{
    static GameState _state;
    public static GameState State {
        get
        {
            return _state;
        }

        private set
        {
            _state = value;
            UIManager.OnChangePanel.Invoke();
        }
    }


    public static UnityEvent OnStart = new UnityEvent(), OnEnd = new UnityEvent();

    bool isPaused;

    // Start is called before the first frame update
    void Start()
    {
        State = GameState.Root;
        
        OnEnd.AddListener(EndGame);
        CycleControl.OnEndCycle.AddListener(EndCycle);
        CycleControl.OnStartCycle.AddListener(StartCycle);
    }

    public void StartGame()
    {
        OnStart.Invoke();
        StartCycle();
    }

    public void StartCycle()
    {
        isPaused = false;
        State = GameState.InCycle;
    }
    public void EndCycle()
    {
        State = GameState.InReview;
    }

    public void StartSetup()
    {
        State = GameState.InSetup;
    }

    public void EndGame()
    {
        State = GameState.GameOver;

    }

    public void PauseGame()
    {
        isPaused = !isPaused;
        State = isPaused ? GameState.Paused : GameState.InCycle;
    }

}

public enum GameState
{
    Root,
    InCycle,
    InReview,
    InSetup,
    Paused,
    GameOver
}