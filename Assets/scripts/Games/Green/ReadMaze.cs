using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ReadMaze : MonoBehaviour
{
    
    [SerializeField]
    private float scale = 1f, offsetX = 0f, offsetY = 0f, offsetZ = 0f;
    [SerializeField]
    private string filePath = "";

    [SerializeField]
    private Transform free = null, wall = null, innerCurve = null, outerCurve = null, fill = null, pacManSpawn = null, ghostSpawn = null,
                      floor = null, parent = null, player=null;


    //private Vector3 playerSpawn = null;
    private ArrayList enemySpawns = new ArrayList();
    private ArrayList blocks = new ArrayList();

    private System.Random rnd = new System.Random();

    private Vector3 offsetVector;

    // Start is called before the first frame update
    void Start()
    {
        filePath = "./Assets/Scripts/Games/Green/" + filePath + ".txt";
        offsetVector = new Vector3(offsetX, offsetY, offsetZ);
        make();

    }

    void Update()
    {
        
    }

    

    private void make()
    {
        parent.localScale =new Vector3( 1, 1, 1);
        var maze = fileToArray();

        for (int row = 0; row < maze.GetLength(0); row++)
        {
            for (int col = 0; col < maze.GetLength(1); col++)
            {
                Draw(row, col, maze[row, col]);
            }
        }

        makeFloor(maze.GetLength(0), maze.GetLength(1));
        parent.localScale = new Vector3(scale, scale, scale);

        if(TryGetComponent<Interactable>(out Interactable interactable))
        {
            interactable.editColliderRadius();
        }


        placeEnemies();
    }

    // Update is called once per frame
    private void Draw(int row, int col, string entry)
    {
        int.TryParse(entry, out int number);
        int type = (number / 10);
        int rotation = (number % 10);
        Transform obj = null;
        float[] rotate = new float[3] { 0, 0, 0 };
        float[] move = new float[3] { 0, 0, 0 };

        if (type == 0)
        {
            obj = free;
        }
        else if (type == 1)
        {
            obj = wall;

        }
        else if (type == 4)
        {
            obj = innerCurve;
        }
        else if (type == 5)
        {
            obj = outerCurve;
        }
        else if (type == 6)
        {
            obj = fill;
        }
        else if (type == 7)
        {
            obj = pacManSpawn;
        }
        else if (type == 8)
        {
            obj = ghostSpawn;
        }



        var position = new Vector3((parent.position.x + row), (parent.position.y), (parent.position.z + col + move[2]));

        position += offsetVector;
        var insert = Instantiate(obj, position, Quaternion.identity) as Transform;
        insert.SetParent(parent);
        insert.Rotate(rotate[0], rotate[1], rotate[2]);
        blocks.Add(insert);

        if (rotation == 2)
        {
            insert.Rotate(0.0f, 180.0f, 0.0f, Space.World);
        }
        else if (rotation == 3)
        {
            insert.Rotate(0.0f, 90.0f, 0.0f, Space.World);
        }
        else if (rotation == 4)
        {
            insert.Rotate(0.0f, 270.0f, 0.0f, Space.World);

        }

        if (type == 7)
        {
            player.position = new Vector3(insert.position.x, insert.position.y+1, insert.position.z);
        }
        else if(type == 8){
            enemySpawns.Add(new Vector3(insert.position.x, insert.position.y + 1, insert.position.z));
        }

    }




    private void makeFloor(int rows, int cols)
    {
        var insert = Instantiate(floor,(parent.transform.position + offsetVector), Quaternion.identity) as Transform;
        insert.Translate(-0.5f, -1, (cols - 0.5f), Space.World);
        insert.localScale = new Vector3(rows, insert.localScale.y, cols);
        insert.SetParent(parent);
        blocks.Add(insert);
    }

    private void placeEnemies()
    {
        /*for (int i=0; i<enemies.Length; i++)
        {
            placeEnemy( ((Transform)enemies[i]));
        }*/
    }

    private void placeEnemy( Transform enemy)
    {
        /*int r = rnd.Next(enemySpawns.Count);
        enemy.position = ( (Vector3) (enemySpawns[r]));*/
    }



    private string[,] fileToArray()
    {
        StreamReader reader = new StreamReader(filePath);
        var allText = reader.ReadToEnd();
        //Debug.Log(reader.ReadToEnd());
        reader.Close();

        var lines = allText.Split(new char[] { '\n' });
        int rows = lines.Length;
        var cols = lines[0].Split(' ').Length;
        var maze = new string[rows, cols];

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                maze[row, col] = lines[row].Split(' ')[col];
                //Debug.Log(lines[row]);
            }
        }

        return maze;
    }
}
