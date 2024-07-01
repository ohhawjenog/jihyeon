using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Box : MonoBehaviour
{
    public bool isSensorCollider = false;
    public bool isLoaderCollider = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Sensor"))
        {
            print(other.gameObject);
            isSensorCollider = true;
        }

        if (other.gameObject.name == "LoadingDetector")
        {
            isLoaderCollider = true;
            this.transform.parent = other.gameObject.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Sensor"))
        {
            print(other.gameObject);
            isSensorCollider = false;
        }

        if (other.gameObject.name == "LoadingDetector")
        {
            isLoaderCollider = false;
        }
    }
}
