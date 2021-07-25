using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerExit : MonoBehaviour
{
    public int myIndex;
    public Spawner mySpawner;

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Vehicle")
        {
            mySpawner.SafeToSpawn(myIndex);
        }
    }
}
