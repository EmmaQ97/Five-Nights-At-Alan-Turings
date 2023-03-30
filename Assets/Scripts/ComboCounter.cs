using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ComboCounter : MonoBehaviour {

	// Number of frames before a combo times out.
	private const int TIME_AMOUNT = 60;
    // Number of frames between each reduction of combo counter.
    private const int SUBTIME_AMOUNT = 3;
    // A reference to the counter text object.
    private Text counterText;

    public int counter = 0;
	public int timer = TIME_AMOUNT;
	public int subtimer = SUBTIME_AMOUNT;

    private void Awake()
    {
        // Find the Combo Counter GameObject and retrieve its text component.
        counterText = GameObject.Find("ComboCounter").GetComponent<Text>();
        // Start off by making the combo counter invisible, because we have 0 combos when we start the level.
        counterText.enabled = false;
    }

	void Update()
	{
		// If we run out of time...
		if (timer == 0)
		{
			// Decrease the counter.
			if (counter > 0)
			{
				if (subtimer == 0)
                {
					--counter;
                    subtimer = SUBTIME_AMOUNT;
                    // If the counter just hit 0...
                    if (counter == 0)
                    {
                        // Hide the counter.
                        counterText.enabled = false;
                    }
                    else
                    {
                        // Update the combo counter text.
                        counterText.text = "COMBO COUNTER: " + counter;
                    }
				}
                else
                {
					--subtimer;
				}
			}
		}
		else
		{
			// Decrease the timer by 1.
			--timer;
		}
	}

	void OnCollisionEnter(Collision other)
	{
		++counter;
		timer = TIME_AMOUNT;
		subtimer = SUBTIME_AMOUNT;
        // Update the combo counter text.
        counterText.text = "COMBO COUNTER: " + counter;
        // Make the combo counter visible.
        counterText.enabled = true;
    }
}
