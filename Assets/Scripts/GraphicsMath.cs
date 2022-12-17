using System;
using UnityEngine;

public class GraphicsMath : MonoBehaviour
{
    
    // return v1 + v2 (vectors MUST be of the same dimensions)
    private Vector2 AddVector(Vector2 v1, Vector2 v2)
    {
        float xAddition = v1.x + v2.x;
        float yAddition = v1.y + v2.y;
        return new Vector2(xAddition, yAddition);
    }

// return v1 - v2 (anti-commutative: order is important)
    private Vector2 SubVector(Vector2 v1, Vector2 v2)
    {
        float xSubtracted = v1.x - v1.x;
        float ySubtracted = v1.y - v1.y;
        return new Vector2(xSubtracted, ySubtracted);
    }

    private float GetVectorMagnitude(Vector2 v1)
    {
        float xSquared = v1.x * v1.x;
        float ySquared = v1.y * v1.y;
        // dirty use of a cast use to the way built-in Math.Sqrt works (it returns a double)
        return (float)Math.Sqrt(xSquared + ySquared); // Can we use the built in Sqrt method?
    }

    private Vector2 NormaliseVector(Vector2 v1)
    {
        float xNormalised = v1.x / GetVectorMagnitude(v1);
        float yNormalised = v1.y / GetVectorMagnitude(v1);
        return new Vector2(xNormalised, yNormalised);
    }

    private Vector2 MultiplyByScalar(Vector2 v1, int scalar)
    {
        float xMultiplied = v1.x * scalar;
        float yMultiplied = v1.y * scalar;
        return new Vector2(xMultiplied, yMultiplied);
    }

    // return the dot product of v1 & v2
    private float CalculateDot(Vector2 v1, Vector2 v2)
    {
        // The dot product is the projection of one vector on another
        // I can use it to work out if a vector is facing the same way as another, and if so, by how much
        float dotX = v1.x * v2.x;
        float dotY = v1.y * v2.y;
        return dotX + dotY;
    }

    private Vector3 CalculateCross(Vector3 v1, Vector3 v2)
    {
        // Cross Product returns a new Vector3 perpendicular to both v1 and v2
        // https://www.mathsisfun.com/algebra/vectors-cross-product.html
        float crossX = (v1.y * v2.z) - (v1.z * v2.y);
        float crossY = (v1.z * v2.x) - (v1.x * v2.z);
        float crossZ = (v1.x * v2.y) - (v1.y * v2.x);
        
        return new Vector3(crossX, crossY, crossZ);
    }
}
