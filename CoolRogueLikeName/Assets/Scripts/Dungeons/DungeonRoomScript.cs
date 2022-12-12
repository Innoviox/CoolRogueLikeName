using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DungeonRoomScript : MonoBehaviour
{
    float difficulty = 0.5f; // todo
    public Transform player;
    public Room room;
    public bool generateEnemies = true; // todo make private with getter/setter
    public new Camera camera;
    public float cameraMoveDuration;
    public AnimationCurve cameraMoveCurve;

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
    private DungeonDoorScript entryPoint = null;
    private bool roomDone = false;
    private bool enemiesActivated = false;
    private Vector3 cameraPosition; // todo set camera rotation as well

    private EnemyCreator enemyCreator;

    public delegate void DoorToggleDelegate(bool on, int roomId);
    public DoorToggleDelegate showDoors;
    public DoorToggleDelegate lockDoors;
    private bool started = false;
    private int nEnemies;
    private List<Transform> bosses;
    private Tutorial t;
    private Slider bossSlider;

    // Start is called before the first frame update
    void Start()
    {
        roomTransform = GetComponent<Transform>();
        enemyCreator = GetComponent<EnemyCreator>();

        doors = new List<DungeonDoorScript>();

        cameraPosition = room.CameraPosition();

        willSpawnPowerups = Random.Range(0.0f, 1.0f) < 0.25;
        willSpawnWeapon = (!willSpawnPowerups) && Random.Range(0.0f, 1.0f) < 0.25;

        t = roomTransform.parent.GetComponent<DungeonGenerator>().tutorialComp;
    }

    public void StartRoom()
    {
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
    }

    public void OnTriggerEnter(Collider other)
    {
        if (started && other.transform == player)
        {
            WalkedInto();
        }
    }

    private void EnemyDestroyed()
    {
        nEnemies--;

        if (nEnemies == 0)
        {
            RoomFinished();
        }
    }

    private void RoomFinished()
    {
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
            if (willSpawnPowerups)
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
            else if (room.hasBossDoor && !room.isBossRoom)
            {
                t.TickTutorial(8, room.x, room.y, room.size);
            }
            else
            {
                t.ClearTutorial(); // remove tutorial when entering a room
            }
        }

        if (room.isBossRoom)
        {
            roomTransform.parent.GetComponent<MusicPlayer>().state = 2;
        }
        else
        {
            roomTransform.parent.GetComponent<MusicPlayer>().state = 1;
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
            bosses = new List<Transform>();

            if (dungeonN == 1)
            {
                // special case: single boss spawns in the center of the room
                bosses.Add(enemyCreator.CreateBoss(player, room.Center(0.64f)));
                nEnemies = 1;
            }
            else
            {
                // at lower dungeon levels, bosses get extra minions or even multiple bosses
                int nBosses = (dungeonN + 1) / 2;
                int nMinions = (dungeonN % 2 == 0) ? dungeonN : 0;

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

            foreach (Transform t in bosses)
            {
                t.BroadcastMessage("SetHud", bossSlider); // set up hud
            }
        }
        else
        {
            // restrict some enemies to the first rooms
            int roomN = roomTransform.parent.GetComponent<DungeonGenerator>().TickRoomN();
            if (roomN < 4)
            {
                enemyCreator.SetOnly(1);
            }
            else if (roomN < 8)
            {
                enemyCreator.SetOnly(0, 1);
            }
            else if (roomN < 12)
            {
                enemyCreator.SetOnly(0, 1, 2);
            }

            var enemyScaling = player.GetComponent<Movement>().stats.enemySpawnFactor + roomN * 0.125f;
            nEnemies = 0;
            for (int i = 0; i < nEnemiesBase * enemyScaling; i++)
            {
                int enemyType = enemyCreator.CreateEnemy(player, room.RandomLocation(2.0f));
                if (enemyType == 0 && roomN >= 4)
                {
                    t.TickTutorial(11, room.x, room.y, room.size);
                }

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
        float t = 0;
        Vector3 startT = camera.transform.position; // todo use transforms
        while (t < cameraMoveDuration)
        {
            t += Time.deltaTime;
            camera.transform.position = Vector3.Lerp(startT, cameraPosition, t / cameraMoveDuration);
            yield return null;
        }
        Debug.Log("setting camera position to " + cameraPosition);
        camera.transform.position = cameraPosition;
        yield break;
    }

    public bool RoomDone()
    {
        return roomDone;
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

    public void SetHud(Slider bossSlider)
    {
        this.bossSlider = bossSlider;
    }
}
