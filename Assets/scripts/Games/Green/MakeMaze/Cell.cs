using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell
{
    public int row, col,id, l, w;
    public Cell left = null, right = null, up = null, down = null;
    public bool ghostSpawn = false;

    public Cell(int row, int col)
    {
        this.row = row;
        this.col = col;
        l = 3;
        w = 3;
        id = -1;
    }

    public void reverse()
    {
        Cell temp = left;
        left = right;
        right = temp;
    }
}