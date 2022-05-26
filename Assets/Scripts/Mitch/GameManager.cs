using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckWinCondition();
    }

    public void GameOver()
    {
        SceneManager.LoadScene(3);
    }

    public void CheckWinCondition()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("NPC");
        foreach (GameObject enemy in enemies)
        {
            Debug.Log(enemy.name); 
        }
        
        if(enemies.Length == 0)
        {
            SceneManager.LoadScene(4);
        }
    }
}
