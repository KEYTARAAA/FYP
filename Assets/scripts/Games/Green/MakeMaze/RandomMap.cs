// Converted from UnityScript to C# at http://www.M2H.nl/files/js_to_c.php - by Mike Hergaarden
// Do test the code! You usually need to change a few small bits.

using System.Collections;
using System;
using UnityEngine;
using System.IO;
using System.Threading.Tasks;

public class RandomMap
{
    System.Random rnd = new System.Random();
    Cell[,] tetris = new Cell[10, 5];
    string[,] maze;

    public RandomMap()
    {
        rnd = new System.Random(DateTime.Now.Millisecond);

        resetTetris();
        setGhostSpawn();
        randomize();
        correctHeight();
        correctWidth();
        neighbourAll();
        checkDeadEnds();
        wallConnections();
    }

    void resetTetris()
    {
        for (int row = 0; row < tetris.GetLength(0); row++)
        {
            for (int col = 0; col < tetris.GetLength(1); col++)
            {
                tetris[row, col] = new Cell(row, col);
            }
        }
    }

    void setLeft(Cell cell)
    {
        tetris[cell.row, cell.col].left = tetris[cell.row, (cell.col - 1)];
        tetris[cell.row, (cell.col - 1)].right = tetris[cell.row, cell.col];
    }

    void setRight(Cell cell)
    {
        //Debug.Log("("+cell.row+","+cell.col+")"+ " connecting right to : ("+ (cell.row ) + ","+ (cell.col + 1) + ")");
        tetris[cell.row, cell.col].right = tetris[cell.row, (cell.col + 1)];
        tetris[cell.row, (cell.col + 1)].left = tetris[cell.row, cell.col];
    }

    void setUp(Cell cell)
    {
        tetris[cell.row, cell.col].up = tetris[(cell.row - 1), cell.col];
        tetris[(cell.row - 1), cell.col].down = tetris[cell.row, cell.col];
    }

    void setDown(Cell cell)
    {
        tetris[cell.row, cell.col].down = tetris[(cell.row + 1), cell.col];
        tetris[(cell.row + 1), cell.col].up = tetris[cell.row, cell.col];
        //Debug.Log("(" + cell.row + "," + cell.col + ")" + " connecting down to : (" + cell.down.row + "," + cell.down.col + ")");
    }

    void pathLeft(Cell cell)
    {
        tetris[cell.row, cell.col].left = null;
        tetris[cell.row, (cell.col - 1)].right = null;
    }

    void pathRight(Cell cell)
    {
        tetris[cell.row, cell.col].right = null;
        tetris[cell.row, (cell.col + 1)].left = null;
        //Debug.Log("("+cell.row+","+cell.col+")"+ " connecting right to : ("+ cell.right.row+","+cell.right.col+")");
    }

    void pathUp(Cell cell)
    {
        tetris[cell.row, cell.col].up = null;
        tetris[(cell.row - 1), cell.col].down = null;
    }

    void pathDown(Cell cell)
    {
        tetris[cell.row, cell.col].down = null;
        tetris[(cell.row + 1), cell.col].up = null;
        //Debug.Log("(" + cell.row + "," + cell.col + ")" + " connecting down to : (" + cell.down.row + "," + cell.down.col + ")");
    }

    void setGhostSpawn()
    {

        tetris[4, 3].ghostSpawn = true;
        tetris[4, 3].up = null;
        tetris[4, 3].left = null;
        tetris[4, 3].down = new Cell(5, 3);
        tetris[4, 3].right = new Cell(4, 4);

        tetris[5, 3].ghostSpawn = true;
        tetris[5, 3].down = null;
        tetris[5, 3].left = null;
        tetris[5, 3].up = new Cell(4, 3); ;
        tetris[5, 3].right = new Cell(5, 4);

        tetris[4, 4].ghostSpawn = true;
        tetris[4, 4].up =null; //opening
        tetris[3, 4].down = null; //opening
        tetris[4, 4].right = new Cell(-1,-1);
        tetris[4, 4].down = new Cell(5, 4);
        tetris[4, 4].left = new Cell(4, 3);

        tetris[5, 4].ghostSpawn = true;
        tetris[5, 4].down = null;
        tetris[5, 4].right = new Cell(-1,-1);
        tetris[5, 4].up = new Cell(4, 4);
        tetris[5, 4].left = new Cell(5, 3);

        //makeTetris(cellGroup.plus(tetris[2,2]));

    }

    string[,] setBorders(string[,] oldMaze)
    {
        string[,] maze = new string[oldMaze.GetLength(0)+1, oldMaze.GetLength(1)+2];

        
        for (int row = 1; row < maze.GetLength(0); row++)
        {
            maze[row, 0] = "60";//11
            maze[row, (maze.GetLength(1) - 1)] = "60";//12
        }

        for (int col = 1; col < maze.GetLength(1); col++)
        {
            maze[0, col] = "60";//13
            maze[(maze.GetLength(0) - 1), col] = "60";//14
        }

        maze[0, 0] = "60";//43
        maze[0, maze.GetLength(1) - 1] = "60";//42
        maze[maze.GetLength(0) - 1, 0] = "60";//41
        maze[maze.GetLength(0) - 1, maze.GetLength(1) - 1] = "60";//44

        for (int row=0; row<oldMaze.GetLength(0); row++)
        {
            for (int col = 0; col < oldMaze.GetLength(1); col++)
            {
                maze[row + 1, col + 1] = oldMaze[row, col];
            }
        }

        return maze;
    }

