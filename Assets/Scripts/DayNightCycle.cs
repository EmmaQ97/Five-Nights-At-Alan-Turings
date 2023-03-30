// Paul Calande 2016

// If the following line of code is not commented out, Mushroom Boys will not continue to exist at night.
//#define MUSH_NIGHT_KILL

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DayNightCycle : MonoBehaviour
{
    // The last night of the game.
    private const int FINAL_NIGHT = 5;

    // Whether it's night or not.
    [System.NonSerialized]
    public bool isNight = false;
    // The initial number of ticks it takes for night to arrive.
    [System.NonSerialized]
    public int timeForNight = 3000;
    // Timer that counts down for 0 for night time.
    [System.NonSerialized]
    public int time;
    // The number of power cubes needed to fully restore the Sun's power.
    public int powerCubesNeeded = 5;

    // Number of power cubes collected.
    private int powerCubesHave = 0;
    // Reference to the sun battery life text object.
    private Text sunCounter;
    // References to other components.
    private MusicPlayer mp;
    private FlavorText ft, ft2;
    private Light lt;
    // The current night.
    private int nightNumber = 0;
    // Keeps track of the duck light object that is used at night.
    private GameObject duckLight;
    private AudioSource aud;
    private AudioClip poweron;
    private AudioClip poweroff;
    private AudioClip powerCubeSound;
    
    //private int turingTimer = -1;
    //private const int TIME_FOR_TURING = 100;
    // A reference to Alan Turing himself.
    private GameObject turing = null;

    private void Awake()
    {
        // Get various object components.
        sunCounter = GameObject.Find("SunCounter").GetComponent<Text>();
        mp = GameObject.Find("Playlist").GetComponent<MusicPlayer>();
        ft = GameObject.Find("FlavorText").GetComponent<FlavorText>();
        ft2 = GameObject.Find("FlavorTextUpper").GetComponent<FlavorText>();
        lt = GetComponent<Light>();
        // Set the skybox and ambient light to their default settings.
        RenderSettings.skybox.SetColor("_Tint", Color.white);
        //RenderSettings.ambientLight = Color.blue;
        //RenderSettings.ambientIntensity = 1.0f;
        RenderSettings.ambientIntensity = 0.0f;

        // Set the time to its initial value at the start of the game.
        time = timeForNight;
        aud = GameObject.Find("AudioController").GetComponent<AudioSource>();
        poweron = (AudioClip)Resources.Load("Sound Effects/EMotorDoor");
        poweroff = (AudioClip)Resources.Load("Sound Effects/Power Down");
        powerCubeSound = (AudioClip)Resources.Load("Sound Effects/electriccurrent");
    }

	private void Update ()
    {
	    if (!isNight && time != 0)
        {
            // Decrease time by 1.
            --time;
            // If we just ran out of time...
            if (time == 0)
            {
                // Switch to night.
                BecomeNight();
            }
            else
            {
                // Calculate battery life for displaying in the HUD.
                int batterylife = time * 100 / timeForNight;
                // Update sun text to reflect the remaining battery life.
                sunCounter.text = "Sun Battery Life Remaining: " + batterylife + '%';
            }
        }
    }

    private void UpdateSunBatteryText()
    {
        sunCounter.text = "Sun Batteries Collected: " + powerCubesHave + '/' + powerCubesNeeded + " (Night " + nightNumber + ')';
    }

    public void ToggleNight()
    {
        if (isNight)
        {
            BecomeDay();
        }
        else
        {
            BecomeNight();
        }
    }

    // Change night back to day.
    public void BecomeDay()
    {
        // If it's already daytime, exit this function immediately.
        if (isNight == false)
        {
            return;
        }
        if (nightNumber == FINAL_NIGHT)
        {
            // You beat the game!
            SceneManager.LoadScene("Credits");
            return;
        }
        // It's morning now.
        isNight = false;
        // Stop the music.
        // It will automatically be replaced by the correct type of music by the MusicPlayer.
        mp.audioo.Stop();
        // Play the power on sound.
        aud.PlayOneShot(poweron);
        // Print a message.
        ft.Print("The Sun has successfully rebooted.", Color.green);
        // Reset the timer.
        time = timeForNight;
        // Bring back the sunlight.
        lt.intensity = 1.25f;
        // Make the skybox normal again.
        RenderSettings.skybox.SetColor("_Tint", Color.white);
        // Give the ambient light somewhat natural-looking settings.
        //RenderSettings.ambientLight = Color.blue;
        //RenderSettings.ambientIntensity = 1.0f;
        // Destroy all existing power cubes.
        PurgeObjectsWithTag("PowerCube");
        PurgeObjectsWithTag("Spikes");
        PurgeObjectsWithTag("Laser");
        PurgeObjectsWithTag("Missile");
        PurgeObjectsWithTag("Searcher");
        // Set the number of power cubes we have back to 0 for the next night.
        powerCubesHave = 0;
        // Increase the number of power cubes needed needed.
        // This will make the next night more difficult.
        //powerCubesNeeded = (int)(powerCubesNeeded * 1.2f);
        //powerCubesNeeded += 5;
        //powerCubesNeeded *= 2;
        // Destroy the duck light object.
        Destroy(duckLight);
        // Destroy Alan Turing.
        Destroy(turing);
    }

    // Change day back to night.
    public void BecomeNight()
    {
        // If it's already nighttime, exit this function immediately.
        if (isNight == true)
        {
            return;
        }
        // It's night now.
        isNight = true;
        // It's a new night, too.
        ++nightNumber;
        // Stop the music.
        // It will automatically be replaced by the correct type of music by the MusicPlayer.
        mp.audioo.Stop();
        // Play the power off sound.
        aud.PlayOneShot(poweroff);
        // Print a message.
        string postText = "\n\nThe Sun ran out of batteries. Auxiliary duck-based light source enabled.";
        if (nightNumber == FINAL_NIGHT)
        {
            postText = " (FINAL NIGHT)\n\nThis is it! Free Alan Turing's soul once and for all!";
            // Loop the final boss music during the final night.
            mp.audioo.loop = true;
        }
        ft.Print("NIGHT " + nightNumber + postText, Color.yellow);
        // Remove light intensity. This makes the world go dark.
        lt.intensity = 0.0f;
        // Make the skybox black. This will enhance the "night" effect.
        RenderSettings.skybox.SetColor("_Tint", Color.black);
        // Make the ambient light black too.
        //RenderSettings.ambientLight = Color.black;
        //RenderSettings.ambientIntensity = 0.0f;
        // Change the sun counter text to reflect the fact that you have to collect Sun Batteries now.
        UpdateSunBatteryText();
        // Instantiate a light around the player that will help them see.
        Transform duckTransform = GameObject.Find("Duck").transform; 
        duckLight = (GameObject)Instantiate(Resources.Load("Prefabs/DuckLight"), duckTransform.position, duckTransform.rotation, duckTransform);
        // Instantiate Alan Turing.
        Vector3 turingStartPos = new Vector3(250, -100, 250);
        turing = (GameObject)Instantiate(Resources.Load("Prefabs/AlanTuring"), turingStartPos, Quaternion.identity);
        // If MUSH_NIGHT_KILL was not commented out at the top of the file...
#if MUSH_NIGHT_KILL
        // Destroy all Mushroom Boys.
        GameObject[] mbs = GameObject.FindGameObjectsWithTag("MushroomBoy");
        if (mbs.Length != 0)
        {
            foreach (GameObject mush in mbs)
            {
                Destroy(mush);
            }
        }
#endif
    }

    // This method is called when a power cube (Sun Battery) is collected.
    public void CollectPowerCube()
    {
        // Add a power cube.
        ++powerCubesHave;
        // Play a sound.
        aud.PlayOneShot(powerCubeSound);
        // Update the UI text about Sun Batteries.
        UpdateSunBatteryText();
        // Print a message about collecting the Sun Battery.
        ft2.Print("+1 Sun Battery!", Color.yellow);
        // If the Sun has been fully recharged...
        if (powerCubesHave == powerCubesNeeded)
        {
            // Make it daytime again.
            BecomeDay();
        }
    }

    public int GetNightNumber()
    {
        return nightNumber;
    }

    public int GetFinalNight()
    {
        return FINAL_NIGHT;
    }

    void PurgeObjectsWithTag(string tag)
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag(tag);
        if (objs.Length != 0)
        {
            foreach (GameObject obj in objs)
            {
                Destroy(obj);
            }
        }
    }
}
