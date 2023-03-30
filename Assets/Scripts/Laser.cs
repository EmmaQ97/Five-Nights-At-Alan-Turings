// Paul Calande 2016

using UnityEngine;
using System.Collections;

public class Laser : MonoBehaviour
{
    Vector3 moveVector = 10*Vector3.down;
    float cutoff;

    private void Awake()
    {
        cutoff = -transform.localScale.y - 50;
    }

	void Update ()
    {
        transform.position += moveVector;
        // If the laser is underground...
        if (transform.position.y < cutoff)
        {
            // Destroy it.
            Destroy(gameObject);
        }
	}
}
