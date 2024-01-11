using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStar
{
    MazeCell[,] cells;
    Dictionary<int, AStarCell> allCells;
    Dictionary<int, AStarCell> open;
    List<int> closed;
    int targetIndex;
    public AStar(MazeCell[,] cells)
    {
        this.cells = cells;
        allCells = new Dictionary<int, AStarCell>();
        open = new Dictionary<int, AStarCell>();
        closed = new List<int>();

        targetIndex = cellGetIndex(cells[cells.GetLength(0)-1, cells.GetLength(1)-1]);

        foreach (MazeCell mazeCell in cells)
        {
            AStarCell aStarCell = mazeCellToAstarCell(mazeCell);
            allCells.Add(aStarCell.index, aStarCell);
        }

        allCells[0].gCost = 0;
        allCells[0].fCost = 0;
        open.Add(allCells[0].index ,allCells[0]);
    }

    private AStarCell mazeCellToAstarCell(MazeCell mazeCell)
    {
        int index = cellGetIndex(mazeCell);
        List<int> neighbours = new List<int>();
        foreach (MazeCell neighb in mazeCell.acessableNeighbours)
        {
            int neighbIndex = cellGetIndex(neighb);
            neighbours.Add(neighbIndex);
        }
        return new AStarCell(mazeCell.row, mazeCell.col, index, neighbours);

    }

    private int cellGetIndex(MazeCell cell)
    {
        return cell.col + (cell.row * (cells.GetLength(1)));
    }

    public List<int> getPath()
    {
        findPath();
        return closed;
    }

    private void findPath()
    {
        bool loop = true;
        while (loop)
        {
            AStarCell current =  getLowestFCost();
            open.Remove(current.index);
            closed.Add(current.index);
            current.closed = true;

            if (current.index == targetIndex)
            {
                loop = false;
            }

            foreach (int neighbourIndex in current.neighbours)
            {
                AStarCell neighbour = allCells[neighbourIndex];
                if (neighbour.closed == false)
                {
                    float fCost = calculateFCost(neighbour.index, current.index);
                    if (fCost< neighbour.fCost)
                    {
                        neighbour.fCost = fCost;
                        neighbour.gCost = current.gCost + Vector2.Distance(current.coOrds, neighbour.coOrds);
                    }

                    if (!open.ContainsKey(neighbourIndex))
                    {
                        open.Add(neighbourIndex, neighbour);
                    }
                }
            }
        }
    }

    private AStarCell getLowestFCost()
    {
        AStarCell lowest = new AStarCell();
        foreach (AStarCell cell in open.Values)
        {
            if (cell.fCost < lowest.fCost)
            {
                lowest = cell;
            }
        }
        return lowest;
    }

    private float calculateFCost(int investigate, int comingFrom)
    {
        return (Vector2.Distance(allCells[investigate].coOrds, allCells[targetIndex].coOrds) + (Vector2.Distance(allCells[investigate].coOrds, allCells[comingFrom].coOrds) + allCells[comingFrom].gCost));
    }

}
