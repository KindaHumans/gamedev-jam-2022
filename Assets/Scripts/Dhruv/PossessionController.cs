using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PossessionController : MonoBehaviour
{
    private Vector2 move;
    private float speed;
    private Rigidbody2D rb;

    private GameObject player;

    private ActionController actionC;


    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        speed = 5f;

        player = GameObject.FindGameObjectsWithTag("Player")[0];
        actionC = GameObject.FindGameObjectsWithTag("GameController")[0].GetComponent<ActionController>();
        actionC.ToggleUnPossessBtn();
        // Instantiate(clone, new Vector3(3f, 3f, 0f), Quaternion.identity);
    }

    
    // Update is called once per frame
    void Update()
    {
        // Check if object is allowed to move
        if (rb.bodyType != RigidbodyType2D.Static && this.tag != "StaticPossession")
            Movement();
        
        // Allow object to perform an action
        if (Input.GetKeyDown(KeyCode.Space))
            DoAction();

        // Stop possession the object and return control as main player (ghost)
        if (Input.GetKeyDown(KeyCode.Q))
        {
            UnPossessObject();
        }

    }


    // Dictate player movement
    void Movement()
    {
        move.x = Input.GetAxisRaw("Horizontal");
        move.y = Input.GetAxisRaw("Vertical");

        rb.MovePosition(rb.position + move.normalized * speed * Time.fixedDeltaTime);
    }


    // Checks for an NPC that is within the collider (which activates when a sound is played)
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "NPC")
            Debug.Log("Entering: " + collider.name);
        try
        {
             collider.gameObject.GetComponent<EnemyPathfinding>().AlertAction(transform.position);
        }
        catch
        {
        }

    }

    // Checks for an NPC is no longer in the collider (This method should likely not be needed)
    void OnTriggerExit2D(Collider2D collider)
    {
        // if (collider.tag == "NPC")
        //     Debug.Log("Leaving: " + collider.name);
    }


    // Perform an action when possessing something, actions may differ
    void DoAction()
    {
        // Debug.Log("Action");

        SpriteRenderer sr = this.transform.GetChild(0).GetComponent<SpriteRenderer>();
        AudioSource audio = this.transform.GetChild(1).GetComponent<AudioSource>();
        AudioClip clip;

        // If-else statement to check the current state of the object and perform an action accordingly
        if (sr.color == Color.blue)
            sr.color = Color.red;
        else
        {
            // Try-Catch error handling for if the length of a non-clone item is shorter than 7 characters
            // Audio clip is set and played once every two actions (account for once being a reset state)
            try 
            {
                if (this.name.Substring(this.name.Length - 7) == "(Clone)")
                    clip = Resources.Load<AudioClip>("Audio/" + this.name.Substring(0, this.name.Length - 7));
                
                else
                    clip = Resources.Load<AudioClip>("Audio/" + this.name);
            }
            catch {
                    clip = Resources.Load<AudioClip>("Audio/" + this.name);
            }

            // Start coroutine to enable and disable collider trigger to simulate sound radius
            StopAllCoroutines();
            StartCoroutine("SoundTimer");

            // Change state of object and play sound accordingly            
            sr.color = Color.blue;
            audio.PlayOneShot(clip);
        }
    }


    // Coroutine function to simulate sound effect radius
    IEnumerator SoundTimer()
    {
        Collider2D audioRadius = this.transform.GetChild(1).GetComponent<Collider2D>();

        audioRadius.enabled = true;
        yield return new WaitForSeconds(0.5f);
        audioRadius.enabled = false;
    }

    public void UnPossessObject()
    {
        // Debug.Log("Unposessing");

        // Prevent object from dragging when collided with
        rb.bodyType = RigidbodyType2D.Static;

        // Set appropriate properties for the player
        player.GetComponent<PlayerController>().enabled = true;
        player.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        player.GetComponent<SpriteRenderer>().enabled = true;
        player.transform.position = this.transform.position;

        // Enable collider trigger for player to possess object again, then disable self script
        // this.GetComponent<Collider2D>().enabled = true;
        player.GetComponent<Collider2D>().enabled = true;
        this.enabled = false;
    }
}
