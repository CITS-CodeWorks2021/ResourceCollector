using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Inventory : MonoBehaviour
{
    public int startingMoney;
    public int money;
    public int repairCost;

    public List<Resource> collected = new List<Resource>();
    public List<Depot> depots = new List<Depot>();

    public static UnityEvent<Resource> OnCollect = new UnityEvent<Resource>();
    public static UnityEvent<int> OnAttemptRepair = new UnityEvent<int>();
    // Start is called before the first frame update
    void Start()
    {
        GameLogic.OnStart.AddListener(HandleOnStart);
        CycleControl.OnStartCycle.AddListener(HandleOnCycleStart);
        OnCollect.AddListener(HandleOnCollect);
        OnAttemptRepair.AddListener(HandleRepair);
    }

    void HandleRepair(int costAmount)
    {
        repairCost += costAmount;
    }

    private void HandleOnCycleStart()
    {
        collected = new List<Resource>();
        repairCost = 0;
    }

    private void HandleOnStart()
    {
        money = startingMoney;
        depots.Capacity = 2;
    }

    private void HandleOnCollect(Resource which)
    {
        bool hasResource = false;

        foreach (Resource r in collected)
        {
            if (r.resourceName == which.resourceName) hasResource = true;
        }
        if (!hasResource)
        {
            Resource newAdd = new Resource();
            newAdd.resourceName = which.resourceName;
            newAdd.amount = which.amount;
            newAdd.cost = which.cost;
            collected.Add(newAdd);
        }
        else
        {
            for (int i = 0; i < collected.Count; i++)
            {
                if (collected[i].resourceName == which.resourceName)
                {
                    collected[i].amount += which.amount;
                }
            }
        }
    }
}
