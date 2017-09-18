using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Contains the functionality for the blob
public class Blob : MonoBehaviour {
    public Vector2 A;
    public Vector2 B;
    public Vector2 C;
    public Vector2 D;

    public int pixelCount = 0;

    public Blob(Vector2 topCorner, Vector2 bottomCorner) {
        A = new Vector2(topCorner.x, topCorner.y);
        B = new Vector2(bottomCorner.x, topCorner.y);
        C = new Vector2(topCorner.x, bottomCorner.y);
        D = new Vector2(bottomCorner.x, bottomCorner.y);
    }
    //Determines if a point lies within
    public bool isInBox(int x, int y) {
        //A Case
        if (!(x >= A.x && y <= A.y)) {
            return false;
        }
        
        //B Case
        if (!(x <= B.x && y <= B.y)) {
            return false;
        }

        //C Case
        if (!(x >= C.x && y >= C.y)) {
            return false;
        }

        //D Case
        if (!(x <= D.x && y >= D.y)) {
            return false;
        }

        return true;

    }

    //Determines the distance from the nearest blob using pythagorean 
   public void incorporatePoint(Vector2 point, float threshold = 25) {
        int countX = 0;
        int countY = 0;

        if (isInBox(Mathf.RoundToInt(point.x), Mathf.RoundToInt(point.y))) {
            print("IS INSIDE BOX");
            return;
        }
        //Determines if the point is near the pixel
        if (point.x <= A.x) {
            countX = Mathf.RoundToInt(A.x - point.x);
        } else if (point.x >= B.x) {
            countX = Mathf.RoundToInt(point.x - B.x);
        }

        if (point.y >= A.y) {
            countY = Mathf.RoundToInt(point.y - A.y);
        } else if (point.y <= C.y) {
            countY = Mathf.RoundToInt(C.y - point.y);
        }

        float distance = Mathf.Sqrt(Mathf.Pow(countX, 2) + Mathf.Pow(countY, 2));
        print(distance);

        //If the pixel is within a certain threshold of the box, change the box to incorporate
        if (distance < threshold) {
            pixelCount += 1;
            fitToBoundingBox(point);
        }
    }
    
    //Stretches the bounding box to incorporate the new element
    public void fitToBoundingBox(Vector2 point) {
        if (point.x < A.x) {
            A = new Vector2(point.x, A.y);
            C = new Vector2(point.x, C.y);
        }

        if (point.x > B.x) {
            B = new Vector2(point.x, B.y);
            D = new Vector2(point.x, D.y);
        }


        if (point.y > A.y) {
            A = new Vector2(A.x, point.y);
            B = new Vector2(B.x, point.y);
        }

        if (point.y < C.y) {
            C = new Vector2(C.x, point.y);
            D = new Vector2(D.x, point.y);
        }
    }

    public override string ToString() {
        return "Current Blob Dimensions: A:" + A.ToString() + " |B:" + B.ToString() + " |C:" + C.ToString() + " |D:" + D.ToString(); 
    }
}
