using UnityEngine;
using UnityEngine.SceneManagement;

public class GameContext : MonoBehaviour
{
    public static GameContext instance;
    public Preferences preferences;
    public Upgrades upgrades;
    public MazeOptions mazeOptions;
    public GameObject player;
    public GameObject playerBody;
    public GameObject exitPoint;

    public enum Scenes
    {
        START = 0, MENU, MAZE
    }

    public static string[] sceneNames = { "StartScene", "MenuScene", "MazeScene" };
    public static string[] sceneSpawnPointTags = {"StartSpawnPoint","MenuSpawnPoint","MazeSpawnPoint" };

    void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            DontDestroyOnLoad(player);
            DontDestroyOnLoad(playerBody);
            DontDestroyOnLoad(exitPoint);
            instance = this;
            preferences = new Preferences();
            upgrades = new Upgrades();
            mazeOptions = new MazeOptions();
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public static void LoadScene(Scenes sceneToLoad)
    {
        SceneManager.LoadSceneAsync(sceneNames[(int)sceneToLoad], LoadSceneMode.Single);
 
        if(sceneToLoad == Scenes.MENU)
        {
            //Move the exit point out of vision
            instance.exitPoint.transform.position = new Vector3(0, 0, -100);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)     //Called when new scene is loaded
    {
        int sceneIndex = 0 ;
        for(int i = 0; i < sceneNames.Length; i++)
        {
            if(scene.name == sceneNames[i])
            {
                sceneIndex = i;
                break;
            }
        }
        GameObject startPoint = GameObject.FindGameObjectWithTag(GameContext.sceneSpawnPointTags[sceneIndex]);
        player.transform.position = new Vector3(startPoint.transform.position.x, player.transform.position.y, startPoint.transform.position.z);
        playerBody.transform.position = new Vector3(startPoint.transform.position.x, playerBody.transform.position.y, startPoint.transform.position.z);
    }

    public static void OnLevelComplete()
    {
        int activeLevel = instance.mazeOptions.level;
        if(activeLevel <= 0)
        {
            return;
        }
        SaveData saveData = instance.preferences.saveData;
        saveData.unspentUpgradePoints += activeLevel;
        if(saveData.lastUnlockedLevel <= activeLevel && activeLevel < MazeOptions.levelSizes.Length)
        {
            saveData.lastUnlockedLevel = activeLevel + 1;
        }
        instance.preferences.SaveProgress();

        instance.mazeOptions.level = 0;
        LoadScene(Scenes.MENU);
    }
}
