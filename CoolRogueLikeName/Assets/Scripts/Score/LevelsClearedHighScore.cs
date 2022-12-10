using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelsClearedHighScore : MonoBehaviour
{
    public ScoreManager scoreManager;
    private TextMeshProUGUI textMeshPro;

    // Start is called before the first frame update
    void Start()
    {
        textMeshPro = gameObject.GetComponent<TextMeshProUGUI>();
        textMeshPro.text = "Levels Cleared (" + scoreManager.levelsClearedHighScore + ")";
    }
}
