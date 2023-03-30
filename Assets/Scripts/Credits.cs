// Paul Calande
// Fall 2016

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour
{
    // How quickly the text fades in. Higher number = faster.
    // Make sure it can be multiplied to 1 by some integer.
    const float ALPHA_SPEED = 0.02f;
    const int CYCLES_PER_CHARACTER = 7;

    string[] strings =
    {
        "And so the Sun rose high into the beautiful morning sky, as did Alan Turing's soul as he was finally put to rest.",
        "Before Death's eyes, the grossly obese vessel known as the Duck Overlord, now empty, was vaporizing into thin air.",
        "With Death's job accomplished, he unpossessed the body of the duck, which continued to live the rest of its days unknowing of the raging battle that it had been part of, or who it had been possessed by.",
        "The infinite while loop has been brought to a close and the mayhem of the Duck Overlord is over.",
        "Peace has at last been returned to the nights. All is well.",
        "CREDITS TIME!!!",
        @"Emmanuel Quinones
(aka Trusty Group Member)",
        @"Julian Celestin
(aka Julian Celestin)",
        @"Paul Calande
(aka Overachiever Extraordinaire)",
        "No, really, this class didn't warrant such an ambitious project, Paul. You dingus.",
        @"MUSIC USED...

-=- DAYTIME MUSIC -=-
Astronaut - Rain (Raider Remix)
MitiS - Born
Raider - Tremble (feat. Yoe Mase)
Tritonal & Paris Blohm ft. Sterling Fox - Colors (Raider Remix)",
        @"-=- NIGHTTIME MUSIC -=-
14.ogg (Wonderland Adventures 3: Planet of the Z-Bots)

-=- FINAL NIGHT MUSIC -=-
Enmity of the Dark Lord (The Binding of Isaac)

-=- MAIN MENU MUSIC -=-
Kevin MacLeod - Awkward Meeting

-=- CREDITS MUSIC -=-
Ballad For You (Dynamite Headdy)",
        "Special thanks to all of the beautiful people who made the sound effects. I have no idea who you are since I didn't read the names of the authors, but thank you.",
        "Oh yeah. I almost forgot. Thanks for playing, I guess. Whatever.",
        "As our way of thanking you, we're going to shove you directly back to the title screen for no reason. Goodbye!"
    };

    // Here, have some private variables.
    // ...such as the index of the current string in strings[].
    int currentString = 0;
    // Current alpha value of the text.
    float alpha = 0.0f;
    // Number of cycles that the text stays at 100% opacity.
    int alphaTimer;
    int stage = 0;
    Text txt;
    int numberOfStrings;

    private void Awake()
    {
        txt = GetComponent<Text>();
        numberOfStrings = strings.Length;
        txt.text = strings[currentString];
        //Debug.Log("The number of strings is " + strings.Length);
    }

    void Update ()
    {
        // Stage 0: Text fade in.
        if (stage == 0)
        {
            if (alpha < 1.0f)
            {
                alpha += ALPHA_SPEED;
                UpdateTextAlpha();
            }
            else
            {
                alphaTimer = strings[currentString].Length * CYCLES_PER_CHARACTER;
                stage = 1;
            }
        }
        // Stage 1: Text remain at full opacity.
        if (stage == 1)
        {
            --alphaTimer;
            if (alphaTimer == 0)
            {
                stage = 2;
            }
        }
        if (stage == 2)
        {
            if (alpha > 0.0f)
            {
                alpha -= ALPHA_SPEED;
                UpdateTextAlpha();
            }
            else
            {
                //Debug.Log(currentString + ":" + numberOfStrings);
                // If there are no strings left...
                if (currentString == numberOfStrings - 1)
                {
                    // Go back to the title screen.
                    SceneManager.LoadScene("Title Screen");
                    return;
                }
                // Otherwise, move on to the next string.
                ++currentString;
                txt.text = strings[currentString];
                // Let's do it all over again.
                stage = 0;
            }
        }
	}

    void UpdateTextAlpha()
    {
        Color temp = txt.color;
        temp.a = alpha;
        txt.color = temp;
    }
}
