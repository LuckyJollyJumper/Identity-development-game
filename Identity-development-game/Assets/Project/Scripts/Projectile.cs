using UnityEngine;

public class Projectile : MonoBehaviour
{
    Vector3 direction;
    float speed;
    float maxDistance;
    Vector3 startPos;
    ProjectileSpawner owner;

    public void SetOwner(ProjectileSpawner spawner)
    {
        owner = spawner;
    }

    public void Initialize(Vector3 dir, float speed, float maxDistance)
    {
        this.direction = dir.normalized;
        this.speed = speed;
        this.maxDistance = maxDistance;
        startPos = transform.position;
    }

    void OnEnable()
    {
        startPos = transform.position;
    }

    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
        if (Vector3.Distance(startPos, transform.position) >= maxDistance)
        {
            Despawn();
        }
    }

    void Despawn()
    {
        if (owner != null)
            owner.ReturnToPool(gameObject);
        else
            Destroy(gameObject);
    }
}
