using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private Rigidbody2D ghostBody; 
    private float horizontalInput; 
    private float verticalInput; 

    // Start is called before the first frame update
    void Start()
    {
        ghostBody = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal"); 
        verticalInput = Input.GetAxis("Vertical"); 
        ghostBody.AddForce(transform.up * verticalInput, ForceMode2D.Force);
        ghostBody.AddForce(transform.right * horizontalInput, ForceMode2D.Force); 
    }
}
