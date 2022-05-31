using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public Transform ghost;

    public float speed;

    // public Vector3 offset;

    void Start()
    {
        speed = 0.125f;
        // offset = new Vector3(0f, 0f, -1f);
    }

    void LateUpdate()
    {
        transform.position = new Vector3(ghost.position.x, ghost.position.y, -10f); 
    }
}

