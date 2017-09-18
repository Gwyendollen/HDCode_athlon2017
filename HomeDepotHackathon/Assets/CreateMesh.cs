using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateMesh : MonoBehaviour {
    public GameObject cube;
    public GameObject cylinder;
    public Transform originPoint;
    int matrixSize = 10;
    ArrayList renderList;
    // Use this for initialization
    void Start () {
        renderList = new ArrayList();
	}

    //Creates an analysis of the blobs. Gives a good idea of average size of each blob
    ArrayList blobAnalysis(ArrayList blobs) {
        int meanSize = 0;
        int averagePixelCount = 0;
        foreach (Blob blob in blobs) {
            averagePixelCount += blob.pixelCount;
            meanSize += Mathf.RoundToInt(blob.getArea());
        }
        meanSize = meanSize / blobs.Count;
        averagePixelCount = averagePixelCount / blobs.Count;

        print(averagePixelCount);
        ArrayList tempList = new ArrayList();
        foreach (Blob blob in blobs) {
            print(blob.pixelCount);
            if (blob.pixelCount > 0 && averagePixelCount > 0 && (Mathf.Max(blob.pixelCount, averagePixelCount) / Mathf.Min(blob.pixelCount, averagePixelCount)) > 0.6f) {
                tempList.Add(blob);
            } 
        }

        print("The Average Pixel Count is: " + averagePixelCount);
        return tempList;
    }
	

	// Update is called once per frame
	void Update () {
		
	}

    public void createMesh(ArrayList blobs) {
        foreach (GameObject obj in renderList) {
            Destroy(obj);
        }
        renderList.Clear();
        blobs = blobAnalysis(blobs);
        //Creates the base mesh for the Braille to stand on
        GameObject baseCube = Instantiate(cube, originPoint.position, Quaternion.identity);
        baseCube.transform.Translate(new Vector3(-DisplayTexture.targetWidth/2, -DisplayTexture.targetHeight/2, 0));
        baseCube.transform.localScale = new Vector3(DisplayTexture.targetWidth, DisplayTexture.targetHeight, 0);

        foreach (Blob blob in blobs) {
            GameObject cyl = Instantiate(cylinder, blob.A, Quaternion.identity);
            cyl.transform.localScale = new Vector3(14, 14, 3);
            renderList.Add(cyl);
        }

        //Index through the blob list to determine the position of the rounded braille
    }
}
