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
    private List<Transform> doors;

    private Transform room;
    private Bounds bounds;
    private Bounds playerBounds;
    private DoorScript entryPoint = null;
    private bool roomDone = false;
    private bool enemiesActivated = false;
    private Vector3 cameraPosition; // todo set camera rotation as well

    // Start is called before the first frame update
    void Start()
    {
        doors = new List<Transform>();

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

        if (generateEnemies)
        {
            WalkedInto();
        }

        room = GetComponent<Transform>();
        var boxCollider = room.GetComponent<BoxCollider>();
        bounds = boxCollider.bounds;
        playerBounds = player.GetComponent<Collider>().bounds;

        // fit room bounds to children
        foreach (Collider collider in GetComponentsInChildren<Collider>())
        {
            Debug.Log($"RoomScript: {collider.name} {collider.bounds}");
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

    }

    private void EnemyDestroyed()
    {
        nEnemies--;
        Debug.Log($"destroyed enemy: {nEnemies} enemies");

        if (nEnemies == 0)
        {
            // finish room
            roomDone = true;

            // open all doors
            foreach (var door in doors)
            {
                // todo make list of DoorScripts so this is faster
                door.gameObject.SendMessage("Unlock");
            }

            if (entryPoint != null)
            {
                entryPoint.Unlock();
            }
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
            var newRoom = door.gameObject.GetComponent<DoorScript>().GenerateRoom(player);
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

    public void AddDoor(Transform door)
    {
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