    void correctHeight()
    {

        int lower = rnd.Next(4);
        int higher = rnd.Next(6, 10);
        int choice = 0;

        if (getRandomBool())
        {
            choice = lower;
        }
        else
        {
            choice = higher;
        }

        for (int col = 0; col < tetris.GetLength(1); col++)
        {
        
            tetris[choice, col].l += 1;
        }
    }

    void correctWidth()
    {
        /*int cols = tetris.GetLength(1);
        int tries = 0;
        bool loop = true;
        for (int row = 0; row < tetris.GetLength(0); row++)
        {
                while (true)
                {
                
                    if (tetris[row, 0].left == null || tetris[row, 0].down == right)//rnd.Next(4)].up == null && tetris[row, rnd.Next(4)].down == null)
                    {
                        loop = false;
                    }

                if (tries>cols)
                {
                    
                        tetris[row, 1].right = new Cell(row, );
                }
                tries++;
                }
            tetris[row, 0].w -= 1;
            }
        }

        for (int row = 0; row < tetris.GetLength(0); row++)
        {
            tetris[row, rnd.Next(4)].w -= 1;
        }*/
    }

    void printTetrisDimesnsions()
    {
        int totalR = 0, totalC = 0;

        foreach (Cell cell in tetris)
        {
            if (cell.col == 0)
            {
                totalR += cell.l;
            }
            if (cell.row == 0)
            {
                totalC += cell.w;
            }

        }
        Debug.Log("Tetris Rows: " + totalR + "\nTetris Cols: " + totalC);
    }


    public string[,] getMaze()
    {

        string[,] maze = new string[33, 28];
        for (int row = 0; row < maze.GetLength(0); row++)
        {
            for (int col = 0; col < maze.GetLength(1); col++)
            {
                maze[row, col] = "60";
            }
        }
        int mRow = 0, mCol = 0;

        maze = makeHalf(maze, 0, false);
        maze = makeHalf(maze, 27, true);


        maze = setPlayerSpawn(maze);
        maze = setBorders(maze);
        maze = format(maze);


        return maze;
    }

    int calculateMRow(int row, int col)
    {
        int total = 0;
        if (row != 0)
        {
            return total += calculateMRow((row - 1), col) + tetris[row, col].l;
        }
        else
        {
            return tetris[row, col].l;
        }
    }

    int calculateMCol(int row, int col)
    {
        int total = 0;
        if (col != 0)
        {
            return total += calculateMCol(row, (col - 1)) + tetris[row, col].w;
        }
        else
        {
            return tetris[row, col].w;
        }
    }

    Cell[,] reverse(Cell[,] tetris)
    {
        Cell[,] temp = new Cell[tetris.GetLength(0), tetris.GetLength(1)];

        foreach (Cell cell in tetris)
        {
            Debug.Log("old");
            printCell(cell);

            int newRow = cell.row; //(temp.GetLength(0) - 1) - cell.row;
            int newCol = (temp.GetLength(1) - 1) - cell.col;
            //Debug.Log("temp rows = "+ temp.GetLength(0)+ " temp cols = "+ temp.GetLength(1));
            temp[newRow, newCol] = cell;
            //Debug.Log("temp (" + (cell.row) + "," + ( cell.col) + ") = (" + ((temp.GetLength(0) - 1) - cell.row) + "," + ((temp.GetLength(1) - 1) - cell.col) + ")");
            //Debug.Log("cell = "+cell);
            temp[newRow, newCol].row = newRow;
            temp[newRow, newCol].col = newCol;
            temp[newRow, newCol].reverse();
            Debug.Log("new");
            printCell(temp[newRow, newCol]);
        }

        return temp;
    }

    void printCell(Cell cell)
    {
        Debug.Log("Cell =  (" + (cell.row) + "," + (cell.col) + ")");
        if (cell.left != null)
        {
            Debug.Log("left =  (" + (cell.left.row) + "," + (cell.left.col) + ")");
        }
        else
        {
            Debug.Log("left =  null");
        }

        if (cell.right != null)
        {
            Debug.Log("right =  (" + (cell.right.row) + "," + (cell.right.col) + ")");
        }
        else
        {
            Debug.Log("right =  null");
        }

        if (cell.up != null)
        {
            Debug.Log("up =  (" + (cell.up.row) + "," + (cell.up.col) + ")");
        }
        else
        {

            Debug.Log("up =  null");
        }

        if (cell.down != null)
        {
            Debug.Log("down =  (" + (cell.down.row) + "," + (cell.down.col) + ")");
        }
        else
        {

            Debug.Log("up =  null");
        }



        Debug.Log("l =  " + (cell.l));
        Debug.Log("w =  " + (cell.w));
        Debug.Log("===============================================");
    }

