using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateMesh : MonoBehaviour {
    public GameObject cube;
    public GameObject cylinder;
    public Transform originPoint;

	// Use this for initialization
	void Start () {
        //createMesh();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void createMesh() {
        //Creates the base mesh for the Braille to stand on
        GameObject baseCube = Instantiate(cube, originPoint.position, Quaternion.identity);
        baseCube.transform.Translate(new Vector3(0, 0, 0));
        baseCube.transform.localScale = new Vector3(12, 8, 0);

        //Index through the blob list to determine the position of the rounded braille
    }
}
