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

    public Animator animP;


    [HideInInspector]
    public float timestamp;

    private GameObject NPC;

    private bool sound;


    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        speed = 5f;

        player = GameObject.FindGameObjectsWithTag("Player")[0];
        actionC = GameObject.FindGameObjectsWithTag("GameController")[0].GetComponent<ActionController>();

        animP = this.transform.GetChild(0).GetComponent<Animator>();

        // Instantiate(clone, new Vector3(3f, 3f, 0f), Quaternion.identity);
    }

    
    // Update is called once per frame
    void Update()
    {
        CheckForKillable();

        // Check if object is allowed to move
        if (rb.bodyType != RigidbodyType2D.Static && this.tag != "StaticPossession")
            Movement();
        
        // Allow object to perform an action
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (timestamp <= Time.time)
            {
                timestamp = Time.time + 0.7f;
                DoAction();
            }
        }

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


    public List<GameObject> go = new List<GameObject> ();

    // Checks for an NPC that is within the collider (which activates when a sound is played)
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "NPC")
            NPC = collider.gameObject;
            //Debug.Log("Entering: " + collider.name);
        try
        {
            EnemyPathfinding enemyPathfinding = collider.gameObject.GetComponent<EnemyPathfinding>();
            if(this.isActiveAndEnabled && enemyPathfinding.alertState != EnemyPathfinding.AlertState.Chase)
            {
                enemyPathfinding.InvestigateBehavior(transform.position);
            }
        }
        catch
        {
        }

        if (this.tag == "Possession")
        {
            if (!go.Contains(collider.gameObject) && collider.tag != "Untagged")
                go.Add(collider.gameObject);
        }

    }

    // Checks for an NPC is no longer in the collider (This method should likely not be needed)
    void OnTriggerExit2D(Collider2D collider)
    {
        // if (collider.tag == "NPC")
        // if (collider.tag != "Untagged"){

        // }
        if (this.tag == "Possession" && collider.tag != "Untagged" && Vector3.Distance(this.transform.position, collider.transform.position) >= 2f)
        {
            Debug.Log("Leaving: " + collider.name);
            Debug.Log(Vector3.Distance(this.transform.position, collider.transform.position));
            if(go.Contains(collider.gameObject))
            {
                go.Remove(collider.gameObject);
                NPC = null;
            }
        }
    }


    // Perform an action when possessing something, actions may differ
    public void DoAction()
    {
        // Debug.Log("Action");

        SpriteRenderer sr = this.transform.GetChild(0).GetComponent<SpriteRenderer>();
        AudioSource audio = this.transform.GetChild(1).GetComponent<AudioSource>();
        AudioClip clip;

        animP.SetTrigger("Action");
        // If-else statement to check the current state of the object and perform an action accordingly
        // if (sr.color == Color.blue)
            // sr.color = Color.red;
        // else
        // {
            // Try-Catch error handling for if the length of a non-clone item is shorter than 7 characters
            // Audio clip is set and played once every two actions (account for once being a reset state)

            // Debug.Log(this.name);
            // try 
            // {
            //     if (this.name.Substring(this.name.Length - 7) == "(Clone)")
            //         clip = Resources.Load<AudioClip>("Audio/" + this.name.Substring(0, this.name.Length - 7));
                
            //     else
            //         clip = Resources.Load<AudioClip>("Audio/" + this.name);
            // }
            // catch {
            //         clip = Resources.Load<AudioClip>("Audio/" + this.name);
            // }

            

            // Start coroutine to enable and disable collider trigger to simulate sound radius
            StopAllCoroutines();
            StartCoroutine("SoundTimer");

            // Change state of object and play sound accordingly            
            // sr.color = Color.blue;
            // audio.PlayOneShot(clip);
            audio.Play();
        // }
    }


    private void CheckForKillable()
    {
        if (this.tag == "Possession" && go.Count >= 3 && !actionC.action1Btn.activeSelf && !sound)
        {
            actionC.ToggleAction1Btn();
        }
        else if (this.tag == "Possession" && go.Count < 3 && actionC.action1Btn.activeSelf)
        {
            actionC.ToggleAction1Btn();
        }
    }


    // Kill npc from here, Mitch
    public void DoKill()
    {   

        animP.SetTrigger("Kill");

        Debug.Log(NPC);

        NPC.transform.GetChild(1).GetComponent<AudioSource>().Play();
    }

    // Coroutine function to simulate sound effect radius
    IEnumerator SoundTimer()
    {
        sound = true;
        Collider2D audioRadius = this.transform.GetChild(1).GetComponent<Collider2D>();

        audioRadius.enabled = true;
        yield return new WaitForSeconds(0.5f);
        audioRadius.enabled = false;
        sound = false;
    }

    public void UnPossessObject()
    {
        // Debug.Log("Unposessing");

        GameObject.FindGameObjectsWithTag("MainCamera")[0].GetComponent<CameraFollow>().ghost = player.transform;


        // Prevent object from dragging when collided with
        rb.bodyType = RigidbodyType2D.Static;

        // Set appropriate properties for the player
        player.GetComponent<PlayerController>().enabled = true;
        player.GetComponent<PlayerController>().anim.SetBool("Possess", false);
        player.GetComponent<PlayerController>().anim.SetBool("Right", false);
        player.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        player.GetComponent<SpriteRenderer>().enabled = true;
        player.transform.position = this.transform.position;

        animP.SetBool("PossessObj", false);


        actionC.ToggleUnPossessBtn();
        actionC.ToggleAction2Btn();

        if (actionC.action1Btn.activeSelf)
        {
            actionC.ToggleAction1Btn();
        }

        // Enable collider trigger for player to possess object again, then disable self script
        // this.GetComponent<Collider2D>().enabled = true;
        player.GetComponent<Collider2D>().enabled = true;
        this.enabled = false;
    }
}
