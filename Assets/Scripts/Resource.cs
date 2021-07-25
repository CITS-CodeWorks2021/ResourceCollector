using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Resource
{
    public ResourceType resourceName;
    public int amount;
    public int cost;
}

public enum ResourceType
{
    Fuel,
    IceCream,
    Souvenirs,
    Electronics,
    Soda,
    Snacks
}