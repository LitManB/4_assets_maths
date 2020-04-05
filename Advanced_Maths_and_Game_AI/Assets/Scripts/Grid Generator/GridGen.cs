using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGen : MonoBehaviour
{
    public Vector2 GridSize;
    public GameObject Segment;
    public GridSection[,] grid;
    public float gridScale = 1f;
    public float noiseScale = 1f;
    public float noiseCutoff = 5f;
    public Material wallMat;

    public GameObject testTube;
    public GameObject enviroment;
    public GameObject rats;
    public GameObject bioAssets;

    public GameObject objectParent;
    public GameObject clusterManager;

    public Material pathMat;
    public bool showPath = false;
    public int targetX = 10;
    public int targetY = 15;


    private Texture2D perlinNoise;
    private AStar pathFinder;
    private KMeans cluster;
    private int objectNumber;
    private List<GameObject> objectList;

    void Start()
    {
        objectList = new List<GameObject>();
        cluster = clusterManager.GetComponent<KMeans>();
        grid = new GridSection[(int)GridSize.x, (int)GridSize.y];
        pathFinder = new AStar(grid);
        perlinNoise = CreateNoise();
        CreateGrid();
        CreateObjects();
        CreateCluster();


        if (showPath) //-------- makes path visable --------//
        {
            List<GridSection> path = pathFinder.FindPath(5, 4, targetX, targetY);

            foreach (GridSection p in path)
            {
                p.segment.transform.GetChild(0).GetComponent<MeshRenderer>().material = pathMat;
            }
        }


    }

    public void CreateGrid() //---------------------------------------Creates terrain----------------------------------//
    {
        for (int x = 0; x < GridSize.x; x++) //------loops in x------//
        {
            for (int y = 0; y < GridSize.y; y++) //-----loops in y------//
            {
                grid[x, y] = new GridSection(new Vector2(x, y)); //------------ creates the new girdSection ---------------//
                grid[x, y].segment = Instantiate(Segment, new Vector3(x * gridScale, 0, y * gridScale), Quaternion.identity); //----- spawn prefab -------//
                grid[x, y].segment.transform.localScale = new Vector3(gridScale, 1, gridScale);

                if (x == 0 || y == 0 || x == GridSize.x - 1 || y == GridSize.y - 1) //----if on border
                {
                    grid[x, y].segment.transform.localScale = new Vector3(gridScale, 25, gridScale); //------------------------- create wall border -----------//
                    grid[x, y].wall = true;
                }
                else
                {

                    float pixelValue = perlinNoise.GetPixel(x, y).r * 10;   //----------get pixel from perlin noise --------//

                    if (pixelValue == 0)
                        pixelValue = 1;

                    if(pixelValue <= noiseCutoff)  //---if above 5 = wall if below = floor ----//
                    {
                        pixelValue = 1;
                    }
                    else 
                    {
                        grid[x, y].wall = true;

                    }


                    grid[x, y].segment.transform.localScale = new Vector3(gridScale, pixelValue, gridScale); //------scales on y axis by pixels value to create walls -----//

                    if (grid[x, y].wall)
                    {
                        grid[x, y].segment.transform.GetChild(0).GetComponent<MeshRenderer>().material = wallMat;  //-----color of wall -------//
                    }


                }
            }
        }
    }

    public Texture2D CreateNoise() //-------------- creates and returns purlin noise texture -----//
    {
        Texture2D text = new Texture2D((int)GridSize.x, (int)GridSize.y);


        for (int x = 0; x < GridSize.x; x++)
        {
            for (int y = 0; y < GridSize.y; y++)  // ------ loops through pixels of texture and created noise based on maths function ------ //
            {
                float xPos = (float)x / GridSize.x * noiseScale;
                float yPos = (float)y / GridSize.y * noiseScale;
                float value = Mathf.PerlinNoise(xPos, yPos);
                Color c = new Color(value, value, value);
                text.SetPixel(x, y, c);
            }
        }

        text.Apply();
        return text;


    }

    public void CreateObjects()
    {
        for (int x = 0; x < GridSize.x; x++)
        {
            for (int y = 0; y < GridSize.y; y++)  // ---- loops through all grid section 
            {

                if (!grid[x, y].wall) // --- checks if not a wall 
                {
                    if (pathFinder.FindPath(5, 4, x, y) != null) // ----- checks if a path can be made between the player and grid section ------//
                    {

                        if (Random.Range(0, 100) <= 5)  // ----- each grid section has a 5% chance of spawing an object 
                        {
                            int randomNumber = Random.Range(0, 4);  //------- 25% chance o fbing each object 

                            GameObject tempObject = null;

                            if (randomNumber == 0)
                            {
                                tempObject = Instantiate(testTube, new Vector3(x * gridScale, 1, y * gridScale), Quaternion.identity, objectParent.transform); // spawn testTube 
                            }
                            else
                            if (randomNumber == 1)
                            {
                                tempObject = Instantiate(enviroment, new Vector3(x * gridScale, 1, y * gridScale), Quaternion.identity, objectParent.transform);// spawn enviroments
                            }
                            else
                            if (randomNumber == 2)
                            {
                                tempObject = Instantiate(rats, new Vector3(x * gridScale, 1, y * gridScale), Quaternion.Euler(-90,0,0), objectParent.transform);
                            }
                            else
                            if (randomNumber == 3)
                            {
                                tempObject = Instantiate(bioAssets, new Vector3(x * gridScale, 1, y * gridScale), Quaternion.Euler(-90, 0, 0), objectParent.transform);
                            }



                            objectList.Add(tempObject);
                            objectNumber++;

                        }
                    }
                }
            }
        }
    }

    public void CreateCluster() //--------------------------------- creates cluster and check if one of each items exists 
    {
        cluster.points = objectList;
        cluster.Points = objectNumber;
        cluster.newStart();

        if (cluster.finished && !cluster.CheckOneOfEach()) // -----------------------if it cant find one of each it starts again 
        {
            foreach (GameObject obj in objectList)
            {
                Destroy(obj);
            }
            objectList.Clear();
            objectNumber = 0;

            CreateObjects();
            CreateCluster();

        }
    }
 
}
