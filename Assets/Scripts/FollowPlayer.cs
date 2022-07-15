using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Follow the player
public class FollowPlayer : MonoBehaviour
{
    //Offset from player
    [SerializeField] private float playerzOffset;
    [SerializeField] private float playeryOffset;
    //Player to follow
    [SerializeField] private Transform player;
    //Rotate Speed
    [SerializeField] private float rotateSpeed;
    //Follow Speed
    [SerializeField] private float followSpeed;
    //Camera Floats
    private float mouseX;
    private float mouseY;
    //Camera Rotation Bounds
    [SerializeField] private float minPitch = 0.0f;
    [SerializeField] private float maxPitch = 90.0f;
    //Activation Bool
    private bool isActive = false;

    // Start is called before the first frame update
    void Start()
    {
        isActive = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isActive) return;
        //Get Mouse Input
        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");
    }
    //Late Update is called after update
    private void LateUpdate()
    {
        if (!isActive) return;
        //Rotate the camera around the player
        RotateCamera();

        //calculate speed and destination
        Vector3 destination = player.position + (transform.forward * playerzOffset)+ (transform.up * playeryOffset);
        float distance = followSpeed * Time.deltaTime;

        //Move the camera to the player
        transform.position = Vector3.Lerp(transform.position, destination, distance);
    }
    //Rotate the camera
    private void RotateCamera()
    {
        //Rotate the camera around the player
        //X Rotation
        float rotationAmountX = mouseX * rotateSpeed * Time.deltaTime;
        transform.RotateAround(player.position, Vector3.up, rotationAmountX);
        //Y Rotation
        float rotationAmountY = -mouseY * rotateSpeed * Time.deltaTime;
        transform.RotateAround(player.position, transform.right, rotationAmountY);

        //Clamp the camera rotation
        //Set the pitch
        Vector3 eulerRotation = transform.eulerAngles;
        eulerRotation.x = Mathf.Clamp(eulerRotation.x, minPitch, maxPitch);
        //fix the z rotation
        eulerRotation.z = 0;
        //Set the rotation
        transform.eulerAngles = eulerRotation;

    }
}
