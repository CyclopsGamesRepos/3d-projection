using System;
using UnityEngine;

public class GraphicsMath : MonoBehaviour
{
    public static Matrix4x4 OrthogonalProjection4x4Matrix = new Matrix4x4(new Vector4(1, 0, 0, 0),
                                                                          new Vector4(0, 1, 0, 0),
                                                                          new Vector4(0, 0, 0, 0),
                                                                          new Vector4(0, 0, 0, 1) );

    // return v1 + v2 (vectors MUST be of the same dimensions)
    public static Vector3 AddVector(Vector3 v1, Vector3 v2)
    {
        float xAddition = v1.x + v2.x;
        float yAddition = v1.y + v2.y;
        float zAddition = v1.z + v2.z;
        return new Vector3(xAddition, yAddition, zAddition);
    }

    // return v1 - v2 (anti-commutative: order is important)
    public static Vector3 SubVector(Vector3 v1, Vector3 v2)
    {
        float xSubtracted = v1.x - v1.x;
        float ySubtracted = v1.y - v1.y;
        float zSubtracted = v1.z - v2.z;
        return new Vector3(xSubtracted, ySubtracted, zSubtracted);
    }

    public static float GetVectorMagnitude(Vector3 v1)
    {
        float xSquared = v1.x * v1.x;
        float ySquared = v1.y * v1.y;
        float zSquared = v1.z * v1.z;
        // dirty use of a cast use to the way built-in Math.Sqrt works (it returns a double)
        return (float)Math.Sqrt(xSquared + ySquared + zSquared); // Can we use the built in Sqrt method?
    }

    public static Vector3 NormaliseVector(Vector3 v1)
    {
        float xNormalised = v1.x / GetVectorMagnitude(v1);
        float yNormalised = v1.y / GetVectorMagnitude(v1);
        float zNormalised = v1.z / GetVectorMagnitude(v1);
        return new Vector3(xNormalised, yNormalised, zNormalised);
    }

    public static Vector3 MultiplyByScalar(Vector3 v1, int scalar)
    {
        float xMultiplied = v1.x * scalar;
        float yMultiplied = v1.y * scalar;
        float zMultiplied = v1.z * scalar;
        return new Vector3(xMultiplied, yMultiplied, zMultiplied);
    }

    // return the dot product of v1 & v2
    public static float CalculateDot(Vector3 v1, Vector3 v2)
    {
        // The dot product is the projection of one vector on another
        // I can use it to work out if a vector is facing the same way as another, and if so, by how much
        float dotX = v1.x * v2.x;
        float dotY = v1.y * v2.y;
        float dotZ = v1.z * v2.z;
        return dotX + dotY + dotZ;
    }

    public static Vector3 CalculateCross(Vector3 v1, Vector3 v2)
    {
        // Cross Product returns a new Vector3 perpendicular to both v1 and v2
        // https://www.mathsisfun.com/algebra/vectors-cross-product.html
        float crossX = (v1.y * v2.z) - (v1.z * v2.y);
        float crossY = (v1.z * v2.x) - (v1.x * v2.z);
        float crossZ = (v1.x * v2.y) - (v1.y * v2.x);

        return new Vector3(crossX, crossY, crossZ);
    }

    public static void MutiplyMatricesByScalar(Matrix4x4 matrix, float scalar)
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

    /// <summary>
    /// Takes in a 4x4 matrix and multiplies it by a Vector 4
    /// </summary>
    /// <param name="squareMatrix">The matrix to be multipled</param>
    /// <param name="columnVector">The vector 4 as a column vector</param>
    /// <returns></returns>
    public static Vector4 MultiplyMatrix4x4ByVector4(Matrix4x4 squareMatrix, Vector4 columnVector)
    {

        float vecX = (squareMatrix.m00 * columnVector.x) + (squareMatrix.m01 * columnVector.y) + 
                        (squareMatrix.m02 * columnVector.z) + (squareMatrix.m03 * columnVector.w);
        float vecY = (squareMatrix.m10 * columnVector.x) + (squareMatrix.m11 * columnVector.y) +
                        (squareMatrix.m12 * columnVector.z) + (squareMatrix.m13 * columnVector.w);
        float vecZ = (squareMatrix.m20 * columnVector.x) + (squareMatrix.m21 * columnVector.y) +
                        (squareMatrix.m22 * columnVector.z) + (squareMatrix.m23 * columnVector.w);
        float vecW = (squareMatrix.m30 * columnVector.x) + (squareMatrix.m31 * columnVector.y) +
                        (squareMatrix.m32 * columnVector.z) + (squareMatrix.m33 * columnVector.w);

        Vector4 output = new Vector4(vecX, vecY, vecZ, vecW);
        return output;
    }

