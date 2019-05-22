using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelsCorridor : MonoBehaviour
{
    public GameObject[] corridorCellsInOrder;

    private GameContext gameContext;

    // Start is called before the first frame update
    void Start()
    {
        gameContext = GameContext.instance;

        int lastWalkableCell = Math.Min((gameContext.preferences.saveData.lastUnlockedLevel - 1) / 2,// Intentional integer division
                                        corridorCellsInOrder.Length - 1);

        for (int i = 0; i <= lastWalkableCell; i++)
        {
            foreach(GameObject wall in corridorCellsInOrder[i].GetComponent<GridCellData>().walls)
            {
                wall.SetActive(false);
            }
            
        }
    }

}