    string[,] makeHalf(string[,] maze, int offSet, bool reflect)
    {
        int reflectionFactor = 0;
        int mRow = 0;
        int mCol = 0;

        if (reflect)
        {
            reflectionFactor = -1;
        }
        else
        {
            reflectionFactor = 1;
        }

        for (int row = 0; row < tetris.GetLength(0); row++)
        {
            for (int col = 0; col < tetris.GetLength(1); col++)
            {
                Cell cell = tetris[row, col];
                if (!cell.ghostSpawn) { 
                    int upRow = 0;
                    int leftCol = 0;
                    bool checkSize = false;
                    bool extend = false;
                    if (cell.row != 0)
                    {
                        mRow = calculateMRow(cell.row - 1, cell.col);
                        upRow = mRow;// - tetris[cell.row - 1, cell.col].l;
                    }
                    else
                    {
                        mRow = 0;
                        upRow = 0;

                    }
                    if (cell.col != 0)
                    {
                        mCol = calculateMCol(cell.row, cell.col - 1);
                        //mCol -= (1 * reflectionFactor);
                        //leftCol = mCol -= (1 * reflectionFactor);
                    }
                    else
                    {
                        mCol = 0;
                    }



                    for (int cRow = 0; cRow < cell.l + 1; cRow++)
                    {
                        if (cell.left == null)
                        {
                            if (cRow == 0 && cell.col != 0)
                            {
                                if (Math.Min(cell.l, tetris[cell.row, cell.col - 1].l) == cell.l &&
                                    cell.l != tetris[cell.row, cell.col - 1].l && !checkSize)
                                {
                                    cRow--;
                                    checkSize = true;
                                    extend = true;
                                }
                            }

                            if (cRow == 0 && cell.row == tetris.GetLength(0) - 1)
                            {
                                //cRow++;
                            }


                            //printCell(cell);
                            //Debug.Log("maze[" + mRow + "," + ( offSet + (mCol * reflectionFactor) + (1 * reflectionFactor)) + ")]");
                            maze[mRow, offSet + (mCol * reflectionFactor)] = "00";
                        }

                        if (cell.right == null)
                        {
                            if (cRow == 0 && cell.col != 0)
                            {
                                if (Math.Min(cell.l, tetris[cell.row, cell.col - 1].l) == cell.l &&
                                    cell.l != tetris[cell.row, cell.col - 1].l && !checkSize)
                                {
                                    cRow--;
                                    checkSize = true;
                                    extend = true;
                                }
                            }

                            if (cRow == 0 && cell.row == tetris.GetLength(0) - 1)
                            {
                                //cRow++;
                            }
                            //printCell(cell);
                            //Debug.Log("maze[" + mRow + "," + ( offSet + (mCol * reflectionFactor) + (1 * reflectionFactor)) + ")]");
                            if (offSet + (mCol * reflectionFactor) + cell.w < (maze.GetLength(1) / 2) - 1)
                            {
                                maze[mRow, offSet + (mCol * reflectionFactor) + cell.w] = "00";
                            }

                        }
                        mRow++;
                    }

                    for (int cCol = 0; cCol < cell.w; cCol++)
                    {
                        if (cell.up == null)
                        {
                            //Debug.Log("maze[(upRow), offSet + (mCol * reflectionFactor)] = maze[(" + upRow + "," + (offSet + (mCol * reflectionFactor)) + ")]");
                            maze[(upRow), offSet + (mCol * reflectionFactor)] = "00";
                            if (extend && cCol == cell.w - 1)
                            {
                                //maze[(upRow), offSet + (mCol * reflectionFactor) + 1] = "00";
                            }
                        }

                        if (cell.down == null)
                        {
                            if (upRow + cell.l < maze.GetLength(0) - 1)
                            {
                                //Debug.Log("maze[(upRow), offSet + (mCol * reflectionFactor)] = maze[(" + upRow + "," + (offSet + (mCol * reflectionFactor)) + ")]");
                                maze[(upRow + cell.l), offSet + (mCol * reflectionFactor)] = "00";
                                if (extend && cCol == cell.w - 1)
                                {
                                    //maze[(upRow + cell.l), offSet + (mCol * reflectionFactor) + 1] = "00";
                                }
                            }
                        }
                        /*if (cell.down == null)
                        {
                            maze[(mRow-1), offSet + (mCol * reflectionFactor)] = "00";
                        }*/
                        mCol++;
                    }
                }
                else
                {
                    if (reflectionFactor == -1 && cell.col == 3)
                    {
                        mRow = calculateMRow(cell.row - 1, cell.col);
                        for (int cRow = 0; cRow < cell.l + 1; cRow++)
                        {
                                //printCell(cell);
                                //Debug.Log("maze[" + mRow + "," + ( offSet + (mCol * reflectionFactor) + (1 * reflectionFactor)) + ")]");
                                
                                    maze[mRow, offSet + (mCol * reflectionFactor)] = "00";
                                

                            mRow++;
                        }
                    }
                    maze = formatGhostSpwan(maze, offSet, reflectionFactor, cell);
                }
            }
        }
        return maze;
    }

