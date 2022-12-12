using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class Tutorial : MonoBehaviour
{
    public int unlockFirstRoom = 0;
    private TextMeshPro tm;
    private Vector3 position; // textmeshpro overrides position or something so we set it manually

    private string[] strings = {
        "Use the WASD or arrow\nkeys to move",
        "Use Space to jump",
        "Use Q to dash",
        "Click to shoot",
        "Move the mouse to aim",
        "Walk close to a door to open it",
        "Defeat the enemies\nto progress",
        "Pick up a new weapon by\npressing E",
        "The red door represents \nthe boss room - beware!",
        "Get to the teleporter to get to the next level",
        "Press E to pick up\na new power, but\nchoose carefully",
        "These new enemies\ncan fight from afar"
    };
    private List<int> unusedTutorials;
    private int tutorialIdx = 0;

    // Start is called before the first frame update
    void Start()
    {
        unusedTutorials = new List<int>();
        for (int i = 0; i < strings.Length; i++)
        {
            unusedTutorials.Add(i);
        }

        position = new Vector3(0, 0f, 0);
        transform.rotation = Quaternion.Euler(90, 0, 0);

        tm = GetComponent<TextMeshPro>();
    }

    public void StartTutorial()
    {
        TickTutorial(0, 0, 0, 10);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = position;

        // tutorial in first room
        if (tutorialIdx == 0 && (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0))
        {
            TickTutorial(1, 0, 0, 10);
        }

        if (tutorialIdx == 1 && Input.GetKeyDown(KeyCode.Space))
        {
            TickTutorial(2, 0, 0, 10);
        }

        if (tutorialIdx == 2 && Input.GetKeyDown(KeyCode.Q))
        {
            TickTutorial(3, 0, 0, 10);
        }

        if (tutorialIdx == 3 && Input.GetMouseButtonDown(0))
        {
            TickTutorial(4, 0, 0, 10);
        }

        if (tutorialIdx == 4 && Input.GetAxis("Mouse X") != 0)
        {
            TickTutorial(5, 0, 0, 10);
        }
    }

    public void TutorialUpdate(int idx)
    {
        if (tutorialIdx < strings.Length)
        {
            tm.text = strings[idx];
        }
        else
        {
            tm.text = " ";
        }
    }

    public void TickTutorial(int idx, int roomX, int roomY, int roomSize)
    {
        if (!unusedTutorials.Contains(idx))
        {
            // only see each tutorial once
            return;
        }

        if (idx == 5)
        {
            unlockFirstRoom = 1;
        }

        unusedTutorials.Remove(idx);

        position = new Vector3(roomX + xDelta(roomSize), 1f, roomY + yDelta(roomSize));
        tutorialIdx = idx;

        TutorialUpdate(idx);
    }

    public void ClearTutorial()
    {
        tm.text = " ";
    }

    public bool Unused(int idx)
    {
        return unusedTutorials.Contains(idx);
    }

    public void CustomTutorial(string s, int roomX, int roomY, int roomSize)
    {
        position = new Vector3(roomX + xDelta(roomSize), 1f, roomY + yDelta(roomSize));
        tm.text = s;
    }

    // hardcode some offsets to make sure text fits
    private int xDelta(int roomSize)
    {
        switch (roomSize)
        {
            case 7: return 4;
            case 8: return 3;
            case 9: return 2;
            case 10: case 11: return 2;
            case 12: return 0;
            default: return 0;
        }
    }

    private int yDelta(int roomSize)
    {
        switch (roomSize)
        {
            case 7: case 8: return 3;
            case 9: case 10: return 4;
            case 11: return 6;
            case 12: return 7;
            default: return 7;
        }
    }
}
