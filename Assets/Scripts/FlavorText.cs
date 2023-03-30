// Paul Calande 2016

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FlavorText : MonoBehaviour
{
    [System.NonSerialized]
    public Text textComponent;

	void Awake()
    {
        textComponent = GetComponent<Text>();
        SetColor(new Color(0f, 0f, 0f, 0f));
    }

    void SetColor(Color col)
    {
        textComponent.color = col;
    }

    // Prints a message into the flavor text with message = msg and color = col.
    public void Print(string msg, Color col)
    {
        textComponent.text = msg;
        StopAllCoroutines();
        SetColor(col);
        StartCoroutine(Disappear());
    }

    IEnumerator Disappear()
    {
        // Wait for 5 seconds.
        yield return new WaitForSeconds(5f);
        float alpha = 1f;
        // Get the current color.
        Color c = textComponent.color;
        float r = c.r, g = c.g, b = c.b;
        // Run until the alpha is 0.
        while (alpha > 0)
        {
            alpha -= 0.01f;
            SetColor(new Color(r, g, b, alpha));
            yield return new WaitForSeconds(0.01f);
        }
    }
}
