using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DisplayTexture : MonoBehaviour {
    public Texture2D camTexture;
    public WebCamTexture webcamTex;
    public RawImage displayImage;
    Color[] buffer;

    ArrayList blobArray;
    public Text snapshotButton;

    static float filterSensitivity = 0.3f;

    public static int targetWidth = 320;
    public static int targetHeight = 240;

    //This boolean represents if a current feed is running or not 
    public bool running = true;

    //If set to true, will show a render of the braille
    bool showRender = false;

	// Use this for initialization
	void Start () {
        initializeWebCam();
        buffer = new Color[targetWidth * targetHeight];
        blobArray = new ArrayList();
        snapshotButton.text = "TAKE SNAPSHOT";
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
            processImage();
        }

	}

    public void changeSensitivity(Slider slider) {
        filterSensitivity = Mathf.RoundToInt(slider.value);
        if (running) {
            running = false;
            processImage();
        }
    }
    public void processImage() {
        print("PROCESSING WILL BEGIN");
        FindObjectOfType<UIManager>().toggleIntro(false);
        running = !running;

        if (running == false) {
            snapshotButton.text = "START OVER";
        } else {
            snapshotButton.text = "TAKE SNAPSHOT";
        }
        Texture2D newTex = new Texture2D(targetWidth, targetHeight, TextureFormat.ARGB32, false);
        buffer = blobExpansion(webcamTex.GetPixels(), targetWidth);
        //buffer = blobExpansion(getEdges(webcamTex.GetPixels(), 0.1f), targetWidth);
        //buffer = blobExpansion(getAverageSobel(webcamTex.GetPixels(), 2.5f), targetWidth);
        //buffer = getAverageSobel(webcamTex.GetPixels());
        newTex.SetPixels(buffer);
        newTex.Apply();
        displayImage.texture = newTex;
        GetComponent<CreateMesh>().createMesh(blobArray);
    }

    //Takes in a webcam feed and returns a snapshot in the form of a Color array
    public static Color[] takeSnapshot(WebCamTexture webtex) {
        return webtex.GetPixels();
    }



    //TRANSFER TO IMAGE PROCESSING
    //Converts 2-dimensional coordinate into a one-dimensional coordinate
    public int convertToArrayPos(float x, float y) {
        return Mathf.RoundToInt((y * targetWidth) + x);
    }

    //Converts one-dimensional array index into a 2-dimensional coordinate
    public Vector2 convertToPair(int index, int width) {
        int y = Mathf.RoundToInt(index / width);
        int x = index - (y * width);
        return new Vector2(Mathf.RoundToInt(x), y);
    }

    //Performs the growing blob detection algorithm on each pixel
    public Color[] blobExpansion(Color[] image, int width) {
        //Clears the results of the last blob scan
        blobArray.Clear();

        int j = 0;
        ArrayList tempBlobArray = new ArrayList();
        while (j < image.Length) {
            Color col = image[j];
            //grayscale formula r*.21 + g*.72 + b*.07
            //change 0 to 1 value to 0 to 255
            float y = col.grayscale;
            //change values for writing to black
            if (y < filterSensitivity) {
                image[j] = Color.black;

                bool expanded = false;
                //Converts the array index into a 2D coordinate
                Vector2 pair = convertToPair(j, width);


                ////Indexes through the list of current blobs to see if it is within one's proximity
                foreach (Blob blob in tempBlobArray) {
                    expanded = blob.incorporatePoint(pair, 3);
                    if (expanded) break;
                }
                //If there is no blob near it, create a new Blob at this point to expand.
                if (expanded == false) {
                    tempBlobArray.Add(new Blob(pair, pair));
                }
            } else {
                //change all other colors to white
                image[j] = Color.white;
            }

            
            j += 1;
        }
        //Factors out the blobs that have too few members, and too many members
        foreach (Blob blob in tempBlobArray) {
            //if (blob.pixelCount > 5 && blob.pixelCount < 400) {
                //blobArray.Add(blob);
                drawSquare(image, blob.A, blob.D);
            //}
        }

        blobArray = tempBlobArray;

        return image;
        
    }


    //Draws a Vertical Line from (x,y1) to (x,y2);
    public Color[] drawVerticalLine(Color[] image, int x, int y1, int y2) {
        if (y1 < 0) {
            y1 = 0;
        }
        if (y1 > targetHeight - 1) {
            y1 = targetHeight - 1;
        }

        if (y2 < 0) {
            y2 = 0;
        }
        if (y2 > targetHeight - 1) {
            y2 = targetHeight - 1;
        }

        if (y1 < y2) {
            int i = y1;
            while (i < y2) {
                int pos = convertToArrayPos(x, i);
                image[pos] = Color.yellow;
                i += 1;
            }
        }

        if (y1 > y2) {
            int i = y1;
            while (i > y2) {
                int pos = convertToArrayPos(x, i);
                image[pos] = Color.yellow;
                i -= 1;
            }
        }
        return image;
    }

    //Draws a horizontal line from (x1, y) to (x2, y)
    public Color[] drawHorizontalLine(Color[] image, int y, int x1, int x2) {
        if (x1 < 0) {
            x1 = 0;
        }
        if (x1 > targetHeight - 1) {
            x1 = targetHeight - 1;
        }

        if (x2 < 0) {
            x2 = 0;
        }
        if (x2 > targetHeight - 1) {
            x2 = targetHeight - 1;
        }
        if (x1 < x2) {
            int i = x1;
            while (i < x2) {
                int pos = convertToArrayPos(i, y);
                image[pos] = Color.yellow;
                i += 1;
            }
        }

        if (x1 > x2) {
            int i = x1;
            while (i > x2) {
                int pos = convertToArrayPos(i, y);
                image[pos] = Color.yellow;
                i -= 1;
            }
        }
        return image;
    }

    public void drawSquare(Color[] image, Vector2 topCorner, Vector2 bottomCorner) {
        drawHorizontalLine(image, (int)topCorner.y, (int)topCorner.x, (int)bottomCorner.x);
        drawHorizontalLine(image, (int)bottomCorner.y, (int)topCorner.x, (int)bottomCorner.x);

        drawVerticalLine(image, (int)topCorner.x, (int)topCorner.y, (int)bottomCorner.y);
        drawVerticalLine(image, (int)bottomCorner.x, (int)topCorner.y, (int)bottomCorner.y);

    }

    //Finds immediate discontinuities, using the standard outline Image Kernel Matrix:
    // -1   -1  -1
    // -1    8  -1
    // -1   -1  -1
    //
    public Color[] getEdges(Color[] image, float threshold) {
        Vector4 lastPixel = Vector4.zero;
        for (int x = 0; x < targetWidth; x++) {
            for (int y = 0; y < targetHeight; y++) {
                int jpos = convertToArrayPos(x, y);
                Color colorVector = image[jpos];
                if (x != 0 && Vector4.Distance(colorVector, lastPixel) > threshold) {
                    image[jpos] = Color.black;
                    
                } else {
                    image[jpos] = Color.white;
                }
                lastPixel = colorVector;
            }
        }
        return image;
    }
}
