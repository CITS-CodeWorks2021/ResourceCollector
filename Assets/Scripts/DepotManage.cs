using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DepotManage : MonoBehaviour
{
    public List<Depot> topDepots = new List<Depot>();
    public List<Depot> botDepots = new List<Depot>();

    public List<EmptyLot> emptyLots = new List<EmptyLot>();
    List<Depot> spawnedDepots = new List<Depot>();

    public Inventory myInventory;
    public GameObject depotMenu;
    EmptyLot currentLot;

    private void Start()
    {
        depotMenu.SetActive(false);
        foreach(EmptyLot lots in emptyLots)
        {
            lots.OnLotSelect.AddListener(HandleLotSelect);
        }
        GameLogic.OnStart.AddListener(HandleOnStart);
        DepotPicker.OnPick.AddListener(HandleDepotPicked);
    }

    void HandleDepotPicked(ResourceType which)
    {
        Debug.Log("1: " + which.ToString());
        if (currentLot == null) return;
        Debug.Log("2");
        Depot newDepot;
        List<Depot> depList = currentLot.isTop ? topDepots : botDepots;
        Debug.Log("3");
        for (int i = 0; i < depList.Count; i++)
        {
            Debug.Log("4");
            if (depList[i].myResource.resourceName == which)
            {
                Debug.Log("5");
                if (depList[i].buildCost <= myInventory.money)
                {
                    Debug.Log("Building Something!");
                    GameObject obj = Instantiate(depList[i].gameObject);
                    obj.transform.position = currentLot.transform.position;
                    newDepot = obj.GetComponent<Depot>();
                    spawnedDepots.Add(newDepot);
                    myInventory.money -= depList[i].buildCost;

                    currentLot.SelectMe(false);
                    currentLot.gameObject.SetActive(false);
                    currentLot = null;
                }
                break;
            }
        }
    }

    void HandleLotSelect(EmptyLot lot)
    {
        currentLot = lot;
        if(lot == null)
        {
            depotMenu.SetActive(false);
        }
        else
        {
            depotMenu.SetActive(true);
        }
    }

    private void HandleOnStart()
    {
        foreach(Depot deps in spawnedDepots)
        {
            Destroy(deps.gameObject);
        }
        spawnedDepots.Clear();

        foreach(EmptyLot empties in emptyLots)
        {
            empties.gameObject.SetActive(true);
        }
    }
}
