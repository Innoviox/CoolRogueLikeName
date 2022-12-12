using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordBehaviour : MonoBehaviour
{
    public Transform player;
    public Transform OriginalPos;
    public Transform EnemyCenter;
    bool CollidedEnv; // Check when sword has collided with the floor or a wall
    LayerMask mask;   // mask for floor and wall
    public SwordShootAttack sword;
    BoxCollider coll;

    private void Awake()
    {
        CollidedEnv = false;
        mask = LayerMask.GetMask("Wall", "Door", "Floor");
        coll = GetComponent<BoxCollider>();
        coll.enabled = false;
    }

    /// <summary>
    /// Aims the sword at the player
    /// </summary>
    /// <returns></returns>
    public IEnumerator AimTowardsPlayer()
    {
        while (true)
        {
            var lookPos = player.position - transform.position;
            var rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 1);

            yield return new WaitForSeconds(0.01f);
        }
    }

    /// <summary>
    /// Shoots the Sword forward towards the direction the sword is aiming.
    /// </summary>
    /// <returns></returns>
    public IEnumerator ThrustForward()
    {
        coll.enabled = true;

        do
        {
            Vector3 temp = 20 * Time.deltaTime * transform.forward;
            transform.position += temp;
            yield return null;
        } while (!CollidedEnv);

        // Collision is only checked while sword is being shot forward
        coll.enabled = false;
        CollidedEnv = false;
        yield return new WaitForSeconds(1.5f);

        // When colliding with floor or wall we go back to original position
        while (!BackToOriginal())
        {
            Vector3 temp = -1 * 10 * Time.deltaTime * transform.forward;
            transform.position += temp;
            yield return null;
        }
        sword.currentState = SwordShootAttack.swordState.finishedAttack;
    }

    /// <summary>
    /// Checks if the sword is close enough to its original position
    /// </summary>
    /// <returns></returns>
    private bool BackToOriginal()
    {
        if (transform.position.x <= OriginalPos.position.x + 1 && transform.position.x >= OriginalPos.position.x - 1 &&
            transform.position.y <= OriginalPos.position.y + 1 && transform.position.y >= OriginalPos.position.y - 1 &&
            transform.position.z <= OriginalPos.position.z + 1 && transform.position.z >= OriginalPos.position.z - 1)
        {
            transform.rotation = OriginalPos.rotation;
            transform.position = OriginalPos.position;
            return true;
        }
        return false;
    }

    // Check if sword has collided with the environment
    private void OnTriggerEnter(Collider other)
    {
        if (InLayer(other, mask))
        {
            CollidedEnv = true;
        }
    }

    private bool InLayer(Collider other, LayerMask mask)
    {
        return ((1 << other.gameObject.layer) & mask.value) > 0;
    }

    public void SetPlayer(Transform player)
    {
        this.player = player;
    }
}
