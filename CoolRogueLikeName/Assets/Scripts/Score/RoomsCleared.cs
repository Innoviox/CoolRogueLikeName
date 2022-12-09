using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RoomsCleared : MonoBehaviour
{
    public ScoreManager scoreManager;
    private TextMeshProUGUI textMeshPro;

    // Start is called before the first frame update
    void Start()
    {
        textMeshPro = gameObject.GetComponent<TextMeshProUGUI>();
        textMeshPro.text = "Rooms Cleared (" + scoreManager.roomsCleared + ")";
    }
}
