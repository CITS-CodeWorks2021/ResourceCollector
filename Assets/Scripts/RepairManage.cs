using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairManage : MonoBehaviour
{
    [Range(0, 1)] public float chanceToRepair = 0.25f;
    public int costToAttemptRepair;

    public bool AttemptRepair()
    {

        return true;
    }

}
