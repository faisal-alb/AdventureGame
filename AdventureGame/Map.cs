using System;

namespace AdventureGame;

public class Map
{
    public char[,] grid;
    public int gridWidth { get; }
    public int gridHeight { get; }

    public Map(int width, int height)
    {
        this.gridWidth = width;
        this.gridHeight = height;
        this.grid = new char[gridWidth, gridHeight];

        InitializeMap();
    }

    private void InitializeMap()
    {
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                grid[x, y] = '0';
            }
        }
    }
}