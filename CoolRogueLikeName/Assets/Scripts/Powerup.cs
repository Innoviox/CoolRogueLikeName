using System.Collections;
using System.Collections.Generic;
using static System.Math;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    public PowerupManager stats;

    public int numEffects = 3;

    public SortedDictionary<string, int> effects;

    public string useKey;

    public List<Material> materialList;

    public List<GameObject> otherOptions;

    public Vector3 oscillation;
    public float oscillationSpeed;
    private Vector3 startPos;
    private float t;
    public DungeonRoomScript dungeonRoomScript;

    // Start is called before the first frame update
    void Start()
    {
        effects = new SortedDictionary<string, int>();
        List<string> effectList = new List<string> {
            "Enemy Movement Speed",     //enemyMoveSpeedFactor
            "Enemy Health",             //enemyHealthFactor
            "Enemy Damage",             //enemyDamageFactor
            "Player Movement Speed",    //playerMoveSpeedFactor
            "Max Health",               //playerHealthFactor
            "Weapon Damage",            //playerDamageFactor
            "Weapon Reload Speed",      //playerReloadSpeedFactor
            "Bullet Speed",             //bulletSpeedFactor
            "Heal Yourself"
        };

        string effect;
        for (int i = 0; i < numEffects; i++)
        {
            effect = effectList[Random.Range(0, effectList.Count)];
            if (effects.ContainsKey(effect))
            {
                effects[effect] += effects[effect] > 0 ? 1 : -1;
            }
            else if (effect == "Heal Yourself")
            {
                effects[effect] = 1;
            }
            else
            {
                effects[effect] = Random.Range(0, 1.0f) > 0.5 ? 1 : -1;
            }
        }
        Debug.Log(GetText());

        startPos = transform.position;
        t = Random.Range(0, 1.0f) * oscillationSpeed;
        gameObject.GetComponent<Renderer>().material = materialList[Random.Range(0, materialList.Count)];
    }

    void Update()
    {
        t += Time.deltaTime % oscillationSpeed;
        transform.position = startPos + (float)System.Math.Sin(t / oscillationSpeed) * oscillation;
    }

    string GetText()
    {
        string text = "";
        foreach (string key in effects.Keys)
        {
            if (effects[key] > 0)
            {
                text += new string('+', effects[key]) + " " + key + "\n";
            }
            else
            {
                text += new string('-', -effects[key]) + " " + key + "\n";
            }
        }
        text = text.Remove(text.Length - 1, 1); // get rid of trailing newline
        return text;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.name.Contains("Player"))
        {
            DisplayText();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.name.Contains("Player"))
        {
            DestroyText();
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.name.Contains("Player"))
        {
            if (Input.GetKey(useKey))
            {
                DestroyText();
                ApplyEffects(other);
                foreach (GameObject o in otherOptions)
                {
                    Destroy(o);
                }
                Destroy(gameObject);
            }
        }
    }

    void DisplayText()
    {
        dungeonRoomScript.DisplayText(GetText());
    }

    void DestroyText()
    {
        dungeonRoomScript.DestroyText();
    }

    // pass other so we can heal the player if we need
    void ApplyEffects(Collider other)
    {
        int count;
        if (effects.TryGetValue("Enemy Movement Speed", out count))
        {
            stats.enemyMoveSpeedFactor *= 1 + count * 0.05f;
        }
        if (effects.TryGetValue("Enemy Health", out count))
        {
            stats.enemyHealthFactor *= 1 + count * 0.05f;
        }
        if (effects.TryGetValue("Enemy Damage", out count))
        {
            stats.enemyDamageFactor *= 1 + count * 0.05f;
        }
        if (effects.TryGetValue("Player Movement Speed", out count))
        {
            stats.playerMoveSpeedFactor *= 1 + count * 0.05f;
        }
        if (effects.TryGetValue("Max Health", out count))
        {
            // TODO: scale up player health, do this better maybe
            stats.playerHealthFactor *= 1 + count * 0.05f;
        }
        if (effects.TryGetValue("Weapon Damage", out count))
        {
            stats.playerDamageFactor *= 1 + count * 0.05f;
        }
        if (effects.TryGetValue("Weapon Reload Speed", out count))
        {
            stats.playerReloadSpeedFactor *= 1 + count * 0.05f;
        }
        if (effects.TryGetValue("Bullet Speed", out count))
        {
            stats.bulletSpeedFactor *= 1 + count * 0.05f;
        }
        if (effects.TryGetValue("Heal Yourself", out count))
        {
            Debug.Log("This would heal the player if it was implemented");
            // TODO: This
            /*            HealthComponent healthcomp = other.gameObject.GetComponent<Health?>();
                        other.gameObject.GetComponent<Health?>().health += count * 0.2f * healthcomp.maxhealth;*/
        }
        stats.acquiredPowerups.Add(GetText());
    }

    void SetDungeonRoomScript(DungeonRoomScript dungeonRoomScript)
    {
        this.dungeonRoomScript = dungeonRoomScript;
    }
}