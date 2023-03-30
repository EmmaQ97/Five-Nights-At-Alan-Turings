// Paul Calande, Emmanuel Quinones 2016

// Keeping the following line uncommented enables cheats (for easier testing).
//#define CHEATS

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    // How quickly the player moves. Higher number = faster.
    [System.NonSerialized]
    public float movementForce = 10.0f;
    // Reference to hitmarker texture.
	public Texture hitMark;
	public Vector3 jump;
	public float jumpForce = 2.0f;
	// Bool for if the duck is touching the ground.
	public bool isGrounded;

    // Variables for keeping track of invincibility frames.
    int timeForDamageCooldown = 50;
    int damageCooldown = 0;

    // A reference to the camera that keeps track of the player.
    private GameObject myCamera;
    // A private variable keeping track of the Rigidbody component. This lets us access the associated variables and methods.
    private Rigidbody rb;
    // A private variable keeping track of the camera's MouseAimCamera component.
    // Once again, this grants us access to that component's stuff.
    private MouseAimCamera mac;
#if CHEATS
    private DayNightCycle dnc;
#endif
    private AudioSource myaud;
    // Timer keeping track of the hitmarker.
	private int displayHits = 0;
    private Text hptext;
    private FlavorText ft2;
    // Amount of HP remaining. There is no cap.
    private int hp = 3;
    private AudioSource aud;
    private AudioClip ouch;
    // The direction in which the player will move this frame.
    // This vector will change based on the player's inputs.
    Vector3 effectiveDirection = Vector3.zero;
    // The number of movement inputs on this frame.
    int numberOfMovementInputs = 0;
    // rot will rotate each basis vector so that the player adjusts their movement direction based on the camera's rotation.
    Quaternion rot = Quaternion.identity;
    AudioClip spottedPlayer;
    GameObject greenscript;
    GameObject canvas;

    // Necessary to check and see if the duck is touching the ground or not.
    void OnCollisionStay()
    {
        isGrounded = true;
    }

    private void Start()
    {
        // Find the main camera and store a reference to it in myCamera.
        myCamera = GameObject.Find("MainCamera");
        // Get the Rigibody component and store it in rb.
        rb = GetComponent<Rigidbody>();
        // Do this for the camera's MouseAimCamera component as well.
        mac = myCamera.GetComponent<MouseAimCamera>();
#if CHEATS
        dnc = GameObject.Find("Sun").GetComponent<DayNightCycle>();
#endif
        hptext = GameObject.Find("HPCounter").GetComponent<Text>();
        ft2 = GameObject.Find("FlavorTextUpper").GetComponent<FlavorText>();
        aud = GameObject.Find("AudioController").GetComponent<AudioSource>();
        myaud = GetComponent<AudioSource>();
        ouch = (AudioClip)Resources.Load("Sound Effects/Yeouch");
        jump = new Vector3(0.0f, 2.0f, 0.0f);
        spottedPlayer = (AudioClip)Resources.Load("Sound Effects/AirHorn");
        greenscript = (GameObject)Resources.Load("Prefabs/GreenScript");
        canvas = GameObject.Find("HUDCanvas");

        UpdateHPText();
    }

    void Update()
    {
        if (damageCooldown != 0)
        {
            --damageCooldown;
        }
        // Set rot to the camera's rotation.
        rot = mac.rotation;
        // Reset the number of movement inputs and effective direction.
        numberOfMovementInputs = 0;
        effectiveDirection = Vector3.zero;

        // If you press the spacebar...
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(jump * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
        // If you press A or left arrow key...
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            // Increment the number of movement inputs by 1.
            ++numberOfMovementInputs;
            // Add left direction to the effectiveDirection vector.
            effectiveDirection += Vector3.left;
        }
        // If you press D or right arrow key...
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            // Increment the number of movement inputs by 1.
            ++numberOfMovementInputs;
            // Add right direction to the effectiveDirection vector.
            effectiveDirection += Vector3.right;
        }
        // If you press W or up arrow key...
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            // Increment the number of movement inputs by 1.
            ++numberOfMovementInputs;
            // Add forward direction to the effectiveDirection vector.
            effectiveDirection += Vector3.forward;
        }
        // If you press S or down arrow key...
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            // Increment the number of movement inputs by 1.
            ++numberOfMovementInputs;
            // Add backward direction to the effectiveDirection vector.
            effectiveDirection += Vector3.back;
        }
#if CHEATS
        // If cheats are enabled, let the player toggle Day/Night by pressing E.
        if (Input.GetKeyDown(KeyCode.E))
        {
            dnc.ToggleNight();
        }
        // Gain HP by pressing F.
        if (Input.GetKeyDown(KeyCode.F))
        {
            TakeHeal(1000);
        }
