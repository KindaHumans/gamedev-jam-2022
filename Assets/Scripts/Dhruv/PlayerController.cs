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

    // public GameObject clone;


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

        // Instantiate(clone, new Vector3(3f, 3f, 0f), Quaternion.identity);
    }

    
    // Update is called once per frame
    void Update()
    {

        if (rb.bodyType != RigidbodyType2D.Static && this.tag != "StaticPossession")
            Movement();
        
        if (this.tag != "Player" && Input.GetKeyDown(KeyCode.Space))
            DoAction();


        if (this.tag == "Player" && Input.GetKeyDown(KeyCode.E) && item != null)
        {
            Debug.Log("Possess");
            rb.bodyType = RigidbodyType2D.Static;
            item.GetComponent<PlayerController>().enabled = true;
            item.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;

            this.GetComponent<SpriteRenderer>().enabled = false;
            this.enabled = false;
        }

        else if (this.tag != "Player" && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Unposessing");

            rb.bodyType = RigidbodyType2D.Static;

            GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<PlayerController>().enabled = true;
            GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<SpriteRenderer>().enabled = true;

            GameObject.FindGameObjectsWithTag("Player")[0].transform.position = this.transform.position;

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



    void DoAction()
    {
        Debug.Log("Action");
        SpriteRenderer sr = this.GetComponent<SpriteRenderer>();

        AudioSource audio = this.GetComponent<AudioSource>();

        AudioClip clip;

        if (sr.color == Color.blue)
            sr.color = Color.red;
        else
        {
            // Try-Catch error handling for if the length of a non-clone item is shorter than 7 characters
            try 
            {
                if (this.name.Substring(this.name.Length - 7) == "(Clone)")
                    clip = Resources.Load<AudioClip>("Audio/" + this.name.Substring(0, this.name.Length - 7));
                
                else
                    clip = Resources.Load<AudioClip>("Audio/" + this.name);
            }
            catch {
                    clip = Resources.Load<AudioClip>("Audio/" + this.name);}


            // AudioClip clip = Resources.Load<AudioClip>("Audio/Toaster");
            sr.color = Color.blue;
            audio.PlayOneShot(clip);
        }
    }
}
