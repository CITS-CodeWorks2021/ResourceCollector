using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EmptyLot : MonoBehaviour
{
    public Color selected = Color.cyan, deSelected = Color.gray;

    public UnityEvent<EmptyLot> OnLotSelect = new UnityEvent<EmptyLot>();

    private static EmptyLot curSelected;
    public bool isTop;
    private void Start()
    {
        SelectMe(false);
        CycleControl.OnStartCycle.AddListener(HandleStartCycle);
    }

    void HandleStartCycle()
    {
        if (curSelected != null) curSelected.SelectMe(false);
        curSelected = null;
    }

    private void OnMouseDown()
    {
        if (GameLogic.State != GameState.InSetup) return;

        if(curSelected == null)
        {
            SelectMe(true);
            curSelected = this;
        }
        else if (this == curSelected)
        {
            SelectMe(false);
            curSelected = null;
        }
        else
        {
            curSelected.SelectMe(false);
            SelectMe(true);
            curSelected = this;
        }

        OnLotSelect.Invoke(curSelected);
    }


    public void SelectMe(bool isMe)
    {
        GetComponent<SpriteRenderer>().color = isMe ? selected : deSelected;
    }
}
