using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEditor;
using Unity.AI.Navigation;

public class GenerateRandomMap : MonoBehaviour
{
    [SerializeField]
    private float scale = 1f, offsetX = 0f, offsetY = 0f, offsetZ = 0f;
    [SerializeField]
    private string filePath = "";

    [SerializeField]
    private Transform free = null, wall = null, innerCurve = null, outerCurve = null, fill = null, pacManSpawn = null, ghostSpawn = null, exit = null,
                      floor = null, parent = null, container = null, mazePlayer = null, worldPlayer, enemies = null, fog = null, UI = null,
                      collectable = null, collectableParent = null, structure = null, sign = null, mainUI;

    private Transform spawn, exitTransform;
    int collectables=0;


    //private Vector3 playerSpawn = null;
    private List<Transform> enemySpawns = new List<Transform>();
    private ArrayList blocks = new ArrayList();

    private System.Random rnd = new System.Random();

    private Vector3 offsetVector;

    // Start is called before the first frame update
    void Start()
    {
        filePath = "./" + filePath + ".txt";
        rnd = new System.Random(DateTime.Now.Millisecond);
        offsetVector = new Vector3(offsetX, offsetY, offsetZ);
        make();
    }

    void Update()
    {
    }

    private void remake()
    {
        foreach (Transform block in blocks)
        {
            //GameObject block = ((GameObject)(obj));

            Destroy(block.gameObject);
        }

        blocks.Clear();
        make();
    }

    private void make()
    {
        parent.localScale =new Vector3( 1, 1, 1);
        var getMaze = new RandomMap();
        getMaze.writeMaze(filePath);
        var maze = fileToArray();

        for (int row = 0; row < maze.GetLength(0); row++)
        {
            for (int col = 0; col < maze.GetLength(1); col++)
            {
                Draw(row, col, maze[row, col], maze);
            }
        }

        makeFloor(maze.GetLength(0), maze.GetLength(1));
        parent.localScale = new Vector3(scale, scale, scale);

        if(TryGetComponent<Interactable>(out Interactable interactable))
        {
            interactable.editColliderRadius();
        }
        mazePlayer.GetComponent<GreenMazeCollection>().setGoal(collectables);
        NavMeshSurface surface = structure.GetComponent<NavMeshSurface>();//.BuildNavMesh();
        surface.BuildNavMesh();
        placeParticipants();
    }

    // Update is called once per frame
    private void Draw(int row, int col, string entry, string[,] maze)
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
        //GameObjectUtility.SetStaticEditorFlags(insert.gameObject, StaticEditorFlags.NavigationStatic);
        insert.SetParent(container);
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
            spawn = insert;
            mazePlayer.gameObject.SetActive(false);
        }
        else if(type == 0){
            enemySpawns.Add(insert);
            var c = Instantiate(collectable, new Vector3(insert.position.x, insert.position.y + .5f, insert.position.z), Quaternion.identity);
            c.SetParent(collectableParent);
            collectables++;
        }

        if (type == 8)
        {
            if (checkExit(row, col, maze))
            {
                exitTransform = Instantiate(exit, position, Quaternion.identity) as Transform;
                exitTransform.SetParent(container);
                exitTransform.Rotate(rotate[0], rotate[1], rotate[2]);
                exitTransform.GetComponent<GreenMazeExit>().setUp(enemies.gameObject, mazePlayer.gameObject, worldPlayer.gameObject, UI.gameObject, sign.gameObject, mainUI.gameObject);
                blocks.Add(exitTransform);
            }
            else if (!checkExit(row, col-3, maze))
            {
                enemySpawns.Add(insert);
            }
        }


    }

    private bool checkExit(int row, int col, string[,] maze)
    {
        if(maze[row + 1, col] == "80" && maze[row - 1, col] == "80" && maze[row, col-2] == "12")
        {
            return true;
        }
        return false;
    }




    private void makeFloor(int rows, int cols)
    {
        floor.position = (parent.transform.position + offsetVector);
        floor.Translate(-0.5f, -1, (cols - 0.5f), Space.World);
        floor.localScale = new Vector3(((float)(rows)) + 0.125f, floor.localScale.y * 10, ((float)(cols)));
    }

    public void placeParticipants()
    {
        mazePlayer.position = spawn.position;
        placeEnemies();
    }

    private void placeEnemies()
    {
        foreach (Transform enemy in enemies)
        {
            enemy.position = enemySpawns[rnd.Next(enemySpawns.Count)].position;
            enemy.GetComponent<GreenMazeDie>().setExit(exitTransform);
            if (enemy.TryGetComponent<GreenMazePathAI>(out GreenMazePathAI pather))
            {
                pather.setPath(enemySpawns);
            }
        }
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

    public List<Transform> getFreeZones()
    {
        return enemySpawns;
    }
}
