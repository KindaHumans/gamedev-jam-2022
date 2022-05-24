using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Vector2 move;
    private float speed;
    private Rigidbody2D rb;

    private GameObject p_object;


    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        speed = 5f;
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
            // Debug.Log("Possess");

            // Set appropriate properties for the player
            rb.bodyType = RigidbodyType2D.Static;
            p_object.GetComponent<PossessionController>().enabled = true;
            p_object.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            p_object.GetComponent<Collider2D>().enabled = false;

            // Disable player sprite, then disable self script
            this.GetComponent<SpriteRenderer>().enabled = false;
            this.enabled = false;
        }

    }


    // Dictate player movement
    void Movement()
    {
        move.x = Input.GetAxisRaw("Horizontal");
        move.y = Input.GetAxisRaw("Vertical");

        rb.MovePosition(rb.position + move.normalized * speed * Time.fixedDeltaTime);
    }


    // Checks for an object that is within the collider (allowing possession)
    void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log("Possess Option");
        p_object = collider.gameObject;
    }

    // Checks id objects are out of range, resetting possession parameters
    void OnTriggerExit2D(Collider2D collider)
    {
        p_object = null;
    }

}
