using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeCell
{
    public int row, col,id, l, w;
    public bool[] walls = { true, true, true, true };
    public bool[] duplicates = { false, false, false, false };
    public MazeCell[] neighbours = { null, null, null, null };
    public List<MazeCell> acessableNeighbours;
    public bool visited = false;

    public MazeCell(int row, int col)
    {
        this.row = row;
        this.col = col;
        l = 3;
        w = 3;
        id = -1;
        visited = false;
        acessableNeighbours = new List<MazeCell>();
    }

    public void setNeighborLeft()
    {
        neighbours[0] = new MazeCell(row, col - 1);
    }
    public void setNeighborRight()
    {
        neighbours[1] = new MazeCell(row, col + 1);
    }
    public void setNeighborUp()
    {
        neighbours[2] = new MazeCell(row - 1, col);
    }
    public void setNeighborDown()
    {
        neighbours[3] = new MazeCell(row + 1, col);
    }

    public void removeWallLeft()
    {
        walls[0] = false;
        acessableNeighbours.Add(neighbours[0]);
    }
    public void removeWallRight()
    {
        walls[1] = false;
        acessableNeighbours.Add(neighbours[1]);
    }
    public void removeWallUp()
    {
        walls[2] = false;
        acessableNeighbours.Add(neighbours[2]);
    }
    public void removeWallDown ()
    {
        walls[3] = false;
        acessableNeighbours.Add(neighbours[3]);
    }

    public MazeCell[] getNeighbours()
    {
        return neighbours;
    }
}