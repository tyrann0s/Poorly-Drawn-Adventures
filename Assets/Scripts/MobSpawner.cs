using UnityEngine;
using System.Collections.Generic;

public class MobSpawner : MonoBehaviour
{
    [SerializeField, Tooltip("If true, the spawned mob will be mirrored and hostile")]
    private bool isHostile;

    [SerializeField, Tooltip("List of mob prefabs that can be spawned")]
    private List<GameObject> mobPrefabs = new();

    private Mob currentMob;
    private GameManager gameManager;

    private void Awake()
    {
        gameManager = FindFirstObjectByType<GameManager>();
        if (gameManager == null)
        {
            Debug.LogError($"GameManager not found in scene");
        }
    }

    private void Start()
    {
        SpawnMob();
    }

    public void SpawnMob()
    {
        ClearCurrentMob();
        InstantiateMob();

        if (isHostile)
        {
            currentMob.MirrorMob();
        }
    }

    private void InstantiateMob()
    {
        if (mobPrefabs == null || mobPrefabs.Count == 0)
        {
            Debug.LogError($"No mob prefabs assigned to {gameObject.name}");
            return;
        }

        int rand = Random.Range(0, mobPrefabs.Count);
        GameObject mobGo = Instantiate(mobPrefabs[rand], transform);
        
        currentMob = mobGo.GetComponent<Mob>();
        if (currentMob == null)
        {
            Debug.LogError($"Mob component not found on prefab {mobGo.name}");
            Destroy(mobGo);
            return;
        }

        currentMob.IsHostile = isHostile;
        currentMob.OriginPosition = new Vector3(
            transform.position.x,
            transform.position.y + currentMob.transform.localPosition.y,
            transform.position.z
        );

        gameManager.AddMob(currentMob);
    }

    public void ClearCurrentMob()
    {
        if (currentMob != null)
        {
            Destroy(currentMob.gameObject);
            currentMob = null;
        }
    }
}