using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DisplayTexture : MonoBehaviour {
    public Texture2D camTexture;
    public WebCamTexture webcamTex;
    public RawImage displayImage;
    Color[] buffer;

    static int targetWidth = 640;
    static int targetHeight = 480;

    //This boolean represents if a current feed is running or not 
    bool running = true;

	// Use this for initialization
	void Start () {
        initializeWebCam();
        buffer = new Color[targetWidth * targetHeight];
        Blob blob = new Blob(new Vector2(0, 10), new Vector2(10, 0));
        print(blob);
        blob.incorporatePoint(new Vector2(0,0));
        print(blob);
	}

    //Initialize the webcam
    public void initializeWebCam() {
        print("There are currently " + WebCamTexture.devices.Length + " Webcams avilable on this computer");
        webcamTex = new WebCamTexture();
        webcamTex.requestedWidth = targetWidth;
        webcamTex.requestedHeight = targetHeight;
        webcamTex.Play();
    }

    // Update is called once per frame
    Texture2D bufferTex;
	void Update () {
        Destroy(bufferTex);
        Color[] cols = webcamTex.GetPixels();
        if (running) {
            bufferTex = new Texture2D(targetWidth, targetHeight, TextureFormat.ARGB32, false);
            bufferTex.SetPixels(cols);
            bufferTex.Apply();
            displayImage.texture = bufferTex;
        }

        
        if (Input.GetKeyDown(KeyCode.Space)) {
            print("PROCESSING WILL BEGIN");
            print(Screen.width + " " + Screen.height);
            running = !running;
            Texture2D newTex = new Texture2D(targetWidth, targetHeight, TextureFormat.ARGB32, false);
            newTex.SetPixels(purge(webcamTex));
            newTex.Apply();
            displayImage.texture = newTex;
        }


	}

    //Takes in a webcam feed and returns a snapshot in the form of a Color array
    public static Color[] takeSnapshot(WebCamTexture webtex) {
        return webtex.GetPixels();
    }
    //Will purge the background
    //filterVal = The minimum greyscale value for to filter out pixels in the background
    public static Color[] purge(WebCamTexture tex, int filterVal = 128) {
        Color[] colArray = tex.GetPixels();
        int i = 0;
        while (i < colArray.Length) {
            Color col = colArray[i];
            float y = ((col.r * 255) * 0.21f) + ((col.g * 255) * 0.72f) + ((col.b * 255) * 0.07f);
            if (y < filterVal) {
                colArray[i] = Color.black;
            } else {
                colArray[i] = Color.white;
            }
            i += 1;
        }
        return colArray;
    }


    //TRANSFER TO IMAGE PROCESSING
    //Converts 2-dimensional coordinate into a one-dimensional coordinate
    public int convertToArrayPos(float x, float y) {

        return 0;
    }
}
