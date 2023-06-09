using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;



public class IceCube : MonoBehaviour
{
    public UIManager ui;
    public List<IceCube> nextIceCube;

    public Trigger trigger;
    
    public static IceCube Instance;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }



    public void GetNext(Action<IceCube> getNextCallback)
    {
        if (nextIceCube.Count > 1)
        {
            ui.OpenSelectPopup((selectedIdx) =>
            {
                getNextCallback(nextIceCube[selectedIdx]);
            });
        }
        else

        {
            getNextCallback(nextIceCube[0]);
        }
    }
}
