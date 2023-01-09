using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectToPlane : MonoBehaviour
{
    // constant values

    // serialized fields to use in the Unity Editor
    //[Header("Rendering Objects")]
    //[SerializeField] MeshRenderer renderer;

    // public variables used by other scripts

    // private variables used by this script
    private Material rendererMaterial;
    private Texture2D texture;

    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    void Start()
    {
        
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    void Update()
    {
        // when the space bar is pressed, take a snap shot of the current objects and project them to the plane based on the camera view
        if (Input.GetKeyDown(KeyCode.Space) )
        {
            // grab the plane mesh renderer and set up the material
            MeshRenderer planeRenderer = GetComponent<MeshRenderer>();
            rendererMaterial = planeRenderer.material;

            int textureHeight = 256;
            int textureWidth = 256;

            if (rendererMaterial.mainTexture != null)
            {
                // set up the texture to draw to
                textureHeight = rendererMaterial.mainTexture.height;
                textureWidth = rendererMaterial.mainTexture.width;
            }

            texture = new Texture2D(textureWidth, textureHeight, TextureFormat.ARGB32, true);

            // starting with orthographic - basically creating an image as we go
            Debug.Log("Start projection - Othrographic");

            float alpha = 1;
            float red = 0;
            float green = 0;
            float blue = 0;

            // go through the texture image, pixel by pixel to get the value to draw and draw it.
            for (int row = 0; row < textureHeight; row++)
            {
                for (int col = 0; col < textureWidth; col++)
                {
                    //texture.SetPixel(row, col, Color.blue);
                    texture.SetPixel(row, col, new Color(red, green, blue, alpha) );
                }

                red += (1.0f/256);
                //green += (1.0f/256);
                blue += (1.0f/256);
            }

            texture.Apply();

            rendererMaterial.mainTexture = texture;
            planeRenderer.material = rendererMaterial;
        }
        
    }
}
