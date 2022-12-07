using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    private List<GameObject> texts;

    // Start is called before the first frame update
    void Start()
    {
        texts = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void MakeText(int x, int z, string text)
    {
        Debug.Log("making text");
        GameObject gameObject = new GameObject("text");
        gameObject.transform.parent = transform;

        gameObject.transform.position = new Vector3(x, 0.5f, z);

        var tm = gameObject.AddComponent<TextMesh>();
        tm.text = text;
        tm.color = Color.black;
        tm.fontSize = 10;
        tm.transform.rotation = Quaternion.Euler(90, 0, 0);

        texts.Add(gameObject);
    }
}
