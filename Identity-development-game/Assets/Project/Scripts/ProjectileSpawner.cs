using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpawner : MonoBehaviour
{
    [Header("Prefab / Spawn")]
    public GameObject projectilePrefab;
    public Transform spawnPoint;
    public Vector3 spawnDirection = Vector3.forward;

    [Header("Spawn Control")]
    [Tooltip("Maximum number of pooled projectiles")]
    public int poolSize = 20;
    [Tooltip("How many objects are spawned each burst")]
    public int objectsPerSpawn = 1;
    [Tooltip("Seconds between spawn bursts")]
    public float spawnInterval = 0.5f;
    [Tooltip("Movement speed of spawned objects")]
    public float speed = 10f;
    [Tooltip("Distance after which projectiles are despawned")]
    public float maxDistance = 20f;
    public bool spawnOnStart = true;

    List<GameObject> pool;
    Coroutine spawnRoutine;

    void Awake()
    {
        InitializePool();
    }

    void OnEnable()
    {
        if (spawnOnStart)
            spawnRoutine = StartCoroutine(SpawnLoop());
    }

    void OnDisable()
    {
        if (spawnRoutine != null)
            StopCoroutine(spawnRoutine);
    }

    void InitializePool()
    {
        pool = new List<GameObject>(poolSize);
        if (projectilePrefab == null)
            return;

        for (int i = 0; i < poolSize; i++)
        {
            var go = Instantiate(projectilePrefab, transform);
            go.SetActive(false);
            var proj = go.GetComponent<Projectile>();
            if (proj == null) proj = go.AddComponent<Projectile>();
            proj.SetOwner(this);
            pool.Add(go);
        }
    }

    IEnumerator SpawnLoop()
    {
        while (true)
        {
            SpawnBurst();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    public void SpawnBurst()
    {
        if (projectilePrefab == null)
            return;

        for (int i = 0; i < objectsPerSpawn; i++)
        {
            var go = GetFromPool();
            if (go == null)
                break;
            Activate(go, i);
        }
    }

    GameObject GetFromPool()
    {
        if (pool == null)
            return null;

        for (int i = 0; i < pool.Count; i++)
        {
            if (!pool[i].activeInHierarchy)
                return pool[i];
        }

        return null;
    }

    void Activate(GameObject go, int index)
    {
        var pos = spawnPoint ? spawnPoint.position : transform.position;
        var dir = spawnPoint ? spawnPoint.forward : transform.TransformDirection(spawnDirection);
        go.transform.position = pos;
        go.transform.rotation = Quaternion.LookRotation(dir);
        var proj = go.GetComponent<Projectile>();
        proj.Initialize(dir, speed, maxDistance);
        go.SetActive(true);
    }

    public void ReturnToPool(GameObject go)
    {
        if (go == null) return;
        go.SetActive(false);
    }

    public void StartSpawning()
    {
        if (spawnRoutine == null)
            spawnRoutine = StartCoroutine(SpawnLoop());
    }

    public void StopSpawning()
    {
        if (spawnRoutine != null)
        {
            StopCoroutine(spawnRoutine);
            spawnRoutine = null;
        }
    }
}
