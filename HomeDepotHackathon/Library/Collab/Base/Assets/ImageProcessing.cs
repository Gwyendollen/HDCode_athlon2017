using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageProcessing : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
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
