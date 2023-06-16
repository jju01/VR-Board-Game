using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public static StageManager Instance;
    
    public IceCube[] iceCubes;
    
    void Awake()
    {
        Instance = this;
        iceCubes = GetComponentsInChildren<IceCube>();
    }
}
