using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drive : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {var vertical_input = Input.GetAxis( "Vertical" );

        var horizontal_input = Input.GetAxis("Horizontal");

        var forward = transform.forward;

        var upward = transform.up;

        var global_position = transform.position;

        var rotation = transform.rotation;

        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.SetPositionAndRotation(global_position +forward, rotation);
        }

        else if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.SetPositionAndRotation(global_position -forward, rotation);
        }


        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))

            transform.Rotate(0, horizontal_input*3, 0);

    }

    
}
