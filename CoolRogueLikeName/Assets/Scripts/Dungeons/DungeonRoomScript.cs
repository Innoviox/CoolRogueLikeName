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

    public ScoreManager scoreManager;

    public Dictionary<string, Transform> blocksDict;
    public int nEnemiesBase = 2; // todo
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

    public delegate void DoorToggleDelegate(bool on, int roomId);
    public DoorToggleDelegate showDoors;
    public DoorToggleDelegate lockDoors;
    private bool started = false;
    private int nEnemies;
    private List<Transform> bosses;
    private Tutorial t;

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

        t = roomTransform.parent.GetComponent<DungeonGenerator>().tutorialComp;

        Debug.Log("room start() finished");
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

        started = true;
    }

    public void AddDelegates(Door door)
    {
        showDoors += door.Show;
        lockDoors += door.Lock;
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

    public void OnTriggerEnter(Collider other)
    {
        if (started && other.transform == player)
        {
            Debug.Log($"Room {room.id} saw player enter");
            WalkedInto();
        }
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
        scoreManager.roomCleared();

        // open all doors
        lockDoors(false, room.id);

        if (entryPoint != null)
        {
            entryPoint.Unlock();
        }


        if (room.id != 0)
        {
            if (true || willSpawnPowerups)
            {
                SpawnPowerups();
                t.TickTutorial(10, room.x, room.y, room.size);
            }
            else if (willSpawnWeapon)
            {
                SpawnWeaponChest();
                t.TickTutorial(7, room.x, room.y, room.size);
            }

            if (room.isBossRoom)
            {
                roomTransform.parent.GetComponent<DungeonGenerator>().MakeTeleporter();
                t.TickTutorial(9, room.x, room.y, room.size);
            }
        }
    }

    public void WalkedInto()
    {
        if (room.id != 0)
        {
            if (t.Unused(6))
            {
                t.TickTutorial(6, room.x, room.y, room.size);
            }
            else if (room.hasBossDoor)
            {
                t.TickTutorial(8, room.x, room.y, room.size);
            }
            else
            {
                t.ClearTutorial(); // remove tutorial when entering a room
            }
        }

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

        showDoors(visible, room.id);
    }

    public void ActivateEnemies()
    {
        if (roomDone || enemiesActivated)
        {
            return;
        }

        if (room.isBossRoom)
        {
            int dungeonN = roomTransform.parent.GetComponent<DungeonGenerator>().GetDungeonN();
            if (dungeonN == 1)
            {
                // special case: single boss spawns in the center of the room
                bosses = new List<Transform>();
                bosses.Add(enemyCreator.CreateBoss(player, room.Center(0.64f)));
                nEnemies = 1;
            }
            else
            {
                /*
                1 => 1 0
                2 => 1 2
                3 => 2 0
                4 => 2 4
                5 => 3 0
                6 => 3 6
                */
                int nBosses = (dungeonN + 1) / 2;
                int nMinions = (dungeonN % 2 == 0) ? dungeonN : 0;

                bosses = new List<Transform>();
                for (int i = 0; i < nBosses; i++)
                {
                    bosses.Add(enemyCreator.CreateBoss(player, room.RandomLocation(0.64f)));
                }

                for (int i = 0; i < nMinions; i++)
                {
                    enemyCreator.CreateEnemy(player, room.RandomLocation(2.0f));
                    nEnemies++;
                }

                nEnemies = nBosses + nMinions;
            }

        }
        else
        {
            int roomN = roomTransform.parent.GetComponent<DungeonGenerator>().TickRoomN();
            if (roomN < 4)
            {
                enemyCreator.SetOnly(1);
            }
            else if (roomN < 8)
            {
                enemyCreator.SetOnly(0, 1);
                t.TickTutorial(11, room.x, room.y, room.size);
            }
            else if (roomN < 12)
            {
                enemyCreator.SetOnly(0, 1, 2);
            }

            var enemyScaling = player.GetComponent<Movement>().stats.enemySpawnFactor + roomN * 0.125f;
            nEnemies = 0;
            for (int i = 0; i < nEnemiesBase * enemyScaling; i++)
            {
                enemyCreator.CreateEnemy(player, room.RandomLocation(2.0f));
                nEnemies++;
            }
        }

        enemiesActivated = true;

        if (nEnemies == 0)
        {
            RoomFinished();
        }
        else
        {
            lockDoors(true, room.id);
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
        wct.position = new Vector3(room.x, 0, room.y);
    }

    public void SpawnPowerups()
    {
        GameObject pp = Instantiate(powerupPedestal);
        Transform ppt = pp.GetComponent<Transform>();
        ppt.parent = transform;
        ppt.position = new Vector3(room.x, 0, room.y);

        pp.BroadcastMessage("SetDungeonRoomScript", this);
    }

    public void DisplayText(string text)
    {
        t.CustomTutorial(text, room.x, room.y, room.size);
    }

    public void DestroyText()
    {
        t.ClearTutorial();
    }
}
