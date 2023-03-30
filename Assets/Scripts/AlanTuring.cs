// Paul Calande 2016

using UnityEngine;
using System.Collections;

/*
 * As the nights progress, Alan Turing starts to use new techniques to try to murder the player.
 * 
 * Night 1: Turing charges towards the player.
 * Night 2: Spikes rise from the ground beneath the player.
 * Night 3: Search light.
 * Night 4: Homing missile.
 * Night 5: Lasers randomly spawn across the map from the sky.
 */

public class AlanTuring : MonoBehaviour
{
    int timeForCharge = 500;
    int timeForSpike = 1000;
    int timeForLaser = 3;

    // A reference to the player duck.
    GameObject duck;
    int chargeTimer;
    float chargeSpeed = 0.7f;
    Vector3 chargeVector = Vector3.zero;
    DayNightCycle dnc;
    AudioSource aud;
    GameObject spikes;
    AudioClip spikeWarning;
    int night;
    int spikeTimer;
    int laserTimer;
    GameObject laser;
    GameObject missile;
    // Whether a missile exists.
    [System.NonSerialized]
    public bool theresMissile = false;
    int rayCastLayerMask;
    GameObject searcherPrefab;
    int numberOfSearchers = 0;
    int maxNumberOfSearchers;

    private void Start()
    {
        duck = GameObject.Find("Duck");
        dnc = GameObject.Find("Sun").GetComponent<DayNightCycle>();
        spikes = (GameObject)Resources.Load("Prefabs/Spikes");
        aud = GameObject.Find("AudioController").GetComponent<AudioSource>();
        spikeWarning = (AudioClip)Resources.Load("Sound Effects/TempleBell");
        laser = (GameObject)Resources.Load("Prefabs/Laser");
        missile = (GameObject)Resources.Load("Prefabs/Missile");
        searcherPrefab = (GameObject)Resources.Load("Prefabs/Searcher");

        night = dnc.GetNightNumber();

        rayCastLayerMask = ~LayerMask.GetMask("Turing", "Player");

        // Start with a lower charge time and faster charge speed in the later nights.
        timeForCharge -= 50 * (night - 1);
        chargeSpeed += 0.125f * (night - 1);
        chargeTimer = timeForCharge;

        timeForSpike -= 100 * (night - 1);
        spikeTimer = timeForSpike;

        maxNumberOfSearchers = night;

        laserTimer = timeForLaser;

        // Night 3 onwards has spotlights.
        if (night > 2)
        {
            while (numberOfSearchers < maxNumberOfSearchers)
            {
                ++numberOfSearchers;
                Vector3 searcherSpawnPoint = new Vector3(250f, -30f, 250f);
                Instantiate(searcherPrefab, searcherSpawnPoint, Quaternion.identity);
            }
        }
    }

    private void Update()
    {
        --chargeTimer;
        if (chargeTimer == 0)
        {
            chargeTimer = timeForCharge;

            transform.LookAt(duck.transform);
            
            Vector3 heading = duck.transform.position - transform.position + 15*Vector3.down;
            chargeVector = heading / heading.magnitude * chargeSpeed;
            
            if (timeForCharge > 100)
            {
                timeForCharge -= 2;
                chargeSpeed += 0.005f;
            }
        }
        transform.Translate(chargeVector, Space.World);
        // Night 2 onwards.
        if (night > 1)
        {
            --spikeTimer;
            if (spikeTimer == 0)
            {
                spikeTimer = timeForSpike;

                Vector3 targetpos = duck.transform.position;
                targetpos.y = -30f;
                // Spawn spikes at the duck's x and z coordinates, but always at the same height.
                // This makes the spikes spawn below the ground.
                GameObject hazard = (GameObject)Instantiate(spikes, targetpos, spikes.transform.rotation);
                // Make the hazard bigger depending on which night it is.
                hazard.GetComponent<Spikes>().SetScale(0.05f * night);
                // Play warning sound.
                aud.PlayOneShot(spikeWarning);

                if (timeForSpike > 200)
                {
                    timeForSpike -= 10;
                }
            }
        }
        // Night 3 onwards.
        /*
        if (night > 2)
        {

        }
        */
        // Night 4 onwards.
        if (night > 3)
        {
            Vector3 missileOrigin = transform.position + 15 * Vector3.up;
            Vector3 direction = duck.transform.position - missileOrigin;
            Ray ray = new Ray(missileOrigin, direction);
            //Debug.DrawRay(missileOrigin, direction, Color.blue, 0.1f, false);
            RaycastHit hit;
            // If there's no missile and if the path is clear between Alan Turing and the player...
            if (!theresMissile)
            {
                if (Physics.Raycast(ray, out hit, direction.magnitude, rayCastLayerMask))
                {
                    //Debug.Log("Raycast hit " + hit.transform.gameObject + " with tag " + hit.transform.tag);
                }
                else
                {
                    // Fire a missile.
                    theresMissile = true;
                    GameObject hazard = (GameObject)Instantiate(missile, missileOrigin, missile.transform.rotation);
                    hazard.GetComponent<Missile>().SetTuring(this);
                }
            }
        }
        // Night 5 onwards.
        if (night > 4)
        {
            --laserTimer;
            if (laserTimer == 0)
            {
                laserTimer = timeForLaser;
                float xpos = Random.Range(0f, 500f);
                float ypos = 2000f;
                float zpos = Random.Range(0f, 500f);
                Vector3 targetpos = new Vector3(xpos, ypos, zpos);
                Instantiate(laser, targetpos, laser.transform.rotation);
            }
        }
    }
}
