using System;
using DG.Tweening;
using UnityEngine;
using Managers;
using Mobs;

public class MobSpawner : MonoBehaviour//, IComparable<MobSpawner>
{
    [SerializeField, Tooltip("If true, the spawned mob will be mirrored and hostile")]
    private bool isHostile;
    public bool IsHostile => isHostile;

    private Mob currentMob;

    [SerializeField] private Transform readyPosition;

    public Mob SpawnMob(GameObject prefab)
    {
        ClearCurrentMob();
        var currentMob = InstantiateMob(prefab);

        if (isHostile)
        {
            currentMob.MobMovement.MirrorMob();
        }
        
        return currentMob;
    }

    private Mob InstantiateMob(GameObject prefab)
    {
        if (!prefab)
        {
            Debug.LogError($"No mob prefabs assigned to {gameObject.name}");
            return null;
        }
        
        GameObject mobGo = Instantiate(prefab, transform);
        
        currentMob = mobGo.GetComponent<Mob>();

        currentMob.IsHostile = isHostile;
        currentMob.MobMovement.OriginPosition = new Vector3(
            transform.position.x,
            transform.position.y + currentMob.transform.localPosition.y,
            transform.position.z
        );
        currentMob.MobMovement.ReadyPosition = readyPosition.position;

        return currentMob;
    }

    private void ClearCurrentMob()
    {
        if (currentMob)
        {
            Destroy(currentMob.gameObject);
            currentMob = null;
        }
    }

    /*public int CompareTo(MobSpawner other)
    {
        return transform.position.x.CompareTo(other.transform.position.x);
    }*/
}