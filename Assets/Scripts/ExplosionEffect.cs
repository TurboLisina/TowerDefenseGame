using UnityEngine;


[RequireComponent(typeof(SpriteRenderer))]
public class ExplosionEffect : MonoBehaviour
{
    [Header("Кадры анимации взрыв")]
    public Sprite[] frames;

    [Header("Кадров в секунду")]
    public float fps = 12f;

    private SpriteRenderer sr;
    private float timer;
    private int index;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        if (frames == null || frames.Length == 0)
        {
            Destroy(gameObject, 0.5f);  
            return;
        }
        sr.sprite = frames[0];
    }

    void Update()
    {
        if (frames == null || frames.Length == 0) return;

        timer += Time.deltaTime;
        if (timer >= 1f / fps)
        {
            timer = 0f;
            index++;
            if (index >= frames.Length)
            {
                Destroy(gameObject);
                return;
            }
            sr.sprite = frames[index];
        }
    }
}
