using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Webcam : MonoBehaviour
{
    WebCamTexture tex;
    //Establish grid of image seperation
    int numOfWid = 3;
    int numOfHigh = 2;

    // Start is called before the first frame update
    void Start()
    {
        WebCamDevice[] devices = WebCamTexture.devices;

        // for debugging purposes, prints available devices to the console
        for (int i = 0; i < devices.Length; i++)
        {
            print("Webcam available: " + devices[i].name);
        }

        Renderer rend = this.GetComponentInChildren<Renderer>();

        // assuming the first available WebCam is desired
        tex = new WebCamTexture(devices[0].name);
        rend.material.mainTexture = tex;
        tex.Play();
    }

    public Texture2D heightmap;
    public Vector3 size = new Vector3(100, 10, 100);

    // For saving to the _savepath
    //private string _SavePath = "C:/Users/aleks/OneDrive/Desktop/ENSC482/Assets/Snapshots"; //Change the path here!

    string SnapShotName(int picNumber)
    {
        return string.Format("{0}/Snapshots/snap_{1}.png",
            Application.dataPath,
            picNumber,
            System.DateTime.Now.ToString("yyyy-MM-dd_hh-mm-ss"));
    }

    public void TakeSnapShot()
    {
        //Under Development
        // Code to calculate a 3x4 grid and extract 12 individual images for classification
        // Note GetPixels(x,y,imageWidth,imageHeight), (x,y) = (0,0) is bottom left of image
        int imgWidth = tex.width;
        int imgHeight = tex.height;
        int gridWidth, gridHeight;
        int picNumber = 0;

        gridWidth = imgWidth / numOfWid;
        gridHeight = imgHeight / numOfHigh;

        int x = 0;
        int y = imgHeight - gridHeight;

        //Full Pic and declerations
        Texture2D snap = new Texture2D(tex.width, tex.height);
        snap.SetPixels(tex.GetPixels());
        snap.Apply();

        byte[] bytes = snap.EncodeToPNG();                                  //bytes for PNG file type
        string fileName = SnapShotName(0);
        System.IO.File.WriteAllBytes(fileName, bytes);

        //grid pics 1 - 12 taken
        for (int i = 0; i < numOfHigh; i++)
        {
            for (int k = 0; k < numOfWid; k++)
            {
                picNumber++;
                snap = new Texture2D(gridWidth, gridHeight);
                snap.SetPixels(tex.GetPixels(x, y, gridWidth, gridHeight));
                snap.Apply();

                bytes = snap.EncodeToPNG();
                fileName = SnapShotName(picNumber);
                System.IO.File.WriteAllBytes(fileName, bytes);
                x = x + gridWidth;

            }
            y = y - gridHeight;
            x = 0;
        }
        print("Snapshot Taken");

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
