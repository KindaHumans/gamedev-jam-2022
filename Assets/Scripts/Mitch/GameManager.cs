using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    bool gameStart = false;
    float spawnTimer;
    public float spawnDelay = 15.0f;
    [SerializeField] GameObject enemyPrefab;
    // Start is called before the first frame update
    void Start()
    {
        spawnTimer = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time - spawnTimer > spawnDelay)
        {
            Instantiate(enemyPrefab, enemyPrefab.transform.position, enemyPrefab.transform.rotation);
            spawnTimer = Time.time;
            if(!gameStart)
            {
                gameStart = true;
            }
        }
        if(gameStart)
        {
            CheckWinCondition();
        }
    }

    public void GameOver()
    {
        SceneManager.LoadScene(3);
    }

    public void CheckWinCondition()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("NPC");
        // foreach (GameObject enemy in enemies)
        // {
        //     Debug.Log(enemy.name); 
        // }
        
        if(enemies.Length == 0)
        {
            SceneManager.LoadScene(4);
        }
    }
}
