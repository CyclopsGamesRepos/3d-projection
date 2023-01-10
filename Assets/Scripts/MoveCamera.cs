using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    [SerializeField] float speed = 2;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float xPos = (Input.GetAxis("Horizontal") * speed * Time.deltaTime);
        float yPos = (Input.GetAxis("Vertical") * speed * Time.deltaTime);

        transform.Translate(xPos, yPos, 0);

    }
}
