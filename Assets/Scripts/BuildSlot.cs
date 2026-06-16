using UnityEngine;
using UnityEngine.EventSystems;

public class BuildSlot : MonoBehaviour
{
    [Header("Универсальный префаб башн")]
    public GameObject towerPrefab;

    [Header("Цена постройки на этом слоте")]
    public int buildCost = 50;

    [Header("Подпись с ценой рядом со слотом")]
    public TextMesh priceLabel;

    private GameObject builtTower;
    private Collider2D coliderObject;

    void Awake()
    {
        coliderObject = GetComponent<Collider2D>();
    }

    void Start()
    {
        UpdateLabel();
    }

    void OnMouseDown()
    {
        if (GameManager.Instance.IsGameOver) return;
        if (builtTower != null) return; 

        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject()) return;

        if (TowerChoiceMenu.Instance != null && TowerChoiceMenu.Instance.IsOpen) return;
        if (TowerMenu.Instance != null && TowerMenu.Instance.IsOpen) return;

        if (!GameManager.Instance.CanAfford(buildCost))
        {
            AudioManager.NoMoney();
            return;
        }

        TowerChoiceMenu.Instance.Open(this);
    }


    public void Build(TowerStats stats)
    {
        if (builtTower != null) return;
        if (!GameManager.Instance.SpendGold(buildCost)) return;

        Vector3 pos = transform.position;
        pos.z = -1f; 
        builtTower = Instantiate(towerPrefab, pos, Quaternion.identity);
        Tower t = builtTower.GetComponent<Tower>();
        if (t != null)
        {
            t.ApplyStats(stats);
            t.Init(this, buildCost);
        }

        if (coliderObject != null) coliderObject.enabled = false; 
        AudioManager.Build();
        UpdateLabel();
    }

    public void OnTowerRemoved()
    {
        builtTower = null;
        if (coliderObject != null) coliderObject.enabled = true;    
        UpdateLabel();
    }

    public int BuildCost => buildCost;

    void UpdateLabel()
    {
        if (priceLabel == null) return;
        priceLabel.gameObject.SetActive(builtTower == null);
        priceLabel.text = buildCost.ToString();
    }
}