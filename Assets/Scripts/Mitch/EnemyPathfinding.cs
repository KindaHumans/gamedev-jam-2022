using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QPathFinder;

public class EnemyPathfinding : MonoBehaviour
{
    int closest;
    int goal;
    float speed = 3;
    List<Vector3> path;
    [SerializeField] PathFollower pathFollower;
    [SerializeField] EnemyAlertStateManager enemyAlertStateManager;
    [SerializeField] GameManager gameManager;
    [SerializeField] GameObject deadBodyPrefab;
    Vector3 currentPosition;
    float mapRangeX = 10; //-12 to 8
    float mapCenterX = -2;
    float mapRangeY = 9; // -5 to 13
    float mapCenterY = 4;
    public bool stopped = true;
    public enum AlertState {Neutral, Investigate, Cautious, Chase, InvestigateWaiting, CautiousWaiting}
    public AlertState alertState = AlertState.Neutral;
    float waitTime = 0.0f;
    float chaseTime = 0.0f;
    Vector3 lastKnownPosition;
    public GameObject target;
    [SerializeField] SpriteRenderer neutralSprite;
    [SerializeField] SpriteRenderer investigateSprite;
    [SerializeField] SpriteRenderer cautiousSprite;
    [SerializeField] SpriteRenderer chaseSprite;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        //closest = PathFinder.instance.FindNearestNode(transform.position);
        //goal = PathFinder.instance.FindNearestNode(new Vector3(-5, -2, 0));
        //NeutralStateBehavior();
    }

    void NeutralBehavior()
    {
        closest = PathFinder.instance.FindNearestNode(transform.position);
        float randomX = Random.Range(mapCenterX - mapRangeX, mapCenterX + mapRangeX);
        float randomY = Random.Range(mapCenterY - mapRangeY, mapCenterY + mapRangeY);
        goal = PathFinder.instance.FindNearestNode(new Vector3(randomX, randomY, 0));
        PathFinder.instance.FindShortestPathOfPoints(closest, goal, PathLineType.Straight, Execution.Synchronous, (path) =>
        {
            pathFollower.Follow(path, 1, true);
        });
    }

    public void AlertAction()
    {
        PathFinder.instance.FindShortestPathOfPoints(closest, goal, PathLineType.Straight, Execution.Synchronous, (path) =>
        {
            pathFollower.Follow(path, 1, true);
        });
    }
    public void InvestigateBehavior(Vector3 itemLocation)
    {
        alertState = AlertState.Investigate;
        closest = PathFinder.instance.FindNearestNode(transform.position);
        goal = PathFinder.instance.FindNearestNode(itemLocation);
        PathFinder.instance.FindShortestPathOfPoints(closest, goal, PathLineType.Straight, Execution.Synchronous, (path) =>
        {
            pathFollower.Follow(path, 1, true);
        });
    }
    public void ChaseBehavior(Vector3 ghostLocation)
    {
        MoveTo(target.transform.position);
        // closest = PathFinder.instance.FindNearestNode(transform.position);
        // goal = PathFinder.instance.FindNearestNode(ghostLocation);
        // PathFinder.instance.FindShortestPathOfPoints(closest, goal, PathLineType.Straight, Execution.Synchronous, (path) =>
        // {
        //     pathFollower.Follow(path, 1, true);
        // });
    }
    public void CautiousBehavior(Vector3 lastKnownLocation)
    {
        closest = PathFinder.instance.FindNearestNode(transform.position);
        goal = PathFinder.instance.FindNearestNode(lastKnownLocation);
        PathFinder.instance.FindShortestPathOfPoints(closest, goal, PathLineType.Straight, Execution.Synchronous, (path) =>
        {
            pathFollower.Follow(path, 1, true);
        });
    }
    public void DeadBehavior()
    {
        Vector3 spawnPosition = transform.position;
        Quaternion spawnRotation = transform.rotation;
        Destroy(gameObject);
        Instantiate(deadBodyPrefab, spawnPosition, spawnRotation);
    }

    public void MoveTo(Vector3 point)
    {
        Vector3 direction = point - transform.position;
        direction[2] = 0;
        direction = direction.normalized;
        transform.Translate(direction * Time.deltaTime * speed, Space.World);
    }

    public bool IsOnPoint(Vector3 point)
    {
        point[2] = 0;
        if(Vector3.Magnitude(transform.position - point) < 0.1)
        {
            int currentGoal = PathFinder.instance.FindNearestNode(point);
            if(currentGoal == goal)
            {
                stopped = true;
            }
            return true;
        }
        else
        {
            return false;
        }
    }
    public void StopFollowing()
    {
        pathFollower.StopFollowing();
    }
    void Update()
    {
        //Debug.Log(alertState);
        if(alertState != AlertState.Chase)
        {
            chaseTime = 0.0f;
            gameManager.spawnDelay = 15.0f;
        }
        if(stopped && alertState == AlertState.Investigate || alertState == AlertState.Cautious)
        {
            alertState = AlertState.InvestigateWaiting;
            waitTime = Time.time;
            //StopAllCoroutines();
            //StartCoroutine("WaitThenResumePatrol");
            //enemyAlertStateManager.UpdateAlertState();
        }
        else if(!stopped && alertState == AlertState.Chase)
        {
            ChaseBehavior(target.gameObject.transform.position);
            if(chaseTime == 0.0f)
            {
                chaseTime = Time.time;
            }
            else if(Time.time - chaseTime > 5.0f)
            {
                gameManager.GameOver();
            }
            //Debug.Log("target position: " + target.transform.position);
            //Debug.Log("NPC position " + transform.position);
            //Debug.Log("distance to target " + Vector3.Magnitude(transform.position - target.gameObject.transform.position));
            if(Vector3.Magnitude(transform.position - target.gameObject.transform.position) > 5)
            {
                Debug.Log("stopped in chase");
                stopped = true;
                chaseTime = 0.0f;
            }
        }
        else if(stopped && alertState == AlertState.Chase)
        {
            alertState = AlertState.CautiousWaiting;
            waitTime = Time.time;
        }
        else if(stopped && alertState == AlertState.CautiousWaiting)
        {
            if(Time.time - waitTime > 2)
            {
                alertState = AlertState.Investigate;
            }
        }
        else if(stopped && alertState == AlertState.InvestigateWaiting)
        {
            if(Time.time - waitTime > 2)
            {
                alertState = AlertState.Neutral;
            }
        }
        else if(stopped && alertState == AlertState.Neutral)
        {
            NeutralBehavior();
        }
        UpdateAlertIcon();
    }

    void UpdateAlertIcon()
    {
        SpriteRenderer spriteRenderer = transform.GetChild(2).GetComponent<SpriteRenderer>();
        switch (alertState)
        {
            case AlertState.Neutral:
            spriteRenderer.color = Color.white;
            break;
            case AlertState.Investigate:
            case AlertState.InvestigateWaiting:
            spriteRenderer.color = Color.green;
            break;
            case AlertState.Cautious:
            case AlertState.CautiousWaiting:
            spriteRenderer.color = Color.yellow;
            break;
            case AlertState.Chase:
            spriteRenderer.color = Color.red;
            break;
            default:
            break;
        }
    }

    IEnumerator WaitThenResumePatrol()
    {
        alertState = AlertState.Neutral;
        enemyAlertStateManager.UpdateAlertState();
        yield return new WaitForSeconds(3.0f);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("collision with " + collision.gameObject.name);
        if(((collision.gameObject.tag == "Possession" || collision.gameObject.tag == "StaticPossession") && !collision.gameObject.GetComponent<PossessionController>().isActiveAndEnabled))// || collision.gameObject.name == "Walls")
        {
            //Debug.Log("Blocked");
            Vector3 forceDirection = (collision.gameObject.transform.position - transform.position).normalized;
            //collision.gameObject.transform.Translate(forceDirection);
            //collision.gameObject.GetComponent<Rigidbody2D>().AddForce(forceDirection * 100, ForceMode2D.Impulse);
            StopFollowing();
            stopped = true;
            alertState = AlertState.Neutral;
            NeutralBehavior();
        }
    }
}
