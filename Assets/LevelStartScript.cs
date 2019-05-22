using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelStartScript : MonoBehaviour
{
    public int level;

    private void OnTriggerEnter(Collider other)
    {
        GameContext.instance.mazeOptions.level = level;
        GameContext.LoadScene(GameContext.Scenes.MAZE);
    }
}
