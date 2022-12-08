using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    public int unlockFirstRoom = 0;
    private TextMesh tm;

    private string[] strings = {
        "Use the WASD keys to move",
        "Use Space to jump",
        "Use B to dash",
        "Click to shoot",
        "Move the mouse to aim",
        "Walk close to a door to open it",
        // "The doors lock behind you",
        "Defeat the enemies to progress",
        "Pick up powerups or \nweapons to enhance\n your abilities",
        "The red door represents \nthe boss room - beware!",
        "Get to the teleporter to get to the next level",
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

        transform.position = new Vector3(0, 0.5f, 0);

        tm = gameObject.AddComponent<TextMesh>();
        tm.text = " ";
        tm.color = Color.black;
        tm.fontSize = 10;
        tm.transform.rotation = Quaternion.Euler(90, 0, 0);
    }

    public void StartTutorial()
    {
        TickTutorial(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (tutorialIdx == 0 && Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D))
        {
            TickTutorial(1, 0, 0);
        }

        if (tutorialIdx == 1 && Input.GetKeyDown(KeyCode.Space))
        {
            TickTutorial(2, 0, 0);
        }

        if (tutorialIdx == 2 && Input.GetKeyDown(KeyCode.B))
        {
            TickTutorial(3, 0, 0);
        }

        if (tutorialIdx == 3 && Input.GetMouseButtonDown(0))
        {
            TickTutorial(4, 0, 0);
        }

        if (tutorialIdx == 4 && Input.GetAxis("Mouse X") != 0)
        {
            TickTutorial(5, 0, 0);
        }
    }

    public void TutorialUpdate(int idx)
    {
        // transform.position = new Vector3(x, 0.5f, z);
        if (tutorialIdx < strings.Length)
        {
            tm.text = strings[idx];
        }
        else
        {
            tm.text = " ";
        }
    }

    public void TickTutorial(int idx, int roomX, int roomY)
    {
        if (!unusedTutorials.Contains(idx))
        {
            return;
        }

        if (idx == 5)
        {
            unlockFirstRoom = 1;
        }

        unusedTutorials.Remove(idx);

        transform.position = new Vector3(roomX - 8, 0.5f, roomY + 5);
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
}