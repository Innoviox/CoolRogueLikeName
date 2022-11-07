using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDirection : MonoBehaviour
{
    //0.7 is approximately the height of the player's weapon when the player is on the floor, but 0.5 sorta feels better
    public float staticAimPlaneHeight = 0.5f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        UpdatePlayerDirectionStaticPlane();
    }

    void UpdatePlayerDirectionStaticPlane()
    {
        // Update direction the player faces.
        // Have to manually calculate the intersection of player height plane and mouseRay for some reason.
        // I tried Plane.Raycast() and it was super incorrect... no idea why.
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        float dy = mouseRay.origin.y - staticAimPlaneHeight;
        float ratio = dy / mouseRay.direction.y;
        Vector3 direction = mouseRay.direction * -ratio + mouseRay.origin - transform.position;
        direction.y = 0;
        Quaternion lookRot = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);
        lookRot.SetLookRotation(direction, Vector3.up);
        transform.SetPositionAndRotation(transform.position, lookRot);

    }

    void UpdatePlayerDirectionPlayerPlane()
    {
        // Update direction the player faces.
        // Have to manually calculate the intersection of player height plane and mouseRay for some reason.
        // I tried Plane.Raycast() and it was super incorrect... no idea why.
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        float dy = mouseRay.origin.y - transform.position.y;
        float ratio = dy / mouseRay.direction.y;
        Vector3 direction = mouseRay.direction * -ratio + mouseRay.origin - transform.position;
        direction.y = 0;
        Quaternion lookRot = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);
        lookRot.SetLookRotation(direction, Vector3.up);
        transform.SetPositionAndRotation(transform.position, lookRot);
    }

    void UpdatePlayerDirectionRaycast()
    {
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit rch;
        if (Physics.Raycast(mouseRay, out rch))
        {
            Quaternion lookRot = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);
            Vector3 direction = rch.point - transform.position;
            direction.y = 0;
            lookRot.SetLookRotation(direction, Vector3.up);
            transform.SetPositionAndRotation(transform.position, lookRot);
        }
        else
        {
            UpdatePlayerDirectionPlayerPlane();
        }

    }

}