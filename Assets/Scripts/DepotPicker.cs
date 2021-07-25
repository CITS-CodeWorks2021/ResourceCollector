using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DepotPicker : MonoBehaviour
{
    public static UnityEvent<ResourceType> OnPick = new UnityEvent<ResourceType>();
    public ResourceType myType;

    public void PickedMe()
    {
        if(GameLogic.State == GameState.InSetup)
        {
            Debug.Log("I Picked Something!");
            OnPick.Invoke(myType);
        }
    }
}