// Paul Calande 2016

using UnityEngine;
using System.Collections;

public class MenuSun : MonoBehaviour
{
    private void Update()
    {
        float newx = Random.Range(200f, 200.01f);
        transform.eulerAngles = new Vector3(newx, transform.eulerAngles.y, transform.eulerAngles.z);
    }
}