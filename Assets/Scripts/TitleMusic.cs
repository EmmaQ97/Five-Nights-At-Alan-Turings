using UnityEngine;
using System.Collections;

public class TitleMusic : MonoBehaviour {

    private static TitleMusic instance = null;
    public static TitleMusic Instance
    {
        get
        {
            return instance;
        }
    }

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }

    public void End()
    {
        instance = null;
        GetComponent<AudioSource>().Stop();
        Destroy(gameObject);
    }
}
