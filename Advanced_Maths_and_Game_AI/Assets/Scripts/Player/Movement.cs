using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    float speed = 2.5f;
    float jumpSpeed = 3.5f;
    bool forwards;
    Rigidbody rb;
    Collider collider;
    float distToGround;

    public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
    public RotationAxes axes = RotationAxes.MouseXAndY;
    float sensitivityX = 3.5f;
    float sensitivityY = 3.5f;

    public float minimumX = -360F;
    public float maximumX = 360F;

    public float minimumY = -60F;
    public float maximumY = 60F;

    float rotationY = 0F;

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        rb = GetComponent<Rigidbody>();
        collider = gameObject.GetComponent<Collider>();

        distToGround = collider.bounds.extents.y;
    }

    void Update()
    {
        Vector3 pos = transform.position;

        cameraRotation();

        
        if (Input.GetKey(KeyCode.S))
        {
            transform.position -= removeY(transform.forward) * Time.deltaTime * speed;
            forwards = false;
        }
        else
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += removeY(transform.forward) * Time.deltaTime * speed;
            forwards = true;
        }
        else
        forwards = false;

        if (Input.GetKey(KeyCode.D))
            transform.position += removeY(transform.right) * Time.deltaTime * speed;

        if (Input.GetKey(KeyCode.A))
            transform.position -= removeY(transform.right) * Time.deltaTime * speed;

        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
            rb.AddForce(new Vector3(0, jumpSpeed, 0), ForceMode.Impulse);


        if (Input.GetKeyDown(KeyCode.LeftShift) && forwards)
        {
            if (speed < 4)
                speed = 5f;
            else
                speed = 2.5f;
        }
        else
        if (!forwards)
            speed = 2.5f;
    }

    void cameraRotation()
    {
        if (axes == RotationAxes.MouseXAndY)
        {
            float rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityX;

            rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
            rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

            transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
        }
        else if (axes == RotationAxes.MouseX)
        {
            transform.Rotate(0, Input.GetAxis("Mouse X") * sensitivityX, 0);
        }
        else
        {
            rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
            rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

            transform.localEulerAngles = new Vector3(-rotationY, transform.localEulerAngles.y, 0);
        }
    }

    Vector3 removeY(Vector3 vec)
    {
        vec.y = 0;
        return vec;
    }

    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.1f);
    }

    private void OnTriggerEnter(Collider other) //--------------- destory object when in contact-----//
    {
        if (other.gameObject.tag == "testTube" || other.gameObject.tag == "rats" || other.gameObject.tag == "bioAssets")
        {
            Destroy(other.gameObject.GetComponent<Collider>());
            Destroy(other.gameObject);
        }
    }
}
