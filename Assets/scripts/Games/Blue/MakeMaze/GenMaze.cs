// Converted from UnityScript to C# at http://www.M2H.nl/files/js_to_c.php - by Mike Hergaarden
// Do test the code! You usually need to change a few small bits.

using System.Collections;
using System;
using UnityEngine;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;

public class GenMaze
{
    System.Random rnd = new System.Random();
    MazeCell[,] grid;
    MazeCell current;
    Stack<MazeCell> stack;
    private int rows, cols;

    public GenMaze(int rows, int cols)
    {
        rnd = new System.Random(DateTime.Now.Millisecond);
        this.rows = rows;
        this.cols = cols;
        gen();
    }

    private void gen()
    {
        reset();
        draw();
    }

    private void reset()
    {
        stack = new Stack<MazeCell>();
        grid = new MazeCell[rows, cols];
        for (int row = 0; row < grid.GetLength(0); row++)
        {
            for (int col = 0; col < grid.GetLength(1); col++)
            {
                MazeCell cell = new MazeCell(row, col);

                if (col > 0)
                {
                    cell.setNeighborLeft();
                }
                if (col < grid.GetLength(1) - 1)
                {
                    cell.setNeighborRight();
                }
                if (row > 0)
                {
                    cell.setNeighborUp();
                }
                if (row < grid.GetLength(0) - 1)
                {
                    cell.setNeighborDown();
                }
                grid[row, col] = cell;
                //stack.Add(grid[row,col]);
            }
        }
        current = grid[0, 0];
    }

    private void draw()
    {
        bool loop = true;
        while (loop)
        {
            //STEP 1
            current.visited = true;


            MazeCell next = checkNeighbours(current);

            if (next.row > -1)
            {
            //STEP 2
            stack.Push(grid[current.row, current.col]);

                //STEP 3
                if (next.col < current.col)
                {
                    grid[current.row, current.col].removeWallLeft();
                    grid[next.row, next.col].removeWallRight();
                }

                if (next.col > current.col)
                {
                    grid[current.row, current.col].removeWallRight();
                    grid[next.row, next.col].removeWallLeft();
                }

                if (next.row < current.row)
                {
                    grid[current.row, current.col].removeWallUp();
                    grid[next.row, next.col].removeWallDown();
                }

                if (next.row > current.row)
                {
                    grid[current.row, current.col].removeWallDown();
                    grid[next.row, next.col].removeWallUp();
                }


                //STEP 4
                current = grid[next.row, next.col];
            }
            else if (stack.Count > 0) { 
                current = stack.Pop();
            }else{
                    loop = false;
            }
        }

        removeDouplacateWalls();
    }

    private MazeCell checkNeighbours(MazeCell cell)
    {
        List<MazeCell> neighbours = new List<MazeCell>();
        if (cell.neighbours[0] != null)
        {
            if (grid[cell.neighbours[0].row, cell.neighbours[0].col].visited == false)
            {
                neighbours.Add(grid[cell.neighbours[0].row, cell.neighbours[0].col]);
            }
        }
        if (cell.neighbours[1] != null)
        {
            if (grid[cell.neighbours[1].row, cell.neighbours[1].col].visited == false)
            {
                neighbours.Add(grid[cell.neighbours[1].row, cell.neighbours[1].col]);
            }
        }
        if (cell.neighbours[2] != null)
        {
            if (grid[cell.neighbours[2].row, cell.neighbours[2].col].visited == false)
            {
                neighbours.Add(grid[cell.neighbours[2].row, cell.neighbours[2].col]);
            }
        }
        if (cell.neighbours[3] != null)
        {
            if (grid[cell.neighbours[3].row, cell.neighbours[3].col].visited == false)
            {
                neighbours.Add(grid[cell.neighbours[3].row, cell.neighbours[3].col]);
            }
        }

        if (neighbours.Count > 0)
        {
            return neighbours[rnd.Next(neighbours.Count)];
        }

        return new MazeCell(-1,-1);
    }

    private string[,] getMaze()
    {
        string[,] maze = new string[rows, cols];
        for (int row = 0; row < grid.GetLength(0); row++)
        {
            for (int col = 0; col < grid.GetLength(1); col++)
            {
                MazeCell cell = grid[row, col];
                string entry = cell.walls[0].ToString() + "," + cell.walls[1].ToString() + "," + cell.walls[2].ToString() + "," + cell.walls[3].ToString() +
                            "," + cell.duplicates[0].ToString() + "," + cell.duplicates[1].ToString() + "," + cell.duplicates[2].ToString() + "," + cell.duplicates[3].ToString();
                maze[row, col] = entry;
            }
        }
        return maze;
    }
    private void removeDouplacateWalls()
    {
        for (int row = 0; row < grid.GetLength(0); row++)
        {
            for (int col = 0; col < grid.GetLength(1); col++)
            {
                if (cellExists(row, col - 1))
                {
                    if (grid[row, col].walls[0] && grid[row, col - 1].walls[1] && !grid[row, col].duplicates[0])
                    {
                        grid[row, col - 1].duplicates[1] = true;
                    }
                }
                if (cellExists(row, col + 1))
                {
                    if (grid[row, col].walls[1] && grid[row, col + 1].walls[0] && !grid[row, col].duplicates[1])
                    {
                        grid[row, col + 1].duplicates[0] = true;
                    }
                }
                if (cellExists(row - 1, col))
                {
                    if (grid[row, col].walls[2] && grid[row - 1, col].walls[3] && !grid[row, col].duplicates[2])
                    {
                        grid[row - 1, col].duplicates[3] = true;
                    }
                }
                if (cellExists(row + 1, col))
                {
                    if (grid[row, col].walls[3] && grid[row + 1, col].walls[2] && !grid[row, col].duplicates[3])
                    {
                        grid[row + 1, col].duplicates[2] = true;
                    }
                }
            }
        }
    }

    private bool cellExists(int row, int col)
    {
        if (row < 0)
        {
            return false;
        }
        if (row >= rows)
        {
            return false;
        }
        if (col < 0)
        {
            return false;
        }
        if (col >= cols)
        {
            return false;
        }
        return true;
    }
    public void writeMaze(string path)
    {
        StreamWriter fileStream = File.CreateText(path); ;
        string[,]maze = getMaze();
        string line = "";
        for (int row = 0; row < maze.GetLength(0); row++)
        {
            for (int col = 0; col < maze.GetLength(1); col++)
            {
                line += maze[row, col];
                if (col != maze.GetLength(1)-1)
                {
                    line += " ";
                }
            }
            if (row != maze.GetLength(0) - 1)
            {
                line += "\n";
            }
        }
        fileStream.Write(line);
        fileStream.Close();

    }
    public MazeCell[,] getGrid()
    {
        return grid;
    }
}