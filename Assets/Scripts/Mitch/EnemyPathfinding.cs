using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QPathFinder;

public class EnemyPathfinding : MonoBehaviour
{
    int closest;
    int goal;
    List<Vector3> path;
    [SerializeField] PathFollower pathFollower;
    Vector3 currentPosition;
    // Start is called before the first frame update
    void Start()
    {
        closest = PathFinder.instance.FindNearestNode(transform.position);
        goal = PathFinder.instance.FindNearestNode(new Vector3(-5, -2, 0));
        PathFinder.instance.FindShortestPathOfPoints(closest, goal, PathLineType.Straight, Execution.Synchronous, (path) =>
        {
            pathFollower.Follow(path, 1, true);
        });
        
    }

    public void MoveTo(Vector3 point)
    {
        float speed = 3;
        Vector3 direction = point - transform.position;
        direction[2] = 0;
        direction = direction.normalized;
        transform.Translate(direction * Time.deltaTime * speed, Space.World);
    }

    public bool IsOnPoint(Vector3 point)
    {
        Vector3 point2D = point;
        point[2] = 0;
        if(Vector3.Magnitude(transform.position - point) < 0.1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
