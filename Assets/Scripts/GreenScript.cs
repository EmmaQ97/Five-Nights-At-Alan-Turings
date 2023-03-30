// Paul Calande 2016

using UnityEngine;
using System.Collections;

public class GreenScript : MonoBehaviour
{
    int lifeTimer = 40;
    float shakeAmount = 25.0f;

    RectTransform rt;
    Vector2 originalpos;

	// Use this for initialization
	void Start ()
    {
        rt = GetComponent<RectTransform>();
        originalpos = transform.position;
	}
	
	// Update is called once per frame
	void Update ()
    {
        float xp = originalpos.x + Random.Range(-shakeAmount, shakeAmount);
        float yp = originalpos.y + Random.Range(-shakeAmount, shakeAmount);
        rt.transform.position = new Vector2(xp, yp);
        --lifeTimer;
        if (lifeTimer == 0)
        {
            Destroy(gameObject);
        }
	}
}
