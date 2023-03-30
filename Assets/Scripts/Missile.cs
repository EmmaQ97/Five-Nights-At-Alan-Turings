// Paul Calande 2016

using UnityEngine;
using System.Collections;

public class Missile : MonoBehaviour
{
    float moveSpeed = 25f;
    float accelSpeed = 0.001f;
    float rotSpeed = 3f;

    GameObject duck;
    AlanTuring turing;
    AudioSource aud;
    AudioClip explode;
    Rigidbody rb;

    float rot = 0f;

    private void Start()
    {
        duck = GameObject.Find("Duck");
        aud = GameObject.Find("AudioController").GetComponent<AudioSource>();
        explode = (AudioClip)Resources.Load("Sound Effects/GrenadeExplosion");
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // Face the player.
        Vector3 direction = duck.transform.position - transform.position;
        Quaternion newRotation = Quaternion.FromToRotation(Vector3.up, direction);
        // Rotate the missile around its axis for a cool spinning effect.
        newRotation *= Quaternion.Euler(Vector3.up * rot);
        // Apply the rotation to the rigidbody.
        rb.MoveRotation(newRotation);
        // Continuing rotating around the y-axis.
        rot += rotSpeed;
        if (rot > 360f)
        {
            rot -= 360f;
        }
        // Move towards the player.
        rb.MovePosition(transform.position + transform.up * moveSpeed * Time.deltaTime);
        // Accelerate.
        moveSpeed += accelSpeed;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerMovement mov = duck.GetComponent<PlayerMovement>();
            mov.TakeDamage(5);
            Rigidbody orb = duck.GetComponent<Rigidbody>();
            const int TOTAL_POWER = 200;
            orb.AddForce(rb.velocity.x * TOTAL_POWER, 0, rb.velocity.z * TOTAL_POWER);
            BlowUp();
        }
        else if (!other.gameObject.CompareTag("Turing"))
        {
            //Debug.Log("Missile hit "+other.gameObject.name+" with tag "+other.gameObject.tag);
            BlowUp();
        }
    }

    public void BlowUp()
    {
        Destroy(gameObject);
        aud.PlayOneShot(explode);
        turing.GetComponent<AlanTuring>().theresMissile = false;
    }

    public void SetTuring(AlanTuring at)
    {
        turing = at;
    }
}
