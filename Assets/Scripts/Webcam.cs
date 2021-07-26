using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Webcam : MonoBehaviour
{
    WebCamTexture tex;
    int resWidth = 1080, resHeight = 1080;

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

    string SnapShotName()
    {
        return string.Format("{0}/Snapshots/snap_{1}x{2}_.png",
            Application.dataPath,
            resWidth, resHeight,
            System.DateTime.Now.ToString("yyyy-MM-dd_hh-mm-ss"));
    }

    public void TakeSnapShot()
    {
        Texture2D snap = new Texture2D(tex.width, tex.height);
        snap.SetPixels(tex.GetPixels());
        snap.Apply();

        byte[] bytes = snap.EncodeToPNG();                                  //bytes for PNG file type
        string fileName = SnapShotName();
        System.IO.File.WriteAllBytes(fileName, bytes);

        /*
        System.IO.File.WriteAllBytes(_SavePath + _CaptureCounter.ToString() + ".png", snap.EncodeToPNG());
        */

        print("Snapshot Taken");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
