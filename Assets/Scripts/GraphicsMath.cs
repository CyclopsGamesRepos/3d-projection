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

    private void MutiplyMatricesByScalar(Matrix4x4 matrix, float scalar)
    {
        matrix.m00 *= scalar;
        matrix.m01 *= scalar;
        matrix.m02 *= scalar;
        matrix.m03 *= scalar;
        matrix.m10 *= scalar;
        matrix.m11 *= scalar;
        matrix.m12 *= scalar;
        matrix.m13 *= scalar;
        matrix.m20 *= scalar;
        matrix.m21 *= scalar;
        matrix.m22 *= scalar;
        matrix.m23 *= scalar;
        matrix.m30 *= scalar;
        matrix.m31 *= scalar;
        matrix.m32 *= scalar;
        matrix.m33 *= scalar;
    }

    private Matrix4x4 MultiplyMatricesByMatrices(Matrix4x4 rowMatrix, Matrix4x4 columnMatrix)
    {
        // https://www.mathsisfun.com/algebra/matrix-multiplying.html
        // https://docs.unity3d.com/ScriptReference/Matrix4x4.html
        Matrix4x4 returnMatrix;
        
        // First row
        float tempSum1 = rowMatrix.m00 * columnMatrix.m00;
        float tempSum2 = rowMatrix.m01 * columnMatrix.m10;
        float tempSum3 = rowMatrix.m02 * columnMatrix.m20;
        float tempSum4 = rowMatrix.m03 * columnMatrix.m30;
        returnMatrix.m00 = tempSum1 + tempSum2 + tempSum3 + tempSum4;
        
        tempSum1 = rowMatrix.m00 * columnMatrix.m01;
        tempSum2 = rowMatrix.m01 * columnMatrix.m11;
        tempSum3 = rowMatrix.m02 * columnMatrix.m21;
        tempSum4 = rowMatrix.m03 * columnMatrix.m31;
        returnMatrix.m01 = tempSum1 + tempSum2 + tempSum3 + tempSum4;
        
        tempSum1 = rowMatrix.m00 * columnMatrix.m02;
        tempSum2 = rowMatrix.m01 * columnMatrix.m12;
        tempSum3 = rowMatrix.m02 * columnMatrix.m22;
        tempSum4 = rowMatrix.m03 * columnMatrix.m32;
        returnMatrix.m02 = tempSum1 + tempSum2 + tempSum3 + tempSum4;
        
        tempSum1 = rowMatrix.m00 * columnMatrix.m03;
        tempSum2 = rowMatrix.m01 * columnMatrix.m13;
        tempSum3 = rowMatrix.m02 * columnMatrix.m23;
        tempSum4 = rowMatrix.m03 * columnMatrix.m33;
        returnMatrix.m03 = tempSum1 + tempSum2 + tempSum3 + tempSum4;
        
        // Second row
        tempSum1 = rowMatrix.m10 * columnMatrix.m00;
        tempSum2 = rowMatrix.m11 * columnMatrix.m10;
        tempSum3 = rowMatrix.m12 * columnMatrix.m20;
        tempSum4 = rowMatrix.m13 * columnMatrix.m30;
        returnMatrix.m10 = tempSum1 + tempSum2 + tempSum3 + tempSum4;
        
        tempSum1 = rowMatrix.m10 * columnMatrix.m01;
        tempSum2 = rowMatrix.m11 * columnMatrix.m11;
        tempSum3 = rowMatrix.m12 * columnMatrix.m21;
        tempSum4 = rowMatrix.m13 * columnMatrix.m31;
        returnMatrix.m11 = tempSum1 + tempSum2 + tempSum3 + tempSum4;
        
        tempSum1 = rowMatrix.m10 * columnMatrix.m02;
        tempSum2 = rowMatrix.m11 * columnMatrix.m12;
        tempSum3 = rowMatrix.m12 * columnMatrix.m22;
        tempSum4 = rowMatrix.m13 * columnMatrix.m32;
        returnMatrix.m12 = tempSum1 + tempSum2 + tempSum3 + tempSum4;
        
        tempSum1 = rowMatrix.m10 * columnMatrix.m03;
        tempSum2 = rowMatrix.m11 * columnMatrix.m13;
        tempSum3 = rowMatrix.m12 * columnMatrix.m23;
        tempSum4 = rowMatrix.m13 * columnMatrix.m33;
        returnMatrix.m13 = tempSum1 + tempSum2 + tempSum3 + tempSum4;
        
        // Third row
        tempSum1 = rowMatrix.m20 * columnMatrix.m00;
        tempSum2 = rowMatrix.m21 * columnMatrix.m10;
        tempSum3 = rowMatrix.m22 * columnMatrix.m20;
        tempSum4 = rowMatrix.m23 * columnMatrix.m30;
        returnMatrix.m20 = tempSum1 + tempSum2 + tempSum3 + tempSum4;
        
        tempSum1 = rowMatrix.m20 * columnMatrix.m01;
        tempSum2 = rowMatrix.m21 * columnMatrix.m11;
        tempSum3 = rowMatrix.m22 * columnMatrix.m21;
        tempSum4 = rowMatrix.m23 * columnMatrix.m31;
        returnMatrix.m21 = tempSum1 + tempSum2 + tempSum3 + tempSum4;
        
        tempSum1 = rowMatrix.m20 * columnMatrix.m02;
        tempSum2 = rowMatrix.m21 * columnMatrix.m12;
        tempSum3 = rowMatrix.m22 * columnMatrix.m22;
        tempSum4 = rowMatrix.m23 * columnMatrix.m32;
        returnMatrix.m22 = tempSum1 + tempSum2 + tempSum3 + tempSum4;
        
        tempSum1 = rowMatrix.m20 * columnMatrix.m03;
        tempSum2 = rowMatrix.m21 * columnMatrix.m13;
        tempSum3 = rowMatrix.m22 * columnMatrix.m23;
        tempSum4 = rowMatrix.m23 * columnMatrix.m33;
        returnMatrix.m23 = tempSum1 + tempSum2 + tempSum3 + tempSum4;
        
        // Fourth row
        tempSum1 = rowMatrix.m30 * columnMatrix.m00;
        tempSum2 = rowMatrix.m31 * columnMatrix.m10;
        tempSum3 = rowMatrix.m32 * columnMatrix.m20;
        tempSum4 = rowMatrix.m33 * columnMatrix.m30;
        returnMatrix.m30 = tempSum1 + tempSum2 + tempSum3 + tempSum4;
        
        tempSum1 = rowMatrix.m30 * columnMatrix.m01;
        tempSum2 = rowMatrix.m31 * columnMatrix.m11;
        tempSum3 = rowMatrix.m32 * columnMatrix.m21;
        tempSum4 = rowMatrix.m33 * columnMatrix.m31;
        returnMatrix.m31 = tempSum1 + tempSum2 + tempSum3 + tempSum4;
        
        tempSum1 = rowMatrix.m30 * columnMatrix.m02;
        tempSum2 = rowMatrix.m31 * columnMatrix.m12;
        tempSum3 = rowMatrix.m32 * columnMatrix.m22;
        tempSum4 = rowMatrix.m33 * columnMatrix.m32;
        returnMatrix.m32 = tempSum1 + tempSum2 + tempSum3 + tempSum4;
        
        tempSum1 = rowMatrix.m30 * columnMatrix.m03;
        tempSum2 = rowMatrix.m31 * columnMatrix.m13;
        tempSum3 = rowMatrix.m32 * columnMatrix.m23;
        tempSum4 = rowMatrix.m33 * columnMatrix.m33;
        returnMatrix.m33 = tempSum1 + tempSum2 + tempSum3 + tempSum4;

        return returnMatrix;
    }
}
