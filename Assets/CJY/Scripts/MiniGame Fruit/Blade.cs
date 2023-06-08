using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blade : MonoBehaviour
{
    private Collider bladeCollider;
    private bool slicing;

    // Start is called before the first frame update
    void Start()
    {
        bladeCollider = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            StartSlicing();
        }
        else if(Input.GetMouseButtonUp(0))
        {
            StopSlicing();
        }
        else if(slicing)
        {
            ContinueSlicing();
        }
    }

    private void StartSlicing()
    {
        slicing = true;
        bladeCollider.enabled = true;
    }

    private void StopSlicing()
    {
        slicing = false;
        bladeCollider.enabled = false;
    }

    private void ContinueSlicing()
    {

    }
}
