using UnityEngine;
using UnityEngine.SceneManagement;

public class MazeExitScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (GameContext.instance.mazeOptions.level == 0)
        {
            GameContext.LoadScene(GameContext.Scenes.MENU);
        }
        else
        {
            GameContext.OnLevelComplete();
        }
    }
}