    private string[,] formatGhostSpwan(string[,] maze, int offSet, int reflectionFactor, Cell cell)
    {
        //4,3
        //5,3
        //4,4
        //5,4

        bool entrance = false;
        bool up = false;
        bool down = false;
        bool left = false;
        bool right = false;
        int fillCols = 0;
        int fillRows = 0;
        int mRow = calculateMRow(cell.row - 1, cell.col)+1;
        int mCol = calculateMCol(cell.row, cell.col - 1); // + 1;
        int originalCol = mCol;
        offSet += reflectionFactor;

        if (cell.row == 5)
        {
            down = true;
            fillRows = mRow + cell.l;
        }
        else if (cell.row == 4 && cell.col == 4)
        {
            up = true;
            entrance = true;
            fillRows = mRow;
        }
        else
        {
            up = true;
            fillRows = mRow;
        }

        if (cell.col == 4)
        {
            right = true;
            fillCols = mCol + cell.w-1;
        }
        else if (entrance)
        {
            left = true;
            fillCols = mCol;
        }
        else
        {
            left = true;
            fillCols = mCol;
        }

        int quickFix = 0;
        if (down)
        {
            quickFix = 1;
        }

        for (int row = 0; row < cell.l-quickFix; row++)
        {
            for (int col = 0; col < cell.w; col++)
            {
                if (up)
                {
                    maze[mRow+1, offSet + ((mCol+1) * reflectionFactor)] = "80";
                }
                if (down)
                {
                    maze[mRow-1, offSet + ((mCol+1) * reflectionFactor)] = "80";
                }
                if (entrance && row == 0 && col == cell.w-1)
                {
                    maze[mRow , offSet + ((mCol-1) * reflectionFactor)] = "00";
                }
                mCol++;
                
            }
            mRow++;
            mCol = originalCol;
        }
        return maze;
    }


    public void randomize()
    {
        ArrayList toInvestigate = new ArrayList();

        foreach (Cell cell in tetris)
        {
            if (!cell.ghostSpawn)
            {
                toInvestigate.Add(cell);
            }
            
        }

        int id = 0;
        int investigate = 0;
        for(int row=0; row<tetris.GetLength(0)-2; row++)//-2 for ghost area
        {
            for (int col = 0; col < tetris.GetLength(1)-2; col++)// -2 for ghost area
             {
                investigate = rnd.Next(toInvestigate.Count);

                if (((Cell)(toInvestigate[investigate])).id == -1)
                {
                    Cell cell = ((Cell)(toInvestigate[investigate]));

                    id = set(tetris[cell.row, cell.col], 2, id);
                }
                toInvestigate.RemoveAt(investigate);
                //((Cell)(toInvestigate[id])).id = id;
                //id++;
            }
        }

        foreach(object obj in toInvestigate)
        {
            Cell cell = ((Cell)(obj));
            //Debug.Log("id = " + cell.id);
        }
    }

    int set(Cell cell, int probability, int id)
    {

        tetris[cell.row, cell.col].id = id;
        id++;

        bool lefty = false;
        bool upity = false;

        //up
        if (tetrisExists(cell.row - 1, cell.col))
            {
                if (tetris[cell.row-1, cell.col].id == -1 && !tetris[cell.row - 1, cell.col].ghostSpawn)
                {
                int pass = rnd.Next(probability);
                if (pass > 0)
                {
                    tetris[cell.row, cell.col].up = new Cell(cell.row - 1, cell.col);
                    tetris[cell.row, cell.col].id = id;

                    

                    tetris[cell.row-1, cell.col].down = new Cell(cell.row, cell.col);

                    id = set(tetris[cell.row - 1, cell.col], ((int)(probability / 2)), id);

                    upity = true;

                }
                    
                }
        }
        else
        {
            
                int pass = rnd.Next(tetris.GetLength(1) * 2);
                if (pass == 0)
                {
                    tetris[cell.row, cell.col].up = new Cell( - 1, -1);
                }
        }

            //down
            if (tetrisExists(cell.row + 1, cell.col))// && (cell.row!=3 && (cell.col!=3 || cell.col!=4)))
            {
            int downProb = 0;
            if(upity){
                downProb = ((int)(probability / 2));
            }
            else
            {
                downProb = probability;
            }

            if (tetris[cell.row + 1, cell.col].id == -1 && !tetris[cell.row + 1, cell.col].ghostSpawn)
            {
                int pass = rnd.Next(downProb);
                if (pass > 0)
                {
                    tetris[cell.row, cell.col].down = new Cell(cell.row + 1, cell.col);
                    tetris[cell.row, cell.col].id = id;



                    tetris[cell.row + 1, cell.col].up = new Cell(cell.row, cell.col);

                    id = set(tetris[cell.row + 1, cell.col], ((int)(downProb / 2)), id);

                }

            }
        }
        else
        {
            int pass = rnd.Next(tetris.GetLength(1) * 2);
            if (pass == 0)
            {
                tetris[cell.row, cell.col].down = new Cell(-1, -1);
            }
        }

            //left
            if (tetrisExists(cell.row, cell.col - 1))
            {
            if (tetris[cell.row, cell.col - 1].id == -1 && !tetris[cell.row, cell.col - 1].ghostSpawn)
            {
                int pass = rnd.Next(probability);
                if (pass > 0)
                {
                    tetris[cell.row, cell.col].left = new Cell(cell.row, cell.col - 1);
                    tetris[cell.row, cell.col].id = id;



                    tetris[cell.row, cell.col - 1].right = new Cell(cell.row, cell.col);

                    id = set(tetris[cell.row, cell.col - 1], ((int)(probability / 2)), id);
                    lefty = true;

                }

            }
        }
        else
        {

            int pass = rnd.Next(tetris.GetLength(0) * 2);
            if (pass == 0)
            {
                tetris[cell.row, cell.col].left = new Cell(-1, -1);
            }
        }

        //right
        if (tetrisExists(cell.row, cell.col + 1))
        {
            int rightProb = 0;
            if (lefty)
            {
                rightProb = ((int)(probability / 2));
            }
            else
            {
                rightProb = probability;
            }

            if (tetris[cell.row, cell.col + 1].id == -1 && !tetris[cell.row, cell.col + 1].ghostSpawn)
            {
                int pass = rnd.Next(rightProb);
                if (pass > 0)
                {
                    tetris[cell.row, cell.col].right = new Cell(cell.row, cell.col + 1);
                    tetris[cell.row, cell.col].id = id;



                    tetris[cell.row, cell.col + 1].left = new Cell(cell.row, cell.col);

                    id = set(tetris[cell.row, cell.col + 1], ((int)(rightProb / 2)), id);

                }

            }
        }
        //((Cell)(toInvestigate[id])).id = id;
        return id;
    }

