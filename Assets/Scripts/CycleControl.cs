using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CycleControl : MonoBehaviour
{
    public int numCycles;
    [Tooltip("Time In Minutes")]
    public float cycleLength;

    float curTimer;
    int curCycle;

    static bool _activeCycling;
    public static bool ActiveCycle
    {
        get { return _activeCycling; }
    }

    public static UnityEvent OnStartCycle = new UnityEvent();
    public static UnityEvent OnEndCycle = new UnityEvent();

    // Start is called before the first frame update
    void Start()
    {
        GameLogic.OnStart.AddListener(StartGame);
        
    }

    void StartGame()
    {
        curCycle = 0;
        StartCycle();
    }

    // Update is called once per frame
    void Update()
    {
        if(GameLogic.State == GameState.InCycle)
        {
            curTimer -= Time.deltaTime;
            if(curTimer <= 0)
            {
                // We are done. . . but we might want to hang out for a bit
                _activeCycling = false;
                // Safety Catch to prevent ending before vehicles leave
                if (Spawner.numActiveVehicle > 0) return;
                
                if (curCycle >= numCycles) GameLogic.OnEnd.Invoke();
                else
                {
                    OnEndCycle.Invoke();
                }
            }
        }
    }

    public void StartCycle()
    {
        Debug.Log("Start Cycle!");
        // That time is in Minutes!
        curTimer = cycleLength * 60;
        _activeCycling = true;
        // Increment the # of cycles, for limited cycle style game
        curCycle++;
        OnStartCycle.Invoke();
    }
}
