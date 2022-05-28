using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAlertStateManager : MonoBehaviour
{
    [SerializeField] EnemyPathfinding enemyPathfinding;
    [SerializeField] GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        // set up triggerenter methods to detect posessed objects, and set alert to red when detected
        // set up trigger exit methods, then wait a few seconds before setting alert state to yellow, then again for neutral
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void UpdateAlertState()
    {

    }
    void OnTriggerEnter2D(Collider2D collider)
    {
        if((collider.CompareTag("Possession") || collider.CompareTag("StaticPossession")) && collider.gameObject.GetComponent<PossessionController>().isActiveAndEnabled)
        {
            enemyPathfinding.StopFollowing();
            //Debug.Log("NPC approaching " + collider.tag);
            enemyPathfinding.target = collider.gameObject;
            enemyPathfinding.alertState = EnemyPathfinding.AlertState.Chase;
            enemyPathfinding.stopped = false;
            gameManager.spawnDelay = 1.0f;
        }
    }
    void OnTriggerExit2D(Collider2D collider)
    {        
        // if((collider.CompareTag("Possession") || collider.CompareTag("StaticPossession")) && collider.gameObject.GetComponent<PossessionController>().isActiveAndEnabled)
        // {
        //     //Debug.Log("NPC lost sight of " + collider.tag);
        //     enemyPathfinding.alertState = EnemyPathfinding.AlertState.Cautious;
        //     enemyPathfinding.Cautious(collider.transform.position);
        // }
    }
}