#endif
    }

    // FixedUpdate is called once per frame.
    // We're using FixedUpdate instead of Update here because the player object is a rigidbody.
    // Rigidbodies should use FixedUpdate instead of Update when applying force.
    void FixedUpdate()
    {
        // If we entered any inputs on this cycle...
        if (numberOfMovementInputs != 0)
        {
            // Normalize the effectiveDirection vector.
            effectiveDirection.Normalize();
            // Apply force to the player object based on the effectiveDirection vector.
            // Also takes the camera rotation into account.
            rb.AddForce(rot * effectiveDirection * movementForce);
        }
    }
		
	public void OnGUI()
	{
		if (displayHits != 0)
		{
			--displayHits;
            // Draw the hitmarker in the middle of the screen.
			GUI.DrawTexture(new Rect(Screen.width / 2 - hitMark.width / 2,  Screen.height/2- hitMark.height/2, hitMark.width, hitMark.height), hitMark);
		}
	}

	// OnCollisionEnter is called when the object collides with a collider.
	// "Solid" objects like trees and terrain all have colliders.
	void OnCollisionEnter(Collision other)
    {
        // Give the player a very slight bounce to make it less difficult to go up hills.
        // ForceMode.Impulse makes sure the force is applied instantly.
        rb.AddForce(new Vector3(0, 5, 0), ForceMode.Impulse);
        
        // If the duck isn't bouncing high enough, force it to bounce some more.
        // This will prevent the duck from getting stuck on the ground.
        const float MIN_VELOCITY_Y = 0.3f;
        if (Mathf.Abs(rb.velocity.y) < MIN_VELOCITY_Y)
        {
            rb.AddForce(new Vector3(0f, MIN_VELOCITY_Y, 0f), ForceMode.Impulse);
        }
        
        // Start the hitmarker timer.
        displayHits = 10;
        // Play the hitmarker sound.
		myaud.Play();

        /*
        // If we hit the level boundary...
        if (other.gameObject.CompareTag("LevelBoundary"))
        {
            // Bounce wayyy off.
            // We don't include the y component of the vector because we don't want the player to go flying out of bounds.
            const int TOTAL_POWER = 200;
            rb.AddForce(-rb.velocity.x * TOTAL_POWER, 0, -rb.velocity.z * TOTAL_POWER);
        }
        */

        // If we hit Alan Turing himself...
        if (other.gameObject.CompareTag("Turing"))
        {
            // Take damage!
            // The higher the night, the more damage you take from touching Turing.
            //TakeDamage(dnc.GetNightNumber());
            TakeDamage(5);
            // Bounce wayyy off.
            const int TOTAL_POWER = 200;
            rb.AddForce(-rb.velocity.x * TOTAL_POWER, 0, -rb.velocity.z * TOTAL_POWER);
        }
        if (other.gameObject.CompareTag("Spikes"))
        {
            //TakeDamage(dnc.GetNightNumber());
            TakeDamage(5);
            // Bounce up high.
            const int TOTAL_POWER = 10;
            rb.AddForce(0, TOTAL_POWER, 0, ForceMode.Impulse);
            // Make the spikes go back.
            other.gameObject.GetComponent<Spikes>().BackOff();
        }
        if (other.gameObject.CompareTag("Laser"))
        {
            //TakeDamage(dnc.GetNightNumber());
            TakeDamage(5);
            const int TOTAL_POWER = 5;
            rb.AddForce(-rb.velocity.x * TOTAL_POWER, 0, -rb.velocity.z * TOTAL_POWER);
        }
    }

    private void UpdateHPText()
    {
        hptext.text = "HP: " + hp;
    }

    public void TakeDamage(int damage)
    {
        if (damageCooldown == 0)
        {
            damageCooldown = timeForDamageCooldown;
            hp -= damage;
            ft2.Print("Ouch! Lost " + damage + " HP.", Color.red);
            // Update the HP HUD text.
            UpdateHPText();
            // Play damage sound effect.
            aud.PlayOneShot(ouch);
            // If we're out of HP...
            if (hp <= 0)
            {
                // Game over!
                SceneManager.LoadScene("Death Screen");
                return;
            }
        }
    }

    public void TakeHeal(int heal)
    {
        hp += heal;
        UpdateHPText();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Searcher"))
        {
            // Play the "player spotted" sound.
            aud.PlayOneShot(spottedPlayer);
            // Take damage.
            TakeDamage(5);
            // Spawn greenscript object.
            GameObject gs = (GameObject)Instantiate(greenscript, greenscript.transform.position, greenscript.transform.rotation);
            //gs.transform.parent = canvas.transform;
            gs.transform.SetParent(canvas.transform, false);
            // Bounce off.
            const int TOTAL_POWER = 5;
            rb.AddForce(-rb.velocity.x * TOTAL_POWER, 0, -rb.velocity.z * TOTAL_POWER, ForceMode.Impulse);
        }
    }
}
