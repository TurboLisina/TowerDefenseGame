using UnityEngine;

public enum EnemyType { Normal, Tank, Explosive, Elemental }

public enum Element { None, Fire, Water, Ice, Electricity, Poison, Plant }

public class Enemy : MonoBehaviour
{
    [Header("Тип и параметры")]
    public EnemyType type = EnemyType.Normal;
    public float speed = 2f;
    public int maxHealth = 30;
    public int goldReward = 10;   
    public int damageToBase = 1;   

    [Header("Стихи")]
    public Element element = Element.None;

    [Header("Взрывной моб")]
    public float explosionRadius = 1.5f;
    public int explosionDamage = 15;
    public GameObject explosionEffect;  
    private int health;
    private int waypointIndex = 0;
    private Transform target;

    void Start()
    {
        health = maxHealth;
        if (Waypoints.points != null && Waypoints.points.Length > 0)
            target = Waypoints.points[0];
    }


    public void Scale(float healthMult, float goldMult)
    {
        maxHealth = Mathf.RoundToInt(maxHealth * healthMult);
        goldReward = Mathf.RoundToInt(goldReward * goldMult);
        health = maxHealth;  
    }

    void Update()
    {
        if (target == null) return;

        Vector3 dir = target.position - transform.position;
        transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);

        if (Vector3.Distance(transform.position, target.position) <= 0.1f)
            GetNextWaypoint();
    }

    void GetNextWaypoint()
    {
        if (waypointIndex >= Waypoints.points.Length - 1)
        {
            ReachBase();
            return;
        }
        waypointIndex++;
        target = Waypoints.points[waypointIndex];
    }

    void ReachBase()
    {
        GameManager.Instance.TakeDamage(damageToBase);
        WaveSpawner.enemiesAlive--;
        Destroy(gameObject);
    }


    public void TakeDamage(int amount, Element fromElement = Element.None)
    {
        int finalDamage = amount;


        if (type == EnemyType.Elemental)
        {
            if (fromElement == element && element != Element.None)
                finalDamage = 0;                 
            else if (IsOpposite(fromElement, element))
                finalDamage = amount * 2;          
            else if (fromElement == Element.None)
            {
                finalDamage = amount/2;
            }
        }

        health -= finalDamage;
        if (health <= 0) Die();
    }

    bool IsOpposite(Element a, Element b)
    {
        return (a == Element.Fire && b == Element.Ice) ||
               (a == Element.Ice && b == Element.Fire) ||
               (a == Element.Water && b == Element.Electricity) ||
               (a == Element.Electricity && b == Element.Water);
    }

    void Die()
    {
        GameManager.Instance.AddGold(goldReward);
        AudioManager.Money();
        WaveSpawner.enemiesAlive--;

        if (type == EnemyType.Explosive)
            Explode();

        Destroy(gameObject);
    }


    void Explode()
    {

        if (explosionEffect != null)
            Instantiate(explosionEffect, transform.position, Quaternion.identity);

        AudioManager.Explosion();

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        foreach (var h in hits)
        {
            Enemy e = h.GetComponent<Enemy>();
            if (e != null && e != this)
                e.TakeDamage(explosionDamage);
        }
    }

    void OnDrawGizmosSelected()
    {
        if (type == EnemyType.Explosive)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(transform.position, explosionRadius);
        }
    }
}