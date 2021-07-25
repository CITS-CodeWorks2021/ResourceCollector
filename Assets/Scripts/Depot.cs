using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Depot : MonoBehaviour
{
    public Transform stopLoc;
    [Range(0,1)] public float chanceToStop;
    [Range(1, 5)] public float timePerUnit;
    public Resource myResource;
    public int upkeepCost;
    public int buildCost;
    float curUnitTimer;
    Vehicle currentVehicle;
    bool isGiving;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(GameLogic.State == GameState.InCycle && isGiving)
        {
            if (curUnitTimer <= 0)
            {
                bool shouldContinue = currentVehicle.CollectResource(myResource);
                curUnitTimer = timePerUnit;
                if (!shouldContinue)
                {
                    isGiving = false;
                    currentVehicle.FinishCollecting();
                }
            }
            else
            {
                
                curUnitTimer -= Time.deltaTime;
            }
        }
    }

    public void StartGiving(Vehicle toWhom)
    {
        curUnitTimer = timePerUnit;
        currentVehicle = toWhom;
        isGiving = true;
    }
}
