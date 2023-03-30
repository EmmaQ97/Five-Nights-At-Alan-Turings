// Paul Calande 2016

using UnityEngine;
using System.Collections;

public class Searcher : MonoBehaviour
{
    // How far the spotlight can rotate from the origin.
    float maxAngle = 30f;
    // Keeps track of the spotlight's progress.
    float time = 0f;
    // How quickly the Searcher moves its spotlight.
    float timeStep = 0.001f;
    // How fast the Searcher moves horizontally.
    float moveSpeed = 0.1f;
    // The desired height that the Searcher will remain at.
    float targety = 80f;
    // The speed at which the Searcher rises to this height. Make sure it can divide targety evenly!
    float riseSpeed = 0.5f;

    float eulery;
    Vector3 destination;

    void Start ()
    {
        eulery = transform.eulerAngles.y;
        time = Random.Range(-1f, 1f);
        UpdateRotation();
        ChooseNewDestination();
    }
	
	void Update ()
    {
        time += timeStep;
        UpdateRotation();
        //Vector3 direction = transform.position - destination;
        Vector3 direction = destination - transform.position;
        if (direction.magnitude < moveSpeed)
        {
            ChooseNewDestination();
        }
        transform.position += Vector3.Normalize(direction) * moveSpeed;
        if (transform.position.y < targety)
        {
            transform.position += Vector3.up * riseSpeed;
        }
    }

    void UpdateRotation()
    {
        transform.eulerAngles = new Vector3(maxAngle * Mathf.Cos(7 * time), eulery, maxAngle * Mathf.Sin(3 * time));
    }

    void ChooseNewDestination()
    {
        float px = Random.Range(10f, 490f);
        float py = 80f;
        float pz = Random.Range(10f, 490f);
        destination = new Vector3(px, py, pz);
    }
}