// Paul Calande 2016

using UnityEngine;
using System.Collections;

public class MouseAimCamera : MonoBehaviour
{
    // The speed at which the camera rotates. Higher = faster.
    private const float ROTATESPEED = 5f;

    // A quaternion keeping track of the camera rotation.
    public Quaternion rotation;

    // The object that the camera follows.
    private GameObject target;
    // Offset between the camera and the target.
    private Vector3 offset;
    // How far away the camera is from the target. Higher number = farther away.
    private const float OFFSET_SIZE = 2.5f;
    // Keeps track of the camera's yaw.
    private float yaw = 0.0f;

    void Start()
    {
        // Find the Duck and set it as the target.
        target = GameObject.Find("Duck");
        //Debug.Log(target.name);

        // Initialize the offset between the camera's position and the target's position.
        offset = new Vector3(0.0f, -0.5f, OFFSET_SIZE);
        //offset = target.transform.position - transform.position;
    }

    // LateUpdate() runs after the Update() function of all other objects.
    // It is recommended that you use LateUpdate() instead of Update() for camera scripts.
    void LateUpdate()
    {
        // Change the camera's yaw based on the mouse's x position.
        yaw += ROTATESPEED * Input.GetAxis("Mouse X");
        // Prepare a rotation based on the yaw.
        Vector3 direction = new Vector3(0, yaw, 0);
        rotation = Quaternion.Euler(direction);

        /*
        Vector3 targetPosition = target.transform.position;
        //Vector3 predictedPosition = targetPosition - (rotation * offset);
        RaycastHit hit;
        if (Physics.Raycast(targetPosition, direction, out hit, offset.magnitude, LayerMask.GetMask("Player")))
        {
            Debug.Log("Something is between the camera and the Player!");
            offset *= hit.distance / offset.magnitude;
        }
        */

        // Put the camera a short distance behind the target using the proper rotation.
        transform.position = target.transform.position - (rotation * offset);
        // Point the camera at the target.
        transform.LookAt(target.transform);
    }
}