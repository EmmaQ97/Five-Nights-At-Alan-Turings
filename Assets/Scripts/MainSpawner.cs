// Paul Calande 2016

using UnityEngine;
using System.Collections;

public class MainSpawner : MonoBehaviour
{
    // Reference to PowerCube prefab.
    GameObject pc;
    // And another reference to MushroomBoy prefab.
    GameObject mb;

    DayNightCycle dnc;
    int timeSinceLastSpawn = 0;

    const int TIME_BETWEEN_POWER_CUBE_SPAWNS = 400;
    const int TIME_BETWEEN_MUSHROOM_BOY_SPAWNS = 40;

    private void Awake()
    {
        dnc = GameObject.Find("Sun").GetComponent<DayNightCycle>();
        pc = (GameObject)Resources.Load("Prefabs/PowerCube");
        mb = (GameObject)Resources.Load("Prefabs/MushroomBoy");
    }

    private void Update()
    {
        ++timeSinceLastSpawn;
        // If it's night time...
        if (dnc.isNight)
        {
            if (timeSinceLastSpawn >= TIME_BETWEEN_POWER_CUBE_SPAWNS)
            {
                timeSinceLastSpawn = 0;
                // Spawn power cube.
                SpawnObject(pc);
            }
        }
        // If it's day time...
        else
        {
            if (timeSinceLastSpawn >= TIME_BETWEEN_MUSHROOM_BOY_SPAWNS)
            {
                timeSinceLastSpawn = 0;
                // Spawn mushroom boy.
                SpawnObject(mb);
            }
        }
    }

    // Spawns a new object.
    // I strongly recommended to make sure it only spawns objects with Rigidbody components.
    public void SpawnObject(GameObject obj)
    {
        // Instantiate a new object.
        GameObject spawned = (GameObject)Instantiate(obj, transform.position, transform.rotation);
        //GameObject spawned = (GameObject)Instantiate(obj, Vector3.zero, Quaternion.identity);
        //Debug.Log("Spawned a thing.");
        // Apply a random force to this new object to send it in a random direction.
        const float RANGE_SCALE = 25f;
        float v1 = Random.Range(-RANGE_SCALE, RANGE_SCALE);
        //float v2 = Random.Range(-RANGE_SCALE, RANGE_SCALE);
        float v2 = 0f;
        float v3 = Random.Range(-RANGE_SCALE, RANGE_SCALE);
        spawned.GetComponent<Rigidbody>().AddForce(new Vector3(v1, v2, v3), ForceMode.Impulse);
    }
}