    bool tetrisExists(int row, int col)
    {
        bool check = true;

        if (row < 0)
        {
            check = false;
        }

        if (row > (tetris.GetLength(0) - 1))
        {
            check = false;
        }

        if (col < 0)
        {
            check = false;
        }

        if (col > (tetris.GetLength(1) - 1))
        {
            check = false;
        }


        return check;
    }

    bool mazeExists(int row, int col, string[,] maze)
    {
        bool check = true;

        if (row < 0)
        {
            check = false;
        }

        if (row > (maze.GetLength(0) - 1))
        {
            check = false;
        }

        if (col < 0)
        {
            check = false;
        }

        if (col > (maze.GetLength(1) - 1))
        {
            check = false;
        }


        return check;
    }

    bool getRandomBool()
    {
        int choice = rnd.Next(2);
        if (choice == 0)
        {
            return false;
        }
        else
        {
            return true;
        }

    }

    void makeTetris(Cell[] cells)
    {
        if (checkFit(cells))
        {
            foreach (Cell cell in cells)
            {
                tetris[cell.row, cell.col] = cell;
            }
        }
    }

    bool checkFit(Cell[] cells)
    {
        bool check = true;

        foreach (Cell cell in cells)
        {
            printCell(cell);
            if (cell.row < 0)
            {
                check = false;
            }

            if (cell.row > (tetris.GetLength(0) - 1))
            {
                check = false;
            }

            if (cell.col < 0)
            {
                check = false;
            }

            if (cell.col > (tetris.GetLength(1) - 1))
            {
                check = false;
            }
        }

        //Debug.Log(check);


        return check;
    }

    void neighbourAll()
    {
        foreach (Cell cell in tetris)
        {
            if(cell.up == null && cell.down == null && cell.left == null && cell.right == null)
            {
                if (getRandomBool())
                {
                    if (tetrisExists(cell.row, cell.col-1))
                    {
                        tetris[cell.row, cell.col].left = tetris[cell.row, cell.col - 1];
                        tetris[cell.row, cell.col - 1].right = tetris[cell.row, cell.col];
                    }
                    else
                    {
                            tetris[cell.row, cell.col].right = tetris[cell.row, cell.col + 1];
                            tetris[cell.row, cell.col + 1].left = tetris[cell.row, cell.col];
                    }
                }
                else
                {
                    if (tetrisExists(cell.row - 1, cell.col))
                    {
                        tetris[cell.row, cell.col].up = tetris[cell.row - 1, cell.col];
                        tetris[cell.row - 1, cell.col].down = tetris[cell.row, cell.col];
                    }
                    else
                    {
                        tetris[cell.row, cell.col].down = tetris[cell.row + 1, cell.col];
                        tetris[cell.row + 1, cell.col].up = tetris[cell.row, cell.col];
                    }
                }
            }
        }
    }

    public string[,] getTetris()
    {
        string[,] toString = new string[28,31];
        int f = 5;

        foreach (Cell cell in tetris)
        {
            for (int row = 0; row < cell.l; row++)
            {
                for (int col = 0; col < cell.w; col++)
                {
                    
                    toString[2, col] = cell.id.ToString() + "60";
                }
            }
        }

        
        return toString;
    }

    string[,] format(string[,] maze)
    {
        for (int row = 0; row < maze.GetLength(0); row++)
        {
            for (int col = 0; col < maze.GetLength(1); col++)
            {
                maze = innerLeft(row, col, maze);
                maze = innerRight(row, col, maze);
                maze = innerUp(row, col, maze);
                maze = innerDown(row, col, maze);
                maze = outterLeft(row, col, maze);
                maze = outterUp(row, col, maze);
                maze = outterRight(row, col, maze);
                maze = outterDown(row, col, maze);
                maze = wallLeft(row, col, maze);
                maze = wallRight(row, col, maze);
                maze = wallUp(row, col, maze);
                maze = wallDown(row, col, maze);

            }
        }

        return maze;
    }

