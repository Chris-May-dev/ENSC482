/*
 Description: Creating Shapes using Mesh Generator
 
Square - 0
Circle - 1
Triangle - 2
Pentagon - 3
Star - 4
 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Sample : MonoBehaviour
{
    //Used in Square()
    float width = 1;
    float height = 1;
    int pos = 0;

    //ShapePoint Size
    private int radius = 1;

    int shapeTypes = 5;
    static int maxObjects = 20;
    int[] objects = new int[maxObjects];

    List<Mesh> meshList = new List<Mesh>();

    List<Vector3> verticePoints = new List<Vector3>();
    List<int> trianglePoints = new List<int>();


    //Mesh mesh = new Mesh();


    // Start is called before the first frame update
    void Start()
    {


        //Square();
        //Triangle();
        //ShapePoint(3, false);          //Triangle
        //ShapePoint(4, false);          //Diamond (Square)
        //ShapePoint(5, false);          //Pentagon
       // ShapePoint(4, false, 0);        //Circle
       // ShapePoint(3, false, 3);        //Circle
                                          // ShapePoint(10, true);         // Star
        Pattern(5);
        //Render();
    }

    // Update is called once per frame
    void Update()
    {
        Render();
        ShapePoint(3, false, 3);
    }

    void Square()
    {
        // Mesh mesh = new Mesh();
        Mesh mesh = new Mesh();

        Vector3[] vertices = new Vector3[4];

        //Size and Position
        vertices[0] = new Vector3(-width, -height);
        vertices[1] = new Vector3(-width, height);
        vertices[2] = new Vector3(width, height);
        vertices[3] = new Vector3(width, -height);

        mesh.vertices = vertices;

        mesh.triangles = new int[] { 0, 1, 2, 0, 2, 3 };

        //Add to Mesh Filter category to set material
        GetComponent<MeshFilter>().mesh = mesh;
        mesh.RecalculateBounds();
    }

    void Triangle()
    {
        //Mesh meshTriangle = new Mesh();
        Mesh mesh = new Mesh();
        Vector3[] vertices = new Vector3[3];

        vertices[0] = new Vector3(-width, -height);
        vertices[1] = new Vector3(pos, height / 2);
        vertices[2] = new Vector3(width, -height);

        mesh.vertices = vertices;

        mesh.triangles = new int[] { 0, 1, 2 };

        //Add to Mesh Filter category to set material
        GetComponent<MeshFilter>().mesh = mesh;
    }

    //Des: Create shapes Cirlce/Pentagon/Triangle/Diamond(Square)
    void ShapePoint(int points, bool isStar, int position)
    {
        Mesh mesh = new Mesh();
        //List<Vector3> verticePoints = new List<Vector3>();
        //List<int> trianglePoints = new List<int>();

        //Vertices - Position + Size
        for (int i = 0; i < points; i++)
        {
            float x = radius * Mathf.Sin(Mathf.Deg2Rad * (360 / ((float)points) * i));
            float y = radius * Mathf.Cos(Mathf.Deg2Rad * (360 / ((float)points) * i));

            //ISSUE - Star looks duuuuuumb - values check out, Unity doesn't show properly?
            //Star
            if (!isStar)
            {
                verticePoints.Add(new Vector3(x + position, y, 0f));
            }
            else
            {
                if (i % 2 != 0)
                {
                    verticePoints.Add(new Vector3((x + position) / 2f, y / 2f, 0f));

                }
                else
                {
                    verticePoints.Add(new Vector3(x + position, y, 0f));
                }
            }
        }

        //Triangles
        for (int i = 0; i < (points - 2); i++)
        {
            trianglePoints.Add(0);
            trianglePoints.Add(i + 1);
            trianglePoints.Add(i + 2);
        }

        //initialise
       // mesh.vertices = verticePoints.ToArray();
       // mesh.triangles = trianglePoints.ToArray();

       // meshList.Add(mesh.vertices = verticePoints.ToArray(), mesh.triangles = trianglePoints.ToArray());

        //GetComponent<MeshFilter>().mesh = mesh;

    }


    //Des: Randomly generating pattern
    void Pattern(int numberOfObjects)
    {
        for (int i = 0; i < numberOfObjects; i++)
        {
            objects[i] = Random.Range(0, shapeTypes + 1);
        }
    }

    void DisplayPattern()
    {
        for (int i = 0; i < objects.Length; i++)
        {
            if(objects[i] == 0)
            {
                //Square
                ShapePoint(4, false, 0);
            }
            else if (objects[i] == 1)
            {
                //Circle
                ShapePoint(100, false, 0);
            }
            else if (objects[i] == 2)
            {
                //Triangle
                ShapePoint(3, false, 0);
            }
            else if (objects[i] == 3)
            {
                //Pentagon
                ShapePoint(5, false, 0);
            }
            else
            {
                //Star
                ShapePoint(10, false, 0);
            }
        }
    }

    void Render()
    {
        Mesh mesh = new Mesh();
        mesh.vertices = verticePoints.ToArray();
        mesh.triangles = trianglePoints.ToArray();
        GetComponent<MeshFilter>().mesh = mesh;
    }

};
