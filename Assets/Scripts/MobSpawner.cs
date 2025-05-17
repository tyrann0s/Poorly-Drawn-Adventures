using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobSpawner : MonoBehaviour
{
    [SerializeField]
    private bool isHostile;

    [SerializeField]
    private List<GameObject> mobPrefabs = new List<GameObject>();

    private Mob currentMob;

    private void Start()
    {
        SpawnMob();
    }

    public void SpawnMob()
    {
        InstantiateMob();

        if (isHostile)
        {
            currentMob.MirrorMob();
        }
    }

    private void InstantiateMob()
    {
        int rand = Random.Range(0, mobPrefabs.Count);
        GameObject mobGO = Instantiate(mobPrefabs[rand],  transform);
        currentMob = mobGO.GetComponent<Mob>();
        currentMob.IsHostile = isHostile;
        currentMob.OriginPosition = new Vector3 (transform.position.x, transform.position.y + currentMob.transform.localPosition.y, transform.position.y);

        FindObjectOfType<GameManager>().AddMob(currentMob);
    }
}
