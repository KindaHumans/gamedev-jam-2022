using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyAlertListener : MonoBehaviour
{
    [SerializeField] EnemyPathfinding enemyPathfinding;
    UnityEvent alertEvent = new UnityEvent();
    // Start is called before the first frame update
    void Start()
    {
        alertEvent.AddListener(AlertAI);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown && alertEvent != null)
        {
            //alertEvent.Invoke();
        }
    }

    void AlertAI()
    {
        Debug.Log("AI alerted");
        enemyPathfinding.AlertAction();
    }
}
