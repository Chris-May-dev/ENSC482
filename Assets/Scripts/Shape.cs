using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class Shape : MonoBehaviour
{
    private float radius = 1.5f;
    public int position = 0;
    public int points = 3;
    public bool isStar = false;


    // Start is called before the first frame update
    void Start()
    {
        ShapeMaker();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void Init(int position, int points, bool isStar)
    {
        this.position = position;
        this.points = points;
        this.isStar = isStar;
    }

    void ShapeMaker()
    {
        Mesh mesh = new Mesh();
        List<Vector3> verticePoints = new List<Vector3>();
        List<int> trianglePoints = new List<int>();

        //Vertices - Position + Size
        for (int i = 0; i < points; i++)
        {
            float x = radius * Mathf.Sin(Mathf.Deg2Rad * (360 / ((float)points) * i));
            float y = radius * Mathf.Cos(Mathf.Deg2Rad * (360 / ((float)points) * i));
            
            
            
            
            if (!isStar)
            {
                verticePoints.Add(new Vector3(x + position, y, 0f));
            }
            else
            {
                if (i % 2 != 0)
                {
                    verticePoints.Add(new Vector3(x / 2f + position, y / 2f, 0f));
                }
                else
                {
                    verticePoints.Add(new Vector3(x + position, y, 0f));
                }
            }
        }

        //Triangles
        if (isStar)
        {
            for (int i = 2; i < (points - 4); i++)   
            {
                trianglePoints.Add(0);
                trianglePoints.Add(i + 1);
                trianglePoints.Add(i + 2);
            }

            trianglePoints.Add(2);
            trianglePoints.Add(6);
            trianglePoints.Add(9);

            trianglePoints.Add(1);
            trianglePoints.Add(4);
            trianglePoints.Add(8);
        }
        else
        {
            for (int i = 0; i < (points - 2); i++)     
            {
                trianglePoints.Add(0);
                trianglePoints.Add(i + 1);
                trianglePoints.Add(i + 2);
            }
        }

        mesh.vertices = verticePoints.ToArray();
        mesh.triangles = trianglePoints.ToArray();

        GetComponent<MeshFilter>().mesh = mesh;
    }
}