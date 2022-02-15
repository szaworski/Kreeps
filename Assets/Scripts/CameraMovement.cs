using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private Camera camera;

    void Start()
    {
        camera = Camera.main;
    }

    void Update()
    {
        WasdCameraMovement();
        CameraScrolling();
    }


    public void WasdCameraMovement()
    {
        float xAxis = Input.GetAxis("Horizontal");
        float yAxis = Input.GetAxis("Vertical");
        Vector3 applyMovement = new Vector3(xAxis, yAxis, 0);
        transform.position += applyMovement;
    }

    public void CameraScrolling()
    {
        float scrollSpeed = 10f;

        if (Input.GetAxisRaw("Mouse ScrollWheel") > 0 || Input.GetAxisRaw("Mouse ScrollWheel") < 0)
        {
            if (camera.orthographic)
            {
                camera.orthographicSize -= Input.GetAxis("Mouse ScrollWheel") * scrollSpeed;
                camera.orthographicSize = Mathf.Clamp(camera.orthographicSize, 2f, 12f);
            }
        }
    }
}
