using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UIManager : MonoBehaviour
{
    public UIPanel[] panels;
    public static UnityEvent OnChangePanel = new UnityEvent();

    private void Awake()
    {
        OnChangePanel.AddListener(ChangePanel);
    }

    public void ChangePanel()
    {
        foreach (UIPanel ui in panels)
        {
            if (ui.myState == GameLogic.State) ui.Activation(true);
            else ui.Activation(false);
        }
    }
}
