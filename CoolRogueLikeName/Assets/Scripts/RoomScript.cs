using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomScript : MonoBehaviour
{
    float difficulty = 0.5f; // todo
    public Transform player;

    public int pregenerateDepth = 2; // for now, just pregenerate 1 rooms ahead


    public bool generateEnemies = true; // todo make private with getter/setter
    public Camera camera;
    private int nEnemies;
    private List<DoorScript> doors;

    private Transform room;
    private Renderer[] renderers;
    private Bounds bounds;
    private Bounds playerBounds;
    private DoorScript entryPoint = null;
    private bool roomDone = false;
    private bool enemiesActivated = false;
    private Vector3 cameraPosition; // todo set camera rotation as well


    // Start is called before the first frame update
    void Start()
    {
        room = GetComponent<Transform>();
        renderers = GetComponentsInChildren<Renderer>();


        doors = new List<DoorScript>();

        Transform door;
        int n = 1;

        while ((door = transform.Find($"Door{n}")) != null)
        {
            n++;
            var doorScript = door.gameObject.GetComponent<DoorScript>();
            doorScript.player = player;
            doorScript.roomThisDoorLeadsFrom = this;
            doors.Add(doorScript);
        }

        if (cameraPosition == Vector3.zero)
        {
            cameraPosition = camera.transform.position;
        }

        PreGenerate();

        if (roomActiveAtStart)
        {
            WalkedInto();
        }
        else
        {
            ShowRoom(false);
        }

        var boxCollider = room.GetComponent<BoxCollider>();
        bounds = boxCollider.bounds;
        playerBounds = player.GetComponent<Collider>().bounds;

        // fit room bounds to children
        foreach (Collider collider in GetComponentsInChildren<Collider>())
        {
            bounds.Encapsulate(collider.bounds);
        }

        boxCollider.center = bounds.center - transform.position;
        boxCollider.size = bounds.size;

        // hide ceiling for now
        transform.Find("Ceiling").gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // maybe this should exist? unsure
        // if (PlayerInRoom())
        // {
        //     camera.transform.position = cameraPosition;
        // }
    }

    private void EnemyDestroyed()
    {
        nEnemies--;
        Debug.Log($"destroyed enemy: {nEnemies} enemies");

        if (nEnemies == 0)
        {
            RoomFinished();
        }
    }

    private void RoomFinished()
    {
        // finish room
        roomDone = true;

        // open all doors
        foreach (var door in doors)
        {
            door.Unlock();
        }

        if (entryPoint != null)
        {
            entryPoint.Unlock();
        }
    }

    private void PreGenerate()
    {
        if (pregenerateDepth == 0)
        {
            return;
        }

        foreach (var door in doors)
        {
            // todo make this better
            var newRoom = door.GenerateRoom(player);
            newRoom.gameObject.GetComponent<RoomScript>().pregenerateDepth = pregenerateDepth - 1;
            newRoom.gameObject.GetComponent<RoomScript>().generateEnemies = false;
            newRoom.gameObject.GetComponent<RoomScript>().camera = camera;
            Debug.Log($"aoeirjg {cameraPosition} {newRoom.position} {transform.position}");
            newRoom.gameObject.GetComponent<RoomScript>().SetCameraPosition(cameraPosition + newRoom.position - transform.position);
        }
    }

    public void WalkedInto()
    {
        if (!RoomDone())
        {
            RemoveLid();
            ActivateEnemies();
        }

        StartCoroutine(MoveCamera());
    }

    public void ShowRoom(bool visible)
    {
        foreach (Renderer r in renderers)
        {
            r.enabled = visible;
        }
    }

    private void RemoveLid()
    {
        transform.Find("Ceiling").gameObject.SetActive(false);
    }

    public void ActivateEnemies()
    {
        if (roomDone || enemiesActivated)
        {
            return;
        }

        Transform enemy;
        int n = 1;

        while ((enemy = transform.Find($"Enemy{n}")) != null)
        {
            nEnemies++;
            n++;

            enemy.SendMessage("CreateEnemy", player);
        }

        enemiesActivated = true;

        if (nEnemies == 0)
        {
            RoomFinished();
        }
    }

    private IEnumerator MoveCamera()
    {
        Debug.Log("setting camera position to " + cameraPosition);
        camera.transform.position = cameraPosition;
        yield break;
    }

    public bool PlayerInRoom()
    {
        return bounds.Contains(player.position + playerBounds.min) && bounds.Contains(player.position + playerBounds.max);
    }

    public void AddDoor(DoorScript door)
    {
        door.player = player;
        doors.Add(door);
    }

    public void SetEntryPoint(DoorScript door)
    {
        entryPoint = door;
    }

    public bool RoomDone()
    {
        return roomDone;
    }

    public void SetCameraPosition(Vector3 position)
    {
        cameraPosition = position;
    }
}
