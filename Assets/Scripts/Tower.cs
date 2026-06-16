using UnityEngine;
using UnityEngine.EventSystems;

public class Tower : MonoBehaviour
{
    [Header("Параметры башни")]
    public string title = "Башня";
    public float range = 3f;
    public float fireRate = 1f;
    public int damage = 10;
    public Element element = Element.None;

    [Header("Ссылки")]
    public GameObject bulletPrefab;
    public Transform firePoint;

    [Header("Улучшение")]
    public int upgradeCost = 40;

    [Header("Поворот к цели")]
    public Transform rotatingPart;       
    public float turnSpeed = 720f;      
    public float spriteAngleOffset = 0f; 

    private Transform target;
    private float fireCountdown = 0f;

    private BuildSlot slot;    
    private int totalSpent;    

    void Update()
    {
        FindTarget();
        if (target == null) return;

        AimAt(target);  

        fireCountdown -= Time.deltaTime;
        if (fireCountdown <= 0f)
        {
            Shoot();
            fireCountdown = 1f / Mathf.Max(fireRate, 0.1f);
        }
    }

    void AimAt(Transform t)
    {
        Transform pivot = (rotatingPart != null) ? rotatingPart : transform;

        Vector3 dir = t.position - pivot.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + spriteAngleOffset;
        Quaternion targetRot = Quaternion.Euler(0f, 0f, angle);

        if (turnSpeed <= 0f)
            pivot.rotation = targetRot;                 
        else
            pivot.rotation = Quaternion.RotateTowards(  
                pivot.rotation, targetRot, turnSpeed * Time.deltaTime);
    }

    void FindTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float shortest = Mathf.Infinity;
        Transform nearest = null;
        foreach (var e in enemies)
        {
            float d = Vector3.Distance(transform.position, e.transform.position);
            if (d < shortest && d <= range)
            {
                shortest = d;
                nearest = e.transform;
            }
        }
        target = nearest;
    }

    void Shoot()
    {
        if (bulletPrefab == null || firePoint == null) return;
        GameObject b = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Bullet bullet = b.GetComponent<Bullet>();
        if (bullet != null)
            bullet.Seek(target, damage, element);

        AudioManager.Shoot();
    }

    public void ApplyStats(TowerStats s)
    {
        title = s.title;
        range = s.range;
        fireRate = s.fireRate;
        damage = s.damage;
        element = s.element;

        SpriteRenderer sr = null;
        if (rotatingPart != null) sr = rotatingPart.GetComponentInChildren<SpriteRenderer>();
        if (sr == null) sr = GetComponent<SpriteRenderer>();
        if (sr != null) sr.color = s.color;
    }

    public void Init(BuildSlot ownerSlot, int buildCost)
    {
        slot = ownerSlot;
        totalSpent = buildCost;
    }

    void OnMouseDown()
    {
        if (GameManager.Instance.IsGameOver) return;

        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject()) return;

        if (TowerMenu.Instance != null)
            TowerMenu.Instance.Open(this);
    }

    public int SellValue => totalSpent / 2;      
    public int UpgradeCost => upgradeCost + totalSpent / 3;


    public bool Upgrade()
    {
        if (!GameManager.Instance.SpendGold(UpgradeCost)) return false;
        if (this.element != Element.None)
        {
            UpgradeElemental();
            return true;
        }
        if (fireRate < 3f)
        {
            damage += 11;
            range += 0.03f;
            if (fireRate < 2.63f) fireRate += 0.3f;
        }
        else
        {
            damage += 1;
            fireRate += 0.7f;
            range += 0.06f;
        }

        totalSpent += UpgradeCost;
        return true;
    }
    private void UpgradeElemental()
    {
        damage += 16;
        fireRate += 0.4f;
        range += 0.056f;
        totalSpent += upgradeCost;
    }
    public void Sell()
    {
        GameManager.Instance.AddGold(SellValue);
        if (slot != null) slot.OnTowerRemoved();
        Destroy(gameObject);
    }

    public string Describe()
    {
        string typeName = (element != Element.None) ? TowerStats.ElementName(element) : title;
        return "Тип: " + typeName
             + "\nУрон: " + damage
             + "\nСкорость: " + fireRate.ToString("0.0") + " выстр/с"
             + "\nРадиус: " + range.ToString("0.0");
    }

    private SpriteRenderer rangeCircle;
    private static Sprite rangeSprite;

    public void ShowRange()
    {
        if (rangeCircle == null) CreateRangeCircle();
        rangeCircle.transform.localScale = Vector3.one * (range * 2f); 
        rangeCircle.enabled = true;
    }

    public void HideRange()
    {
        if (rangeCircle != null) rangeCircle.enabled = false;
    }

    void CreateRangeCircle()
    {
        GameObject go = new GameObject("RangeCircle");
        go.transform.SetParent(transform, false);
        go.transform.localPosition = Vector3.zero;

        rangeCircle = go.AddComponent<SpriteRenderer>();
        rangeCircle.sprite = GetRangeSprite();
        rangeCircle.color = new Color(0.3f, 0.8f, 1f, 1f);
        rangeCircle.sortingOrder = 40;
    }

    static Sprite GetRangeSprite()
    {
        if (rangeSprite != null) return rangeSprite;

        int size = 128;
        Texture2D tex = new Texture2D(size, size);
        tex.wrapMode = TextureWrapMode.Clamp;

        float r = size / 2f;
        Vector2 c = new Vector2(r, r);

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                float d = Vector2.Distance(new Vector2(x, y), c);
                float a;
                if (d > r) a = 0f;                 
                else if (d > r * 0.9f) a = 0.55f;  
                else a = 0.12f;                   
                tex.SetPixel(x, y, new Color(1f, 1f, 1f, a));
            }
        }
        tex.Apply();

        rangeSprite = Sprite.Create(tex, new Rect(0, 0, size, size),
                                    new Vector2(0.5f, 0.5f), size);
        return rangeSprite;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}