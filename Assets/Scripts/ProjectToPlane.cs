using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.UI;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class ProjectToPlane : MonoBehaviour
{
    public enum ProjectionTypes
    {
        ORTHOGRAPHIC,
        PERSPECTIVE,
    }

    // constant values
    private const int TEXTURE_WIDTH = 256;
    private const int TEXTURE_HEIGHT = 256;
    private const int DRAW_PLANE_DIST = 5;
    private const float TOP = 2;
    private const float BOTTOM = -2;
    private const float LEFT = -1;
    private const float RIGHT = 1;
    private const float NEAR_PLANE = -1f;
    private const float FAR_PLANE = 1;

    // serialized fields to use in the Unity Editor
    [Header("Rendering Objects")]
    [SerializeField] Camera projectionCamera;
    [SerializeField] ProjectionTypes projectionType;
    //[SerializeField] MeshRenderer renderer;

    // public variables used by other scripts

    // private variables used by this script
    //private Vector4 cameraPositionOffset;
    //private Vector4 cameraRotationOffset;
    //private Matrix4x4 cameraProjectionMatrix;

    private Matrix4x4 orhthographixProjectionMatrix;
    private Matrix4x4 perspectiveProjectionMatrix;

    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    void Start()
    {
        BuildOrthoMatrix();
        BuildPespMatrix();
    }

    /// <summary>
    /// Called by the UI button press to have the projections take place (removing the need to press a key)
    /// </summary>
    public void ProjectToPlanes() 
    {
        // TODO: calculate cameras transform and rotation relative to world origin - for rotation when we add it
        //UpdateCameraRelativeToWorldOrigin();

        switch (projectionType)
        {
            case ProjectionTypes.ORTHOGRAPHIC:
                //tryRacast();
                OrthoganalProjection();
                break;

            case ProjectionTypes.PERSPECTIVE:
                PersectiveProjection();
                break;

            // Just drop through if the persepctive hasn't been set
            default:
                Debug.Log("No projection type set - nothing to do!");
                break;
        }

    } // end ProjectToPlanes

    /// <summary>
    /// Updates the vectors to be used in projection based on camera's current position and rotation
    /// </summary>
    /*private void UpdateCameraRelativeToWorldOrigin()
    {
        // make the camera's position a vector4 for multiplication as the scale
        cameraPositionOffset = new Vector4(projectionCamera.transform.position.x,
                                    projectionCamera.transform.position.y,
                                    projectionCamera.transform.position.z, 1);

        cameraRotationOffset = new Vector4(projectionCamera.transform.rotation.x,
                                    projectionCamera.transform.rotation.y,
                                    projectionCamera.transform.rotation.z, 1);

        // for now set up the camera projection matrix based on camera at world origin
        cameraProjectionMatrix = new Matrix4x4( new Vector4(cameraPositionOffset.x,  0, 0, 0),
                                                new Vector4(0, cameraPositionOffset.y, 0, 0),
                                                new Vector4(0, 0, cameraPositionOffset.z, 0),
                                                new Vector4(0, 0, 0, 1) );

    } // end UpdateCameraRelativeToWorldOrigin*/

    /// <summary>
    /// Creates an orthographic projection onto the plane for this object
    /// </summary>
    private void OrthoganalProjection()
    {
        // grab all the objects in the scene and store the transforms
        GameObject[] gameObjectsInScene = GameObject.FindGameObjectsWithTag("Projection");
        Vector3[] oldTransforms = new Vector3[gameObjectsInScene.Length];

        // go through each game object and remove the z component (orthaganol)
        for (int i = 0; i < gameObjectsInScene.Length; i++)
        {
            Vector3 oldPosition = gameObjectsInScene[i].transform.position;
            oldTransforms[i] = gameObjectsInScene[i].transform.position;

            // get the vector 3 position and make it a vector 4
            Vector4 updatedPosition = new Vector4(oldPosition.x, oldPosition.y, oldPosition.z, 1);

            // Transform to orthogonal perspective
            updatedPosition = GraphicsMath.MultiplyMatrix4ByVector4(orhthographixProjectionMatrix, updatedPosition);

            // remove the Z component
            updatedPosition = GraphicsMath.MultiplyMatrix4ByVector4(GraphicsMath.OrthogonalProjection4x4Matrix, updatedPosition);

            // add the plane distance from the camera
            updatedPosition.z += DRAW_PLANE_DIST;

            gameObjectsInScene[i].transform.position = updatedPosition;
        }

        // render the scene to our image
        useRenderTexture();

        // put all the objects back!
        for (int i = 0; i < gameObjectsInScene.Length; i++)
        {
             gameObjectsInScene[i].transform.position = oldTransforms[i];
        }
    }

    /// <summary>
    /// Creates a perspective projection onto the plane for this object
    /// </summary>
    private void PersectiveProjection()
    {
        // grab all the objects in the scene and store the transforms
        GameObject[] gameObjectsInScene = GameObject.FindGameObjectsWithTag("Projection");
        Vector3[] oldTransforms = new Vector3[gameObjectsInScene.Length];

        // go through each game object and remove the z component (orthaganol)
        for (int i = 0; i < gameObjectsInScene.Length; i++)
        {
            Vector3 oldPosition = gameObjectsInScene[i].transform.position;
            oldTransforms[i] = gameObjectsInScene[i].transform.position;

            // get the vector 3 position and make it a vector 4
            Vector4 updatedPosition = new Vector4(oldPosition.x, oldPosition.y, oldPosition.z, 1);

            updatedPosition = GraphicsMath.MultiplyMatrix4ByVector4(perspectiveProjectionMatrix, updatedPosition);

            gameObjectsInScene[i].transform.position = updatedPosition;
        }

        // render the scene to our image
        useRenderTexture();

        // put all the objects back!
        for (int i = 0; i < gameObjectsInScene.Length; i++)
        {
            gameObjectsInScene[i].transform.position = oldTransforms[i];
        }
    }

    private void useRenderTexture()
    {
        // create the texture and sprite that will hold that texture
        Texture2D tex = new Texture2D(TEXTURE_WIDTH, TEXTURE_HEIGHT, TextureFormat.ARGB32, false);
        Sprite sprite = Sprite.Create(tex, new Rect(0, 0, TEXTURE_WIDTH, TEXTURE_HEIGHT), Vector2.zero);
        GetComponent<Image>().sprite = sprite;

        // Initialize the render texture on the camera and render to that texture
        RenderTexture rt = new RenderTexture(TEXTURE_WIDTH, TEXTURE_HEIGHT, 24);
        projectionCamera.targetTexture = rt;
        projectionCamera.Render();

        // set the active render texture so we can use read pixels to grab the pixle data into our rectangle
        RenderTexture.active = rt;

        // create a rectangle to store the pixel data that gets stored in the rendered texture
        Rect rectReadPicture = new Rect(0, 0, TEXTURE_WIDTH, TEXTURE_HEIGHT);

        // Read pixels into our texture and apply them
        tex.ReadPixels(rectReadPicture, 0, 0);
        tex.Apply();

        // Clean up
        projectionCamera.targetTexture = null;
        RenderTexture.active = null;                // added to avoid errors 
        //DestroyImmediate(rt);

    }

    /// <summary>
    /// sets up the orthogonal matrix for this camera
    /// </summary>
    private void BuildOrthoMatrix()
    {
        orhthographixProjectionMatrix = new Matrix4x4(new Vector4((2 / (RIGHT - LEFT)), 0, 0, 0),
                                                      new Vector4(0, (2 / (TOP - BOTTOM)), 0, 0),
                                                      new Vector4(0, 0, (-2 / (FAR_PLANE - NEAR_PLANE)), 0),
                                                      new Vector4(-(RIGHT + LEFT) / (RIGHT - LEFT), -(TOP + BOTTOM)/ (TOP - BOTTOM), 
                                                                  -(FAR_PLANE + NEAR_PLANE) / (FAR_PLANE - NEAR_PLANE), 1));
        /*orhthographixProjectionMatrix = new Matrix4x4(new Vector4((2 / (RIGHT - LEFT)), 0, 0, -(RIGHT + LEFT) / (RIGHT - LEFT)),
                                                      new Vector4(0, (2 / (TOP - BOTTOM)), 0, -(TOP + BOTTOM) / (TOP - BOTTOM)),
                                                      new Vector4(0, 0, (-2 / (FAR_PLANE - NEAR_PLANE)), -(FAR_PLANE + NEAR_PLANE) / (FAR_PLANE - NEAR_PLANE)),
                                                      new Vector4(0, 0, 0, 1));*/
    }

    /// <summary>
    /// sets up the perspective matrix for this camera
    /// </summary>
    private void BuildPespMatrix()
    {
        // This one was set up as row oriented, not column
        /*perspectiveProjectionMatrix = new Matrix4x4(new Vector4((2 * NEAR_PLANE) / (RIGHT - LEFT), 0, 0, 0),
                                                      new Vector4(0, (2 * NEAR_PLANE) / (TOP - BOTTOM), 0, 0),
                                                      new Vector4(0, 0, -(FAR_PLANE + NEAR_PLANE) / (FAR_PLANE - NEAR_PLANE), -1),
                                                      new Vector4(-NEAR_PLANE * (RIGHT + LEFT) / (RIGHT - LEFT), -NEAR_PLANE * (TOP + BOTTOM) / (TOP - BOTTOM),
                                                                  (2 * FAR_PLANE * NEAR_PLANE) / (NEAR_PLANE - FAR_PLANE), 0));*/

        // We decided on column orientation was what worked - we think
        perspectiveProjectionMatrix = new Matrix4x4(new Vector4(-(2 * NEAR_PLANE) / (RIGHT - LEFT), 0, 0, NEAR_PLANE * (RIGHT + LEFT) / (RIGHT - LEFT)),
                                                      new Vector4(0, -(2 * NEAR_PLANE) / (TOP - BOTTOM), 0, NEAR_PLANE * (TOP + BOTTOM) / (TOP - BOTTOM)),
                                                      new Vector4(0, 0, (FAR_PLANE + NEAR_PLANE) / (FAR_PLANE - NEAR_PLANE), -(2 * FAR_PLANE * NEAR_PLANE) / (NEAR_PLANE - FAR_PLANE)),
                                                      new Vector4(0, 0, -1, 0));
    }

    // Originally we wanted to raycast to get the position of each pixel - this was the wrong approach but helped us learn what we were missing.
    /*private void tryRacast()
    {
        // grab the plane mesh renderer and set up the material
        Texture2D texture = new Texture2D(TEXTURE_WIDTH, TEXTURE_HEIGHT, TextureFormat.ARGB32, true);
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, TEXTURE_WIDTH, TEXTURE_HEIGHT), Vector2.zero);
        GetComponent<Image>().sprite = sprite;

        // starting with orthographic - basically creating an image as we go
        Debug.Log("Start projection - Othrographic");
        RaycastHit hitPixel;

        Vector3 forward = transform.TransformDirection(Vector3.forward);
        float currentXPos = projectionCamera.transform.position.x - (TEXTURE_WIDTH / 2000);
        float currentYPos = projectionCamera.transform.position.y - (TEXTURE_HEIGHT / 2000);

        // go through the texture image, pixel by pixel to get the value to draw and draw it.
        for (int row = 0; row < TEXTURE_HEIGHT; row++)
        {
            for (int col = 0; col < TEXTURE_WIDTH; col++)
            {
                Vector3 startPos = new Vector3(currentXPos, currentYPos, projectionCamera.transform.position.z);

                if (Physics.Raycast(startPos, forward, out hitPixel, 10))
                {
                    Renderer renderer = hitPixel.collider.GetComponent<MeshRenderer>();
                    Texture2D texture2D = renderer.material.mainTexture as Texture2D;
                    Vector2 pCoord = hitPixel.textureCoord;
                    pCoord.x *= texture2D.width;
                    pCoord.y *= texture2D.height;

                    Color pColor = texture2D.GetPixel((int)pCoord.x, (int)pCoord.y);
                    // TODO: Get the coordinate as a vector 3 for the hit pixel and use math functions to matrix manipulate to plane
                    *//*Texture2D tMap = (Texture2D)hitPixel.collider.GetComponent<Renderer>().sharedMaterial.mainTexture;

                    int x = Mathf.FloorToInt(hitPixel.point.x);
                    int y = Mathf.FloorToInt(hitPixel.point.y);

                    Color pColor = tMap.GetPixel(x, y);*//*
                    texture.SetPixel(row, col, pColor);
                }

                currentXPos += 0.001f;
                currentYPos += 0.001f;
            }
        }

        texture.Apply();
    }*/

}
