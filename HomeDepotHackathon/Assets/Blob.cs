using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Contains the functionality for Blob (Feature) Detection
public class Blob  {
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
   public bool incorporatePoint(Vector2 point, float threshold = 10) {
        int countX = 0;
        int countY = 0;

        //Determines if the point is near the pixel
        if (point.x < A.x) {
            countX = Mathf.Abs(Mathf.RoundToInt(A.x - point.x));
        } else if (point.x > B.x) {
            countX = Mathf.Abs(Mathf.RoundToInt(point.x - B.x));
        }

        if (point.y > A.y) {
            countY = Mathf.Abs(Mathf.RoundToInt(point.y - A.y));
        } else if (point.y < C.y) {
            countY = Mathf.Abs(Mathf.RoundToInt(C.y - point.y));
        }

        float distance = Mathf.Abs(Mathf.Sqrt(Mathf.Pow(countX, 2) + Mathf.Pow(countY, 2)));

        //If the pixel is within a certain threshold of the box, change the box to incorporate
        if (Mathf.Abs(distance) < threshold) {
            pixelCount += 1;
            fitToBoundingBox(point);
            return true;
        } else {
            return false;
        }
    }

    public float getArea() {
        float ab = Vector2.Distance(A, B);
        float ac = Vector2.Distance(A, C);

        return Mathf.Pow((ab * ac), 2); 
    }
    
    //Stretches the bounding box to incorporate the new element
    public void fitToBoundingBox(Vector2 point) {
        if (point.x <= A.x) {
            A = new Vector2(point.x, A.y);
            C = new Vector2(point.x, C.y);
        }

        if (point.x >= B.x) {
            B = new Vector2(point.x, B.y);
            D = new Vector2(point.x, D.y);
        }


        if (point.y >= A.y) {
            A.y = point.y;
            B.y = point.y;
        }

        if (point.y <= C.y) {
            C.y = point.y;
            D.y = point.y;
        }
    }
    
    public Vector2 getCenter() {
        return new Vector2((B.x - A.x) / 2, (A.y - C.y) / 2);
    }

    public override string ToString() {
        return "Current Blob Dimensions: A:" + A.ToString() + " |B:" + B.ToString() + " |C:" + C.ToString() + " |D:" + D.ToString(); 
    }
}
