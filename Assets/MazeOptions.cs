using System.Collections;
using System.Collections.Generic;

public class MazeOptions
{
    public static int[] levelSizes = { 3, 5, 9, 11, 17, 19, 27, 29, 39, 41};
    public int level = 0;

    public int Width { get { return levelSizes[level - 1]; } }
    public int Height { get { return levelSizes[level - 1]; } }
}