    /// <summary>
    /// Takes in a Vctor 4 and multiply it by a 4x4 matrix
    /// </summary>
    /// <param name="rownVector">The vector 4 as a column vector</param>
    /// <param name="squareMatrix">The matrix to be multipled</param>
    /// <returns></returns>
    public static Vector4 MultiplyVector4ByMatrix4x4(Vector4 rowVector, Matrix4x4 squareMatrix)
    {

        float vecX = (rowVector.x * squareMatrix.m00) + (rowVector.y * squareMatrix.m10) +
                        (rowVector.z * squareMatrix.m20) + (rowVector.w * squareMatrix.m30);
        float vecY = (rowVector.x * squareMatrix.m01) + (rowVector.y * squareMatrix.m11) +
                        (rowVector.z * squareMatrix.m21) + (rowVector.w * squareMatrix.m31);
        float vecZ = (rowVector.x * squareMatrix.m02) + (rowVector.y * squareMatrix.m12) +
                        (rowVector.z * squareMatrix.m22) + (rowVector.w * squareMatrix.m32);
        float vecW = (rowVector.x * squareMatrix.m03) + (rowVector.y * squareMatrix.m13) +
                        (rowVector.z * squareMatrix.m23) + (rowVector.w * squareMatrix.m33);

        Vector4 output = new Vector4(vecX, vecY, vecZ, vecW);
        return output;
    }

    public static Matrix4x4 MultiplyMatricesByMatrices(Matrix4x4 rowMatrix, Matrix4x4 columnMatrix)
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
    public static float getMatrix4x4Determinant(Matrix4x4 matrix)
    {
        // https://gamemath.com/book/matrixmore.html#determinant_arbitrary_size
        float m00 = matrix.m11 * (matrix.m22 * matrix.m33 - matrix.m23 * matrix.m32) +
                    matrix.m12 * (matrix.m23 * matrix.m31 - matrix.m21 * matrix.m33) +
                    matrix.m13 * (matrix.m21 * matrix.m32 - matrix.m22 * matrix.m31);

        float m01 = matrix.m10 * (matrix.m22 * matrix.m33 - matrix.m23 * matrix.m32) +
                    matrix.m12 * (matrix.m23 * matrix.m30 - matrix.m20 * matrix.m33) +
                    matrix.m13 * (matrix.m20 * matrix.m32 - matrix.m22 * matrix.m30);

        float m02 = matrix.m10 * (matrix.m21 * matrix.m33 - matrix.m23 * matrix.m31) +
                    matrix.m11 * (matrix.m23 * matrix.m30 - matrix.m20 * matrix.m33) +
                    matrix.m13 * (matrix.m20 * matrix.m31 - matrix.m21 * matrix.m30);

        float m03 = matrix.m10 * (matrix.m21 * matrix.m32 - matrix.m22 * matrix.m31) +
                    matrix.m11 * (matrix.m22 * matrix.m30 - matrix.m20 * matrix.m32) +
                    matrix.m12 * (matrix.m20 * matrix.m31 - matrix.m21 * matrix.m30);

        return (m00 - m01 + m02 - m03);
    }

