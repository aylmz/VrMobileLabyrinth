using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{   
    private GameContext gameContext;
    public GameObject gridCellPrefab;
    private GameObject exitPoint;
    private GameObject startPoint;
    private float cellWidth;
    
    private System.Random rand = new System.Random();
    
    private GridCellData exitCell;
    private GridCellData[,] mazeData;

    // Start is called before the first frame update
    void Awake()
    {
        exitPoint = GameObject.FindGameObjectWithTag("ExitPoint");
        startPoint = GameObject.FindGameObjectWithTag(GameContext.sceneSpawnPointTags[(int)GameContext.Scenes.MAZE]);
        gameContext = GameContext.instance;
        // calculate cell width
        GameObject floor = gridCellPrefab.GetComponent<GridCellData>().floor;
        cellWidth = floor.transform.localScale.x * floor.GetComponent<MeshFilter>().sharedMesh.bounds.size.x;        
    }

    void Start()
    {
        StartCoroutine(GenerateGrid(gameContext.mazeOptions));
    }

    private IEnumerator GenerateMaze(GridCellData[,] mazeData, MazeOptions mazeOptions, GridCellData startingCell)
    {
        int maxX = mazeData.GetLength(0) - 1;
        int maxY = mazeData.GetLength(1) - 1;
        int startX = startingCell.x;
        int startY = startingCell.y;
        List<GridCellData> deadEnds = new List<GridCellData>();
        // Stack for recursive generation algorithm
        Stack<GridCellData> stack = new Stack<GridCellData>();

        GridCellData currentCell = startingCell;

        bool nextCellFound = false;
        do
        {
            currentCell.isVisited = true;// Marking current cell as visited
            GridCellData nextCell = GetRandomNeighbour(currentCell, mazeData);// Get random neighbor cell as a next one
            if (nextCell != null)// If there is available next cell, push current cell to stack and assigning next to current
            {
                // If there is at least one available neighbor, remove walls between current and next cell
                RemoveWalls(currentCell, nextCell);
                stack.Push(currentCell);
                currentCell = nextCell;
                nextCellFound = true;
            }
            else// Else backtrack to cell that has at least one available neighbor
            {
                if (nextCellFound // If next cell was found in previous iteration but not in this one, this is a dead end
                    && (currentCell.x != startX || currentCell.y != startY) // Dont make the starting point also the exit point
                    )
                {
                    deadEnds.Add(currentCell);
                }
                if (stack.Count > 0)
                {
                    currentCell = stack.Pop();
                }
                nextCellFound = false;
            }
            yield return null;
        } while (stack.Count != 0);
        // Relocate exit point
        exitCell = deadEnds.Count > 0 ? deadEnds[rand.Next(deadEnds.Count)] : mazeData[maxX, maxY];
        exitPoint.transform.position = new Vector3(exitCell.floor.transform.position.x, exitCell.floor.transform.position.y, exitCell.floor.transform.position.z);
    }

    private void RemoveWalls(GridCellData currentCell, GridCellData nextCell)
    {
        if(currentCell.x < nextCell.x)//goes right
        {
            currentCell.walls[GridCellData.Right].SetActive(false);
            nextCell.walls[GridCellData.Left].SetActive(false);
        }
        else if (currentCell.x > nextCell.x)//goes left
        {
            currentCell.walls[GridCellData.Left].SetActive(false);
            nextCell.walls[GridCellData.Right].SetActive(false);
        }
        else if (currentCell.y > nextCell.y)//goes down
        {
            currentCell.walls[GridCellData.Bottom].SetActive(false);
            nextCell.walls[GridCellData.Top].SetActive(false);
        }
        else if (currentCell.y < nextCell.y)//goes up
        {
            currentCell.walls[GridCellData.Top].SetActive(false);
            nextCell.walls[GridCellData.Bottom].SetActive(false);
        }
    }

    private GridCellData GetRandomNeighbour(GridCellData cell, GridCellData[,] mazeData)
    {
        List<GridCellData> neighbours = new List<GridCellData>();
        int maxX = mazeData.GetLength(0) - 1;
        int maxY = mazeData.GetLength(1) - 1;

        // Add left neighbour
        if(cell.x > 0 && !mazeData[cell.x - 1, cell.y].isVisited)
        {
            neighbours.Add(mazeData[cell.x - 1, cell.y]);
        }
        //Add right neighbour
        if (cell.x < maxX && !mazeData[cell.x + 1, cell.y].isVisited)
        {
            neighbours.Add(mazeData[cell.x + 1, cell.y]);
        }
        //Add lower neighbour
        if (cell.y > 0 && !mazeData[cell.x, cell.y - 1].isVisited)
        {
            neighbours.Add(mazeData[cell.x, cell.y - 1]);
        }
        // Add upper neighbour
        if (cell.y < maxY && !mazeData[cell.x, cell.y + 1].isVisited)
        {
            neighbours.Add(mazeData[cell.x, cell.y + 1]);
        }

        // Returning random neigbor from a list
        if (neighbours.Count > 0)
        {
            return neighbours[rand.Next(neighbours.Count)];
        }
        // Else return no neighbor
        return null;
    }

    /**
     * Generate grid centered on (0,0) with all walls intact
     */
    private IEnumerator GenerateGrid(MazeOptions mazeOptions)
    {
        mazeData = new GridCellData[mazeOptions.Width, mazeOptions.Height];
        int centerX = (mazeOptions.Width - 1) / 2;
        int centerY = (mazeOptions.Height - 1) / 2;
        //center X and center Y will be located at 0, 0
        for (int y = 0; y < mazeOptions.Height; y++)
        {
            for (int x = 0; x < mazeOptions.Width; x++)
            {
                mazeData[x, y] = CreateCell(x, y, centerX, centerY);
                if(x == 0 && y == 0)
                {
                    startPoint.transform.position = mazeData[0, 0].transform.position;
                    gameContext.player.transform.position = new Vector3(
                                                                    startPoint.transform.position.x,
                                                                    gameContext.player.transform.position.y,
                                                                    startPoint.transform.position.z);
                    gameContext.playerBody.transform.position = new Vector3(
                                                                    startPoint.transform.position.x,
                                                                    gameContext.playerBody.transform.position.y,
                                                                    startPoint.transform.position.z);
                }
                yield return null;
            }
            
        }
        
        StartCoroutine(GenerateMaze(mazeData, gameContext.mazeOptions, mazeData[0, 0]));
    }

    private GridCellData CreateCell(int x, int y, int centerX, int centerY)
    {
        GameObject gridCell = Instantiate(  gridCellPrefab, 
                                            new Vector3((x - centerX) * cellWidth, 0f, (y - centerY) * cellWidth), 
                                            Quaternion.identity);
        GridCellData cellData = gridCell.GetComponent<GridCellData>();
        cellData.x = x;
        cellData.y = y;
        cellData.isVisited = false;
        return cellData;
    }
}
