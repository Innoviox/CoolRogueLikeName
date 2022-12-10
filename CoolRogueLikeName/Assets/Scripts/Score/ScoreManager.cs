using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScoreManager")]
public class ScoreManager : ScriptableObject
{
    public int enemiesDestroyed;
    public int roomsCleared;
    public int levelsCleared;

    public int enemiesDestroyedHighScore;
    public int roomsClearedHighScore;
    public int levelsClearedHighScore;

    // Start is called before the first frame update
    void Start()
    {
        // These are not being called in Unity editor
        enemiesDestroyed = 0;
        roomsCleared = 0;
        levelsCleared = 0;
        enemiesDestroyedHighScore = 0;
        roomsClearedHighScore = 0;
        levelsClearedHighScore = 0;
    }

    public void enemyDestroyed()
    {
        enemiesDestroyed++;
        Debug.Log("Enemy Destroyed");
    }

    public void roomCleared()
    {
        roomsCleared++;
        Debug.Log("Room Cleared");
    }

    public void levelCleared()
    {
        levelsCleared++;
        Debug.Log("Level Cleared");
    }

    public void playerDeath()
    {
        if(enemiesDestroyed > enemiesDestroyedHighScore)
        {
            enemiesDestroyedHighScore = enemiesDestroyed;
        }
        if(roomsCleared > roomsClearedHighScore)
        {
            roomsClearedHighScore = roomsCleared;
        }
        if(levelsCleared > levelsClearedHighScore)
        {
            levelsClearedHighScore = levelsCleared;
        }
    }
}
