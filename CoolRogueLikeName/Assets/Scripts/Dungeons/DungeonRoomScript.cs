using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonRoomScript : MonoBehaviour
{
    float difficulty = 0.5f; // todo
    public Transform player;
    public Room room;
    public bool generateEnemies = true; // todo make private with getter/setter
    public new Camera camera;

    public bool willSpawnPowerups;
    public bool willSpawnWeapon;
    public GameObject weaponChest;
    public GameObject powerupPedestal;

    public Dictionary<string, Transform> blocksDict;
    private int nEnemies = 2; // todo
    private List<DungeonDoorScript> doors;

    private Transform roomTransform;
    private Renderer[] renderers;
    private Bounds bounds;
    private Bounds playerBounds;
    private DungeonDoorScript entryPoint = null;
    private bool roomDone = false;
    private bool enemiesActivated = false;
    private Vector3 cameraPosition; // todo set camera rotation as well

    private EnemyCreator enemyCreator;

    /* These members are used for sending messages to the HUD */
    public GameObject killedEnemiesScore;
    public GameObject clearedRoomsScore;

    delegate void DoorVisibleDelegate(bool visible, int roomId);
    DoorVisibleDelegate doorsTouchingShow;
    delegate void DoorsTouchingUnlock(int roomId);
    DoorsTouchingUnlock doorsTouchingUnlock;

    // Start is called before the first frame update
    void Start()
    {
        roomTransform = GetComponent<Transform>();
        enemyCreator = GetComponent<EnemyCreator>();

        doors = new List<DungeonDoorScript>();

        cameraPosition = room.CameraPosition();

        bounds = room.GetBounds();
        playerBounds = player.GetComponent<Collider>().bounds;

        willSpawnPowerups = Random.Range(0.0f, 1.0f) < 0.25;
        willSpawnWeapon = (!willSpawnPowerups) && Random.Range(0.0f, 1.0f) < 0.25;
    }

    public void StartRoom()
    {
        playerBounds = player.GetComponent<Collider>().bounds;
        renderers = GetComponentsInChildren<Renderer>();
        if (generateEnemies)
        {
            WalkedInto();
        }
        else
        {
            ShowRoom(false);
        }
    }

    public void AddDelegates(Door door)
    {
        doorsTouchingShow += door.DoorVisibleDelegate;
        doorsTouchingUnlock += door.Unlock;
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
        // killedEnemiesScore.SendMessage("Increment"); // increases the enemies killed score

        if (nEnemies == 0)
        {
            RoomFinished();
            // clearedRoomsScore.SendMessage("Increment"); // increases the rooms cleared score
        }
    }

    private void RoomFinished()
    {
        // Debug.Log("room finished");
        // finish room
        roomDone = true;

        // open all doors
        // foreach (var door in doors)
        // {
        //     door.Unlock();
        // }
        doorsTouchingUnlock(room.id);

        if (entryPoint != null)
        {
            entryPoint.Unlock();
        }

        if (willSpawnPowerups)
        {
            SpawnPowerups();
        } else if (willSpawnWeapon)
        {
            SpawnWeaponChest();
        }
    }

    public void WalkedInto()
    {
        if (!RoomDone())
        {
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

        doorsTouchingShow(visible, room.id);
    }

    public void ActivateEnemies()
    {
        if (roomDone || enemiesActivated)
        {
            return;
        }

        for (int i = 0; i < nEnemies; i++) {
            enemyCreator.CreateEnemy(player, room.RandomLocation(2.0f));
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
        // Debug.Log($"{bounds.ToString()}");
        // Debug.Log($"min {player.position + playerBounds.min} {bounds.Contains(player.position + playerBounds.min)} max {player.position + playerBounds.max} {bounds.Contains(player.position + playerBounds.max)}");
        return bounds.Contains(player.position + playerBounds.min) && bounds.Contains(player.position + playerBounds.max);
    }

    public void AddDoor(DungeonDoorScript door)
    {
        door.player = player;
        doors.Add(door);
    }

    public void SetEntryPoint(DungeonDoorScript door)
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

    // public void UpdateDoorScript(DungeonDoorScript d, Door door)
    // {
    //     d.player = player;
    //     d.roomThisDoorLeadsFrom = door.from;
    //     d.roomThisDoorLeadsTo = door.to;

    //     doors.Add(d);
    // }

    public Transform Parent()
    {
        return roomTransform;
    }

    public void SpawnWeaponChest()
    {
        GameObject wc = Instantiate(weaponChest);
        Transform wct = wc.GetComponent<Transform>();
        wct.parent = transform;
        wct.position = new Vector3(0, 1, 0);
    }

    public void SpawnPowerups()
    {
        // todo: make powerup pedestal prefab, spawn em
    }
}
