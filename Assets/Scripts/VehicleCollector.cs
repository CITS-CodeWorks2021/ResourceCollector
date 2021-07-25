using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleCollector : MonoBehaviour
{
    public Spawner spawner;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Vehicle")
        {
            Vehicle exiting = collision.gameObject.GetComponent<Vehicle>();
            for (int i = 0; i < exiting.collected.Count; i++)
            {
                Inventory.OnCollect.Invoke(exiting.collected[i]);
            }
            spawner.DespawnVehicle(exiting);
        }
    }

}
