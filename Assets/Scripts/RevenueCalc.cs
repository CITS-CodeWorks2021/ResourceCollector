using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RevenueCalc : MonoBehaviour
{
    public Text grossRevText, upkeepText, netRevText, repairText, finalCoH, startCoH;
    public Text finalResult;
    Inventory myInventory;

    int startCash, grossRev, netRev, totUpkeep, totRepair;

    // Start is called before the first frame update
    void Start()
    {
        myInventory = GetComponentInChildren<Inventory>();
        CycleControl.OnEndCycle.AddListener(HandleOnEndCycle);
        GameLogic.OnEnd.AddListener(HandleOnEnd);
    }

    private void HandleOnEnd()
    {
        CalcResults();
        DisplayFinal();
    }

    private void HandleOnEndCycle()
    {
        CalcResults();
        DisplayReview();
        
    }

    void CalcResults()
    {
        startCash = myInventory.money;
        
        grossRev = 0;
        for (int i = 0; i < myInventory.collected.Count; i++)
        {
            grossRev += myInventory.collected[i].cost * myInventory.collected[i].amount;
        }
        
        totUpkeep = 0;
        for (int i = 0; i < myInventory.depots.Count; i++)
        {
            totUpkeep += myInventory.depots[i].upkeepCost;
        }

        // repair calc to come with repair system
        totRepair = myInventory.repairCost;

        netRev = grossRev - totUpkeep - totRepair;
        

        myInventory.money = startCash + netRev;
        
    }

    void DisplayReview()
    {
        Debug.Log("Now we Display");
        startCoH.text = "Starting Cash: $" + startCash.ToString();
        
        grossRevText.text = "Gross Revenue: $" + grossRev.ToString();
        
        upkeepText.text = "Cycle's Upkeep Cost: $" + totUpkeep.ToString();
        
        repairText.text = "Repair Costs: $" + totRepair.ToString();

        netRevText.text = "Net Revenue: $" + netRev.ToString();

        finalCoH.text = "Final Cash On Hand: $" + myInventory.money.ToString();
    }

    void DisplayFinal()
    {
        Debug.Log("Now we Display Finals");
        finalResult.text = "$" + myInventory.money.ToString();
    }
}
