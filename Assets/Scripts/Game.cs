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

//DontDestroyOnLoad test  


// LAst notes: Need to check how long Alek's computing matlab script takes and adjust "loading" time to match that of the CNN computing time to create
// the output script.
// also need to verify if "checkAnswers" function works correctly.

public class Game : MonoBehaviour
{

    public GameObject[] centres = new GameObject[6];
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
    public GameObject ObjectGrid;
    public GameObject Loading;
    public bool showWebcam = false;
    public string myGameState = "Menu"; // Game ,  Webcamview , Computing , Next , Next2
    public Webcam snapCam;

    bool temp = false;

    Timer timeValue;

    private string m_Path;
    private string[] lines;

    // Start is called before the first frame update
    void Start()
    {

        Initialize();
        HideObject(next);
        HideObject(exit);
        HideObject(webcam);
        HideObject(ObjectGrid);
        HideObject(Loading);

        Level();

        //Level();

    }

    // Update is called once per frame
    void Update()
    {
        if (timeValue.timeValue == 0 && myGameState == "Game")
        {
            myGameState = "Webcam";
            timeValue.timeValue = 10;
            ShowObject(webcam);
            ShowObject(ObjectGrid);
        }
        else if (timeValue.timeValue == 0 && myGameState == "Webcam")
        {
            myGameState = "Computing";
            HideObject(webcam);
            HideObject(ObjectGrid);

        }
        else if (timeValue.timeValue == 0 && myGameState == "Computing")
        {
            ShowObject(Loading);
            timeValue.timeValue = 4;
            myGameState = "Next";
          
        }
        
        if (timeValue.timeValue < 0.3f && myGameState == "Webcam")
        {
            snapCam.TakeSnapShot();
        }

        if(timeValue.timeValue == 0 && myGameState =="Next")
        {
            
            m_Path = Application.dataPath;
            print(m_Path);
            myGameState = "DoNothing";
            try
            {
                lines = System.IO.File.ReadAllLines(m_Path + "\\predictedShapes.txt");
            }
            catch
            {
                print("oops! file read error. try again");
            }



            for (int i = 0; i<level+3;i++)
            {
                Passed = true;
                if ((int.Parse(lines[5-i]) == objects[i]))
                {
                    print("good!");

                }
                else
                {
                    Passed = false;
                    Failed = true;
                    print(lines[5-i]);
                    print(objects[i]);
                    print(i);
                }
            }
/*        if(int.Parse(lines[2]) == objects[0] && int.Parse(lines[1]) == objects[1] && int.Parse(lines[0]) == objects[2] &&
           int.Parse(lines[5]) == objects[3] && int.Parse(lines[4]) == objects[4] && int.Parse(lines[3]) == objects[5])
            {
                Passed = true;
    
            }
        else
            {
                Failed = true;
            }*/
           /* print(lines[0]);
            print(lines[1]);
            print(lines[2]);
            print(lines[3]);
            print(lines[4]);
            print(lines[5]);*/
            //checkAnswers(lines);
        }



        if (Passed)
        {
            HideObject(Loading);
            ShowObject(next);
            HideObject(webcam);
            HideObject(ObjectGrid);
            Passed = false;

        }
        if (Failed)
        {
            HideObject(Loading);
            ShowObject(exit);
            HideObject(webcam);
            HideObject(ObjectGrid);
            Failed = false;
        }

    }


    public void checkAnswers( string[] answers)
    {
        Passed = true;
        for(int i=0; i<answers.Length; i++)

        {
            if (int.Parse(answers[i]) != objects[i])
            {
                Passed = false;
            }
        }
    }

    public void Initialize()
    {
        myGameState = "Game";
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
            objects[i] = Random.Range(0, shapeTypes );
        }
    }

    //Des: Creates number of objects, spaces them and color
    //Pre: Set numberOfObjects
    //Post: Deletes Gameobjects after set time
    void CreatePattern()
    {
        //float position = numberOfObjects * (-1.6f) + 1.6f;      //Object Spacing (Centering)
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

            shapes.transform.position = centres[i].transform.position;//new Vector3(position, 0, 0);
            shapes.AddComponent<MeshFilter>();
            shapes.AddComponent<MeshRenderer>().material.color = new Color(randomR / 255, randomG / 255, randomB / 255, 0f);
            shapes.GetComponent<MeshRenderer>().material.shader = Shader.Find("Unlit/Color");

            

            //position += 3.2f;       //Object Spacing
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
        myGameState = "Game";
        timeValue.timeValue = 10f;
        level++;
        Level();
        timeValue.timeValue = 5f +level*2;
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
                numberOfObjects = 4;
                Pattern();
                CreatePattern();
                break;
            case 2:
                numberOfObjects = 5;
                Pattern();
                CreatePattern();
                break;
            case 3:
                numberOfObjects = 6;
                Pattern();
                CreatePattern();
                break;

            default:
                numberOfObjects = 6;
                Pattern();
                CreatePattern();
                break;
        }
    }
}
