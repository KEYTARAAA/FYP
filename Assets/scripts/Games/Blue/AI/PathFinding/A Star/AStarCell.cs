using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarCell
{
    public int index;
    public Vector2 coOrds;
    public float gCost = 1000000000000, hCost= 1000000000000, fCost= 1000000000000;
    public bool closed = false;
    public List<int> neighbours;

    public AStarCell(int row, int col, int index, List<int> neighbours)
    {
        coOrds = new Vector2(col, row);
        this.index = index;
        this.neighbours = neighbours;
    }
    public AStarCell()
    {

    }


}