    public static Matrix4x4 GetMatrix4X4Inverse(Matrix4x4 matrix)
    {
        // calculate the determinant and divide 1 by it 
        float inverseDeterminant = 1/getMatrix4x4Determinant(matrix);

        // https://gamemath.com/book/matrixmore.html#inverse
        //pos
        float c00 = (matrix.m11 * (matrix.m22 * matrix.m33 - matrix.m23 * matrix.m32) +
                     matrix.m12 * (matrix.m23 * matrix.m31 - matrix.m21 * matrix.m33) +
                     matrix.m13 * (matrix.m21 * matrix.m32 - matrix.m22 * matrix.m31) ) * inverseDeterminant;

        // neg
        float c01 = -(matrix.m10 * (matrix.m22 * matrix.m33 - matrix.m23 * matrix.m32) +
                      matrix.m12 * (matrix.m23 * matrix.m30 - matrix.m20 * matrix.m33) +
                      matrix.m13 * (matrix.m20 * matrix.m32 - matrix.m22 * matrix.m30) ) * inverseDeterminant;

        // pos
        float c02 = (matrix.m10 * (matrix.m21 * matrix.m33 - matrix.m23 * matrix.m31) +
                     matrix.m11 * (matrix.m23 * matrix.m30 - matrix.m20 * matrix.m33) +
                     matrix.m13 * (matrix.m20 * matrix.m31 - matrix.m21 * matrix.m30) ) * inverseDeterminant;

        // neg
        float c03 = -(matrix.m10 * (matrix.m21 * matrix.m32 - matrix.m22 * matrix.m31) +
                      matrix.m11 * (matrix.m22 * matrix.m30 - matrix.m20 * matrix.m32) +
                      matrix.m12 * (matrix.m20 * matrix.m31 - matrix.m21 * matrix.m30) ) * inverseDeterminant;

        // neg
        float c10 = -(matrix.m01 * (matrix.m22 * matrix.m33 - matrix.m23 * matrix.m32) +
                      matrix.m02 * (matrix.m23 * matrix.m31 - matrix.m21 * matrix.m33) +
                      matrix.m03 * (matrix.m21 * matrix.m32 - matrix.m22 * matrix.m33) ) * inverseDeterminant;

        // pos
        float c11 = (matrix.m00 * (matrix.m22 * matrix.m33 - matrix.m23 * matrix.m32) +
                     matrix.m02 * (matrix.m23 * matrix.m30 - matrix.m20 * matrix.m33) +
                     matrix.m03 * (matrix.m20 * matrix.m32 - matrix.m22 * matrix.m30) ) * inverseDeterminant;

        // neg
        float c12 = -(matrix.m00 * (matrix.m21 * matrix.m33 - matrix.m23 * matrix.m31) +
                      matrix.m01 * (matrix.m23 * matrix.m30 - matrix.m20 * matrix.m31) +
                      matrix.m03 * (matrix.m20 * matrix.m31 - matrix.m21 * matrix.m30) ) * inverseDeterminant;

        // pos
        float c13 = (matrix.m00 * (matrix.m21 * matrix.m32 - matrix.m22 * matrix.m31) +
                     matrix.m01 * (matrix.m22 * matrix.m30 - matrix.m20 * matrix.m32) +
                     matrix.m02 * (matrix.m20 * matrix.m31 - matrix.m21 * matrix.m30) ) * inverseDeterminant;

        // pos
        float c20 = (matrix.m01 * (matrix.m12 * matrix.m33 - matrix.m13 * matrix.m32) +
                     matrix.m02 * (matrix.m13 * matrix.m31 - matrix.m11 * matrix.m33) +
                     matrix.m03 * (matrix.m11 * matrix.m32 - matrix.m12 * matrix.m31) ) * inverseDeterminant;

        // neg
        float c21 = -(matrix.m00 * (matrix.m11 * matrix.m33 - matrix.m13 * matrix.m32) +
                      matrix.m02 * (matrix.m13 * matrix.m30 - matrix.m10 * matrix.m33) +
                      matrix.m03 * (matrix.m10 * matrix.m32 - matrix.m12 * matrix.m30) ) * inverseDeterminant;

        // pos
        float c22 = (matrix.m00 * (matrix.m11 * matrix.m33 - matrix.m13 * matrix.m31) +
                     matrix.m01 * (matrix.m13 * matrix.m30 - matrix.m10 * matrix.m33) +
                     matrix.m03 * (matrix.m10 * matrix.m31 - matrix.m11 * matrix.m30) ) * inverseDeterminant;

        // neg
        float c23 = -(matrix.m00 * (matrix.m11 * matrix.m32 - matrix.m12 * matrix.m31) +
                      matrix.m01 * (matrix.m12 * matrix.m30 - matrix.m10 * matrix.m32) +
                      matrix.m02 * (matrix.m10 * matrix.m31 - matrix.m11 * matrix.m30) ) * inverseDeterminant;

        // neg 
        float c30 = -(matrix.m00 * (matrix.m12 * matrix.m23 - matrix.m13 * matrix.m22) +
                      matrix.m02 * (matrix.m13 * matrix.m21 - matrix.m11 * matrix.m23) +
                      matrix.m03 * (matrix.m11 * matrix.m22 - matrix.m12 * matrix.m21) ) * inverseDeterminant;

        // pos
        float c31 = (matrix.m00 * (matrix.m11 * matrix.m23 - matrix.m13 * matrix.m21) +
                     matrix.m02 * (matrix.m13 * matrix.m20 - matrix.m10 * matrix.m23) +
                     matrix.m03 * (matrix.m10 * matrix.m21 - matrix.m11 * matrix.m20) ) * inverseDeterminant;

        // neg 
        float c32 = -(matrix.m00 * (matrix.m11 * matrix.m23 - matrix.m13 * matrix.m21) +
                      matrix.m01 * (matrix.m13 * matrix.m20 - matrix.m10 * matrix.m23) +
                      matrix.m03 * (matrix.m10 * matrix.m21 - matrix.m11 * matrix.m20) ) * inverseDeterminant;

        // pos
        float c33 = (matrix.m00 * (matrix.m11 * matrix.m22 - matrix.m12 * matrix.m21) +
                     matrix.m01 * (matrix.m12 * matrix.m20 - matrix.m10 * matrix.m22) +
                     matrix.m02 * (matrix.m10 * matrix.m21 - matrix.m11 * matrix.m20) ) * inverseDeterminant;

        // The classical adjoint of is the transpose of the matrix of cofactors
        // M = { [c00, c01, c02, c03],      adj M = M transpose = { [c00, c10, c20, c30],
        //       [c10, c11, c12, c13],                              [c01, c11, c21, c31],
        //       [c20, c21, c22, c23],                              [c02, c12, c22, c32],
        //       [c30, c31, c32, c33], }                            [c03, c13, c23, c33],

        return new Matrix4x4(new Vector4(c00, c10, c20, c30),
                              new Vector4(c01, c11, c21, c31),
                              new Vector4(c02, c12, c22, c32),
                              new Vector4(c03, c13, c23, c33));

    } // end GetMatrix4X4Inverse
}
