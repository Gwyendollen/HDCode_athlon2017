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


   	/**
   	 * Fuction Name: purge
   	 * 
   	 * Function: Will purge the background
   	 * Input: WebCamTexture 
   	 * Output: Color Array
	**/
    public static Color[] purge(WebCamTexture tex) {
		//create the array of pixel colors
        Color[] colArray = tex.GetPixels();
        int i = 0;
		//iterate through array turn into grayscale
        while (i < colArray.Length) {
            Color col = colArray[i];
			//grayscale formula r*.21 + g*.72 + b*.07
			//change 0 to 1 value to 0 to 255
            float y = ((col.r * 255) * 0.21f) + ((col.g * 255) * 0.72f) + ((col.b * 255) * 0.07f);
            //change values for writing to black
			if (y < 128) {
                colArray[i] = Color.black;
			} if ( r >= 128){
				colArray[i] = Color.red;
			}else {
				//change all other colors to white
                colArray[i] = Color.white;
            }
            i += 1;
        }
		//returns the grayscaled color array
        return colArray;
    }
}
