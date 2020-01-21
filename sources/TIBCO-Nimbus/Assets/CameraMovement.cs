using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour
{
    CharacterController characterController;

    private float zoomSpeed = 2.0f;
    private float speed = 3.0f;

    public float minX = -360.0f;
    public float maxX = 360.0f;

    public float minY = -45.0f;
    public float maxY = 45.0f;

    public float sensX = 100.0f;
    public float sensY = 100.0f;

    float rotationY = 0.0f;
    float rotationX = 0.0f;

    private Vector3 moveDirection = Vector3.zero;

    int mode = 1;

    // Use this for initialization
    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (mode == 1)
        {
            // Zoom
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            //transform.Translate(0, scroll * zoomSpeed, scroll * zoomSpeed, Space.World);
            //transform.Translate(new Vector3(0, 0, scroll * zoomSpeed));
            transform.position += Vector3.forward *scroll * zoomSpeed;

            // Keys
            if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            {
                transform.position += Vector3.right * speed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            {
                transform.position += Vector3.left * speed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
            {
                transform.position += Vector3.forward * speed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
            {
                transform.position += Vector3.back * speed * Time.deltaTime;
            }

            // Watch around
            if (Input.GetMouseButton(0))
            {
                rotationX += Input.GetAxis("Mouse X") * sensX * Time.deltaTime;
                rotationY += Input.GetAxis("Mouse Y") * sensY * Time.deltaTime;
                rotationY = Mathf.Clamp(rotationY, minY, maxY);
                transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
                //transform.rotation = Quaternion.Euler(-rotationY, rotationX, 0);
            }
        } else if (mode==2)
        {
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
            moveDirection *= speed;
            characterController.Move(moveDirection * Time.deltaTime);
        }
    }
}