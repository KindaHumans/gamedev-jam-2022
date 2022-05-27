using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Vector2 move;
    private float speed;
    private Rigidbody2D rb;

    private GameObject p_object;

    private ActionController actionC;

    public Animator anim;


    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        speed = 5f;

        actionC = GameObject.FindGameObjectsWithTag("GameController")[0].GetComponent<ActionController>();
        // Instantiate(clone, new Vector3(3f, 3f, 0f), Quaternion.identity);
    }

    
    // Update is called once per frame
    void Update()
    {
        // Always have movement enabled for player, while script is enabled
        Movement();
        

        // Initiate possession of a specific possession object and transfer control to it
        if (this.tag == "Player" && Input.GetKeyDown(KeyCode.E) && p_object != null)
        {
            PossessObject();
        }

    }


    // Dictate player movement
    void Movement()
    {
        move.x = Input.GetAxisRaw("Horizontal");
        move.y = Input.GetAxisRaw("Vertical");
        if (move.x > 0)
            anim.SetBool("Right", true);
        else if (move.x < 0)
            anim.SetBool("Right", false);

        rb.MovePosition(rb.position + move.normalized * speed * Time.fixedDeltaTime);
    }


    // Checks for an object that is within the collider (allowing possession)
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Possession" || collider.tag == "StaticPossession")
        {
            // Debug.Log("Possess Option");
            actionC.TogglePossessBtn();
            p_object = collider.gameObject;
        }
    }

    // Checks id objects are out of range, resetting possession parameters
    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.tag == "Possession" || collider.tag == "StaticPossession")
        {
            actionC.TogglePossessBtn();
            p_object = null;
        }
    }


    public void PossessObject()
    {
        // Debug.Log("Possess");

        // Set appropriate properties for the player
        rb.bodyType = RigidbodyType2D.Static;
        p_object.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        
        // Show UI buttons
        actionC.ToggleUnPossessBtn();
        actionC.ToggleAction1Btn();
        actionC.ToggleAction2Btn();

        // Play Animation
        anim.SetBool("Possess", true);
        StopAllCoroutines();
        StartCoroutine("HideGhost", p_object);

        // Disable player sprite, then disable self script
        this.GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
    }


    IEnumerator HideGhost(GameObject p_object)
    {
        yield return new WaitForSeconds(0.25f);
        this.GetComponent<SpriteRenderer>().enabled = false;
        p_object.GetComponent<PossessionController>().enabled = true;
        yield return new WaitForSeconds(0.25f);
        p_object.GetComponent<PossessionController>().animP.SetBool("PossessObj", true);
    }

}
