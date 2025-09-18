using System;
using System.Collections;
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

    [SerializeField] private Lane lane;

    public Mob SpawnMob(GameObject prefab)
    {
        StartCoroutine(ClearCurrentMob());
        var currentMob = InstantiateMob(prefab);

        if (isHostile)
        {
            currentMob.MobMovement.MirrorMob();
        }
        
        currentMob.OriginLane = lane;
        currentMob.ChangeLane(currentMob.OriginLane);
        
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

    private IEnumerator ClearCurrentMob()
    {
        if (currentMob)
        {
            // Сначала уведомляем менеджера о том, что моб будет удален
            // Это поможет избежать ссылок на уничтоженный объект
            var mobManager = ServiceLocator.Get<MobManager>();
        
            Destroy(currentMob.gameObject);
            currentMob = null;

            // Даем время на полное уничтожение объекта
            yield return new WaitForSeconds(0.1f);
        
            // Принудительно очищаем списки после уничтожения
            mobManager?.CleanupAllDestroyedMobs();
        }
    }

    /*public int CompareTo(MobSpawner other)
    {
        return transform.position.x.CompareTo(other.transform.position.x);
    }*/
}