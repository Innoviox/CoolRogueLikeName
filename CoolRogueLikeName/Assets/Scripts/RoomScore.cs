using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RoomScore : MonoBehaviour
{
    private int score;
    private TextMeshProUGUI textMeshPro;

    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        textMeshPro = gameObject.GetComponent<TextMeshProUGUI>();
        textMeshPro.text = score + " rooms cleared";
    }

    void Increment()
    {
        score++;
        if (score == 1)
        {
            textMeshPro.text = score + " room cleared";
        }
        else
        {
            textMeshPro.text = score + " rooms cleared";
        }
    }
}