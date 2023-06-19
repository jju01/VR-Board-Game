using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public static StageManager Instance;
    
    public List<IceCube> iceCubes;
    
    [SerializeField] private IceCube startCube;
    [SerializeField] private IceCube islandCube;

    public int startCubeIdx;
    public int islandCubeIdx;
    
    void Awake()
    {
        Instance = this;
        
        iceCubes = GetComponentsInChildren<IceCube>().ToList();
        
        startCubeIdx = iceCubes.IndexOf(startCube);
        islandCubeIdx = iceCubes.IndexOf(islandCube);
    }
}
