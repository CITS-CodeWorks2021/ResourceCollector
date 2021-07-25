using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPanel : MonoBehaviour
{
    public GameState myState;

    public void Activation(bool beOn)
    {
        gameObject.SetActive(beOn);
    }
}
