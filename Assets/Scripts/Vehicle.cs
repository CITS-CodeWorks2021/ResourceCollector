using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Vehicle : MonoBehaviour, IBreak
{
    #region public variables
    [Header("A Resource Hogging Vehicle")]
    public float speed;
    [Range(0,1)]public float chanceToBreak;
    [Range(1,10)] public float breakChanceTimer;
    [Range(0, 1)] public float chanceToRepair = 0.25f;
    public int repairAttemptCost;
    public Color workingColor, brokeColor;
    public SpriteRenderer vehicleArt;
    [Range(1,100)] public int fuelCapacity = 10;
    [Range(1, 5)] public int maxOtherToCollect = -1;
    public List<Resource> collected = new List<Resource>();
    
    [Header("To Check if a Vehicle is in front")]
    public float safetyCheckDistance;
    public LayerMask safetyCheckLayers;
    #endregion

    #region private variables
    bool isClearToMove = true, isBroken = false, isCollecting, isSafe;
    float curBreakCheckTime, targetLoc;
    int currentFuel;
    Depot currentDepot;
    #endregion

    public void Init()
    {
        isSafe = true;
        collected.Clear();
        isClearToMove = true;
        targetLoc = 99999;
        maxOtherToCollect = -1;
        curBreakCheckTime = 0;
        OnFix();
    }

    public void DeInit()
    {
        // ??? Do I actually need this ???
        // For consistancy I suppose it can stay
    }

    // Update is called once per frame
    void Update()
    {
        if (GameLogic.State == GameState.InCycle)
        {
            CheckIfClear();
            if (transform.position.x >= targetLoc && !isCollecting)
                StartCollecting();

            if(isClearToMove && !isBroken && !isCollecting)
                transform.Translate(speed * Time.deltaTime, 0, 0);

            // Check if I should Break
            if(!isBroken && !isCollecting && !isSafe)
            {
                curBreakCheckTime += Time.deltaTime;
                if(curBreakCheckTime >= breakChanceTimer)
                {
                    curBreakCheckTime = 0;
                    float ranNum = Random.value;
                    if (ranNum <= chanceToBreak) OnBreak();
                }
            }
        }
    }

    private void OnMouseDown()
    {
        if(GameLogic.State == GameState.InCycle && isBroken)
        {
            Inventory.OnAttemptRepair.Invoke(repairAttemptCost);
            float ranNum = Random.value;
            if (ranNum <= chanceToRepair) OnFix();
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Depot")
        {
            Depot possibleDepot = collision.gameObject.GetComponent<Depot>();
            float ranNum = Random.value;
            if(ranNum <= possibleDepot.chanceToStop)
            {
                currentDepot = possibleDepot;
                targetLoc = currentDepot.stopLoc.position.x;
            }
        }
        if(collision.gameObject.tag == "Safe")
        {
            isSafe = true;
        }
        if (collision.gameObject.tag == "UnSafe")
        {
            isSafe = false;
        }
    }

    void StartCollecting()
    {
        isCollecting = true;
        currentDepot.StartGiving(this);
    }

    public bool CollectResource(Resource which)
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
            newAdd.amount = 1;
            newAdd.cost = which.cost;
            collected.Add(newAdd);
        }
        else
        {
            for(int i = 0; i < collected.Count; i++)
            {
                if(collected[i].resourceName == which.resourceName)
                {
                    collected[i].amount++;
                }
            }
        }

        if (maxOtherToCollect < 0 && which.resourceName != ResourceType.Fuel)
            maxOtherToCollect = Random.Range(1, maxOtherToCollect);

        if (which.resourceName == ResourceType.Fuel)
        {
            return CheckCanCollect(which, fuelCapacity);
        }
        return CheckCanCollect(which, maxOtherToCollect);
    }

    bool CheckCanCollect(Resource which, int howMuch)
    {
        bool canGetMore = false;
        for(int i = 0; i < collected.Count; i++)
        {
            if(collected[i].resourceName == which.resourceName)
            {
                if (collected[i].amount < howMuch) canGetMore = true;
            }
        }
        return canGetMore;
    }

    public void FinishCollecting()
    {
        targetLoc = 999999;
        maxOtherToCollect = -1;
        isCollecting = false;
    }

    void CheckIfClear()
    {
        // Put in Proper Method
        isClearToMove = true;

        // Be Sure to turn off "Queries Start In Colliders" in
        // Project Settings -> Physics 2D
        RaycastHit2D ray =
            Physics2D.Raycast(transform.position, Vector2.right, safetyCheckDistance, safetyCheckLayers);
        if(ray)
        {
            isClearToMove = false;
        }
    }

    public void OnBreak()
    {
        isBroken = true;
        vehicleArt.color = brokeColor;
    }

    // Need a proper mechanic to do this. . . like % chance per click
    // %chance to fix would be an upgrade option
    // So this would be called elsewhere and assigned here?
    public void OnFix()
    {
        isBroken = false;
        vehicleArt.color = workingColor;
    }

}
