// Paul Calande 2016

using UnityEngine;
using System.Collections;

public class PowerCube : MonoBehaviour {

    Rigidbody rb;
    DayNightCycle dnc;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        dnc = GameObject.Find("Sun").GetComponent<DayNightCycle>();
    }

    private void OnCollisionEnter(Collision other)
    {
        // Bounce upwards off of other objects.
        const int BOUNCE_FORCE = 10;
        rb.AddForce(new Vector3(0, BOUNCE_FORCE, 0), ForceMode.Impulse);
        // If the other object is the player...
        if (other.gameObject.CompareTag("Player"))
        {
            // Add the power cube's power to the Sun.
            dnc.CollectPowerCube();
            // Destroy self.
            Destroy(gameObject);
        }
    }
}
