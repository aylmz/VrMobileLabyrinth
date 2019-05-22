using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCellData : MonoBehaviour
{
    public const int Top = 0;
    public const int Right = 1;
    public const int Bottom = 2;
    public const int Left = 3;

    public int x;
    public int y;
    public bool isVisited;
    public GameObject floor;
    public GameObject[] fires;
    public GameObject[] walls;

    internal void OnCollisionEnterChild(Collision collision)
    {
        foreach (GameObject fire in fires)
        {
            fire.SetActive(true);
        }
        
    }

    internal void OnCollisionExitChild(Collision collision)
    {
        foreach (GameObject fire in fires)
        {
            fire.SetActive(false);
        }
    }

    internal void OnCollisionStayChild(Collision collision)
    {
        foreach (GameObject fire in fires)
        {
            fire.SetActive(true);
        }
    }
}
