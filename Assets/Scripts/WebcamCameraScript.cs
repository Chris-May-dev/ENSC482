using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]

public class WebcamCameraScript : MonoBehaviour
{

    Camera snapCam;
    int resWidth = 1080, resHeight = 1080;


    void Awake()
    {
        snapCam = GetComponent<Camera>();
        if (snapCam.targetTexture == null)
        {
            snapCam.targetTexture = new RenderTexture(resWidth, resHeight, 24);
        }
        else
        {
            resWidth = snapCam.targetTexture.width;
            resHeight = snapCam.targetTexture.height;
        }
        snapCam.gameObject.SetActive(false);
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Sets Webcam Camera On
    public void CallTakeSnapshot()
    {
        snapCam.gameObject.SetActive(true);
    }

    string SnapShotName()
    {
        return string.Format("{0}/Snapshots/snap_{1}x{2}_.png",
            Application.dataPath,
            resWidth, resHeight,
            System.DateTime.Now.ToString("yyyy-MM-dd_hh-mm-ss"));
    }


    public void TakeSnapShot()
    {
        snapCam.gameObject.SetActive(true);
        Texture2D snapshot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
        snapCam.Render();
        RenderTexture.active = snapCam.targetTexture;
        snapshot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);         //Positioning for snapshot to be taken
        byte[] bytes = snapshot.EncodeToPNG();                                  //bytes for PNG file type
        string fileName = SnapShotName();
        System.IO.File.WriteAllBytes(fileName, bytes);
        Debug.Log("Snapshot taken");
        snapCam.gameObject.SetActive(false);
    }
}
