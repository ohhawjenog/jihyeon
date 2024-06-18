using MPS;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Lamp : MonoBehaviour
{
    public Transform lampName;

    public int plcInputValue; 

    private void Update()
    {
        if (plcInputValue > 0)
        {
            lampName.GetComponent<MeshRenderer>().material.color = Color.red;
        }
        else
            lampName.GetComponent<MeshRenderer>().material.color = Color.white;
    }
}
     