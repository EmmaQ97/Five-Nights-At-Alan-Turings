// Paul Calande 2016

// Commenting the following line will prevent Alan Turing from killing the Mushroom Boys.
//#define TURING_KILL_SHROOMS

using UnityEngine;
using System.Collections;

public class MushroomBoy : MonoBehaviour
{
    private Rigidbody rb;
    private int type;
    private PlayerMovement pm;
    private DayNightCycle dnc;
    private FlavorText ft;
    private Renderer ren;
    private AudioSource aud;
    private AudioClip clip;

    private const string PATH = "MushroomBoy/Materials/kinoko_";

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        pm = GameObject.Find("Duck").GetComponent<PlayerMovement>();
        dnc = GameObject.Find("Sun").GetComponent<DayNightCycle>();
        ft = GameObject.Find("FlavorTextUpper").GetComponent<FlavorText>();
        ren = GetComponent<Renderer>();
        aud = GameObject.Find("AudioController").GetComponent<AudioSource>();
        clip = (AudioClip)Resources.Load("Sound Effects/DragonBiteL");
    }

    private void Start()
    {
        const float PROB_BLUE = 0.3f;
        const float PROB_YELLOW = 0.2f;
        // PROB_RED is 1-PROB_BLUE-PROB_YELLOW; that is, the remaining probability.
        // Determine the type of Mushroom Boy based on random chance.
        float prob = Random.Range(0f, 1f);
        if (prob < PROB_BLUE)
        {
            type = 0;
        }
        else if (prob > 1f-PROB_YELLOW)
        {
            type = 1;
        }
        else
        {
            type = 2;
        }
        //type = Random.Range(0, 2);
        switch (type)
        {
            case 0: SetMaterial("blue"); break;
            case 1: SetMaterial("yellow"); break;
            case 2: SetMaterial("red"); break;
        }
    }

    private void SetMaterial(string color)
    {
        Material mat = (Material)Resources.Load(PATH + color);
        ren.material = mat;
    }

    void OnCollisionEnter(Collision other)
    {
        // Bounce upwards off the ground.
        const float BOUNCE_FORCE = 4.0f;
        rb.AddForce(new Vector3(0f, BOUNCE_FORCE, 0f), ForceMode.Impulse);
        // If we collide with the player...
        if (other.gameObject.CompareTag("Player"))
        {
            // Reward the player.
            CollectMushroomBoy();
            // Destroy self.
            Destroy(gameObject);
        }
#if TURING_KILL_SHROOMS
        // Alan Turing destroys Mushroom Boys upon contact.
        if (other.gameObject.CompareTag("Turing"))
        {
            Destroy(gameObject);
        }
#endif
    }

    private void FixedUpdate()
    {
        // Move around randomly.
        const float FORCE_SCALE = 10.0f;
        float compx = Random.Range(-FORCE_SCALE, FORCE_SCALE);
        float compz = Random.Range(-FORCE_SCALE, FORCE_SCALE);
        rb.AddForce(new Vector3(compx, 0f, compz));
    }

    void CollectMushroomBoy()
    {
        // Play a sound.
        aud.PlayOneShot(clip);
        // Do something based on the Mushroom Boy's type.
        switch (type)
        {
            case 0:
                // 0 = blue = increase duck movement speed by 1%
                pm.movementForce *= 1.01f;
                ft.Print("+1% movement speed increase!", Color.blue);
                break;
            case 1:
                // 1 = yellow = increase Sun's battery life by 1%
                dnc.timeForNight = (int)(dnc.timeForNight * 1.01f);
                ft.Print("+1% Sun battery life increase!", Color.yellow);
                break;
            case 2:
                // 2 = red = increase duck HP by 1
                pm.TakeHeal(1);
                ft.Print("+1 HP increase!", Color.red);
                break;
        }
    }
}
