using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 8f;

    private Transform target;
    private Vector3 lastTargetPos;   
    private bool hasLastPos = false;
    private int damage;
    private Element element;

    public void Seek(Transform _target, int _damage, Element _element)
    {
        target = _target;
        damage = _damage;
        element = _element;

        if (target != null)
        {
            lastTargetPos = target.position;
            hasLastPos = true;
        }
    }

    void Update()
    {
        // Куда лететь?
        Vector3 destination;

        if (target != null)
        {
            destination = target.position;
            lastTargetPos = destination;   
        }
        else if (hasLastPos)
        {
            destination = lastTargetPos;   
        }
        else
        {
            Destroy(gameObject);         
            return;
        }

        Vector3 dir = destination - transform.position;
        float distThisFrame = speed * Time.deltaTime;

        if (dir.magnitude <= distThisFrame)
        {
            if (target != null)
                HitTarget();        
            else
                Destroy(gameObject); 
            return;
        }

        transform.Translate(dir.normalized * distThisFrame, Space.World);
    }

    void HitTarget()
    {
        Enemy e = target.GetComponent<Enemy>();
        if (e != null)
            e.TakeDamage(damage, element);

        Destroy(gameObject);
    }
}