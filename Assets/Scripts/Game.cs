using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
Square - 0
Circle - 1
Triangle - 2
Pentagon - 3
Star - 4
*/

//DontDestroyOnLoad

public class Game : MonoBehaviour
{

    int shapeTypes = 5;
    static int maxObjects = 20;
    int[] objects = new int[maxObjects];
    int numberOfObjects = 19;
    public int level = 0;
    public bool Passed = false;
    public bool Failed = false;
    GameObject time;
    public GameObject exit;
    public GameObject next;
    public GameObject webcam;
    public bool showWebcam = false;

    public WebcamCameraScript snapCam;

    bool temp = false;

    Timer timeValue;


    // Start is called before the first frame update
    void Start()
    {
        Initialize();
        HideObject(next);
        HideObject(exit);
        HideObject(webcam);

        Level();

        //Level();

    }

    // Update is called once per frame
    void Update()
    {
        if (timeValue.timeValue == 0 && showWebcam == false)
        {
            timeValue.timeValue = 10;
            showWebcam = true;
            ShowObject(webcam);
        }
        else if (timeValue.timeValue == 0 && showWebcam == true)
        {
            HideObject(webcam);
            temp = true;
        }

        //THIS SHOULD CALL TAKESNAPSHOT() ONCE AND AN IMAGE SHOULD BE SAVED. NEED TO CHECK.
        if (timeValue.timeValue == 0 && showWebcam == true && temp == true)
        {
            snapCam.TakeSnapShot();
            temp = false;
        }

        if (Passed)
        {
            ShowObject(next);
            HideObject(webcam);
        }
        if (Failed)
        {
            ShowObject(exit);
            HideObject(webcam);
        }

    }

    public void Initialize()
    {
        time = GameObject.Find("Timer");
        timeValue = time.GetComponent<Timer>();
        timeValue.timeValue = 5f;
        exit = GameObject.Find("Exit Button");
        next = GameObject.Find("Next Button");
        webcam = GameObject.Find("Webcam");
        Passed = false;
    }

    public void ShowObject(GameObject obj)
    {
        obj.SetActive(true);
    }

    public void HideObject(GameObject obj)
    {
        obj.SetActive(false);
    }

    //Des: Exit Button returns to menu
    public void ReturnToMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    //Des: Randomly generating pattern
    //Pre: Set numberOfObjects < maxObjects
    void Pattern()
    {
        for (int i = 0; i < numberOfObjects; i++)
        {
            objects[i] = Random.Range(0, shapeTypes + 1);
        }
    }

    //Des: Creates number of objects, spaces them and color
    //Pre: Set numberOfObjects
    //Post: Deletes Gameobjects after set time
    void CreatePattern()
    {
        float position = numberOfObjects * (-1.6f) + 1.6f;      //Object Spacing (Centering)
        int points = 3;

        for (int i = 0; i < numberOfObjects; i++)       
        {
            GameObject shapes = new GameObject("Shape" + i);
            if (objects[i] == 0) //Square 
            {
                points = 4;
            }
            else if (objects[i] == 1) //Circle
            {
                points = 50;
            }
            else if (objects[i] == 2) //Triangle
            {
                points = 3;
            }
            else if (objects[i] == 3) //Pentagon
            {
                points = 5;
            }
            else //Star
            {
                points = 10;
            }

            //points = 10;                    //Debug Star

            shapes.AddComponent<Shape>().points = points;

            if (points == 10)
            {
                shapes.GetComponent<Shape>().isStar = true;
            }

            float randomR = Random.Range(50, 256);
            float randomG = Random.Range(50, 256);
            float randomB = Random.Range(50, 256);

            shapes.transform.position = new Vector3(position, 0, 0);
            shapes.AddComponent<MeshFilter>();
            shapes.AddComponent<MeshRenderer>().material.color = new Color(randomR / 255, randomG / 255, randomB / 255, 0f);
            shapes.GetComponent<MeshRenderer>().material.shader = Shader.Find("Unlit/Color");

            

            position += 3.2f;       //Object Spacing
            DestroyObjectDelayed(shapes, 5);
        }
    }

    //Des: Destroys game object after set time
    void DestroyObjectDelayed(GameObject shapes, int delay)
    {
        Destroy(shapes, delay);
    }

    //Des: Next Button sets the next level
    public void NextLevel()
    {
        level++;
        Level();
        timeValue.timeValue = 5f;
        Passed = false;
        HideObject(next);
    }

    //Des: Sets level
    //Pre: set numberOfobjects
    public void Level()
    {
        switch (level)
        {
            case 0:
                numberOfObjects = 3;
                Pattern();
                CreatePattern();
                break;
            case 1:
                numberOfObjects = 5;
                Pattern();
                CreatePattern();
                break;
            case 2:
                numberOfObjects = 6;
                Pattern();
                CreatePattern();
                break;
            case 3:
                numberOfObjects = 7;
                Pattern();
                CreatePattern();
                break;
            case 4:
                numberOfObjects = 8;
                Pattern();
                CreatePattern();
                break;
            case 5:
                numberOfObjects = 9;
                Pattern();
                CreatePattern();
                break;
            case 6:
                numberOfObjects = 10;
                Pattern();
                CreatePattern();
                break;
            default:
                numberOfObjects = 11;
                Pattern();
                CreatePattern();
                break;
        }
    }
}
