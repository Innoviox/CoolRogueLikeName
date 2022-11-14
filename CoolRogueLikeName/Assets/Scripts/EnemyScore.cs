using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyScore : MonoBehaviour
{
    private int score;
    private TextMeshProUGUI textMeshPro;

    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        textMeshPro = gameObject.GetComponent<TextMeshProUGUI>();
        textMeshPro.text = score + " enemies killed";
    }

    void Increment()
    {
        score++;
        if(score == 1)
        {
            textMeshPro.text = score + " enemy killed";
        } else
        {
            textMeshPro.text = score + " enemies killed";
        }
    }
}
