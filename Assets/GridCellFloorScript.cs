using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCellFloorScript : MonoBehaviour
{
    private GridCellData gridCellData;

    private void Awake()
    {
        gridCellData = GetComponentInParent<GridCellData>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        gridCellData.OnCollisionEnterChild(collision);
    }

    private void OnCollisionExit(Collision collision)
    {
        gridCellData.OnCollisionExitChild(collision);
    }

    private void OnCollisionStay(Collision collision)
    {
        gridCellData.OnCollisionStayChild(collision);
    }
}
