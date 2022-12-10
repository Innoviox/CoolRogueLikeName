using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetScores : MonoBehaviour
{
    public ScoreManager scoreManager;

    // Start is called before the first frame update
    void Start()
    {
        scoreManager.enemiesDestroyed = 0;
        scoreManager.roomsCleared = 0;
        scoreManager.levelsCleared = 0;
    }
}
