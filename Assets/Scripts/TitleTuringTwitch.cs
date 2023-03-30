// Paul Calande 2016

using UnityEngine;
using System.Collections;

public class TitleTuringTwitch : MonoBehaviour
{
    // How far the random position can go.
    const float SPAN = 0.2f;

    // Original eulerAngles.
    Vector3 ta;

    private void Start()
    {
        ta = transform.eulerAngles;
    }

    private void Update ()
    {
        float newx = Random.Range(-SPAN, SPAN);
        float newy = Random.Range(-SPAN, SPAN);
        float newz = Random.Range(-SPAN, SPAN);
        transform.eulerAngles = new Vector3(ta.x + newx, ta.y + newy, ta.z + newz);
    }
}
