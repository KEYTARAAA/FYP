using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using Unity.AI.Navigation;
using UnityEditor;
//using UnityEngine.AI;

public class MakeMaze : MonoBehaviour
{
    [SerializeField]
    private int rows=34, cols=14;
    [SerializeField]
    private float scale = 1f, offsetX = 0f, offsetY = 0f, offsetZ = 0f;
    [SerializeField]
    private string filePath = "";

    [SerializeField]
    private Transform free = null, wall = null,
                      floor = null, parent = null, container = null, enemy = null, mazePlayer = null, worldPlayer, structure;
    [SerializeField]
    private ParticleSystem startingParticleSystem = null, endingParticleSystem = null;
    private GenMaze getMaze;

    private List<Transform> nodes = null;
    private Transform start, end;



    //private Vector3 playerSpawn = null;
    private ArrayList enemySpawns = new ArrayList();
    private List<Transform> blocks = new List<Transform>();

    private System.Random rnd = new System.Random();

    private Vector3 offsetVector;

    // Start is called before the first frame update
    void Start()
    {
        filePath = "./"+filePath+".txt";
        mazePlayer.gameObject.SetActive(false);
        enemy.gameObject.SetActive(false);
        rnd = new System.Random(DateTime.Now.Millisecond);
        offsetVector = new Vector3(offsetX, offsetY, offsetZ);
        make();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.F8))
        {
            remake();
            offsetVector = new Vector3(offsetX, offsetY, offsetZ);
        }
        if (Input.GetKey(KeyCode.F4))
        {
            GetComponentInChildren<NavMeshSurface>().BuildNavMesh();
        }
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
        nodes = new List<Transform>();
        parent.localScale =new Vector3( 1, 1, 1);
        getMaze = new GenMaze(rows, cols);
        getMaze.writeMaze(filePath);
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


        NavMeshSurface surface = structure.GetComponent<NavMeshSurface>();//.BuildNavMesh();
        surface.BuildNavMesh();
        placeParticipants();
    }

    // Update is called once per frame
    private void Draw(int row, int col, string entry)
    {
        bool node = true;
        //Debug.Log(entry);
        string[] split = entry.Split(',');
        bool.TryParse(split[0], out bool left);
        bool.TryParse(split[1], out bool right);
        bool.TryParse(split[2], out bool up);
        bool.TryParse(split[3], out bool down);
        bool.TryParse(split[4], out bool duplicateLeft);
        bool.TryParse(split[5], out bool duplicateRight);
        bool.TryParse(split[6], out bool duplicateUp);
        bool.TryParse(split[7], out bool duplicateDown);



        if (left)
        {
            //addWall(row, col, new Vector3(0, 0, 0), 0);
            addWall(row, col, new Vector3(0, 0, 0), duplicateLeft, node);
            node = false;
        }
        if (right)
        {
            //addWall(row, col, new Vector3(0, 180, 0), 0.125f);
            addWall(row, col, new Vector3(0, 180, 0), duplicateRight, node);
            node = false;
        }
        if (up)
        {
            //addWall(row, col, new Vector3(0, 90, 0) , 0);
            addWall(row, col, new Vector3(0, 90, 0), duplicateUp, node);
            node = false;
        }
        if (down)
        {
            //addWall(row, col, new Vector3(0, 270, 0), 0.125f);
            addWall(row, col, new Vector3(0, 270, 0), duplicateDown, node);
            node = false;
        }
    }

    private void addWall(int row, int col, Vector3 rotate, bool duplicate, bool node)
    {

        var position = new Vector3((parent.position.x + row), (parent.position.y+0.2f), (parent.position.z + col));

        position += offsetVector;
        Transform obj = wall;
        if (duplicate)
        {
            obj = free;
        }

        var insert = Instantiate(obj, position, Quaternion.identity) as Transform;

        if (node == true)
        {
            nodes.Add(insert);
        }

//        GameObjectUtility.SetStaticEditorFlags(insert.gameObject, StaticEditorFlags.NavigationStatic);
        insert.SetParent(container);


        if (row ==0 && col ==0 && obj!=free)
        {
        Renderer ren = insert.gameObject.GetComponentInChildren<Renderer>();
            ren.material.color = Color.yellow;
            start = insert;
            startingParticleSystem.gameObject.SetActive(true);
            startingParticleSystem.gameObject.transform.position = insert.position;
            startingParticleSystem.Play();
        }

        if (row == rows-1 && col == cols- 1 && obj != free)
        {
            Renderer ren = insert.gameObject.GetComponentInChildren<Renderer>();
            ren.material.color = Color.red;
            endingParticleSystem.gameObject.SetActive(true);
            endingParticleSystem.gameObject.transform.position = insert.position;
            endingParticleSystem.Play();
        }
        insert.Rotate(rotate[0], rotate[1], rotate[2]);
        blocks.Add(insert);

    }




    private void makeFloor(int rows, int cols)
    {
        floor.position = new Vector3((blocks[0].position.x - 0.55f), parent.position.y, (blocks[0].position.z) - 0.55f);
        floor.localScale = new Vector3(((float)(rows)), floor.localScale.y * 10,((float)( cols)) );
    }

    public void placeParticipants()
    {
        mazePlayer.position = start.position;
        enemy.position = start.position;
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

    public MazeCell[,] getGrid()
    {
        return getMaze.getGrid();
    }

    public List<Transform> getNodes()
    {
        return nodes;
    }
}
