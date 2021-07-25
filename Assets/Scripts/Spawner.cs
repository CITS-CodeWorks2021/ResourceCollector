using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] vehiclesToSpawn;
    public int numToSpawn;
    public Transform[] spawnPoints;
    public float minSpawnTime, maxSpawnTime;
    public int maxSpawnPerCycle;
    float curTimer, ranSpawnTime;

    List<Vehicle> inActive = new List<Vehicle>();
    private static List<Vehicle> active = new List<Vehicle>();
    bool[] safeToSpawn = { false, false };
    int totalSpawned = 0;

    public static int numActiveVehicle
    {
        get { return active.Count; }
    }


    // Start is called before the first frame update
    void Start()
    {
        // Let's spawn those inactives!
        for(int i = 0; i < vehiclesToSpawn.Length; i++)
        {
            for(int j = 0; j < numToSpawn; j++)
            {
                GameObject newVehicle = Instantiate(vehiclesToSpawn[i], Vector3.zero, Quaternion.identity);
                newVehicle.SetActive(false);
                inActive.Add(newVehicle.GetComponent<Vehicle>());
            }
        }

        ranSpawnTime = Random.Range(minSpawnTime, maxSpawnTime);
        CycleControl.OnStartCycle.AddListener(HandleOnStartCycle);
    }

    // Update is called once per frame
    void Update()
    {
        if(GameLogic.State == GameState.InCycle)
        {
            if (totalSpawned >= maxSpawnPerCycle || !CycleControl.ActiveCycle) return;
            //Pause this if there is nowhere to spawn
            if(safeToSpawn[0] || safeToSpawn[1])
            {
                curTimer += Time.deltaTime;
                if(curTimer >= ranSpawnTime)
                {
                    Vehicle nextSpawn = GetRandomInactive();
                    Vector3 posToSpawn;
                    if (safeToSpawn[0])
                    {
                        posToSpawn = spawnPoints[0].position;
                        safeToSpawn[0] = false;
                    } else
                    {
                        posToSpawn = spawnPoints[1].position;
                        safeToSpawn[1] = false;
                    }
                    nextSpawn.transform.position = posToSpawn;
                    nextSpawn.gameObject.SetActive(true);
                    nextSpawn.Init();
                    totalSpawned++;
                    active.Add(nextSpawn);
                    curTimer = 0;
                    ranSpawnTime = Random.Range(minSpawnTime, maxSpawnTime);
                }
            }
        }

    }

    // the which indicates which SpawnPoint is currently safe to spawn something
    public void SafeToSpawn(int which)
    {
        safeToSpawn[which] = true;
    }

    Vehicle GetRandomInactive()
    {
        int ranIndex = Random.Range(0, inActive.Count);
        Vehicle toReturn;
        if (inActive.Count > 0)
        {
            toReturn = inActive[ranIndex];
            inActive.RemoveAt(ranIndex);
        }
        else
        {
            // A safety catch in case we ran out of inactives.
            toReturn = SpawnRandomVehicle();
        }
        
        return toReturn;
    }

    Vehicle SpawnRandomVehicle()
    {
        GameObject newVehic =
            Instantiate(vehiclesToSpawn[Random.Range(0, vehiclesToSpawn.Length)],
            Vector3.zero,
            Quaternion.identity);
        newVehic.SetActive(false);
        return newVehic.GetComponent<Vehicle>();
    }

    public void DespawnVehicle(Vehicle which)
    {
        int whichIndex = -1;
        for(int i = 0; i < active.Count; i++)
        {
            if (active[i] == which) whichIndex = i;
        }

        // Did that search succeed?
        if (whichIndex < 0) return;

        active[whichIndex].DeInit();
        active[whichIndex].gameObject.SetActive(false);
        inActive.Add(active[whichIndex]);
        active.RemoveAt(whichIndex);
    }

    void HandleOnStartCycle()
    {
        ranSpawnTime = Random.Range(minSpawnTime, maxSpawnTime);
        safeToSpawn[0] = true;
        safeToSpawn[1] = true;
        totalSpawned = 0;
    }
}
