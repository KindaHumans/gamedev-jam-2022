using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private Vector2 move;

    private float speed;
    
    private Rigidbody2D rb;

    // private bool possessing;
    
    private bool possessing;
    private GameObject item;


    // Awake is called before whenever the script is enabled
    void Awake()
    {
    }


    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        // rb.bodyType = RigidbodyType2D.Dynamic;
        speed = 5f;
    }

    
    // Update is called once per frame
    void Update()
    {

        if (rb.bodyType != RigidbodyType2D.Static)
            Movement();


        if (this.tag == "Player" && Input.GetKeyDown(KeyCode.Return) && item != null)
        {
            Debug.Log("Possess");
            rb.bodyType = RigidbodyType2D.Static;
            item.GetComponent<PlayerController>().enabled = true;
            item.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;


            this.enabled = false;
        }

        else if (this.tag != "Player" && Input.GetKeyDown(KeyCode.Return))
        {
            Debug.Log("Unposessing");

            rb.bodyType = RigidbodyType2D.Static;

            GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<PlayerController>().enabled = true;
            GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;


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


    void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log("Possess Optionv");
        item = collider.gameObject;
        // possess = true;

    }

    void OnTriggerExit2D(Collider2D Collider2D)
    {
        item = null;
    }

}
