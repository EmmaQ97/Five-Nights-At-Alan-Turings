// Paul Calande

using UnityEngine;
using System.Collections;

public class GameOverScreen : MonoBehaviour {
    const int TIMERMAX = 20;
    int timer = TIMERMAX;
    AudioSource aud;

    private void Awake()
    {
        aud = GetComponent<AudioSource>();
    }

    void Update () {
        --timer;
        if (timer == 0)
        {
            timer = TIMERMAX;
            aud.pitch = Random.Range(0.01f, 2f);
            aud.PlayOneShot(aud.clip);
        }
	}
}