    bool wall(string entry)
    {
        
        int.TryParse(entry, out int number);
        int type = (number / 10);
        if ((type == 6) || (type == 4) || (type == 5) || (type == 1))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    string[,] innerLeft(int row, int col, string[,] maze)
    {
        if (mazeExists(row - 1, col, maze) && mazeExists(row, col + 1, maze) && mazeExists(row - 1, col + 1, maze))
        {
            if (wall(maze[row, col]) && wall(maze[row - 1, col]) && wall(maze[row, col + 1]) && !wall(maze[row - 1, col + 1]))
            {
                if (maze[row-1, col + 1] != "80")
                {
                    maze[row, col] = "41";
                }
                else
                {
                    maze[row, col] = "12";
                }
            }
        }
        return maze;
    }

    string[,] innerRight(int row, int col, string[,] maze)
    {
        if (mazeExists(row + 1, col, maze) && mazeExists(row, col - 1, maze) && mazeExists(row + 1, col - 1, maze))
        {
            if (wall(maze[row, col]) && wall(maze[row + 1, col]) && wall(maze[row, col - 1]) && !wall(maze[row + 1, col - 1]))
            {
                maze[row, col] = "42";
            }
        }
        return maze;
    }

    string[,] innerUp(int row, int col, string[,] maze)
    {
        if (mazeExists(row + 1, col, maze) && mazeExists(row, col + 1, maze) && mazeExists(row + 1, col + 1, maze))
        {
            if (wall(maze[row, col]) && wall(maze[row + 1, col]) && wall(maze[row, col + 1]) && !wall(maze[row + 1, col + 1]))
            {
                maze[row, col] = "43";
            }
        }
        return maze;
    }

    string[,] innerDown(int row, int col, string[,] maze)
    {
        if (mazeExists(row - 1, col, maze) && mazeExists(row, col - 1, maze) && mazeExists(row - 1, col - 1, maze))
        {
            if (wall(maze[row, col]) && wall(maze[row - 1, col]) && wall(maze[row, col - 1]) && !wall(maze[row - 1, col - 1]))
            {

                if (maze[row - 1, col - 1] != "80")
                {
                    maze[row, col] = "44";
                }
                else
                {
                    maze[row, col] = "11";
                }
            }
        }
        return maze;
    }

    string[,] outterLeft(int row, int col, string[,] maze)
    {
        if (mazeExists(row - 1, col, maze) && mazeExists(row, col + 1, maze))
        {
            if (wall(maze[row, col]) && !wall(maze[row - 1, col]) && !wall(maze[row, col + 1]))
            {
                maze[row, col] = "51";
            }
        }
        return maze;
    }

    string[,] outterRight(int row, int col, string[,] maze)
    {
        if (mazeExists(row + 1, col, maze) && mazeExists(row, col - 1, maze))
        {
            if (wall(maze[row, col]) && !wall(maze[row + 1, col]) && !wall(maze[row, col - 1]))
            {
                maze[row, col] = "52";
            }
        }
        return maze;
    }

    string[,] outterUp(int row, int col, string[,] maze)
    {
        if (mazeExists(row + 1, col, maze) && mazeExists(row, col + 1, maze))
        {
            if (wall(maze[row, col]) && !wall(maze[row + 1, col]) && !wall(maze[row, col + 1]) && maze[row+1,col-1]!="80")
            {
                maze[row, col] = "53";
            }
        }
        return maze;
    }

    string[,] outterDown(int row, int col, string[,] maze)
    {
        if (mazeExists(row - 1, col, maze) && mazeExists(row, col - 1, maze))
        {
            if (wall((maze[row, col])) && !wall(maze[row - 1, col]) && !wall(maze[row, col - 1]))
            {
                maze[row, col] = "54";
            }
        }
        return maze;
    }

    string[,] wallLeft(int row, int col, string[,] maze)
    {
        if (mazeExists(row - 1, col, maze) && mazeExists(row + 1, col, maze) && mazeExists(row, col + 1, maze))
        {
            if ((maze[row, col] == "60") && wall(maze[row - 1, col]) && wall(maze[row + 1, col]) && !wall(maze[row, col + 1]))
            {

                if (maze[row, col+1] != "80")
                {
                    maze[row, col] = "11";
                }
                else
                {
                    maze[row, col] = "12";
                }
            }
        }
        return maze;
    }

    string[,] wallRight(int row, int col, string[,] maze)
    {
        if (mazeExists(row - 1, col, maze) && mazeExists(row + 1, col, maze) && mazeExists(row, col - 1, maze))
        {
            if ((maze[row, col] == "60") && wall(maze[row - 1, col]) && wall(maze[row + 1, col]) && !wall(maze[row, col - 1]))
            {
                maze[row, col] = "12";
            }
        }
        return maze;
    }

    string[,] wallUp(int row, int col, string[,] maze)
    {
        if (mazeExists(row, col - 1, maze) && mazeExists(row, col + 1, maze) && mazeExists(row + 1, col, maze))
        {
            if ((maze[row, col] == "60") && wall(maze[row, col - 1]) && wall(maze[row, col + 1]) && !wall(maze[row + 1, col]))
            {
                if (maze[row+1, col]!="80")
                {
                     maze[row, col] = "13";
                }
                else
                {
                    maze[row, col] = "14";
                }
               
            }
        }
        return maze;
    }

    string[,] wallDown(int row, int col, string[,] maze)
    {
        if (mazeExists(row, col - 1, maze) && mazeExists(row, col + 1, maze) && mazeExists(row - 1, col, maze))
        {
            if ((maze[row, col] == "60") && wall(maze[row, col - 1]) && wall(maze[row, col + 1]) && !wall(maze[row - 1, col]))
            {
                maze[row, col] = "14";
            }
        }
        return maze;
    }



    void checkDeadEnds()
    {
        for (int row = 0; row < tetris.GetLength(0); row++)
        {
            for (int col = 0; col < tetris.GetLength(1); col++)
            {
                if (!tetris[row, col].ghostSpawn)
                {
                checkLeftDeadEnd(tetris[row, col]);
                checkUpDeadEnd(tetris[row, col]);
                    checkGhostDeadEnds(tetris[row, col]);
                }
            }
        }
    }



    bool checkLeftDeadEnd(Cell cell)
    {
        bool restart = false;
        bool up = false;
        bool down = false;
        if (cell.left == null)
        {
            if (cell.up != null)
            {
                if (tetrisExists(cell.row - 1, cell.col))
                {
                    if ((tetris[cell.row - 1, cell.col].left != null) && (tetris[cell.row - 1, cell.col].down != null))
                    {
                        if (tetrisExists(cell.row - 1, cell.col - 1))
                        {
                            if ((tetris[cell.row - 1, cell.col - 1].down != null))
                            {
                                //up = true;
                                if (getRandomBool())
                                {
                                    if (getRandomBool() && !tetris[cell.row - 1, cell.col].ghostSpawn)
                                    {
                                        tetris[cell.row - 1, cell.col].left = null;
                                        if (tetrisExists(cell.row - 1, cell.col - 1))
                                        {
                                            tetris[cell.row - 1, cell.col - 1].right = null;
                                        }

                                    }
                                    else
                                    {
                                        tetris[cell.row - 1, cell.col].down = null;
                                        if (tetrisExists(cell.row, cell.col))
                                        {
                                            tetris[cell.row, cell.col].up = null;
                                        }

                                    }
                                }
                                else
                                {
                                    tetris[cell.row - 1, cell.col - 1].down = null;
                                    if (tetrisExists(cell.row, cell.col - 1))
                                    {
                                        tetris[cell.row, cell.col - 1].up = null;
                                    }

                                }
                                //Debug.Log("(" + cell.row + "," + cell.col + ") upwards");
                            }
                        }
                    }
                }
            }

            if (cell.down != null)
            {
                if (tetrisExists(cell.row + 1, cell.col))
                {

                    if ((tetris[cell.row + 1, cell.col].left != null) && (tetris[cell.row + 1, cell.col].up != null))
                    {
                        if (tetrisExists(cell.row + 1, cell.col - 1))
                        {
                            if ((tetris[cell.row + 1, cell.col - 1].up != null))
                            {
                                if (getRandomBool())
                                {
                                    if (getRandomBool() && !tetris[cell.row + 1, cell.col].ghostSpawn)
                                    {
                                        tetris[cell.row + 1, cell.col].left = null;
                                        if (tetrisExists(cell.row + 1, cell.col + 1))
                                        {
                                            tetris[cell.row + 1, cell.col + 1].right = null;
                                        }

                                    }
                                    else
                                    {
                                        tetris[cell.row + 1, cell.col].up = null;
                                        if (tetrisExists(cell.row, cell.col))
                                        {
                                            tetris[cell.row, cell.col].down = null;
                                        }

                                    }
                                }
                                else
                                {
                                    tetris[cell.row + 1, cell.col - 1].up = null;
                                    if (tetrisExists(cell.row, cell.col - 1))
                                    {
                                        tetris[cell.row, cell.col - 1].down = null;
                                    }

                                }
                                //Debug.Log("(" + cell.row + "," + cell.col + ") downwards");
                            }
                        }

                    }
                }
            }
        }




        return restart;
    }


    bool checkUpDeadEnd(Cell cell)
    {
        bool restart = false;
        bool up = false;
        bool down = false;
        if (cell.up == null)
        {
            if (cell.left != null)
            {
                if (tetrisExists(cell.row, cell.col - 1))
                {
                    if ((tetris[cell.row, cell.col - 1].up != null) && (tetris[cell.row, cell.col - 1].right != null))
                    {
                        if (tetrisExists(cell.row - 1, cell.col - 1))
                        {
                            if ((tetris[cell.row - 1, cell.col - 1].right != null))
                            {
                                //up = true;
                                if (getRandomBool())
                                {
                                    if (getRandomBool() && !tetris[cell.row , cell.col- 1].ghostSpawn)
                                    {
                                        tetris[cell.row, cell.col - 1].up = null;
                                        if (tetrisExists(cell.row + 1, cell.col - 1))
                                        {
                                            tetris[cell.row + 1, cell.col - 1].down = null;
                                        }

                                    }
                                    else
                                    {
                                        tetris[cell.row, cell.col - 1].right = null;
                                        if (tetrisExists(cell.row, cell.col))
                                        {
                                            tetris[cell.row, cell.col].left = null;
                                        }

                                    }
                                }
                                else
                                {
                                    tetris[cell.row - 1, cell.col - 1].right = null;
                                    if (tetrisExists(cell.row - 1, cell.col))
                                    {
                                        tetris[cell.row - 1, cell.col].left = null;
                                    }

                                }
                                //Debug.Log("(" + cell.row + "," + cell.col + ") upwards");
                            }
                        }
                    }
                }
            }

            if (cell.right != null)
            {
                if (tetrisExists(cell.row, cell.col + 1))
                {

                    if ((tetris[cell.row, cell.col + 1].up != null) && (tetris[cell.row, cell.col + 1].left != null))
                    {
                        if (tetrisExists(cell.row - 1, cell.col + 1))
                        {
                            if ((tetris[cell.row - 1, cell.col + 1].left != null))
                            {
                                if (getRandomBool())
                                {
                                    if (getRandomBool())
                                    {
                                        tetris[cell.row, cell.col + 1].up = null;
                                        if (tetrisExists(cell.row + 1, cell.col + 1))
                                        {
                                            tetris[cell.row + 1, cell.col + 1].down = null;
                                        }

                                    }
                                    else
                                    {
                                        tetris[cell.row, cell.col + 1].left = null;
                                        if (tetrisExists(cell.row, cell.col))
                                        {
                                            tetris[cell.row, cell.col].right = null;
                                        }

                                    }
                                }
                                else
                                {
                                    if (!tetris[cell.row - 1, cell.col + 1].ghostSpawn) {
                                        tetris[cell.row - 1, cell.col + 1].left = null;
                                        if (tetrisExists(cell.row - 1, cell.col))
                                        {
                                            tetris[cell.row - 1, cell.col].right = null;
                                        }
                                    }
                                }
                                //Debug.Log("(" + cell.row + "," + cell.col + ") downwards");
                            }
                        }

                    }
                }
            }
        }




        return restart;
    }

    void checkGhostDeadEnds(Cell cell)
    {
        if (tetrisExists(cell.row-1, cell.col))
        {
            if (tetris[cell.row-1, cell.col].ghostSpawn && tetris[cell.row, cell.col].right !=null)
            {
                tetris[cell.row, cell.col].up = null;
            }
        }
    }

    void wallConnections()
    {
        foreach (Cell cell in tetris)
        {
            if ((cell.row == tetris.GetLength(0) - 1) && cell.down != null)
            {
                attachAllDown(cell);
            }
            if ((cell.row == 0) && cell.up != null)
            {
                attachAllUp(cell);
            }
            if ((cell.col == 0) && cell.left != null)
            {
                attachAllLeft(cell);
            }
        }
    }




    void attachAllDown(Cell cell)
    {
        if (cell.col == 0 && tetris[cell.row, cell.col].left == null)
        {
            tetris[cell.row, cell.col].left = new Cell(-1, -1);
            attachAllLeft(tetris[cell.row, cell.col]);
        }


        if (tetris[cell.row, cell.col].left != null)
        {
            if (tetrisExists(cell.row, cell.col - 1))
            {
                if (tetris[cell.row, cell.col - 1].down == null)
                {
                    tetris[cell.row, cell.col - 1].down = new Cell(-1, -1);

                    attachAllDown(tetris[cell.row, cell.col - 1]);
                }
            }
        }

        if (tetris[cell.row, cell.col].right != null)
        {
            if (tetrisExists(cell.row, cell.col + 1))
            {
                if (tetris[cell.row, cell.col + 1].down == null)
                {
                    tetris[cell.row, cell.col + 1].down = new Cell(-1, -1);
                    attachAllDown(tetris[cell.row, cell.col + 1]);
                }
            }
        }
    }


    void attachAllUp(Cell cell)
    {

        if (cell.col == 0 && tetris[cell.row, cell.col].left == null)
        {
            tetris[cell.row, cell.col].left = new Cell(-1, -1);
            attachAllLeft(tetris[cell.row, cell.col]);
        }

        if (tetris[cell.row, cell.col].left != null)
        {
            if (tetrisExists(cell.row, cell.col - 1))
            {
                if (tetris[cell.row, cell.col - 1].up == null)
                {
                    tetris[cell.row, cell.col - 1].up = new Cell(-1, -1);


                    attachAllUp(tetris[cell.row, cell.col - 1]);
                }
            }
        }

        if (tetris[cell.row, cell.col].right != null)
        {
            if (tetrisExists(cell.row, cell.col + 1))
            {
                if (tetris[cell.row, cell.col + 1].up == null)
                {
                    tetris[cell.row, cell.col + 1].up = new Cell(-1, -1);
                    attachAllUp(tetris[cell.row, cell.col + 1]);
                }
            }
        }
    }


    void attachAllLeft(Cell cell)
    {
        if (cell.row == 0 && tetris[cell.row, cell.col].up == null)
        {
            tetris[cell.row, cell.col].up = new Cell(-1, -1);
            attachAllUp(tetris[cell.row, cell.col]);
        }


        if (tetris[cell.row, cell.col].up != null)
        {
            if (tetrisExists(cell.row - 1, cell.col))
            {
                if (tetris[cell.row - 1, cell.col].left == null)
                {
                    tetris[cell.row - 1, cell.col].left = new Cell(-1, -1);

                    attachAllLeft(tetris[cell.row - 1, cell.col]);
                }
            }
        }
        if (cell.row == tetris.GetLength(0) - 1 && tetris[cell.row, cell.col].down == null)
        {
            tetris[cell.row, cell.col].down = new Cell(-1, -1);
            attachAllDown(tetris[cell.row, cell.col]);
        }

        if (tetris[cell.row, cell.col].down != null)
        {
            if (tetrisExists(cell.row + 1, cell.col))
            {
                if (tetris[cell.row + 1, cell.col].left == null)
                {
                    tetris[cell.row + 1, cell.col].left = new Cell(-1, -1);

                    attachAllLeft(tetris[cell.row + 1, cell.col]);
                }
            }
        }
    }

    private string[,] setPlayerSpawn(string[,] maze)
    {
        bool done = false;
        System.Random rnd = new System.Random(DateTime.Now.Millisecond);
        while (!done)
        {
            int row = rnd.Next(maze.GetLength(0));
            int col = rnd.Next(maze.GetLength(1));
            if (maze[row,col] == "00")
            {
                maze[row, col] = "70";
                done = true;
            }
        }
        return maze;
    }

    public void writeMaze(string path)
    {
        StreamWriter fileStream = File.CreateText(path);
        string[,] maze = getMaze();
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
}