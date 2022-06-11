using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Camera camera;
    [SerializeField] private float panSpeed = 5f;
    [SerializeField] private float scrollSpeed = 2.5f;
    [SerializeField] private Vector2 panLimit;
    [SerializeField] private Vector3 pos;

    void Start()
    {
        pos = transform.position;
        panLimit = new Vector2(250, 250);
        camera = Camera.main;
    }

    void Update()
    {
        WasdCameraMovement();
        CameraScrolling();
    }

    public void WasdCameraMovement()
    {
        //Move camera Up
        if (Input.GetKey("w"))
        {
            pos.y += panSpeed * Time.deltaTime;
        }
        //Move camera Down
        if (Input.GetKey("s"))
        {
            pos.y -= panSpeed * Time.deltaTime;
        }
        //Move camera Left
        if (Input.GetKey("a"))
        {
            pos.x -= panSpeed * Time.deltaTime;
        }
        //Move camera Right
        if (Input.GetKey("d"))
        {
            pos.x += panSpeed * Time.deltaTime;
        }
        //Keep the camera confined to the panLimit space
        pos.x = Mathf.Clamp(pos.x, -panLimit.x, panLimit.x);
        pos.y = Mathf.Clamp(pos.y, -panLimit.y, panLimit.y);
        //Move the camera position
        transform.position = pos;
    }

    public void CameraScrolling()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll > 0 || scroll < 0)
        {
            if (camera.orthographic)
            {
                camera.orthographicSize = Mathf.Clamp(camera.orthographicSize, 1.8f, 2.5f);
                camera.orthographicSize -= Input.GetAxis("Mouse ScrollWheel") * scrollSpeed;
            }
        }
    }
}
