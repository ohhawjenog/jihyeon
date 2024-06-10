using System.Collections;
using System.Collections.Generic;
using UnityEditor.MemoryProfiler;
using UnityEngine;
using ActUtlType64Lib; // MX Component v5 Library »ç¿ë

public class PalletCylinder : MonoBehaviour
{
    public Transform PalletMoveCylinder;
    public Transform PMC_start;
    public Transform PMC_end;
    public Transform PalletFixCylinder;
    public Transform PFC_start;
    public Transform PFC_end;

    public int Y30 = 0;
    public int Y31 = 0;
    public int Y32 = 0;
    public int Y33 = 0;
    public bool isCylinderMoving = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    int GetDevice(string device)
    {
        if (connection == Connection.Connected)
        {
            int data = 0;
            int returnValue = mxComponent.GetDevice(device, out data);

            if (returnValue != 0)
                print(returnValue.ToString("X"));

            return data;
        }
        else
            return 0;


        IEnumerator MoveCylinder(Transform cylinder, Vector3 startPosition, Vector3 endPosition, float duration)
    {
        isCylinderMoving = true;
        float currentTime = 0;

        while (true)
        {
            currentTime += Time.deltaTime;

            if (currentTime >= duration)
            {
                currentTime = 0;
                break;
            }

            cylinder.position = Vector3.Lerp(startPosition, endPosition, currentTime / duration);

            yield return new WaitForSeconds(Time.deltaTime);
        }

        isCylinderMoving = false;
    }

    IEnumerator CoRunMPS()
    {
        while (true)
        {
            if (Y30 == 1)
            {
                yield return MoveCylinder(PalletMoveCylinder, PMC_start.position, PMC_end.position, 1);
            }
            if (Y31 == 1)
            {
                yield return MoveCylinder(PalletMoveCylinder, PMC_end.position, PMC_start.position, 1);
            }
            if (Y32 == 1)
            {
                yield return MoveCylinder(PalletFixCylinder, PMC_start.position, PMC_end.position, 1);
            }
            if (Y33 == 1)
            {
                yield return MoveCylinder(PalletFixCylinder, PMC_end.position, PMC_start.position, 1);
            }
        }
    }
}
