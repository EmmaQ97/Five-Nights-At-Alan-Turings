using UnityEngine;
using System.Collections;

public class Spikes : MonoBehaviour {

    int thrustTimer = 100;

    AudioSource aud;
    AudioClip spikeThrust;

    Vector3 moveVector = Vector3.zero;

	private void Start ()
    {
        aud = GameObject.Find("AudioController").GetComponent<AudioSource>();
        spikeThrust = (AudioClip)Resources.Load("Sound Effects/StrangeMechanicalSound");
    }

    private void Update()
    {
        --thrustTimer;
        // After waiting for a bit...
        if (thrustTimer == 0)
        {
            // Play spike thrusting sound.
            aud.PlayOneShot(spikeThrust);
            // Set the move vector's y component.
            moveVector.y = 1;
        }
        // Once the spikes exist for long enough...
        if (thrustTimer == -1000)
        {
            Destroy(gameObject);
        }
        transform.position += moveVector;
    }

    public void BackOff()
    {
        moveVector.y = -1;
    }

    public void SetScale(float scale)
    {
        transform.localScale = new Vector3(scale, scale, transform.localScale.z);
    }
}
