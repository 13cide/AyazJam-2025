using UnityEngine;

public class Potion : MonoBehaviour
{
    [Header("Stats")]
    public float speed = 50f;
    public int damage = 50;
    public float explosionRadius = 2f;

    [Header("Setup")]
    public LayerMask enemyLayer;    // Optimization: Only look for enemies

    private Transform target;

    public void Seek(Transform _target)
    {
        target = _target;
    }

    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 dir = target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        if (dir.magnitude <= distanceThisFrame)
        {
            transform.position = target.position;
            Explode();
            return;
        }

        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
        transform.Rotate(0, 0, -10 * Time.deltaTime);
    }

    void Explode()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius, enemyLayer);

        foreach (Collider2D nearbyObject in colliders)
        {
            if (nearbyObject.CompareTag("Enemy"))
            {
                Enemy e = nearbyObject.GetComponent<Enemy>();
                if (e != null) e.GetDamage(damage);
                
                Debug.Log("Hit " + nearbyObject.name);
            }
        }

        Destroy(gameObject);
    }
}
