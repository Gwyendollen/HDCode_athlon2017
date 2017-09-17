using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DisplayTexture : MonoBehaviour {
    public Texture2D camTexture;
    public WebCamTexture webcamTex;
    public Material displayImage;

    //This boolean represents if a current feed is running or not 
    bool running = true;

	// Use this for initialization
	void Start () {
        initializeWebCam(640, 480);
	}

    //Initialize the webcam
    public void initializeWebCam(int width, int height) {
        print("There are currently " + WebCamTexture.devices.Length + " Webcams avilable on this computer");
        webcamTex = new WebCamTexture();
        webcamTex.requestedWidth = width;
        webcamTex.requestedHeight = height;
        webcamTex.Play();
    }

	// Update is called once per frame
	void Update () {
        Color[] cols = webcamTex.GetPixels();
        if (running) {
            Texture2D newTex = new Texture2D(640, 480, TextureFormat.ARGB32, false);
            newTex.SetPixels(cols);
            newTex.Apply();
            displayImage.mainTexture = newTex;
        }

        if (Input.GetKeyDown(KeyCode.Space)) {
            print("PROCESSING WILL BEGIN");
            running = !running;
            Texture2D newTex = new Texture2D(640, 480, TextureFormat.ARGB32, false);
            newTex.SetPixels(purge(webcamTex));
            newTex.Apply();
            displayImage.mainTexture = newTex;
        }


	}

    //Will purge the background
    public static Color[] purge(WebCamTexture tex) {
        Color[] colArray = tex.GetPixels();
        int i = 0;
        while (i < colArray.Length) {
            Color col = colArray[i];
            float y = ((col.r * 255) * 0.21f) + ((col.g * 255) * 0.72f) + ((col.b * 255) * 0.07f);
            if (y < 128) {
                colArray[i] = Color.black;
            } else {
                colArray[i] = Color.white;
            }
            i += 1;
        }
        return colArray;
    }


}
