using UnityEngine;
using System.Collections;

public class MusicPlayer : MonoBehaviour
{
    public AudioSource audioo; // AudioSource component of this object.

    private Object[] musicDay; // Daytime music
    private Object[] musicNight; // Nighttime music
    private DayNightCycle dnc; // DayNightCycle component of the Sun.
    private AudioClip finalBossMusic;

    void Awake () {
		audioo = GetComponent<AudioSource> ();
        dnc = GameObject.Find("Sun").GetComponent<DayNightCycle>();
		musicDay = Resources.LoadAll("Music/Day", typeof(AudioClip));
        musicNight = Resources.LoadAll("Music/Night", typeof(AudioClip));
        finalBossMusic = (AudioClip)Resources.Load("Music/Finale/EnmityOfTheDarkLord");
        playRandomMusic();
	}

	// Update is called once per frame
	void Update ()
    {
        if (!audioo.isPlaying)
        {
            playRandomMusic();
        }
	}

	void playRandomMusic()
    {
        if (dnc.isNight)
        {
            if (dnc.GetNightNumber() == dnc.GetFinalNight())
            {
                audioo.clip = finalBossMusic;
            }
            else
            {
                audioo.clip = musicNight[Random.Range(0, musicNight.Length)] as AudioClip;
            }
        }
        else
        {
            audioo.clip = musicDay[Random.Range(0, musicDay.Length)] as AudioClip;
        }
        audioo.Play();
	}
}