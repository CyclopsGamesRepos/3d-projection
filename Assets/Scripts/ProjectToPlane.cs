using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.UI;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class ProjectToPlane : MonoBehaviour
{
    // constant values
    private const int TEXTURE_WIDTH = 128;
    private const int TEXTURE_HEIGHT = 128;

    // serialized fields to use in the Unity Editor
    [Header("Rendering Objects")]
    [SerializeField] Camera projectionCamera;
    //[SerializeField] MeshRenderer renderer;

    // public variables used by other scripts

    // private variables used by this script
    private float cameraZOffset;

    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    void Start()
    {
        cameraZOffset = projectionCamera.transform.position.z;
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    void Update()
    {
        // when the space bar is pressed, take a snap shot of the current objects and project them to the plane based on the camera view
        if (Input.GetKeyDown(KeyCode.Space) )
        {
            //tryRacast();
            OrthoganalProjection();
        }
        
    } // end Update

    void OrthoganalProjection()
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

            updatedPosition = GraphicsMath.MultiplyMatrix4ByVector4(GraphicsMath.OrthogonalProjection4x4Matrix, updatedPosition);

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

    public void useRenderTexture()
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

    public void tryRacast()
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
                    /*Texture2D tMap = (Texture2D)hitPixel.collider.GetComponent<Renderer>().sharedMaterial.mainTexture;

                    int x = Mathf.FloorToInt(hitPixel.point.x);
                    int y = Mathf.FloorToInt(hitPixel.point.y);

                    Color pColor = tMap.GetPixel(x, y);*/
                    texture.SetPixel(row, col, pColor);
                }

                currentXPos += 0.001f;
                currentYPos += 0.001f;
            }
        }

        texture.Apply();
    }

}
