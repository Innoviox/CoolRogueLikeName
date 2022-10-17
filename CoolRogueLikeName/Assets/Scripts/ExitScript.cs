using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitScript : MonoBehaviour
{
    public Transform collideWith;
    public Transform[] roomPrefabs;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.gameObject.name != collideWith.gameObject.name)
        {
            return;
        }

        var player = collideWith.gameObject;

        // player.canMove = false;

        ContactPoint contact = collision.contacts[0];
        Quaternion rotation = new Quaternion();
        Vector3 position = contact.point + new Vector3(-2.7f, -0.6f, 5.05f);
        Instantiate(roomPrefabs[0], position, rotation);

        Destroy(gameObject);
    }
}
